---
layout: post
title: "Behind the Major Update to Keep Track of My Games"
date: 2015-05-31 00:13:00 +0200
comments: true
published: true
categories:
- Keep Track of My Games
- Projects
- HTML5
- CSS3
- Front-End
- Javascript
---

![My profile](https://cloud.githubusercontent.com/assets/563819/7899619/b3154994-072a-11e5-8da1-b9a9703a8b57.png)

Today I just pushed a **major** update to [KTOMG](http://keeptrackofmygames.com). *Psst, you should join!*

You can [reference the blog post](http://blog.keeptrackofmygames.com/post/120264549011/updates-for-may-2015) for details on features but on the technical side I'm pretty proud because I got to implement a bunch of new tech that I've been wanting to learn properly.

<!-- More -->

## At a glance

The site is built using [Bootstrap](http://getbootstrap.com), [LESS](http://lesscss.org), and [Knockout.js](http://knockoutjs.com). It is totally responsive and yes, every feature is available on every view. I don't yet have offline mode or some more app-y things like local storage quite yet but it's on my to-do list. By far the hardest thing to make responsive was the list view but using Flexbox and toolbar configurations, it's manageable. It could always be simpler though, my work is never done.

## CSS3 Flexbox

![Lists](https://cloud.githubusercontent.com/assets/563819/7899652/94500830-072c-11e5-8656-59fb097ecf36.png)

The list view is using [CSS3 Flexbox](https://css-tricks.com/snippets/css/a-guide-to-flexbox/) with `display: table` fallback w/JavaScript. A Flexbox grid is perfect for a modern responsive app, which KTOMG is. It scales down to mobile fine (and if you don't have Flexbox or if the fallback fails, the worst you get is a stacked list). For different breakpoints I customize the flex basis of the items to create 1-3 column layouts. The homepage will soon use Flexbox as soon as I get to it (right now it's using `display: table` to maintain equal-height columns). All of the modern browsers support Flexbox and for those that don't, they just get a wrapping grid.

I also use Flexbox for any grid of games using table display as a fallback; this prevents me from needing to have fixed column sizes for different screen sizes so the games will just wrap as they need to.

**Aside:** The box art image grid for the list is simply just the first four games in the list (dynamically updated, since the entire page uses Knockout) positioned using `background-size` and [multiple backgrounds](https://developer.mozilla.org/en-US/docs/Web/Guide/CSS/Using_multiple_backgrounds). It uses a placeholder image repeated 4 times, in case the games are missing artwork and during loading.

Most of the CSS3/HTML5 stuff I have fallbacks by detecting features using [Modernizr](http://modernizr.com/).

## HTML5 Drag and Drop

The other cool thing Flexbox gives you is some hot drag and drop action by allowing dynamic insertion of flex items:

![Drag and drop](https://cloud.githubusercontent.com/assets/563819/7899631/1b7db50c-072b-11e5-9977-49c7b6c3b176.gif)

I wrote the drag and drop code in vanilla HTML5 Javascript, it wasn't too bad [using MDN as a guide](https://developer.mozilla.org/en-US/docs/Web/Guide/HTML/Drag_and_drop). The fallback if that isn't supported is simply toolbar icons (which really is faster anyway for moving things far).

I admit I haven't tried a browser that *doesn't* support Flexbox but *does* support drag/drop, so that case might still need some tweaking.

## Hopscotch Tutorials

![Tutorials](https://cloud.githubusercontent.com/assets/563819/7899658/bda3d91e-072c-11e5-80ea-932d2f309681.png)

Since lists are pretty powerful, people might need some help remembering all the things they can do. 

I use a modified version of [Hopscotch](http://linkedin.github.io/hopscotch) for an introduction tutorial. I struggled at first to figure out how I wanted to do tutorials. I ended up just using a string array on every user that I fill in when they finish a tutorial, allowing for an infinite number of tutorials and everyone not seeing them by default. I keep a master list of valid tutorials on the server. 

It's a simple solution that will let me add tutorials as I can make them. It took me a little finnagling, but I also got the tour working fine on small screens.

## Stats with Chart.js

![Stats](https://cloud.githubusercontent.com/assets/563819/7899666/222e5d5a-072d-11e5-9328-f52ae59c9e76.png)

I use [Chart.js](http://www.chartjs.org/) for profile and list stats. The API is easy to use and easy to extend, as well as being sufficiently sexy. The stats I show right now are just the tip of the iceberg. I look forward to generating some cool charts later on (especially the Radar ones for comparisons).

Also, I apparently really like Action/Adventure and RPGs. But I knew that already, tell me something I don't know! Like maybe my "burndown" for finishing my backlog...

## I'm excited

I worked on this update all month as my wife will attest. I still have [plenty](ktomg.uservoice.com/) to do but [our vacation](http://kamranicus.com/blog/2015/05/21/5-things-for-6-months-abroad/) is coming to an end. I don't think I'll be able to release such massive updates each month but this one definitely gets me closer to completing the vision I have. I'm pretty excited! The updates are just in time for the big gaming conferences, if only I could get public lists out by the end of June...
