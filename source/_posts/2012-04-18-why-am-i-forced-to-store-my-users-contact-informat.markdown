---
layout: post
title: "Why am I forced to store my user's contact information?"
date: 2012-04-18 23:44:51 -0500
comments: true
categories:
permalink: /blog/posts/44/why-am-i-forced-to-store-my-users-contact-informat
---

OpenID and OpenAuth are cool. What's not cool about either approach is that I have no way to say, "Notify this user by whatever means available." Instead, I am forced to collect that information and figure out a way to store it that is safe from prying eyes.

What I'd like to see, and tell me if this exists, is a way to not only have people *authenticate* using their service of choice, but also to be *notified* via that service. In other words, OpenID already lets me ask for a user's email or phone but even that isn't really standard (see Google). That is not *exactly* what I want, though. I just want a way to notify the user. That might be email, it might be phone, it might be proprietary (Facebook notification). Just tell me what's available to me and I can decide based on that what to send (small message for phones, email for email).

As I understand the landscape, the only way to get around storing information myself is to buy into an infrastructure like Facebook to handle it for me... except that not everyone is enthused about being forced to use Facebook to use my app and it's not in my best interest to alienate my user base.