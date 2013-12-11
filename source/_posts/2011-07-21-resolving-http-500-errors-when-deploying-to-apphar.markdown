---
layout: post
title: "Resolving HTTP 500 errors when deploying to AppHarbor"
date: 2011-07-21 22:31:40 -0500
comments: true
categories:
permalink: /blog/posts/14/resolving-http-500-errors-when-deploying-to-apphar
disqus_identifier: 14
---

This is a quick post. I was getting HTTP 500 errors when deploying my new site to AppHarbor. Long story short, if you've brought down the DotNetAuth Nuget package, it adds this to your `web.config`:

```xml
<section name="uri" type="System.Configuration.UriSection, System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
```

AppHarbor already has this defined and it is throwing a duplicate web.config section exception. Remove this line and you should be A-OK!