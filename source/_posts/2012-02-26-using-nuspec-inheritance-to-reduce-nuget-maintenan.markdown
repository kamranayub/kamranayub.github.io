---
layout: post
title: "Using Nuspec inheritance to reduce Nuget maintenance headaches"
date: 2012-02-26 20:16:16 -0600
comments: true
categories:
permalink: /blog/posts/32/using-nuspec-inheritance-to-reduce-nuget-maintenan
disqus_identifier: 32
---

I've been [helping Cassette support .NET 3.5](https://github.com/andrewdavey/cassette/pull/191) and it's the biggest open source contribution I've ever made. Granted, I started it so that we could use Cassette at work but I did it of my own accord. I learned a lot in the process (which I'll write another post on) but I thought I'd talk about a cool trick I came up with to help ease Cassette's Nuget packaging burden.

## The Problem

Cassette has 7 Nuget packages:

* Cassette + symbols
* Cassette.Views + symbols
* Cassette.Web + symbols
* Cassette.MSBuild

This means Andrew has to maintain 7 separate Nuspec files. Imagine now if we were forced to create a new .NET 3.5-specific Nuget package or MVC2-specific package or whatever; we'd have even more Nuspecs to deal with.

What's even worse is that all the Nuspec files share common metadata. Typically the only metadata that differs is: `id`, `description`, `dependencies`, and `references`.

## XDT Transformations to the Rescue

In MSBuild 4, Microsoft introduced the concept of [XML Document Transformation](http://msdn.microsoft.com/en-us/library/dd465326.aspx). This made it really helpful to create Debug/Release-specific `web.config` files for your site.

However, what you may not know is that XDT can be used on *any* document. It can [also be used manually](http://geekswithblogs.net/EltonStoneman/archive/2010/08/20/using-msbuild-4.0-web.config-transformation-to-generate-any-config-file.aspx) in your MSBuild file:

```xml
<UsingTask TaskName="TransformXml" AssemblyFile="$(MSBuildExtensionPath)\Microsoft\VisualStudio\v10.0\Web\Microsoft.Web.Publishing.Tasks.dll"/>

<Target Name="GenerateConfigs">
    <MakeDir Directories="$(BuildOutput)" Condition="!Exists('$(BuildOutput)')"/>
    <TransformXml Source="Web.config"
                  Transform="Web.$(Configuration).config"
                  Destination="$(BuildOutput)\Web.config"/>
</Target>
```

Neat. I thought about this a bit and figured, "Hey, why not use this to reduce Nuspec maintenance headache!"

So I did a toy exercise by creating a `Cassette.Shared.nuspec` file:

```xml
<?xml version="1.0"?>
<!-- Cassette Shared Nuspec File; all packges share this -->
<package xmlns="http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd">
  <metadata>
    <version>$version$</version>
    <authors>Andrew Davey</authors>
    <owners>Andrew Davey</owners>
    <copyright>© 2011 Andrew Davey</copyright>
    <licenseUrl>http://getcassette.net/licensing</licenseUrl>
    <projectUrl>http://getcassette.net/</projectUrl>
    <tags>web javascript coffeescript css html templates asp.net</tags>
  </metadata>
</package>
```

And a "Cassette.Web.nutrans" transformation file that just added the extra data required:

```xml
<?xml version="1.0"?>
<package xmlns="http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd" xmlns:xdt="http://schemas.microsoft.com/XML-Document-Transform">
    <metadata>
        <id xdt:Transform="Insert">Cassette.Web</id>
        <description xdt:Transform="Insert">ASP.NET support for Cassette. Cassette automatically builds JavaScript, CSS and HTML template modules based on the dependencies between files. CoffeeScript and LESS are also supported.</description>       
        <dependencies xdt:Transform="Insert">
            <dependency id="Cassette" version="$version$" />
            <dependency id="Cassette.Views" version="$version$" />
        </dependencies>
    </metadata>
    
    <files xdt:Transform="Insert">
      <file src="..\..\build\bin\lib40\Cassette.Web.dll" target="lib\net40" />
      <file src="..\..\build\bin\lib35\Cassette.Web.dll" target="lib\net35" />
      <file src="CassetteConfiguration.cs.pp" target="content"/>
      <file src="web.config.transform" target="content" />
    </files>
</package>
```

**Mind = blown.** It worked! As I stroked my five-o-clock shadow, I wondered. *Wow, if this works, how far can we take it?*

## Nuspec Inheritance: Say what?!

Let's consider the symbols packaging for Cassette. Being able to transform all these Nuspecs is great and will reduce headaches, but what happens when a Nuspec is a subset of yet another Nuspec? The symbols package typically just adds some PDB files and a `**\*.cs` line.

Furthermore, I'd **really** like to avoid calling the `TransformXml` task on each symbols Nuspec individually. 

No, what I really want is a way to make my Nuspecs inherit from each other to create a chain of transformation. 

**So that's what I did.** Nuget package developers of the world, I present to you an inline MSBuild task that will follow a chain of inheritance and transform your Nuspecs:

<script src="https://gist.github.com/1918022.js?file=Transforms.xml"></script>

It looks complicated but it really isn't. I had to workaround [an issue with MSBuild inline tasks](http://stackoverflow.com/questions/9455354/msbuild-inline-task-reference-non-standard-microsoft-assemblies) by loading up the Microsoft.Web.Publishing.Tasks assembly via Reflection. The rest of the code discovers the inheritance chain and performs the transformations. I developed this in a standalone console application first, then copy/pasted it into the inline task definition, changing whatever I needed. 

I could have made this into a standalone DLL but what fun is that? Then I'd have to manage a solution and project and blah blah blah. No, I wanted this to be simple and easy to just add to any MSBuild project.

## Example Usage

Let's take the symbols example now and see what we can do.

**Cassette.Web.nutrans**

We have to add an extra attribute to the `<package>` element:

```xml
<package inherits="../Cassette.Shared.nuspec" ...>
```

The `inherits` attribute is relative to the directory the nutrans file is in.

**Cassette.Web.symbols.nutrans**

```xml
<?xml version="1.0"?>
<package inherits="Cassette.Web.nutrans" xmlns="http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd" xmlns:xdt="http://schemas.microsoft.com/XML-Document-Transform">
  <files>
    <file xdt:Transform="Insert" src="..\..\build\bin\lib40\Cassette.Web.pdb" target="lib\net40" />
    <file xdt:Transform="Insert" src="..\..\build\bin\lib35\Cassette.Web.pdb" target="lib\net35" />
    <file xdt:Transform="Insert" src="**\*.cs" target="src" />
  </files>
</package>
```

Now, using the `inherits` attribute, we can transform the Cassette.Web transformation file letting us create a chain of transformations.

To use this task, we can easily pass it the list of transforms to use and let it work its magic.

**build.xml**

```xml
<Target Name="NugetPack" DependsOnTargets="Build">
        <ItemGroup>
            <Transforms Include="src\**\*.nutrans" />
        </ItemGroup>

        <!-- Transform Nuspecs -->
        <TransformXmlHierarchy
            Source="%(Transforms.Identity)"
            Destination="src\%(Transforms.RecursiveDir)%(Transforms.Filename).nuspec"
            TaskDirectory="$(MSBuildExtensionsPath)\Microsoft\VisualStudio\v10.0\Web\" />
</Target>
```

And there you have it! For every transform file MSBuild finds, it will execute our task and generate our Nuspec files so they all contain the same shared metadata. I hope this makes someone's Nuget packaging life easier.

You can find the source and examples [in my gist](https://gist.github.com/1918022).