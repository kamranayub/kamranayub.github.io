---
layout: post
title: "Debounced After Render Pattern for Knockout.js"
date: 2013-06-05 19:18:28 -0500
comments: true
categories:
permalink: /blog/posts/74/debounced-after-render-pattern-for-knockoutjs
---

When dealing with Knockout-based lists, I think the built-in events/callbacks provided by Knockout leave a bit to be desired. For example, a common case I here people complain about is that the `afterRender` callback for the `foreach/template` bindings executes for each item in the array, not after all the elements have been rendered.

There are probably better ways to *resolve* this using Promises (pun intended), but here's one way to attack it using [Underscore.js](http://underscorejs.org/docs/underscore.html#section-65)'s `debounce` method that's easy to understand and use.

The `debounce` method:

> Returns a function, that, as long as it continues to be invoked, will not be triggered. The function will be called after it stops being called for N milliseconds. If immediate is passed, trigger the function on the leading edge, instead of the trailing.

This is useful for high-octane events like rendering elements, window resizing, or capture mouse events.

Here's a Fiddle that demonstrates a way to execute a callback after all the elements in a list have been rendered (and filters them to only `li` DOM elements):

<iframe width="100%" height="300" src="http://jsfiddle.net/kamranayub/yEhEt/3/embedded/" allowfullscreen="allowfullscreen" frameborder="0"></iframe>

This can be cleaned up multiple ways, I think a cleaner way to implement this would be through resolving a promise and then overriding the `foreach` and `template` binding to support it, for example, `afterAllRender` or something.

But this gets the job done and is easy to implement!