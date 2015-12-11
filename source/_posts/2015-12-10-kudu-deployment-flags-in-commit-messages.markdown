---
layout: post
title: "Influencing your Kudu deployment through Git commit messages"
date: 2015-12-10 20:30:00 -0500
comments: true
published: true
categories:
- Git
- Kudu
- Testing
- Continuous Deployment
- Continuous Integration
- Windows Azure
---

If you're on Windows Azure and using continuous deployment through Git, you may not know you are using an open source platform called 
[Kudu][1] behind-the-scenes that performs your deployment. If this is the first time you've heard of Kudu and you've been 
using Azure for awhile, it's time to [get acquainted][1]. Kudu is amazing. It has a whole REST API that lets you manage
deployments, trigger builds, trigger webjobs, view processes, a command prompt, and a ton more.

You can get to your Kudu console by visiting

    https://<yoursite>.scm.azurewebsites.net
    
The **.scm.** part is the key, as that is where the Kudu site is hosted.

## Customizing Kudu deployments

One of the other things it offers is a [customized deployment script][2]. I've customized mine because I have a test
project where I run automated tests during the build. This is useful since it'll fail the build if I make any changes
that break my tests and forces me to keep things up-to-date resulting in a higher quality codebase.

If you want to generate your own script, it's pretty easy. Just follow the steps [outlined here][2]. For example, after
customizing my script here's what my section looks like to run my tests:

```
:: 3. Build unit tests
call :ExecuteCmd "%MSBUILD_PATH%" "%DEPLOYMENT_SOURCE%\src\Tests\Tests.csproj" /nologo /verbosity:m /t:Build /p:AutoParameterizationWebConfigConnectionStrings=false;Configuration=Release /p:SolutionDir="%DEPLOYMENT_SOURCE%\.\\" %SCM_BUILD_ARGS%

IF !ERRORLEVEL! NEQ 0 goto error
```

All I really did was copy step 2 in the script that builds my web project and just change the path to my tests project.

Finally, I run the tests using the packaged Nunit test runner (checked into source control):

```
call :ExecuteCmd "%DEPLOYMENT_SOURCE%\tools\nunit\nunit-console.exe" "%DEPLOYMENT_SOURCE%\src\Tests\bin\Release\Tests.dll" /framework:v4.5.1
IF !ERRORLEVEL! NEQ 0 goto error
```

Simple!

## Now the fun part

One thing you'll notice if you start running tests on your builds is that this starts to slow down your continuous deployment workflow. 
For 90% of the time this is acceptable, after all, you can wait a few minutes to see your changes show up on the site. But sometimes, 
especially for production hotfixes or trial-and-error config changes, that 3-5 minutes becomes unbearable.

In cases like this, I've set up a little addition to my script that will read the git commit message and take action depending on what phrases it sees.

For example, let's say I commit a change that is just a config change and I know I don't need to run any tests or I really want the quick build. 
This is what my commit message looks like:

    [notest] just changing App.config

That phrase `[notest]` is something my script looks for at build time and if it's present it will skip running tests! 
You can use this same logic to do pretty much anything you want. Here's what it looks like after step 3 in my script:

```
:: Above at top of file

IF NOT DEFINED RUN_TESTS (
   SET RUN_TESTS=1
)

:: 4. Run unit tests
echo Latest commit ID is "%SCM_COMMIT_ID%"

call git show -s "%SCM_COMMIT_ID%" --pretty=%%%%s > commitmessage.txt
SET /p COMMIT_MESSAGE=<commitmessage.txt

echo Latest commit message is "%COMMIT_MESSAGE%"

IF NOT "x%COMMIT_MESSAGE:[notest]=%"=="x%COMMIT_MESSAGE%" (
   SET RUN_TESTS=0
)

IF /I "%RUN_TESTS%" NEQ "0" (
	echo Running unit tests
	call :ExecuteCmd "%DEPLOYMENT_SOURCE%\tools\nunit\nunit-console.exe" "%DEPLOYMENT_SOURCE%\src\Tests\bin\Release\Tests.dll" /framework:v4.5.1
	IF !ERRORLEVEL! NEQ 0 goto error
) ELSE (
	echo Not running unit tests because [notest] was present in commit message
)
```

Alright, there's definitely some batch file black magic incantations going on here! So let's break it down.

    echo Latest commit ID is "%SCM_COMMIT_ID%"

Kudu defines several useful [environment variables][3] that you have access to, including the current commit ID.
I'm just echoing it out so I can debug when viewing the log output.

    call git show -s "%SCM_COMMIT_ID%" --pretty=%%%%s > commitmessage.txt
    SET /p COMMIT_MESSAGE=<commitmessage.txt

Alright. This took me some real trial and error. Git lets you [`show` any commit message][4] and can format it using a printf format string (`--pretty=%s`).
However, due to the weird escaping rules of batch files and variables, this requires not one but **four** `%` signs. Go figure.

Next I pipe it to a file, this is only so I can read the file back and store the message in a batch variable (`COMMIT_MESSAGE`), on the next line.
**Kudu team:** It would be sweet to add a `SCM_COMMIT_MESSAGE` environment variable!

    IF NOT "x%COMMIT_MESSAGE:[notest]=%"=="x%COMMIT_MESSAGE%" (
       SET RUN_TESTS=0
    )

Okay, what's going on here? I'll [let StackOverflow explain][5]. The `:[notest]=` portion REPLACES the term "[notest]" in 
the preceding variable (`COMMIT_MESSAGE`) with an empty string. The `x` prefix character guards against batch file weirdness.
So if `[notest]` is NOT present, this will return true (the strings match). If it is present, the condition will be false and so we do `IF NOT`
since we want to execute when that is the case.

If `[notest]` is present in the message, we set another variable, `RUN_TESTS` to 0.

    IF /I "%RUN_TESTS%" NEQ "0" (
    	echo Running unit tests
    	call :ExecuteCmd "%DEPLOYMENT_SOURCE%\tools\nunit\nunit-console.exe" "%DEPLOYMENT_SOURCE%\src\Tests\bin\Release\Tests.dll" /framework:v4.5.1
    	IF !ERRORLEVEL! NEQ 0 goto error
    ) ELSE (
    	echo Not running unit tests because [notest] was present in commit message
    )

If `RUN_TESTS` does not evaluate to 0, then we run the tests! Otherwise we echo out an informative message as to why it was skipped.

Phew. So how much time do we save on `[notest]` builds now?

![No test build](https://cloud.githubusercontent.com/assets/563819/11734864/9a5e5e10-9f80-11e5-92ff-b93a1d9c994a.png)

Compared to a build with tests:

![Build with tests](https://cloud.githubusercontent.com/assets/563819/11734880/b874bed0-9f80-11e5-9af2-8e0425a02563.png)

So that flag cuts the build in half! Nice! There are probably some other ways to improve the time. By the way, if you're wondering what's taking so long
in your build, you can use the Kudu [REST endpoint][6] to see your deployment logs (**/api/deployments** endoint) which contain full timestamp information!

Happy continuous deployment!

[1]: https://github.com/projectkudu/kudu
[2]: https://github.com/projectkudu/kudu/wiki/Custom-Deployment-Script
[3]: https://github.com/projectkudu/kudu/wiki/Deployment-Environment
[4]: https://git-scm.com/docs/git-show
[5]: http://stackoverflow.com/questions/7005951/batch-file-find-if-substring-is-in-string-not-in-a-file
[6]: https://github.com/projectkudu/kudu/wiki/REST-API
