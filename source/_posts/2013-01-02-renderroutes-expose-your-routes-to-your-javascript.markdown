---
layout: post
title: "RenderRoutes: Expose your routes to your Javascript"
date: 2013-01-02 22:03:43 -0600
comments: true
categories:
permalink: /blog/posts/62/renderroutes-expose-your-routes-to-your-javascript
---

When you're working on an AJAX-enabled application, you'll soon discover it can be a pain to manage URLs. In fact, when you're working with ASP.NET MVC in general, working with routes is a pain. As soon as you want to change one, you have to go and find everywhere it's used.

I came up with a solution that works well for me and may for you.

```js
// Are you writing code like this?
$.ajax({ url: "/products/" + productId });

// Wouldn't you rather write this?
$.ajax({ url: Router.url("product", productId) });
```

In MVC, you have the notion of "named" routes, like this:

```c#
routes.Add(new { RouteName = "product", Url = "product/{productId}" });
```

(*Side Protip: [Don't use "{id}" in MVC routes](http://kamranicus.com/Blog/Posts/23/mvc-pro-tip-dont-use-the-id-url-parameter-in-your)*)

However, in the default MVC world, you would need to create these entries in your Global ASAX or wherever you set up your routes, and once you have enough, it will quickly get out of hand.

Awhile back, I started using [AttributeRouting](http://attributerouting.net) to help organize my routing and remove the shackles of the default MVC routing experience. I even [contributed the first Web API implementation](/Blog/Posts/43/attributerouting-now-supports-web-api) to the project.

One of the bonuses of using AR is that it uses a standard interface for any routes it creates, `IAttributeRoute`. I knew that when I started using Web API more that I needed an easy way to bring my server-side routes to my client-side in a way that was easy to use and that I didn't need to manually maintain.

Thus was born my `RenderRoutes` extension:

<script src="https://gist.github.com/4438411.js"></script>

In order to use this properly in your Javascript, you'll need to create a helper utility. The one that I use is very simplistic, doing basic search and replace on parameters, which is the only thing I really need with my routes.

The dependencies my solution has are:

* [AttributeRouting](http://attributerouting.net)
* [Underscore.js](http://underscorejs.org)

That's only because it was easier to leverage my existing dependencies. A more intrepid soul might be willing to de-couple my solution from these dependencies, and if you do, let me know so I can update my Gist. In fact, you should just fork my gist. Hmm, that sounds wrong.