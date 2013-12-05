---
layout: post
title: "Getting Cassette's compiled Knockout templates to work in KO 2.0"
date: 2012-03-21 19:57:31 -0500
comments: true
categories:
permalink: /blog/posts/37/getting-cassettes-compiled-knockout-templates-to-w
---

The Knockout [template binding](http://knockoutjs.com/documentation/template-binding.html#note_4_dynamically_choosing_which_template_is_used) documentation does a good job explaining how to use KO with jQuery Templates and even Underscore templates, however, it does not explain how (or if) you can use compiled templates outside of the HTML markup.

[Cassette](http://getcassette.net/documentation/html-templates/knockoutjs-jquery-tmpl) offers functionality to automatically compile and bundle your HTML templates, making it an excellent tool for more advanced Knockout development where you want to separate your templates. Unfortunately, in KO 2.0.0, it's broken!

The fix is to extend KO's built in jQuery Template engine with some logic to check and see if the named template is already precompiled. KO assumes that the name in a template binding refers to a DOM element with the given ID. Unfortunately, in our case, it refers to the precompiled template name.

Here is a plugin I wrote that fixes this issue. I've tested it in a limited capacity and I do wonder if `text` needs to be stubbed out more, but so far this hasn't caused any issues. This should work for adding compiled template support *in general* to Knockout (for jquery.tmpl), not just Cassette.

<script src="https://gist.github.com/2151977.js?file=knockout.compiledTemplateSource.js"></script>