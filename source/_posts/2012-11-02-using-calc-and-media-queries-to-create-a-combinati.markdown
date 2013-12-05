---
layout: post
title: "Using calc() and Media Queries to Create a Combination Variable + Fixed Sidebar"
date: 2012-11-02 20:27:12 -0500
comments: true
categories:
permalink: /blog/posts/58/using-calc-and-media-queries-to-create-a-combinati
---

The title of this post might be confusing, so let me describe our scenario:

I am working on a responsive layout, specifically one right now that has two columns, a content area and a right sidebar. This right sidebar needs to be flexible *up to a certain point* and then it needs to become fixed. In addition, both columns could be variable height, and on top of that, the two columns are sometimes split into two separate rows to achieve proper HTML markup order.

The reason the sidebar needs to have a minimum width is mostly due to the ads we're working with, since they have a fixed size. It might actually be possible to hack at the styles and make them pseudo-responsive, but I still want to share my tip with you, even if I don't end up using it.

I thought this was an interesting challenge because of two issues: the need for both flex *and* fixed for the same column.

### The Demo

Here is the Fiddle I came up with where you can view the result:

<iframe style="width: 100%; height: 300px" src="http://jsfiddle.net/kamranayub/mHvtM/4/embedded/" allowfullscreen="allowfullscreen" frameborder="0"></iframe>

### The Markup

The markup is straightforward and simple. I essentially need two rows of two columns, where the right columns line up and *appear* to be a single sidebar. When I get down to the smaller breakpoints, both columns stop floating and stack vertically (but that is neither here nor there).

### The Styles

Here's where it gets interesting. I start out the design with a 60% left column and 40% right column. However, I need the right column to stop being variable at 300px, and I need the left column to take up the remaining space.

The way I achieved this was by using a media query and the CSS3 `calc()` unit. I wanted to avoid Javascript at all costs.

```css
@media (max-width: 750px) { /* min-width / % = (300 / 0.4) = 750 */
    .alpha {
        width: -moz-calc(100% - 300px);
        width: -o-calc(100% - 300px);
        width: -webkit-calc(100% - 300px);
        width: calc(100% - 300px);
    }
    .beta {
        width: 300px;
        background: orange;
    }
}
```

Turns out, pretty simple!

### Gotchas

Of course this world isn't perfect and neither is CSS3 support. According to [Can I Use](http://caniuse.com/#feat=calc), `calc` isn't supported in: IE < 9, iOS < 6.0, Android, Opera, or BB < 10.0. That's lot of non-support.

So what to do? Well, in our case, it's not essential we maintain the fixed minimum-width. Especially if we can workaround the ads. If you **must** have the support, I don't know what to tell you. You could test for `calc` support using [Modernizr](https://github.com/Modernizr/Modernizr/blob/master/feature-detects/css-calc.js), then decide what to do in browsers that don't have that support (for example, throwing in the towel and just convert the layout to single column). You could also use a Javascript-based solution (which you might as well, if you're relying on Modernizr).

### Have a better solution?

I'd like to know. I'm not a front-end expert, so there may be a more clever way to achieve this and maximize browser support. If you know of any way to do this better, please let me know down below.