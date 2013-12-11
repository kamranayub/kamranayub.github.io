---
layout: post
title: "Tips When Using Backbone.js in a .NET world"
date: 2011-12-15 20:37:51 -0600
comments: true
categories:
permalink: /blog/posts/26/tips-when-using-backbonejs-in-a-net-world
disqus_identifier: 26
---

I am learning [Backbone.js](http://documentcloud.github.com/backbone/) so I can re-write the main GUI for [KTOMG](http://keeptrackofmygames.com). I am still learning and I've run into a couple gotchas that I hope I can clear up for anyone else that dares to tread in these woods. Still, I've been able to rewrite the listing of a user's library in one night using Backbone and all my existing back-end code (the farthest up I edited was my controller). Very impressive for a Backbone n00b!

## Backbone IS your application

After watching the [PeepCode screencasts](http://peepcode.com/products/backbone-iii) (I, II, and III), I have realized something about Backbone.

> Backbone will pretty much replace your MVC views and controller logic.

This didn't hit me until the last episode and at first, this felt threatening. I am working in ASP.NET MVC and it feels wrong to not utilize it. After thinking about it some more, however, there were a few important points to consider:

1. Backbone doesn't *necessarily* have to be your whole app. In fact, right now I am only using Backbone for the main "library" interface and I am keeping the rest of the site in plain-old MVC 3.
2. You still use your controllers and could even use your Razor views as HTML templates.

Considering those points, I felt a lot better about using Backbone. Not only will it be a great experience, it'll also make the most important part of my application very responsive and robust, without needing to worry a ton about the plumbing.

Who knows? Maybe KTOMG will be running on Node.js by the end of the month.

## Tips When Using Backbone with ASP.NET MVC

There were a few gotchas I ran into when using Backbone in my typical .NET environment.

### Use AttributeRouting, like, now

[AttributeRouting](https://github.com/mccalltd/AttributeRouting) is one of my new favorite packages. It axes the mostly-confusing, mostly-convoluted Global.asax way of handling routes in favor of a more-discoverable (I feel) way of doing routes by decorating actual controller methods:

```c#
using AttributeRouting;

[RoutePrefix("games")]
public class GamesController : Controller {
  
  //
  // GET: /games/

  [GET("")
  public ActionResult Index() {}

  //
  // POST: /games/

  [POST("")
  public ActionResult New(Model model) {}

}
```

Hopefully you will see why this is useful. Backbone is opinionated in that it assumes you're using a RESTful interface for your API. In normal vanilla MVC, this could turn your Global.asax into a routing nightmare. However, with AttributeRouting, it's easy! Here's an example of a [Backbone-compatible REST controller](http://stackoverflow.com/a/6263133/109458):

```c#
using AttributeRouting;

[RoutePrefix("api/games")]
public class GamesApiController : Controller {
	
	//
	// GET: /api/games/
	
	[GET("")]
	public JsonResult Index() {}

	//
	// POST: /api/games/
	
	[POST("")]
	public JsonResult Create() {}
	
	//
	// GET: /api/games/{id}
	
	[GET("{id}")]
	public JsonResult Show(int id) {}
	
	//
	// PUT: /api/games/{id}
	
	[PUT("{id}")]
	public JsonResult Update(int id) {}
	
	//
	// DELETE: /api/games/{id}
	
	[DELETE("{id}")]
	public JsonResult Delete(int id) {}
	
}
```

Good fodder for a T4 template. If you have a larger site, consider using an `Api` MVC Area for cleaner separation.

### Be careful when serializing your objects

I ran into two gotchas with serializing my objects.

First, if you have an `Id` property/field, Backbone will not be happy. <strike>It needs to be lowercase id</strike>. You should utilize the [`idAttribute`](https://github.com/documentcloud/backbone/blob/master/backbone.js#L153https://github.com/documentcloud/backbone/blob/master/backbone.js#L153) option on your Backbone model to override the property name. It took me forever to figure out why nothing was happening when I deleted a model.

```js
class MyModel extends Backbone.Model
  idAttribute: "Id"
```

Second, if you are going to serialize EF entities when proxying or lazy-loading is enabled, you need to use a serialization framework like [JSON.NET](http://json.codeplex.com/) **or** you need to make flat DTO objects (or view models). I opted for the latter approach first because it was quick, but I am in the process of implementing JSON.NET to do serialization of my entities for me.

For EF serialization, you need to use a custom `ContractResolver`. I borrowed [this one](http://stackoverflow.com/questions/6991596/serializing-ef4-1-entities-using-json-net). Furthermore, I also needed to set the `ReferenceLoopHandling` to `Ignore` on a new `JsonSerializerSettings` object so it would avoid circular references.

Finally, this is optional but considering the first gotcha, I highly recommend you inherit the EF contract resolver above from `CamelCasePropertyNamesContractResolver` so your JSON is serialized with camelCase properties.

### Consider leveraging Razor/ASPX partial views for your templates

I haven't actually tried this, but I don't see why it wouldn't work. In a Backbone project you'll typically have plenty of templates. Right now I have 3 but I expect it to grow quite a bit in the future. I was planning on moving all these templates to `.cshtml` files and then using `@Html.Partial("_TemplateName")` to include them in my main layout. I may even create a small extension method to wrap that in a `<script>` tag. I will update this post if I do.

If you happen to be working on an OSS project, you should be using [Cassette](http://getcassette.net) because it can package up your HTML/Knockout templates and [include them all in your page for you](http://getcassette.net/documentation/html-templates) **automagically**. If I wasn't using AppHarbor and I was using a dedicated server, I would buy Cassette in a second. I wish they had per-site licenses.

### Use those organization skills

ASP.NET MVC comes with a friendly and intuitive file layout out-of-the-box:

```
Project
 |_ Controllers
 |_ Models
 |_ Scripts
 |_ Styles
 |_ Views
```

I've shortened mine, plus I've made my scripts directory a bit more Backbone friendly:

```
Project
 |_ Controllers
 |_ Models
 |_ Public
   |_ Images
   |_ Scripts
     |_ App
       |_ Collections.coffee
       |_ Backbone CoffeeScript files
     |_ Framework (or Vendor)
   |_ scss files...
 |_ Views
```

It's up to you how you like to organize, and perhaps you'd even like to separate it further down into modules.

### Learn CoffeeScript and SASS

This technically does not have anything to do with .NET or Backbone. However, your life will be *easier* if you learn CoffeeScript as that makes using Backbone that much more pleasant:

```
class GameView extends Backbone.View
  className: 'game'
  template: _.template($("#game-template").html())

  initialize: (options) -> 
    @model.bind 'change', @render, @

  render: -> 
    $(@el).html @template()
    @ # Never forget this line in render() or you'll regret it
```

(*Disclaimer:* This is not real and won't run; it's meant as an illustration of Backbone in CoffeeScript)

It's much nicer to write in Backbone without all those pesky parenthesis and manual `bindAll` or `extend` shenanigans. Again, I'd recommend [PeepCode's CoffeeScript screencast](http://peepcode.com/products/coffeescript), that's about all it takes to learn it properly.

![Mindscape Example](http://www.mindscapehq.com/upload/web-workbench/preview.png)

If you do decide to take the plunge (it's worth it!), get yourself [Mindscape Web Workbench](http://www.mindscapehq.com/products/web-workbench) and you'll be a much happier camper when working with Coffee, SASS, and LESS in Visual Studio.