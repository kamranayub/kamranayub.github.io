---
layout: post
title: "Knockout.js Binding Report for Performance Tuning"
date: 2015-05-25 12:30:00 +0200
comments: true
published: true
categories:
- Performance
- Javascript
- Knockout.js
- Chrome
- Keep Track of My Games
---

I use a lot of [Knockout.js](http://knockoutjs.com) on [Keep Track of My Games](http://keeptrackofmygames.com). I love Knockout but sometimes it makes it difficult to understand what's slowing my page down. 

<!-- More -->

For the game list, there's a lot of binding going on because I have to bind not only the list of games but also the toolbar.

![KTOMG list](http://41.media.tumblr.com/fefa8f95951ed13a2f4d22e758bb807b/tumblr_inline_nonu62Tr1J1qlpzxk_540.png)

Before I deploy the newest update (shown above), I'd prefer it if the list binding performance was better than it is now. Currently there's a visible block on the UI thread when returning results from the API, about 500-1000ms. **This is unacceptable.**

A quick Google search did not yield anything regarding a "real" performance tool for Knockout. What I would love is an extension that would overlay performance statistics over the UI of my app after binding is complete. This way I could visualize and easily pinpoint what bindings are causing performance issues.

I've made a small step towards that dream by creating a simple reporting script that outputs a "binding report" to the Chrome console:

![Binding report](https://cloud.githubusercontent.com/assets/563819/7795861/5a10389a-02d9-11e5-9462-056fa9e4da18.png)

The report displays the total duration of the binding process (which is all bindings that occur after a 500ms wait time) as well as the top binding according to total duration.

It also displays each binding summary in a table (`console.table`). You can drill-down by expanding the array entries underneath the table (sorted by duration).

![Drill-down](https://cloud.githubusercontent.com/assets/563819/7795969/8fb1c828-02da-11e5-8b81-fe88f466812e.png)

The script works by wrapping all the binding handlers (even custom ones) in a wrapper function that calculates the duration of the call to `init` or `update`. You just need to include it after your custom binding handlers and before the `applyBindings` call. 

The script requires [Underscore](http://underscorejs.org) and Google Chrome. Here's the gist:

<script src="https://gist.github.com/kamranayub/65399fa247a6c182bc65.js"></script>

As you can see in my own performance report, I have some work to do to fix my `if` bindings. It doesn't exactly pinpoint the problem binding, but it gets me a step closer to understanding what's going on.

![Uh oh](https://cloud.githubusercontent.com/assets/563819/7796086/f407dc76-02db-11e5-90e7-b89613408174.png)

Now if you'll execuse me, I've got some work to do.
