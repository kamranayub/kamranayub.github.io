---
layout: post
title: "Announcing Keep Track of My Games"
date: 2011-07-24 17:44:08 -0500
comments: true
categories:
permalink: /blog/posts/15/announcing-keep-track-of-my-games
---

I am working on a new side project, [Keep Track of My Games](http://keeptrackofmygames.com/about).

![Yes.](/blog/images/10.png)

### The Rundown

The goal is simple: **Manage your collection of games and track games that haven't been released.**

You can read the tour on my [introduction blog post](http://blog.keeptrackofmygames.com/post/8003937025/what-is-keep-track-of-my-games).

I've setup several social outlets:

* [@keeptrackgames](http://twitter.com/keeptrackgames)
* [Blog](http://blog.keeptrackofmygames.com/)
* [GetSatisfaction](http://getsatisfaction.com/keeptrack)

I'm also providing a public look into my development cycle via the [Roadmap](http://keeptrackofmygames.com/roadmap) page.

### The Technology

I am using MVC 3 with EF 4.1 and SQL Server 2008 deployed via Mercurial, BitBucket, and AppHarbor. I am using the JSON API from [GiantBomb](http://api.giantbomb.com) to fetch search results than caching them locally to the database. I'm not super enthused with the performance but hopefully I can make it better in the coming weeks.

I use [Twilio](http://twilio.com) for SMS and for now, regular old SMTP for emails but I may upgrade depending on load. I use a "scheduled" job that runs through the database and figures out if games are coming out soon or are released or if their release dates have changed.

### The Architecture

I have a pretty straightforward and simple architecture: everything goes through application services that do not have any web dependency.

![Layer Diagram](/blog/images/11.png)

My controllers are pretty stupid; they ask the services for information and process the results:

![Adding item to library](/blog/images/12.png)

The key is my `ActionConfirmation` class which I borrowed from my co-worker. I thought it was awesome for Service => Controller communication. The reason being I can pass error messages / validation results cleanly. 

For example, in this action, if an error occurred when saving tracking settings, I need to go and get the current tracking settings to add some view-specific logic (like, whether or not to show the email/phone checkboxes):

![Saving tracking settings](/blog/images/13.png)

The `currentTracking` is of type `ActionConfirmation<TrackingViewModel>`. So, if a service call was successful, I can retrieve the result via `currentTracking.Value`. I also use a non-generic `ActionConfirmation` class that doesn't require any return value.

Here's an example of the service code returning `ActionConfirmation`s:

![ActionConfirmation results](/blog/images/14.png)

I am not 100% OK with this because for complex business validation, the results are coming back one at a time. I will be refactoring later to avoid this.

### The "Open" Source Agenda

I have been playing around with making this source code public. Only a few things prevent me from doing so:

1. I need a way to hide secret tokens for APIs and my passwords for some services.
2. If people know the code, they'll see all the security holes and it's possible someone could exploit them and steal my user's data (not cool).

Really item #2 is the big decider. I am using OpenID and DotNetOpenAuth but that doesn't mean the site is totally secure. The worst information that could be obtained is someone's email and phone number, both of which I plan to encrypt for extra safety (once I figure out how that works with EF).

I am thinking instead of totally making it open source, to allow select contributors I trust into the codebase for reviews and patches.

If you would be interested in doing that, I have a tester system setup so [let me know](http://twitter.com/kamranayub) and we'll talk.