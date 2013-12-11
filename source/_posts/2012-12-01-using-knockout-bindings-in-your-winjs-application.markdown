---
layout: post
title: "Using Knockout bindings in your WinJS application"
date: 2012-12-01 17:08:38 -0600
comments: true
categories:
permalink: /blog/posts/59/using-knockout-bindings-in-your-winjs-application
disqus_identifier: 59
---

If you've done any serious Windows 8 development with WinJS, you've probably come across this wall:

> I need to do some custom logic and data binding on this element and I wish I could just use `win-data-bind` to do it.

In my case, all I wanted to do was toggle a CSS class based on my view model's property. In WinJS bindings, there is no way to call a custom binding converter with parameters nor can you access the DOM element from a converter, as far as I can tell.

Technically, you could call a function using `markSupportedForProcessing` and pass in these parameters, like this:

```html
<!-- Template -->
<div data-win-bind="className: myprop Converters.toggleClass"></div>
```

```js
// JS
WinJS.Namespace.define("Converters", {
    toggleClass: WinJS.Utilities.markSupportedForProcessing(function (source, sourceProperties, dest, destProperties) {
        if (source[sourceProperties[0]] === true) {
            WinJS.Utilities.toggleClass(dest, sourceProperties[0]);
        }
        
        return dest.className;
    })
});
```

But I am not sure that's a great idea (as the base.js source code has a lot of extra stuff around this method, which is why they expose an easier `WinJS.Binding.coverter`).

## Mix N' Match

It turns out, it *seems* to be relatively straight-forward to make KO and WinJS work together. Why would you want to? Well, WinJS has a lot of useful features like WinJS.Promises, utilities, controls, templates, and lots of other stuff. You lose all that if you use a 3rd party library (like KO or jQuery) exclusively.

With this approach, you can choose what works best. You could go all out and use KO to bind all your stuff and then use WinJS for whatever else you need. I chose to just leverage the custom binding functionality.

## Let's do it!

Once you have Knockout installed (via Nuget or what have you), be sure it's included in your page:

```html
default.html

<!-- WinJS references -->
<link href="//Microsoft.WinJS.1.0/css/ui-dark.css" rel="stylesheet" />
<script src="//Microsoft.WinJS.1.0/js/base.js"></script>
<script src="//Microsoft.WinJS.1.0/js/ui.js"></script>
    
<script src="/scripts/knockout-2.2.0.js"></script>
```

I put it in default.html so it's globally accessible.

Now, all we are going to do is **replace** the call to `WinJS.Binding.processAll` with a function that intercepts the call and calls `ko.applyBindings`, then continues to call the built in WinJS function.

Create a new file, I named mine **ko-winjs.js*:

<script src="https://gist.github.com/4183235.js?file=ko-winjs.js"></script>

Then, just include this before **default.js** in your HTML file:

```html
default.html

<!-- WinJS references -->
<link href="//Microsoft.WinJS.1.0/css/ui-dark.css" rel="stylesheet" />
<script src="//Microsoft.WinJS.1.0/js/base.js"></script>
<script src="//Microsoft.WinJS.1.0/js/ui.js"></script>
    
<script src="/scripts/knockout-2.2.0.js"></script>
<script src="/scripts/ko-winjs.js"></script>
```

And you're all set! Now you can do this in your templates:

<script src="https://gist.github.com/4183235.js?file=template.html"></script>

Pretty awesome!

## Disclaimer

I have no idea how inefficient this is, but my guess is that this adds extra processing time (plus, binding cache issues?). I have a small application so I'm not too worried about it and it's the best thing I could come up with on short notice.

That said, if someone smarter than me and more familiar with KO and WinJS in general could take a peek at this... I'd be super grateful, since I think this is a killer feature. If WinJS could just support KO-style binding syntax, I'd be totally cool with that too!