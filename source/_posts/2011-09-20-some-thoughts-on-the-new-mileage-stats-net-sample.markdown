---
layout: post
title: "Some thoughts on the new Mileage Stats .NET sample application"
date: 2011-09-20 18:35:42 -0500
comments: true
categories:
permalink: /blog/posts/20/some-thoughts-on-the-new-mileage-stats-net-sample
---

I'm [still] reading through the docs on the new Mileage Stats application that was just released by Microsoft. It looks pretty cool! A little more real-world than typical sample applications.

### But I have some questions...

No application design is perfect but as I read some of this documentation, I am wondering about a couple things. **I haven't finished reading all of the docs yet,** so these are just some random thoughts:

#### BackboneJS

It seems like for a sample MVC application that needs to support progressive enhancement and work in JS/non-JS environments, [BackboneJS](http://backbonejs.com/) would be a suitable alternative to the jQuery UI widget + jQuery BBQ implementation they are using.

**Update**:

Hah, further down the same Readme page I was looking at, they mentioned BackboneJS or Knockout as alternative considerations:

> A design the Project Silk team is interested in investigating in the future involves the use of jQuery UI Widgets that can be data bound within an MVVM implementation such as Knockout.js.

I wonder why they didn't want to do that for the sample itself? From the reading, it sounds like it was because they wanted to minimize external dependencies. Perhaps it could also be seen as an official endorsement of whatever library they could have chosen (even though they explicitly state for jqPlot that it shouldn't be taken as an "official" recommendation).

#### AmplifyJS

Client side data manager is a big emphasis in this application. It says in the Readme that:

> "Data is cached in a JavaScript object, rather than using HTML5 local storage or similar APIs, in order to meet the cross-browser requirements of the application."

Hmm, well, won't [AmplifyJs](http://amplifyjs.com) cover data management needs then? It supports all browsers and [unifies the storage mechanism APIs](http://amplifyjs.com/api/store/). It [does some pub/sub stuff](http://amplifyjs.com/api/pubsub/) for you as well. In old browsers, a non-standard way will be used to store data but on modern browsers, localStorage and sessionStorage will be used.

I wonder if the team took a look--and if they did, what they thought of it. From the surface, it looks like it would be better than rolling your own storage and pub/sub routines.

### That's all for now

Like I said, I'm still reading/learning it. I'll update this as I read more.