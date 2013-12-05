---
layout: post
title: "Design with testing in mind to make it easy when you do decide to test"
published: false
comments: true
categories:
permalink: /blog/posts/17/design-with-testing-in-mind-to-make-it-easy-when-y
---

While developing the first version of [Keep Track of My Games](http://keeptrackofmygames.com/) I made a conscious decision: I want to roll out the most basic functionality in a week.

I was able to do that, working at least a few hours every night and all weekend. The end result? I finished a [mostly] working site with 0 tests.

**I'm not proud of it.**

I have a task in my backlog for writing my specs and tests. I have a test project with all the dependencies set up, ready for me to create some specs... so why didn't I?

## Designing with testing in mind

The reason is because I am an impatient jerk who wanted to get my site done quickly and get it out the door. If it was mostly stable, that was fine by me. I could take the time to write proper tests after I tested the waters with my users. Good testing requires effort, effort that may have gone to waste if my site's core purpose didn't even work well.

Let me say that I probably wouldn't recommend this method but let's face it: we all do it. It bites you while you're debugging your site, you know it, you feel it, but you're still scarred from trying to [mock the HttpContext](http://www.volaresystems.com/Blog/post/2010/08/19/Dont-mock-HttpContext.aspx) the last time you did a MVC application.

So what can you do when you *want to test eventually* but don't want to right away?

### Avoid dependencies and coupling in the first place

One lesson I've learned is that starting out with [DI](http://en.wikipedia.org/wiki/Dependency_injection) is one of the best things you can do. That's why the first thing I did when I created my project was to bring down the Ninject.MVC3 Nuget package.

This allowed me to start using DI right away! I created my `BaseController` that set up my services (see [my architecture diagram](/Blog/Posts/15/announcing-keep-track-of-my-games) and I immediately created an `ITokenStore` that wrapped `HttpContextBase`. I **knew** `HttpContext` was the devil from the start due to past experience and I knew I'd love myself in the future if I started out not taking a direct dependency on it.

I call it an `ITokenStore` but you can call it whatever you want; I wanted to support OpenID and Forms authentication and that requires calls like `User.Identity.Name` or `User.Identity.IsAuthenticated` which I planned for. By implementing an interface and an implementation, I could safely inject it into my BaseController (and services!) or wherever I needed it. This meant for future tests, it'd be a piece of cake to mock/stub.

### Avoid `HttpContext` like the plague

And I mean avoiding calling `HttpContext.Current`. I just read the hilarious [Don't mock HttpContext](http://www.volaresystems.com/Blog/post/2010/08/19/Dont-mock-HttpContext.aspx) and it's spot on. I haven't checked but I bet you I still succumbed to the temptation and possibly have such code in a couple places. Luckily, I tried to catch myself and try to wrap it wherever I remember.

The problem is that with a mocking framework like Moq, you can't mock `HttpContext`. So, it's better to wrap it in a service/provider that you can mock later on (like my ITokenStore).

As an example, let's say you need to do stuff with `Request`s and `Responses` in your controllers.

Normally, you'd be fine happily typing this:

```
public ActionResult Do() {
    return Request.Url.ToString();
}
```

The problem is, when you test, that's going to throw an angry exception. HttpContext is totally `null` right there in your tests (unless you stub it, more on that below).

So what can you do to mitigate this? You could wrap the calls in a new interface/class (`IRequest`?) but your wrapper classes might start getting large. 

There are several things you can do.

1. Set up a [HttpContextStub and go from there](https://github.com/techtalk/SpecFlow-Examples/blob/master/ASP.NET-MVC/BookShop/BookShop.AcceptanceTests/Support/HttpContextStub.cs).
 - This has a useful method `SetupController(Controller controller)` that will assign the HttpContext of the controller context to your stub. All subsequent calls in your controllers will now go through your stub (which you can mock).
2. Pass in `HttpRequestBase` or `HttpResponseBase` to your actual Actions and [set up a custom model binder](http://msdn.microsoft.com/en-us/magazine/dd942838.aspx#id0420119).

I have never tried #2 in real life but I have used the SpecFlow sample `HttpContextStub` before and I like it quite a bit (although I have mixed feelings). The new mockable Base classes in MVC are super useful and you should definitely be coding against them rather than the equivalent `sealed` classes.

### Make sure your views stay dumb

