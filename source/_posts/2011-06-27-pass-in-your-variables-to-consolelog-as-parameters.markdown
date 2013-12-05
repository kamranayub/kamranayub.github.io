---
layout: post
title: "Pass in your variables to console.log as parameters to make them navigable"
date: 2011-06-27 14:24:54 -0500
comments: true
categories:
permalink: /blog/posts/12/pass-in-your-variables-to-consolelog-as-parameters
---

I was helping a co-worker debug a JSON issue when I saw he was trying to use `console.log`. I asked how he was passing it in and he was doing exactly what you'd think should work:

```js
// result = some JSON object
console.log("Returned data: " + result);
```

He was seeing:

    Returned data: [Object object]

He was wondering how he could stringify the result. You could do that, I said, but I have a useful tip!

Try doing this instead:

```js
console.log("Returned data: ", result);
```

And voila, the console will not try to `.toString()` your object and it will be navigable:

![Firebug Console Example](/blog/images/9.png)

Clicking the object link will let you view its properties.

I always thought this was a useful and not very well known feature of `console.log` so hopefully that might assist you in debugging!