---
layout: post
title: "PowerShell Script to Generate an HTML5 Offline Manifest"
date: 2015-09-16 21:40:00 -0500
comments: true
published: true
categories:
- PowerShell
- Tips & Tricks
- HTML5
---

In my new role at work I've been learning PowerShell to administrate our systems (I'm a half developer, half sys admin monster). I've been a developer for a long time and been living in .NET for about as long--I still had not really *embraced* PowerShell as something I could use in my daily development routine. I've changed my tune. **PowerShell is awesome.** It's also not too hard to pick up once you learn how it works. I recommend you take a serious look at learning it. I recommend [following the PowerShell 3 Jumpstart course](https://www.microsoftvirtualacademy.com/en-us/training-courses/getting-started-with-powershell-3-0-jump-start-8276) and trial and error.

Anyway, for some of the games we write as part of [Excalibur.js](http://excaliburjs.com) for game jams we would like to run them offline. To do this, you need to create an [HTML5 Application Manifest file](http://www.html5rocks.com/en/tutorials/appcache/beginner/). However, this file is super finicky, as outlined in the linked article. In order to assist, I wrote a small PowerShell script that generates an `appcache` manifest file with each file's MD5 checksum. Therefore, the manifest file will only change when dependent assets change. I do some more work to disable it locally and only enable for release, but you can run this script as part of your build.

<script src="https://gist.github.com/kamranayub/cc2c4d371a83aec8279e.js"></script>

Modify the script to be specific to your project and it should output an appropriate manifest file. Feel free to change as you see fit.
