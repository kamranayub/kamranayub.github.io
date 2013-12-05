---
layout: post
title: "VS Extension for JS Template Support"
date: 2011-12-14 14:27:33 -0600
comments: true
categories:
permalink: /blog/posts/25/vs-extension-for-js-template-support
---

I am thinking of embarking on a journey that involves [Backbone.js](http://documentcloud.github.com/backbone/) in my MVC 3 project. This would involve heavy use of templating. After thinking about it a bit and doing some Googling, I realized that **there is no JS template extension for Visual Studio.**

I realize that you could just name your template files ".html" and be done with it. However, I feel like that isn't "cool" enough.

Based on some [preliminary research](http://stackoverflow.com/questions/3253205/how-do-i-visual-studio-syntax-highlighting-extension), it seems to me it would not be impossible to add support for common JS template syntax highlighting... and perhaps even Intellisense.

In fact, VS 11 [will support jQuery Template syntax highlighting](http://blogs.msdn.com/b/webdevtools/archive/2011/09/15/new-javascript-editing-features-for-web-development-in-visual-studio-11-developer-preview.aspx), but not only is jquery-tmpl [obsolete](http://weblogs.asp.net/stevewellens/archive/2011/12/01/goodby-jquery-templates-hello-jsrender.aspx), it is also not the [only kind of templating system](http://jsperf.com/dom-vs-innerhtml-based-templating/112).

Why am I making this post? I am not sure. Part of me wants to attempt to make such an extension, probably starting out with a simple template syntax like [Mustache](http://mustache.github.com/) (or [Handlebars](http://www.handlebarsjs.com/)). I would be happy with an extension that was pretty much an HTML highlighter that also highlighted tokens (e.g. "{{", "}}", etc.) just like VS does for Razor. Bonus points awarded if it [somehow] does Intellisense. In my dream world, it would act very much like MVC Razor does right now, where you declare the "model" for the template and that is its context for Intellisense. To me, that seems reasonable for a no-logic template system like Mustache but may prove unwieldy once you allow for normal JS expressions like in jquery-tmpl. That kind of syntax may also not make sense in the first place considering you could pass in whatever you wanted into a template's context.