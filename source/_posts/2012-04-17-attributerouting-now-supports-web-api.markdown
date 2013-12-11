---
layout: post
title: "AttributeRouting Now Supports Web API"
date: 2012-04-17 16:08:22 -0500
comments: true
categories:
permalink: /blog/posts/43/attributerouting-now-supports-web-api
disqus_identifier: 43
---

If you're *not* using [AttributeRouting](https://github.com/mccalltd/AttributeRouting) you're missing out. I'm serious, I am not sure how any ASP.NET MVC application can live without this library. Like [Cassette](http://getcassette.net), it's become a standard in my .NET tool belt.

### AttributeRouting at a Glance

If you're like me, you hate being stuck in this `{controller}/{action}` world that is the default MVC experience. I'd rather have URLs like this:

* /library/1 (GET, POST)
* /library/1/games/1 (GET, POST, PUT, DELETE)
* /search/?q=keyword (GET)
* /register (GET, POST)

In other words, more control over my URLs. Sure, you *could* be a caveman and use Global.asax for all of that, but I think you'll find it quickly growing out of hand unless you create methods to take care of different controllers.

Instead, wouldn't this be easier and more discoverable?

```c#
[RoutePrefix("library")]
public class LibraryController : Controller {
    
    // GET: /
    // GET: /library

    [GET("", RouteName = "default", IsAbsoluteUrl = true)]
    [GET("", RouteName = "library")]
    public ActionResult Index() {
    }

    // GET: /library/{userId}
    [GET("{userId}")]
    public ActionResult ByUser(int userId) {
    }

    // POST: /library
    [POST("")]
    public ActionResult Filter(Criteria criteria) {

    }
}
```

That is exactly what AttributeRouting lets you do (and more!).

### Web API

Now what if you want to do the same thing with ASP.NET Web API (or self-hosted Web API)? The short answer is **now you can**, though a week ago you couldn't. Having embarked on using Web API for my latest project, I decided that since I couldn't live without AR in MVC, I also couldn't live without it for Web API. So I forked the project, [did my thing](https://github.com/mccalltd/AttributeRouting/pull/57), and now it's in v2.0 which is [out now on Nuget](http://nuget.org/packages?q=attributerouting).

I tried hard to keep the exact same convention and syntax:

```c#
[RoutePrefix("library")]
public class LibraryController : ApiController {

  [GET("")]
  public LibraryViewModel Get() { }

  [HttpRoute("custom", HttpMethod.Get, HttpMethod.Post)]
  public HttpResponseMessage Custom() { }

}
```

Using Web API in a self-hosted environment? Not a problem, I made sure self-hosted works the same way, just grab the [appropriate Nuget package](http://nuget.org/packages/AttributeRouting.WebApi.Hosted).

It was a fun project and I learned a lot about generics. The end result was that I didn't even need to use generics and that sometimes generics are a hindrance rather than a help (such as having classes that require 7-8 generic arguments). I was also able to [use what I learned](/32/using-nuspec-inheritance-to-reduce-nuget-maintenan) from contributing to Cassette and bring it to AttributeRouting.

It seems Tim is a happy camper and it always gives me a warm, fuzzy feeling whenever someone appreciates help:

![Appreciation Goes A Long Way](/blog/images/39.png)

If you haven't contributed to a project you love, you should give it a chance, you always learn something!