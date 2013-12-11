---
layout: post
title: "Sitecore and .NET Framework 4.5"
date: 2012-05-11 13:24:25 -0500
comments: true
categories:
permalink: /blog/posts/45/sitecore-and-net-framework-45
disqus_identifier: 45
---

**Update 1 (8/28/2012)**: As of Sitecore CMS 6.5.0 rev. 120706 (6.5.0 Update-5), this issue should have been fixed. [See here](http://sdn.sitecore.net/Products/Sitecore%20V5/Sitecore%20CMS%206/ReleaseNotes/ChangeLog.aspx#650update5) under "Miscellaneous" fixes. 

Just a quick note that as of this post, Sitecore (6.5.x) does not yet support .NET Framework 4.5. If you try to install Sitecore, you will be able to access the homepage and login page, but you won't be able to login. You'll see an error like this:

![Error](/blog/images/40.png)

> Object of type 'System.Int32' cannot be converted to type 'System.Web.Security.Cryptography.Purpose'

This is because in .NET 4.5 there are some new namespaces in `System.Web` that I guess return a different value than what Sitecore was expecting (or previous .NET 4 consumers).

I googled this error to no avail. Sitecore knows about it but I am posting it in case anyone else (which there will be more soon) run into this.

## Fix it

Unfortunately, this requires you to (in this order):

* Uninstall VS11 if you have it installed
* Uninstall .NET 4.5 (Multi-Targeting and the core one)
* Reinstall .NET 4 (yes, you need to)

I'm a bit sad since a few of my projects are MVC4 so in order to use SC locally I now can't work on those projects on this computer. If you have the ability to use a VM or equivalent, I'd go that route to avoid having to muck up your main work environment.