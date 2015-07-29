---
layout: post
title: "Install Windows 10 Immediately Before Rollout"
date: 2015-07-28 00:20:13 -0600
comments: true
published: true
categories:
- Windows 10
---

This is only applicable for the next few hours until your machine gets Windows 10 rolled out. If you're impatient like me, a friend tipped me off that he was able to install Windows 10 prematurely by simply forcing Windows Update to download Windows 10 and then setting his system time forward a day (BIOS, I'm thinking).

It's kind of unbelievable but it's working so far. I'm at 57% complete downloading (you can view in Windows Update window).

1. Hit Windows+R to bring up Run command
2. Type in `wuauclt.exe /updatenow`
3. Wait for the download to finish (Control Panel -> Windows Update)
4. Set system time forward a day in BIOS
5. Watch Windows 10 install
6. Try it out!

I will update this post with any new information.
