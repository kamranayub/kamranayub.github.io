---
layout: post
title: "A few hours with WebAPI + MVC, the good and the bad"
date: 2012-04-03 13:42:13 -0500
comments: true
categories:
permalink: /blog/posts/39/a-few-hours-with-webapi-mvc-the-good-and-the-bad
disqus_identifier: 39
---

I'm working on the next version of [Keep Track of My Games](http://keeptrackofmygames.com) and it's an opportunity to upgrade to and play with MVC 4, especially the new [WebAPI](http://asp.net/web-api).

## Getting WebAPI

This was not super apparent from the tutorials on asp.net but to get the WebAPI, you can simply obtain it from Nuget. Most of the tutorials start with File > New Project, but I wanted to incorporate it into a newly upgraded MVC 4 project. *Fun fact:* MVC4 is also available on Nuget, score!

![Nuget](/blog/images/37.png)

Which one?! Since there's no description or information regarding the "Data" package (assuming it will make it easy to integrate with EF), I went with the **ASP.NET Web API (Beta)** package.

## WebAPI is Not MVC

I dislike things that cause friction. Unfortunately, there's some friction with using WebAPI for the first time. I'm not sure if all of it can be removed, but hopefully some of it can be mitigated. If you haven't used WebAPI before, it's important to know **WebAPI is not MVC**.

Here's what I struggled with:

1. Configuration is separate. You have to use the not-very-intuitively-named `GlobalConfiguration.Configuration` singleton to configure WebAPI. Seriously? Why not `HttpConfiguration` or even better, `ApiConfiguration`? To match typical MVC conventions (GlobalFilters.Filters, RouteTable.Routes), I'd propose `HttpApi.Api` or something along those lines.

1. Routing is separate. I use [AttributeRouting](https://github.com/mccalltd/AttributeRouting) and while I haven't explicitly tried it with WebAPI, I don't think it will work, but maybe it will. You register routes using the `MapHttpRoute` extension.

2. It doesn't plug into MVC's `DependencyResolver`. This was annoying. I use Ninject.MVC3 for my DI container, but that doesn't automatically make it available to WebAPI. Nay, [you have to jump through some hoops](http://haacked.com/archive/2012/03/11/itrsquos-the-little-things-about-asp-net-mvc-4.aspx) to get it to work. Once you do, it works just as you'd expect. **Update:** At a presentation today at Twin Cities Code Camp, Matt Milner presented on Web API and I saw he used a simpler method to plug Ninject into Web API:

        configuration.SetResolver(t => kernel.TryGet(t), t => kernel.GetAll(t));

3. Attributes are different. Do you have a custom `[Authorize]` attribute in MVC? You'll have to port it to WebAPI. This isn't a bad thing, it's just not apparent when you see this:

    
        [Authorize]
        Product GetProductById(int id) { }
    

    To follow the convention, I prefixed my custom attributes with `Http` to make it more apparent they're different.

4. JSON.NET *will be* the default JSON serializer, but it's not *yet*. This means you have to register a custom formatter. I used the one [on Rick Strahl's blog](http://www.west-wind.com/weblog/posts/2012/Mar/09/Using-an-alternate-JSON-Serializer-in-ASPNET-Web-API) (with some modifications) but you could try the several JsonNet Nuget packages for WebAPI ([WebAPIContrib.Formatters.JsonNet](http://nuget.org/packages/WebAPIContrib.Formatters.JsonNet/0.6.0) or [NetFx](http://nuget.org/packages/netfx-WebApi.JsonNetFormatter/1.0.0.11)).

And that's about it. Once I overcame those "frictions" everything started to go smoothly. I haven't played with it a ton, but I'm sure I'll write about it as I use it more.

## Removing the friction

While I understand WebAPI is separate (but equal?) from MVC to allow for self-hosting or MVC-independent use cases, let's be honest: who are going to be the majority consumers of the API? MVC peeps. If there isn't already, there needs to be a "bridge" that makes it easier to use for MVC consumers.

The bridge should:

1. <strike>Plug into routing (so AttributeRouting sees it?)</strike> [I've added Web API support](https://github.com/mccalltd/AttributeRouting/pull/57) to AttributeRouting directly. It will be in v2.
2. Plug into whatever dependency resolver you have set
3. Do something else I haven't run into yet

A Nuget package that added a WebActivator start up wouldn't be too hard. Maybe it doesn't even need to "plug" into the pipeline, it can simply make it easier to see and configure WebAPI. The auto-DI integration alone would be a big win.