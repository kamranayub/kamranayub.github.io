---
layout: post
title: "5 Tips to Improve Your ASP.NET MVC Codebase"
date: 2014-01-29 13:16:11 -0600
comments: true
categories:
- MVC
- Tips
---

I have an urge to write a quick list of tips for improving an ASP.NET MVC application because 
I just got done reviewing some code for a support ticket at work. It's still fresh
in my mind and I wanted to get some of my thoughts down to share with others. If you
have been doing MVC for a while, I don't think much of this is news. It's more
for those of you that don't do MVC often or are new to MVC.

<!-- More -->

Imagine this: you've been tasked to figure out why a web application is using 2GB of memory on the
production web servers. You pull down the version that's currently in production and run it locally
to profile and debug.

After looking through the code, doing some profiling, maybe shaking your head a bit, you've figured
out what the issue is and now you need to give some feedback.

That's exactly what happened to me today and out of that experience, 5 tips you can follow to keep
your ASP.NET MVC codebase working as you'd expect.

### 1. Understand the queries in your problem domain

The root cause of the support ticket I received was a simple case of fetching too much
data from the database, causing obscene amounts of memory usage.

It's a common enough issue. You're building a simple blog, it has posts and it has media (images, videos, attachments).
You put a Media array onto your Post domain object. Your Media domain object has all the image
data stored in a byte array. Since you're using an ORM, there's a certain way you need to design your domain model
to play nice; we've all experienced this.

```c#
public class BlogPost {
	
	public ICollection<BlogMedia> Media { get; set; }

}

public class BlogMedia {
	
	public byte[] Data { get; set; }

	public string Name { get; set; }

}
```

There's nothing absolutely wrong with this design. You've modeled your domain accurately. The problem is, when you 
issue a query through your favorite ORM, it eagerly loads all the data associated with your blog post:

```c#
public IList<BlogPost> GetNewestPosts(int take) {
	return _db.BlogPosts.OrderByDescending(p => p.PostDate).Take(take).ToList();
}
```

A seemingly innocuous line (unless you've been bitten), a sneaky monster is lying in wait with big consequences if you haven't disabled
lazy loading or didn't tell your ORM to ignore that big `Data` property on blog media.

It's important to understand how your ORM queries and maps objects and make sure you only query what you need (for example using projection).

```c#
public IList<PostSummary> GetNewestPosts(int take) {
	return _db.BlogPosts.OrderByDescending(p => p.PostDate).Take(take).Select(p => new PostSummary() {
		Title = p.Title,
		Id = p.Id
	}).ToList();
}
```

This ensures we only grab the amount of data we really need for the task. If all you're doing is using the title and ID to build a link on the homepage, *just ask*.

It's OK to have more than 5 methods on a repository; be as granular as you need to be for your UI.

### 2. Don't call your repositories from your views

This one's a little sneaky. Consider this line in an MVC view:

```c#
@foreach(var post in Model.RelatedPosts) {
	...
}
```

It *seems* innocent enough. But if we take a look at what exactly that model property is hiding:

```c#
public class MyViewModel {
	
	public IList<BlogPost> RelatedPosts {
		get { return new BlogRepository().GetRelatedPosts(this.Tags); }
	}

}
```

Yikes! Your "view model" has business logic in it on top of calling a data access method directly. Now you've introduced data
access code somewhere it doesn't belong and hidden it inside a property. Move that into the controller so you can wrangle it in
and populate the view model conciously.

This is a good opportunity to point out that implementing proper unit tests would uncover issues like this; because you definitely can't intercept calls to something like that and then you'd realize injecting a repository into a view model is probably not something you want to be doing.

### 3. Use partials and child actions to your advantage

If you need to perform business logic in a view, that should be a sign you need to revisit your view model and logic.
I don't think it's advisable to do this in your MVC Razor view:

```c#
@{
	var blogController = new BlogController();
}

<ul>
@foreach(var tag in blogController.GetTagsForPost(p.Id)) {
	<li>@tag.Name</li>
}
</ul>
```

Putting business logic in the view is a no-no, but on top of that you're creating a *controller*! Move that into your action method and use
that view model you made for what it's intended for. You can also move that logic into a separate action method that only gets called inside views
so you can cache it separately if needed.

```c#
//In the controller:

[ChildActionOnly]
[OutputCache(Duration=2000)]
public ActionResult TagsForPost(int postId) {
	return View();
}

//In the view:

@{Html.RenderAction("TagsForPost", new { postId = p.Id });}
```

Notice the `ChildActionOnly` attribute. From [MSDN][2]:

> Any method that is marked with `ChildActionOnlyAttribute` can be called only with the `Action` or `RenderAction` HTML extension methods.

This means people can't see your child action by manipulating the URL (if you're using the default route).

