---
layout: post
title: "Tips for Updating From WebAPI Beta to WebAPI RC"
date: 2012-06-01 05:59:01 -0500
comments: true
categories:
permalink: /blog/posts/48/tips-for-updating-from-webapi-beta-to-webapi-rc
---

I just installed VS 2012 RC (previous codename VS11) to make sure I am on the latest and greatest and I'd like to mention a few gotchas I ran into along the way while upgrading [Keep Track of My Games](http://keeptrackofmygames.com).

## Nuget Packages

One of the first things I did when opening the solution was to compile and run. WHAM. You will get a MissingMethodException:

```
[MissingMethodException: Method not found: 'Void System.Net.Http.Headers.HttpHeaders.AddWithoutValidation(System.String, System.Collections.Generic.IEnumerable`1<System.String>)'.]
   System.Web.Http.WebHost.HttpControllerHandler.AddHeaderToHttpRequestMessage(HttpRequestMessage httpRequestMessage, String headerName, String[] headerValues) +0
   System.Web.Http.WebHost.HttpControllerHandler.ConvertRequest(HttpContextBase httpContextBase) +248
   System.Web.Http.WebHost.HttpControllerHandler.BeginProcessRequest(HttpContextBase httpContextBase, AsyncCallback callback, Object state) +79
   System.Web.Http.WebHost.HttpControllerHandler.System.Web.IHttpAsyncHandler.BeginProcessRequest(HttpContext httpContext, AsyncCallback callback, Object state) +48
   System.Web.CallHandlerExecutionStep.System.Web.HttpApplication.IExecutionStep.Execute() +268
   System.Web.HttpApplication.ExecuteStep(IExecutionStep step, Boolean& completedSynchronously) +155
```

First issue: Web API had also been updated along with VS 2012's install. That means your previous beta Nuget packages are out of date. Unfortunately, if you go into *Manage Nuget Packages* you'll find no help in the Updates panel.

Turns out, the IDs have changed! Search for `Microsoft.AspNet.WebApi` and you'll find the RC packages again. In my solution, I am using:

```xml
  <package id="Microsoft.AspNet.Mvc" version="4.0.20505.0" />
  <package id="Microsoft.AspNet.Razor" version="2.0.20505.0" />
  <package id="Microsoft.AspNet.WebApi.Client" version="4.0.20505.0" />
  <package id="Microsoft.AspNet.WebApi.Core" version="4.0.20505.0" />
  <package id="Microsoft.AspNet.WebApi.WebHost" version="4.0.20505.0" />
  <package id="Microsoft.AspNet.WebPages" version="2.0.20505.0" />
  <package id="Microsoft.Net.Http" version="2.0.20505.0" />
  <package id="Microsoft.Web.Infrastructure" version="1.0.0.0" />
```

Once you uninstall your beta packages and install the new RC, you will likely run into a few more issues...

## Dependency Injection

I am using Ninject for dependency injection. You will need to adapt this solution to your needs if you *aren't* using Ninject. You will [need to use a new way](http://www.strathweb.com/2012/05/using-ninject-with-the-latest-asp-net-web-api-source/) of setting the DependencyResolver, see this GitHub source repository: [https://github.com/filipw/Ninject-resolver-for-ASP.NET-Web-API](https://github.com/filipw/Ninject-resolver-for-ASP.NET-Web-API)

Thanks Filip!

## No more generic HttpResponseMessage

If you had previously been using code like this:

    new HttpResponseMessage<T>(someValue)

You will get compilation errors. The new way to handle this is via the `Request` property in your controllers:

    Request.CreateResponse(HttpStatusCode.OK, result);

You will need to change any return types from `HttpResponseMessage<T>` to `HttpResponseMessage`.

## No need for custom JSON.NET formatter

If you had previously been using a variant of [Rick Strahl's JSON.NET formatter](http://www.west-wind.com/weblog/posts/2012/Mar/09/Using-an-alternate-JSON-Serializer-in-ASPNET-Web-API), you do not need it anymore as Web API defaults to JSON.NET now. However, if you, like me, want to customize the settings used, you can still do so:

**Global.asax or App_Start**

```c#
var formatter = GlobalConfiguration.Configuration.Formatters.OfType<JsonMediaTypeFormatter>().First();
formatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
formatter.SerializerSettings.Converters.Add(new StringEnumConverter());
```

Thanks to Matt in the comments for trimming the fat off what I had previously.

## No more GetUserPrincipal

I had previously been using the extension method `GetUserPrincipal` in my custom Authorize attribute. No more! You can now use `System.Threading.Thread.CurrentPrincipal.Identity` if you need the current principal in Web API. There's also a convenient `User` property in the `ApiController` base class.

## That's it!

That's about all I ran into. I am also happy to report that [AttributeRouting](https://github.com/mccalltd/AttributeRouting) still seems to work fine against the Web API RC and MVC 4 RC! I did some limited browsing around on my site with heavy AJAX calls and everything seemed to work.

Hope this saves you some time!