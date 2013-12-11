---
layout: post
title: "Protip: Using Anti-Forgery Token with ASP.NET Web API on MVC 4 on AppHarbor"
date: 2013-04-24 05:14:06 -0500
comments: true
categories:
permalink: /blog/posts/70/protip-using-anti-forgery-token-with-aspnet-web-ap
disqus_identifier: 70
---

I just ran into and solved this problem so I thought I'd share (I'll also be talking about this at my upcoming talk).

### Anti-Forgery in MVC

In vanilla MVC, you'd do anti-forgery like this in your Razor view:

```html
@Html.AntiForgeryToken()
```

Then in a controller (POST):

```c#
[ValidateAntiForgeryToken]
public ActionResult DoSomething() { }
```

Cool. But what about Web API? It uses a totally different pipeline and likely you're interacting with it via JQuery or other AJAX framework.

### ValidateHttpAntiForgeryToken

Here are some references I used when trying to implement Anti-Forgery with Web API:

- [Problems implementing ValidatingAntiForgeryToken attribute for Web API with MVC 4 RC](http://stackoverflow.com/questions/11725988/problems-implementing-validatingantiforgerytoken-attribute-for-web-api-with-mvc)
- [Web API and ValidateAntiForgeryToken](http://stackoverflow.com/questions/11476883/web-api-and-validateantiforgerytoken)
- [Preventing Cross-Site Request Forgery (CSRF) Attacks](http://www.asp.net/web-api/overview/security/preventing-cross-site-request-forgery-(csrf)-attacks)
- MVC 4 SPA template

Here's the rub: the two SO posts above implement this quite differently than the MVC 4 SPA template and the last article referenced. Both approaches actually worked locally for me, but both failed once I deployed to AppHarbor.

The long and short of it is that I was using the HTTP header `__RequestVerificationToken`. This is a no-no and I'm sort of the dumb one here in that I should *know* not to use custom headers like that.

My friends, the *proper* way is to use the `X-*` convention, so the HTTP header in your AJAX requests become `X-XSRF-Token` instead.

Apparently, AppHarbor was stripping off the other header on AJAX POST. I was receiving an error about the given header could not be found.

The working solution is below:

<script src="https://gist.github.com/kamranayub/5449779.js"></script>

The `machineKey` thing is also key to remember on a cloud host/web farm environment.

I hope that helps somebody out there.

### Aside

In the last article referenced, the `AntiForgery.GetTokens()` didn't work for me on AH. I kept getting the error:

    System.Web.Mvc.HttpAntiForgeryException (0x80004005): The required anti-forgery cookie "__RequestVerificationToken" is not present.

I didn't get far enough to see what actual cookies were present because I switched to the other method outlined in the two SO posts. I just thought you should know that it just didn't work. I believe it's because `GetTokens` does not create a new cookie if the `cookieToken` has a value where as using `@Html.AntiForgeryToken()` does. The [source code](http://aspnetwebstack.codeplex.com/SourceControl/changeset/view/9a83b63f3f889e3f2c979274fe3e8f7610e06b98#src/System.Web.WebPages/Helpers/AntiXsrf/AntiForgeryWorker.cs) makes that clear in the comments. Why it worked locally I have no idea and at this point, don't care!