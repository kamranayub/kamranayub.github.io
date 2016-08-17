---
layout: post
title: "Extending PowerShell Pester with Custom Assertions"
date: 2016-08-16 20:10:00 -0600
comments: true
published: true
categories:
- PowerShell
- OSS
- Testing
---

After the initial release of [my OpenWeatherMap PowerShell module](http://kamranicus.com/blog/2016/08/12/posh-openweathermap-powershell-module/) I decided it might be a good idea to write some tests. This was my first time using [Pester](https://github.com/pester/Pester), a BDD-style testing framework for PowerShell. Coming from [Jasmine](http://jasmine.github.io/) Javascript unit testing, I felt right at home.

<!-- More -->

Now because my module deals a lot with dates, I wanted to use an assertion that simplified some of the logic of testing a date. For example, this is what it looks like validating a DateTime:

```powershell
$Result.Time.Year   | Should Be 2015
$Result.Time.Month  | Should Be 8
$Result.Time.Day    | Should Be 15

$Result.Time.Hour   | Should Be 13
$Result.Time.Minute | Should Be 56
$Result.Time.Second | Should Be 32
```

The other way would be to compare formatted date strings:

```powershell
$Result.Time.ToString("MM/dd/yyyy HH:mm:ss") | Should Be "08/15/2015 13:56:32"
```

Which is fine. However, using it as a learning opportunity, I wanted to write my own assertion for Pester, `BeDate` so I could do:

```powershell
$Result.Time | Should BeDate "08/15/2015 13:56:32"
```

In C# using NUnit or MSTest, writing custom assertions is pretty straightforward. However, Pester essentially uses a convention where it loads up commands in its module starting with `Pester` and also having a couple failure message handlers.

So, for example, our `BeDate` assertion is defined as:

```powershell
Function PesterBeDate($Value, $Expected) {
    $Expected = [System.DateTime]::Parse($Expected)

    $Value.Year   | Should Be $Expected.Year
    $Value.Month  | Should Be $Expected.Month
    $Value.Day    | Should Be $Expected.Day

    $Value.Hour   | Should Be $Expected.Hour
    $Value.Minute | Should Be $Expected.Minute
    $Value.Second | Should Be $Expected.Second
}

Function PesterBeDateFailureMessage($Value, $Expected) {
    if (-not (($expected -is [string]) -and ($value -is [System.DateTime])))
    {
        return "Expected: {$expected}`nBut was:  {$value}"
    }
    
    return "Expected: $Expected\nBut was: $($Value.ToString('MM/dd/YYYY h:mm:ss'))"
}

Function NotPesterBeDateFailureMessage($Value, $Expected) {
    return PesterBeDateFailureMessage -Value $Value -Expected $Expected
}
```

Now, I don't actually think my `PesterBeDateFailureMessage` cmdlet ever runs because I'm using the basic `Be` assertion but whatever--this is the convention. 

So, how we do we get Pester to see these functions? Well, like I said, it searches for the assertions **in the scope of the module**. That means if I define these in my own file, it won't see them.

## Defining functions inside a module's scope

We can use a weird PowerShell "hack" to actually declare these functions *inside the scope of the Pester module* if we want.

```powershell
$pesterModule = Import-Module Pester -PassThru

. $pesterModule {
    function PesterBeDate { }
    # etc
}
```

This is what Dave Wyatt suggested [when I asked about it](https://github.com/pester/Pester/issues/590#issuecomment-239977094). Cool, we **could** do that but I would prefer if we could keep my custom assertions separated from my tests since I'd have to include this in my **.Tests.ps1** files.

## Using local modules and extending Pester

The  implication the previous solution has is that the caller (user) has Pester installed globally. Over the years, as a programming community we decided that's a sub-optimal outlook on life hence we have package managers like Nuget and NPM. To my knowledge, there's not yet a `package.json` equivalent to a PowerShell "project" besides the Module Manifest (psd1). Ideally, I would be able to type `Install-Module` in the current directory, PowerShellGet would identify the dependencies (perhaps from psd1 manifest?), and download them. Alas, it doesn't so we can do it ourselves.

To achieve maximum contributability (is that a word?) I decided to roll my own little build script that would bring down the Pester module locally to the project and then extend Pester's assertions by manually copying in my own code.

This only takes a few lines of Powershell:

```powershell
$PesterVersion = '3.4.2'

# Save-module locally
Save-Module -Name Pester -Path '.modules\' -RequiredVersion $PesterVersion

# Copy custom assertions
Copy-Item -Path '.\Assertions\*.ps1' -Destination ".\.modules\Pester\$PesterVersion\Functions\Assertions"

# Import local Pester module so we can extend built-in assertions
Import-Module ".\.modules\Pester\$PesterVersion\Pester.psd1"

# Run tests
Invoke-Pester -Script ".\OpenWeatherMap.Tests.ps1"
```

So this is fairly straightforward:

1. Use `Save-Module` cmdlet to unzip and copy the Pester module locally to a **.modules** folder that we'll exclude from source control
2. Copy my `*.ps1` assertion files under **Assertions\** to the local Pester assertions directory
3. Import the local Pester module explicitly (this will load our new assertions)
4. Invoke Pester on our test script (if you don't qualify it, it will run all the tests in the Pester module also!)

Now we achieve both desired effects:

1. Our repository is now self-contained and can be contributed to without any global dependencies (besides PowerShell 5)
2. We can keep our custom assertions separated

If you want to see the final product, [give my OpenWeatherMap module a gander](https://github.com/kamranayub/posh-openweathermap)!
