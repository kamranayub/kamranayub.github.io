---
layout: post
title: "MVC Pro Tip: Don't use the \"id\" URL parameter in your routes"
date: 2011-11-02 14:25:08 -0500
comments: true
categories:
permalink: /blog/posts/23/mvc-pro-tip-dont-use-the-id-url-parameter-in-your
---

By default, MVC will use a route layout like this:

```c#
routes.MapRoute(
	"Default", // Route name
	"{controller}/{action}/{id}", // URL with parameters
	new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
);
```

Besides the fact that you shouldn't use the default route (use named routes!), there's another issue.

Do you have any domain models like this?

```c#
class Foo {
    public int Id { get; set; }
}
```

Now, let's say you wanted to add a new Edit method for this model:

```c#
public FooController {
    [HttpPost]
    public ActionResult Edit(int id, Foo foo) {

    }
}
```

There will be a problem here if you expect to be able to change this ID (rare but it happens). MVC will model bind the route's `id` parameter to your model's ID, possibly breaking your code. This is because MVC does not differentiate your two `id` and `Id` parameters, it sees them as one in the same (this has to do with the underlying `Request` collection).

### Fix it!

It's simple to fix. Instead of using `id` as a URL parameter throughout your application, use something else! I typically use `identifier`:

```c#
routes.MapRoute(
	"Default", // Route name
	"{controller}/{action}/{identifier}", // URL with parameters
	new { controller = "Home", action = "Index", identifier = UrlParameter.Optional } // Parameter defaults
);

// Controller
public FooController {
    [HttpPost]
    public ActionResult Edit(int identifier, Foo foo) {

    }
}
```

Voila. No more model binding issues!