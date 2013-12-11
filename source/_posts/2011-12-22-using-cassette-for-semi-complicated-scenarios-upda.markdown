---
layout: post
title: "Using Cassette for semi-complicated scenarios [Updated 1/9/12]"
date: 2011-12-22 21:24:19 -0600
comments: true
categories:
permalink: /blog/posts/27/using-cassette-for-semi-complicated-scenarios-upda
disqus_identifier: 27
---

I love [Cassette](http://getcassette.net). You should too, because as of today, the project has gone MIT licensed which means **you**, my dear reader, can use it right now.

It's alright, I'll wait while you download it via Nuget. Done? Great.

### Updates

* **1/9/12**: Added print media example
* **1/9/12**: [Issue #121](https://github.com/andrewdavey/cassette/issues/121) that I opened was fixed in the latest Cassette update, 1.0.1. I've updated my samples accordingly.

### More than a simple case

Cassette is [optimized for programmer happiness](http://hanselminutes.com/260/net-api-design-that-optimizes-for-programmer-joy-with-jonathan-carter). It's easy to use and there's a lot of small features that make it stand out from the crowd (SquishIt, Combres, etc.). If you want to get started using Cassette out of the box, you only need to do a couple things and those things are outlined well [in the documentation](http://getcassette.net/documentation/getting-started).

What I love about Cassette is the flexibility you have when you need to go beyond the simplest use case. For example, let's consider the file structure I am working with on a project right now:

```
Web Project
 |_ Public
   |_ images
   |_ print
     |_ print.css
   |_ scripts
     |_ i8n
       |_ en-US
       |_ zh-CN
     |_ special
       |_ modernizer-2.0.6.js
     |_ vendor
       |_ jquery-1.7.1.js
       |_ other framework files...
     |_ pages
       |_ page1.js
       |_ page2.js
     |_ app.js
   |_ themes
     |_ jquery.ui.core.css
     |_ other jQuery UI files
   |_ base.css
   |_ layout.css
   |_ otherfiles...
```

For the most part, this is how the project was organized and I needed to adapt Cassette to work here. I'll walk through a few things I needed to do first.

### File organization

Before implementing Cassette, [Modernizr](http://modernizr.com) was also in the *vendor* folder. In addition, the localized JS files in *i8n* were not in sub-folders according to their culture, they were just in *i8n*.

In the *vendor* folder, we have both minified and non-minified JS files (mostly from Nuget). We don't want Cassette to minify an already minified file. I'll show you how I avoided that later on.

I point this out because Cassette works better when things are organized so that they are "bundlable." For that reason, I moved the files around so the structure looks like above. Since Modernizr has to be included in the page's `<head>` (it needs to load before the page loads to set up Respond media queries and HTML5 elements) and all other scripts can be included before the `</body>` tag, I separated it into the *special* folder.

As for the *pages* scripts, these need to be only included on a per-page basis. If areas of your site need multiple assets, I would definitely move them into sub-folders (I would go a step farther perhaps and even include them by convention, the same way MVC plays with Views and Controllers but that's for another blog post!).

### Configuring the bundles

I'll walk you through my Cassette configuration file. If you pull down Cassette via Nuget, you will have a new `CassetteConfiguration` file and class. This is what Cassette picks up and uses to bundle your files. You can move it wherever you want, Cassette magically picks it up. It is important to realize that if you want full control over how Cassette bundles your assets, you'll need to do some configuration. Luckily, it's not bad!

#### Excluding directories, minified, and VSDoc files

Out-of-the-box, Cassette does not provide an easy mechanism to exclude directories when adding bundles per sub-directory. The reason I think this is important is because we want to set up a general "catch-all" method to deal with bundling, namely: treat each directory as a bundle. That way we can add folders/files and have them bundled automatically for us. This creates less work for the team.

I also wanted to ignore any pre-minified or -vsdoc files (Cassette by default ignores vsdoc files).

To solve this issue, I just derived a class from `FileSearch` and added my own requirements:

<script src="https://gist.github.com/1583784.js"></script>

I could have also implemented my own `IFileSearch`, perhaps one that used a fluent interface to exclude directories or file suffixes, but for my limited needs, this would do well and I really only needed to build upon the out-of-the-box `FileSearch` class. Maybe I will implement a fluent search interface as a fun project.

The other thing I needed to add into the *Regex* was the ability to exclude any files ending in ".min" or "-min" (just in case). Cassette recommends you simply delete minified files. That's a good tip, except they'll just be back when you do a Nuget update, so why not just ignore them by default as well (*ahem*)?

*Side note: It seems more efficient to me to allow Cassette to reference minified files but to bypass minification of them. Perhaps this will be implemented in the future?*

#### Configuring stylesheets

For our project, we need to include Google Web Fonts in the page, in addition to the CSS files in the *public* directory. We keep the jQuery UI themes separate in case we update via Nuget so we can simply overwrite them.

```c#
private void ConfigureStylesheets(BundleCollection bundles)
{
    // Sub-folder bundled CSS
    bundles.AddPerSubDirectory<StylesheetBundle>("Public",
	new ExcludeDirectorySearch("*.css", new[] { "scripts" }));

    // Print media bundle
    bundles.Get<StylesheetBundle>("~/public/print").Media = "print";


    // Google Web fonts
    bundles.AddUrlWithAlias<StylesheetBundle>(
        Scheme + "://fonts.googleapis.com/css?family=Droid+Sans|Montserrat", "Fonts");
}
```

The first line tells Cassette to add a bundle per sub-directory, so the following bundles will be made:

* public (our CSS)
* public\themes (jQuery UI)
* public\print (print media)

It also uses the class above and excludes the "scripts" directory from search, for good measure.

One of the caveats to the directory exclude is that it technically does *not* exclude directories; Cassette still creates empty bundles. Thus, if you tried to add a `~/public/print` bundle, you'd get an exception saying that it already exists. So, we *cannot* exclude the `print` directory. Instead, you can reference it directly after it's been added and manually set the `Media` to print. Problem solved! Ideally, Cassette should not be creating empty bundles.

The second line includes the direct Google stylesheet reference aliased as *Fonts* (in references, `~/Fonts`). In this project, our site is behind a load balancer so SSL is terminated at the balancer. This means you can't just use the built-in ASP.NET `Request.IsSecureConnection` because by the time the request even hits the web server, SSL is not there. <strike>Therefore, I utilize one of the utilities we have to check if SSL is enabled via server headers that the balancer sets.</strike> The scheme is not identifiable yet in our `Application_Start`, so we cannot use this method, but I will leave it here for posterity. We switched to always using SSL since our site is under SSL to the user. **Note:** You can simply include fonts normally using the Google Font Loader or by a direct `<link>` reference, however, why not use Cassette if you're already going that route?

```c#
// Also in CassetteConfiguration.cs
private string Scheme
{
    get { return Utilities.SharedUtils.IsSecureConnection() ? "https" : "http"; }
}
```

In our *Layout.cshtml* file, we now need to reference these two bundles and render them to the page.

```html
@{
    Bundles.Reference("public"); // Our CSS
}

<!DOCTYPE html>
<html>
<head>
    @Bundles.RenderStylesheets()
</head>
```

So how does Cassette understand in what order to put these references? First, Cassette goes by the order in which you call `@Bundles.Reference`. To make things easier though, I edited our main *layout.css* file to add Cassette-compatible references.

```css
/* For Cassette */
/* @reference ~/Fonts base.css skeleton.css themes */
```

When Cassette parses *layout.css*, it will detect these references. Since all files are included in our *public* bundle, everything goes great and the `<link>`'s are added in order:

```html
<link href="http://fonts.googleapis.com/css?family=Droid+Sans|Montserrat" type="text/css" rel="stylesheet"/>
<link href="/_cassette/asset/Public/themes/jquery.ui.button.css?165be2c1b488debc298a8dbaa77347b326a0d2a2" type="text/css" rel="stylesheet"/>
<link href="/_cassette/asset/Public/themes/jquery.ui.core.css?e5bc0be5bec6729cd338579a99e89c39c844c181" type="text/css" rel="stylesheet"/>
<link href="/_cassette/asset/Public/themes/jquery.ui.datepicker.css?6243e68daf6d6b3c03ce47d66abef2feba488d94" type="text/css" rel="stylesheet"/>
<link href="/_cassette/asset/Public/themes/jquery.ui.dialog.css?45faa27d7c66d72df00c39c40506da46bed885e3" type="text/css" rel="stylesheet"/>
<link href="/_cassette/asset/Public/themes/jquery.ui.resizable.css?a1bf026ec6d01daa0f7853215774c9e3aeb8afa9" type="text/css" rel="stylesheet"/>
<link href="/_cassette/asset/Public/themes/jquery.ui.theme.css?83294a0e12785f8dac8d3f0de4bf65f481b55f13" type="text/css" rel="stylesheet"/>
<link href="/_cassette/asset/Public/base.css?6401a75dfde8196c81d8bb39c3e5fb25dab89ad1" type="text/css" rel="stylesheet"/>
<link href="/_cassette/asset/Public/skeleton.css?d7414571707853cafb3028441861d31291d60789" type="text/css" rel="stylesheet"/>
<link href="/_cassette/asset/Public/layout.css?0ac01597826664dcc3a6c031a61c11ffa17fdd3d" type="text/css" rel="stylesheet"/>
```

#### Configuring Javascript

The Javascript assets were a bit harder to configure; we needed to do a couple things:

* Auto-bundle everything except a few special cases
* Have a "special" folder for `<head>` references
* Avoid editing vendor file references so a Nuget update won't blow the changes away

Here is my Javascript configuration:

```c#
private void ConfigureJavascript(BundleCollection bundles)
{
    // Localized JS - i8n/{culture}
    bundles.AddPerSubDirectory<ScriptBundle>("Public/scripts/i8n", true);

    // Per-page JS (eventually may want separate folders as well)
    bundles.AddPerIndividualFile<ScriptBundle>("Public/scripts/pages");

    // Bundle all scripts except special cases above
    bundles.AddPerSubDirectory<ScriptBundle>("Public/scripts",
	new ExcludeDirectorySearch("*.js", new[] { "i8n", "pages" }));
}
```

Our configuration code above does exactly what we need. We can double check by browsing to */_cassette* and seeing what Cassette decided to turn into bundles:

```
~/Public/scripts
    app.js

~/Public/scripts/pages

~/Public/scripts/special
    modernizr-2.0.6.js

~/Public/scripts/vendor
    jquery-1.7.1.js
    jquery-ui-1.8.16.js
    jQuery.tmpl.js
    jquery.validate.js
    jquery.validate.unobtrusive.js

~/Public/scripts/i8n/zh-CN
    jquery.ui.datepicker-zh-CN.js
    jquery.validate-zh-CN.js

~/Public/scripts/i8n/es-MX
    jquery.ui.datepicker-es-MX.js
    jquery.validate-es-MX.js

~/Public/scripts/pages/page1.js
    ~/Public/scripts/pages/page1.js

~/Public/scripts/pages/page2.js
    ~/Public/scripts/pages/page2.js
```

You can see the exclusions worked, since the *pages* folder is blank (i.e. not bundled up) and each page script is made later on a per-file basis.

The reason I like this is because now I can just add folders to *scripts* without worrying about adding a manual Cassette bundle; it will just do it automatically and ignore the two folders I need to make a special case for.

**Including the script assets**

Remember I said Modernizr should be included in `<head>` but all the rest of the scripts should be before `</body>`? We can do that easily with Cassette.

```
@* _Layout.cshtml *@
@{
    // CSS
    Bundles.Reference("public");
	
    // Scripts (vendor first!)
    Bundles.Reference("public/scripts/special", "head");
    Bundles.Reference("public/scripts");
}

<!DOCTYPE html>
<html>
<head>
    @Bundles.RenderStylesheets()
	
    @* Modernizr needs to be first *@
    @Bundles.RenderScripts("head")
</head>
<body>
	
    @* All other scripts *@    
    @Bundles.RenderScripts()

    @* Localized scripts *@
    @RazorHelpers.LocalizedScripts()
</body>
</html>
```

Notice the reference for Modernizr. I am using the overloaded `Reference` that accepts a `pageLocation` argument. This is the same as how you can include Razor sections in MVC3. The location is just a string you use when you call `@Bundles.RenderScripts({pageLocation})`. **Tip:** You can also specify `pageLocation` when you create a bundle in your `CassetteConfiguration` class. I did not, since I only needed one script in the `head` tag, but if you have multiple folders or bundles that need to be in separate locations, you don't have to specify a page location in your view files.

As with our stylesheets, we need a way to tell Cassette how to order the vendor scripts since we typically have plugins that depend on each other (such as jQuery). You *could* edit each vendor file and manually add file references to them, but that's no fun is it? In order to reduce headaches and maintenance, create a [Bundle Descriptor File](http://getcassette.net/documentation/configuration/bundle-descriptor-file) instead. That way, you will only need to keep that file up-to-date as you add more vendor plugins.

In the *vendors* folder, I simply made a *bundle.txt* file:

```
jquery-1.7.1.js
jquery-ui-1.8.16.js
jQuery.tmpl.js
jquery.validate.js
jquery.validate.unobtrusive.js
*
```

The wildcard at the end tells Cassette, "Yo, do whatever you want for the rest of the scripts you find." How useful! Now there's even *less* maintenance (didn't I tell you this baby was optimized for happiness?).

For localization, I just created a Razor helper to include the right culture files for me (this shows you that you can use the `Bundles` object anywhere in your Razor views and Cassette will include the references):

```c#
//
// ~/App_Code/RazorHelpers.cshtml
//
@helper LocalizedScripts()
    {
    string cultureName = System.Threading.Thread.CurrentThread.CurrentCulture.Name;

    if (cultureName != Constants.DefaultCulture)
    {
        Bundles.Reference(String.Format("/public/scripts/i8n/{0}", cultureName));
    }
}
```

And on individual views, I can now included page-specific JS:

```c#
@* Views\Controller1\View1.cshtml *@
@{
    Bundles.Reference("~/public/scripts/pages/page1.js");
}
```

Cassette is smart and will include the JS automatically in the *Layout.cshtml* file *and* in the proper order (in my case, just at the end. If you had references inside, Cassette would know).

#### To debug or not to debug, that is the question!

In our environment, we don't necessarily want to turn off Cassette as soon as the web.config's `debug` flag turns to *false*. Instead, we have different environments where it might make sense to leave Cassette on. When the site goes out to QA, then it makes sense to turn on Cassette's Release mode, but while developing locally and in staging, we may need to debug a bit more.

```c#
public void Configure(BundleCollection bundles, CassetteSettings settings)
{
	// Debug locally and in staging
	settings.IsDebuggingEnabled = 
	    Server.CurrentEnvironment == Server.ServerEnvironment.Developer ||
	    Server.CurrentEnvironment == Server.ServerEnvironment.Development;
	
	ConfigureStylesheets(bundles);
	ConfigureJavascript(bundles);
}
```

And there's my full `CassetteConfiguration` class!

### So what are you waiting for?

Cassette is awesome and I hope you learned a couple tips for doing things in a semi-complex environment. Let me know if you've found any more good tips & tricks, or if you notice I missed an optimization or hidden feature.

I would really like to do some performance testing with Cassette. It *does* add to your overhead on the site, but in my Firebug timings, I noticed much less time being spent on CSS files and more time being spent on JS files. If you are worried about performance on a massive scale, it will **always** be faster to minify and concatenate your files at compile or build time because then IIS is serving the files directly. Components like Cassette are meant to ease your burden by handling that for you, at the expense of some milliseconds. Remember that due to caching and aggressive expiration headers, the browser should *not* be requesting assets anyway after the first request.