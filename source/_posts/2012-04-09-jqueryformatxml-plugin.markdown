---
layout: post
title: "jquery.formatXml Plugin"
date: 2012-04-09 19:42:53 -0500
comments: true
categories:
permalink: /blog/posts/41/jqueryformatxml-plugin
---

Awhile ago I wrote a [shim for jQuery to add pseudo-XML compatibility](https://github.com/kamranayub/jQuery-XML-Helper) that makes it easier to manipulate and read XML cross-browser on the client. I did it because I was working on a legacy application that decided it was a good idea to store the model in XML in a hidden input. No, I can't switch to Knockout, as much as I'd want to.

However, I really liked the trick of showing a JSON output of a Knockout view model on a page while debugging on another project since it made it easier to see what was happening behind the scenes.

I came back to do some work on this legacy app and missed that in-your-face feedback. So I wrote an extension to my own plugin that traverses an XML jquery object and creates a tree to output to the page:

<script src="https://gist.github.com/2346014.js?file=jquery.formatXml.js"></script>

Here's a jsFiddle of the output:

<iframe style="width: 100%; height: 300px" src="http://jsfiddle.net/kamranayub/XJGLe/embedded/" allowfullscreen="allowfullscreen" frameborder="0"></iframe>