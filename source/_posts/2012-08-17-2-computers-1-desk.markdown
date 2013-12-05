---
layout: post
title: "2 Computers, 1 Desk"
date: 2012-08-17 20:54:51 -0500
comments: true
categories:
permalink: /blog/posts/54/2-computers-1-desk
---

At home, I have two main computers and one desk. I use one for my main everyday tasks and for gaming, and I use the other one as a server/development machine.

As you might guess, this is logistically difficult to manage on a small desk that is meant for only a single PC. You need to have two keyboards/mice and, if you like sound, two sets of speakers. Furthermore, if you're like me, you want to see both computers at once if you have two screens. Personally, I like dual-monitors for my main machine and am fine with only using a single monitor for development.

## Two PC's, One Mouse and Keyboard

The first logistical item to eliminate is how to control both computers with one keyboard and one mouse. There are a couple options:

1. Use a hardware KVM switch
2. Use a software KVM switch

Going with option 1 requires some more cables, but is handy in that you purposefully switch to the device you want to use, plus it is using actual hardware, rather than software emulation (which may have issues on Windows 8, see below).

I went with option 2, using the open source software [Synergy](http://synergy-foss.org/). I have a Windows 7 host (gaming PC) and my server as the client (Windows 8 RTM). If you make sure Synergy starts as a service and is elevated, you won't have issues with starting up and logging in on the client PC.

*Notice for Windows 8 users*: You may run into an issue with any KVM software out right now, which is being unable to see your mouse cursor. If you do not have any mouse plugged in to the computer, I believe Windows 8 hides the cursor because it assumes a touch interface by default. There are two workarounds: 1) Enable "[MouseKeys](http://windows.microsoft.com/is-IS/windows7/Use-Mouse-Keys-to-move-the-mouse-pointer)" but I had to enable this almost every time I restarted or, 2) Plug in a mouse. I opted for #2, with a wireless mouse/kb. This gives me some flexibility where I can use a physical mouse/kb if I really want.

## Two PC's One Set of Speakers

This is the more interesting problem. Is it possible to play both computers through one set of speakers? I am happy to report: yes! However, there is a caveat...

In order to pull this off, your "main" PC needs to have the ability to accept an input device (like a Digital/SPDIF/Optical-In). My gaming PC has a Sound Blaster X-Fi Elite Pro, so I have a big external console that accepts all kinds of inputs.

My server has a modern motherboard which can output either SPDIF or Optical. I had an extra SPDIF cable lying around, so I wired it from the server to my X-Fi's console.

There are a couple things to do in Windows to get this to work:

1. Right-click your sound icon and go to Playback Devices.

2. Make sure SPDIF is the default playback device:

    ![SPDIF](/blog/images/66.png)

3. On the main PC, you can "listen" to any recording device, which plays whatever's being sent to the main playback device. Listen to whatever input you're using to send the output of the client PC:

    ![Listen](/blog/images/68.png)

Voila! As soon as you hit "Apply", you'll start hearing Iron Maiden blasting through your single set of speakers as you code!

**Note:** This problem can also be solved if the monitor you use has speakers. If you use HDMI to connect your monitor to your client, audio and video will pass through (or you can use one of those speaker-jack cables). It'll sound like crap, though!

## Two PCs, Two Monitors

This is straightforward enough. As long as your main monitor accepts multiple inputs (DVI, VGA, HDMI, DisplayPort), you just need to find the right cable from your client to the monitor.

For example, my gaming PC has a DVI cable to my monitor. My server uses an HDMI cable. You're going to have a tougher time if your display doesn't accept multiple digital inputs (for example, only DVI and VGA...). It'll work, but analog signals look like crap on your nice 1080p display.

When I am developing, I set my secondary monitor (which is to the right of my main monitor) as the primary monitor for my gaming PC. That way, I can still use my other PC as I'm using my development machine.

## The End Result

... is awesome! My desk isn't cluttered with multiple sets of things, and the only manual thing I really have to do is switch my monitor input when I need to switch to using one PC more than the other.