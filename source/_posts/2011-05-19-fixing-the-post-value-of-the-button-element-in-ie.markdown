---
layout: post
title: "Fixing the POST value of the button element in IE 7 and below"
date: 2011-05-19 18:07:48 -0500
comments: true
categories:
permalink: /blog/posts/9/fixing-the-post-value-of-the-button-element-in-ie
---

Please [see this write up](http://www.peterbe.com/plog/button-tag-in-IE) for the exact issue. Basically, IE 7 and below do not properly submit the "value" of the `<button>` tag, opting instead to submit the `innerText`. We just hit this issue while testing with our users who used IE7 on a project I'm on.

Here is the fix I'm using which I prefer to all the other alternatives I saw in the comments of that post. It meets two of my objectives:

 - It's totally transparent
 - It's only for IE 7 and 6

<script src="https://gist.github.com/981360.js?file=iebuttonfix.js"></script>