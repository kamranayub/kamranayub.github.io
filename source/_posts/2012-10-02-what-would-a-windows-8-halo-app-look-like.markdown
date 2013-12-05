---
layout: post
title: "What would a Windows 8 Halo app look like?"
date: 2012-10-02 22:49:51 -0500
comments: true
categories:
permalink: /blog/posts/56/what-would-a-windows-8-halo-app-look-like
---

**Update (10/24)**: There's a good indication there'll be an API. If you log into [DewXP](http://dewxp.com) with your Xbox Live account, it will display your spartan player model using a URL I haven't seen before:

> https://stats.svc.halowaypoint.com/players/subkamran/h4/spartans/fullbody

Nothing is at the root of that URL, but I assume the **h4** is for Halo 4-related stats. It also looked like it used OAuth to allow the DewXP "app" access to my profile. This is a good sign!

---


I'm a Halo fan. I enjoy playing with my friends, it's fun to be competitive and it's fun to compete against each other. One of the things me and my friends love is the ability to view our stats and history. It's all about bragging rights. Bungie used to have a *fan-fucking-tastic* site to view stats. Since the handover to 343, seeing stats has just not been the same nor as in-depth, though the core features have remained.

There are mixed reactions to this in the community, I would say leaning more on the negative side, even among my friends. However, after seeing all the footage of Halo 4, I have high hopes for the game as long as it continues to be competitive and to provide a source of stats and feedback.

So what does Halo have to do with Windows 8? Well, before Bungie handed the franchise off, they worked on releasing Halo Waypoint to several devices including [iOS](http://itunes.apple.com/us/app/halo-waypoint/id468457600?mt=8), [Android](https://play.google.com/store/apps/details?id=com.halo.companion&hl=en), and [Windows Phone](http://www.shacknews.com/article/71466/halo-waypoint-premium-mobile-app-add-on-features-real-time). These apps are great for people like us who like to see our stats on the go or when we're not at the computer (like in front of the Xbox). I wanted to build a similar app for Windows 8 that just showed stats of my friends or other players. Simple, right? Wrong.

Bungie had originally released a [poorly documented](http://www.bungie.net/fanclub/statsapi/Group/Resources/Article.aspx?cid=545064) (it's true) public API for their Halo stats but *at least* it was an API. Fan sites were able to run with this, creating sites like [HaloTracker](http://halotracker.com). The problem is, since 343i came into the picture, there has been no effort that we can see to provide an API (the old Bungie one is deprecated). This leaves site developers to resort to less-than-savory tactics to get at the stats, including screen scraping.

The only glimmering light on the horizon is that there are [rumblings](https://forums.halo.xbox.com/yaf_postst97564_Probably-confirmation-of-a-Halo-4-API.aspx) of a Halo 4 API, which would be a god-send for the community. But it's just that, a hope and no confirmation.

## There must be an API somewhere, though

There is. If you can get at the traffic on your mobile device that has Halo Waypoint, and you can get as far as seeing where that traffic goes, you'll discover there *is indeed a JSON web service*. Unfortunately, it requires authentication to the xbox.com domain, because the authentication cookie is sent on every request. In other words, it's not public and 343 probably doesn't want us using it.

I'm not sure if WinRT's [Web Authentication Broker](http://msdn.microsoft.com/en-us/library/windows/apps/hh750287.aspx) could help in this case, but as far as I can tell, Xbox Live does not have an authentication endpoint. There must be *something*, as the Windows 8 Games app can connect to Xbox Live but I'm assuming this isn't available to just anyone and that there's black magic involved. Even if you could somehow manage to authenticate and pass those headers into the WinJS XHR calls, there's still the tiny problem that we probably aren't *supposed* to be able to use this Waypoint API.

## My plea to 343

343: you need to have an API. If the rumors are true that Halo 4 will be getting an API, that's *excellent* news because I strongly feel that if you want to build a community around the game, you really should let us go wild with the data.

Even if that pipe dream does come to pass, there's still the issue of previous Halo games. We know you have the stats. We know you have the API. Just figure out a way to make it public! Release the Waypoint API so we can start somewhere; secure it with API keys or OAuth or whatever else you need to do.

Because if you don't? People can't have an app like this unless *you* release it, and let's be honest: wouldn't you rather spend resources creating a great API to view stats and then let us do the dirty work and build around it?

![MetroHalo](/blog/images/69.png)

![MetroHalo Lookup](/blog/images/70.png)

![MetroHalo Search](/blog/images/71.png)

This is a real app, living on my hard drive right now. It can fetch any player's data, integrates with Search, and is pretty sexy to see in motion. Unfortunately, none of you reading this post will ever get to see it, simply because it violates the Terms of Service. It screen-scrapes the Halo Waypoint site to get this data, which is not an acceptable way of getting data.

343, if you *are* planning to release a Windows 8 application, good on you! But my point still stands: this took me a couple days to put together and you didn't spend a dime on it, or assign Joe Bob 50% to innovation, or call your agency and have them put together mockups and charge you thousands of dollars. I did this in two evenings with a little bit of free time. There are so many things that are possible with a platform like Windows 8:

* Leaderboards among your friends
* Live tile updates of your games
* Beautiful high-res imagery of medals, ranks, and your spartan models
* Awesome win/lose charts and game history
* Map breakdowns
* Weapon overviews and tips
* Clan or group management
* Delivering video and other rich content to your users

And that's what I could think of on-the-spot, I'm sure after seeing these screenshots people could think of a lot more.

Release an API, 343. Your community will thank you for it.