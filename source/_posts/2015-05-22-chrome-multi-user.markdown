---
layout: post
title: "Use a Separate Chrome Profile to Manage Your Apps"
date: 2015-05-21 15:45:00 +0200
comments: true
published: true
categories:
- Tips
- Productivity
- Google
- Chrome
- Keep Track of My Games
---

If you're an app or site developer, you've probably got a bunch of 
tabs or bookmarks for your dashboards, social network accounts,
blog, and more. For [Keep Track of My Games](http://keeptrackofmygames.com),
I have UserVoice, Azure, RavenDB, social profiles, etc. that I
need to manage and track (haha).

<!-- More -->

It can be a chore to manage all of this easily but I have a tip.
A little while back Chrome [introduced a new feature](https://support.google.com/chrome/answer/2364824?hl=en) that lets
you switch between multiple profiles super easily. I use it
for me and my wife, since she has a Google account and goes
to knitting sites and does whatever she likes doing--I don't want that filth in *my* Chrome instance! :)

![Chrome user switcher is in the corner](https://cloud.githubusercontent.com/assets/563819/7749041/784a45e2-ffca-11e4-8a5f-ce5d1c445d4e.png)

The user switcher is in the top-right of Chrome, shown above. You can single-click to bring up a modal or right-click to quickly switch.
Chrome profiles are kept separate, with their own settings, extensions,
and bookmarks. This keeps tabs and work separated neatly and creates a 
so-called "separation of concerns" for your browsing.

Here's the rub: A full Chrome user is tied to a Google account. I don't want to create a separate Google account just for KTOMG. 
The Google+, YouTube, email, etc. is all linked to my primary Google Account.
The issue is, Chrome will not let more than one
Google account exist on a single PC. So how can we take advantage of the profile separation but still
keep my primary Google account? 

The answer is: we'll create a 
[Supervised User Account](https://support.google.com/chrome/answer/3463947). Did you notice I said "full user" before?
A SUA is just a lower privilege profile (meant for children) that you can
control via your primary Google account. BUT, if you **don't restrict anything**,
it'll just be like a "full" profile!

We'll follow the instructions above in the Google help article:

1. Under your Chrome profile, go to Settings
2. Scroll down to Users
3. Click "Add User" and enter your app's name and avatar for a picture
4. Click "Control and view websites this person visits from [your Google account]"

![Supervised user](https://cloud.githubusercontent.com/assets/563819/7749183/8dd90398-ffcb-11e4-8f8a-095f4a2e4beb.png)

Now you've made a "supervised" app account managed by your main Google one. Nice! You can set up all your tabs as you want, extensions with any
specific accounts (TweetDeck), and pinned dashboard tabs, all within a separate context from your primary Chrome profile.

For example, here's what [Keep Track of My Games](http://keeptrackofmygames.com)
looks like in its own Chrome profile:

![KTOMG profile](https://cloud.githubusercontent.com/assets/563819/7749251/1de36eb0-ffcc-11e4-8be9-fa78b45a69da.png)

And the full experience:

![KTOMG browser](https://cloud.githubusercontent.com/assets/563819/7749724/e2d07ed6-ffcf-11e4-93cd-f307718c2c82.png)

When you're the only one managing a bunch of apps, this 
is a real productivity booster. You could even go as far as
creating separate profiles for each environment, if you like. Hell, this doesn't have to be for apps, it could be for whatever browsing contexts you want to separate.

Just make sure you have enough RAM because Chrome will 
suck it all up.

![Task manager](https://cloud.githubusercontent.com/assets/563819/7749876/db3a810c-ffd0-11e4-8a35-5d96e41b874f.png)

Dumb puny laptop.

Hope this helps! Happy managing.