Partial views and child actions are useful tools in the MVC arsenal; use them to your advantage!

### 4. Cache what matters

Given the code smells above, what do you think will happen if you only cached your view model?

```c#
public ActionResult Index() {
	var homepageViewModel = HttpContext.Current.Cache["homepageModel"] as HomepageViewModel;

	if (homepageViewModel == null) {
		homepageViewModel = new HomepageViewModel();
		homepageViewModel.RecentPosts = _blogRepository.GetNewestPosts(5);

		HttpContext.Current.Cache.Add("homepageModel", homepageViewModel, ...);

	}

	return View(homepageViewModel);
}
```

Nothing! There will not be any performance gain because you're accessing the data layer through a controller variable in the view and 
through a property in the view model... caching the view model won't help anything.

Instead, consider caching the *output* of the MVC action instead:

```c#
[OutputCache(Duration=2000)]
public ActionResult Index() {
	var homepageViewModel = new HomepageViewModel();

	homepageViewModel.RecentPosts = _blogRepository.GetNewestPosts(5);

	return View(homepageViewModel);
}
```

Notice the handy `OutputCache` attribute. MVC supports ASP.NET Output Caching; use it to your advantage when it applies. If you are
going to cache the model, your model needs to essentially be a POCO with automatic (and read-only) properties... not something that calls other
repository methods.

As an added benefit, I haven't ever done this but you can [implement different output caching providers][1] allowing you to cache on
AppFabric/NoSQL/anywhere if you ever needed it. MVC is super extensible.

### 5. Don't be afraid to leverage your ORM

If you're not going to take advantage of your ORM's feature set, you are missing out. In the codebase
I was reviewing, they were using NHibernate but they weren't *using* it. They were totally missing out on [its advanced projection 
capabilities][3] to solve some of these memory issues. Some of this stems from rigidity in using a "repository pattern" and some of it stems from lack of knowledge.

By taking advantage of EF or NHibernate's features, your repositories can do a lot more than just use basic generic methods. They can shape and
return the data *you actually want* in your controllers, greatly simplifying your controller logic. Do yourself a favor and read through the ORM's documentation to get a handle on what it can offer.

I think when people adopt the repository pattern, it's almost like they pull down a shade over the bright light shining in from their ORM window. When I started playing with RavenDB, I **got rid** of my repository layer (in fact, my *entire data project*) and went full-metal using Raven queries in my application service layer with a little bit of extension methods to reuse query logic. I found that *a lot* of my logic was really context-specific and benefited from simply taking advantage of Raven's extensive features to project, shape, and batch my queries.

#### That's just, like, your opinion man...

If you think you can abstract your ORM, I challenge you to think about it differently. The ORM *is* your abstraction and if you believe swapping out your existing ORM with another ORM will be a piece of cake because it's "abstracted", you'd be surprised. That's what I thought too until I learned the hard way that switching to Raven really changed my entire codebase in ways I didn't expect. Your ORM doesn't only affect data access, it affects the domain and it affects your business logic, it even will have an effect on your UI. By removing the repository abstraction, I actually *reduced the overall complexity* of my data access code.

### "Common sense is not so common"

Or so my dad loves to remind me at times. Sometimes it just takes a good code review to remind oneself that what you thought everyone knew isn't the case; you probably learned it through experience or frantic Googling and just assumed other people already knew it.

I hope this helped someone else out!

  [1]: http://msdn.microsoft.com/en-us/magazine/gg650661.aspx
  [2]: http://msdn.microsoft.com/en-us/library/system.web.mvc.childactiononlyattribute(v=vs.118).aspx
  [3]: http://nhforge.org/doc/nh/en/index.html#querycriteria-projection