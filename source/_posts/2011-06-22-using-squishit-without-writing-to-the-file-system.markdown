---
layout: post
title: "Using SquishIt Without Writing to the File System"
date: 2011-06-22 14:16:11 -0500
comments: true
categories:
permalink: /blog/posts/11/using-squishit-without-writing-to-the-file-system
---

I'm a big fan of [SquishIt](https://github.com/jetheredge/SquishIt/), which is a library that compresses and minifies CSS and JS. It automatically keeps thing uncompressed locally and then on your web server, versions and compresses your files. The versioning is based on a checksum of your files so it automatically handles any changes you make, it's gold.

The thing that I don't like is that by default, it writes to your file system which is usually a no-no on shared hosts or even enterprise applications.

I asked if you could use SquishIt programmatically and you can! I [wrote up a wiki article](https://github.com/jetheredge/SquishIt/wiki/Using-SquishIt-programmatically-without-the-file-system) on their project page to show how it's done.

If you haven't used SquishIt, I'd highly recommend it!