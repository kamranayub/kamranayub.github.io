---
layout: post
title: "BindableAppBar for Caliburn Micro"
date: 2013-03-05 19:07:28 -0600
comments: true
categories:
permalink: /blog/posts/64/bindableappbar-for-caliburn-micro
disqus_identifier: 64
---

**Update:** Matteo Pagani has a [great introduction](http://wp.qmatteoq.com/first-steps-with-caliburn-micro-the-application-bar/) on setting up the BindableAppBar. Thanks Matteo!

Anyone who has done Windows Phone development will tell you: the appbar sucks.

It sucks for several reasons:

* It's not a `DependencyObject` so you can't use binding
* It doesn't support commands
* It's hard to make it dynamic because it must be maniuplated in code
* It sucks

Given those reasons, it's no surprise people have come up with "bindable app bars" that are basically wrappers around the appbar that allow binding, manipulation, and such.

Unfortunately, none of those solutions have come to Caliburn. Caliburn *does* provide basic message binding support and IsEnabled guarding, but that's it.

So, given that I **really** wanted to start binding some appbars hardcore, I decided to spend a day and make it happen.

**Introducing the Caliburn.Micro.BindableAppBar**: you can enjoy the fruits of my labor on [Nuget](https://nuget.org/packages/Caliburn.Micro.BindableAppBar/) or at the [GitHub](https://github.com/kamranayub/CaliburnBindableAppBar) repository. Here's hoping Rob brings this into core so it'll just "be there" and integrated more into the overall Caliburn system.

The big benefit is the ability to easily use multiple appbars on a single view **and** in conducted Pivot/Panorama views. In my case, I do both, because one pivot page has two states (a selection state and a browsing state) and both requires two different appbars. The other pivot page doesn't need all the cruft of the first pivot's appbar, so only one button is present on it.