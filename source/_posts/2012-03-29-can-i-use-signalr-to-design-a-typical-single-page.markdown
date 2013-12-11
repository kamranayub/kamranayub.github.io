---
layout: post
title: "Can I Use SignalR to Design a \"Typical\" Single Page Application?"
date: 2012-03-29 16:25:00 -0500
comments: true
categories:
permalink: /blog/posts/38/can-i-use-signalr-to-design-a-typical-single-page
disqus_identifier: 38
---

This post is a stream of conciousness kind of post. I know I may be totally off base on some things but I need somewhere to write this so I can review and think about it, maybe pose it to people and see where my thinking is flawed or if there's an obvious thing I'm not seeing.

## The Context

You're designing some sort of client-based application that communicates via AJAX (it can be Backbone, it can be Knockout, whatever). You're using ASP.NET MVC as the back end, maybe even the new fancy Web API.

## The Problem

Let's say your API is supposed to be only used by your application; you have no intention of making it available for all to use. The problem is that no matter what measures you take, it's all HTTP and it's all public. For many of us, that isn't what we really want. If I make a URL `/api/users/{id}`, I really don't want other people requesting that URL.

So far in my Googling and experience, there are a few things you can do to **reduce** malicious abuse of your API:

1. Issue session-based or cookie-based tokens with timeouts
2. Include Anti-CSRF tokens in your AJAX headers
3. Make sure people are authenticated before issuing authorizable requests
4. Do checks and throttle requests (no more than x requests from this user per second)

I think these are problems we generally face even in a "non-rich" application (simple server-side design). I could easily issue 100 Fiddler requests per second against a page on your site and maybe bring it down. Including an AntiXssToken might help with that. Maybe we don't care about that. However, it seems the problem is exacerbated, or at least more apparent, when we're talking about AJAX requests because we're exposing our data in an easy-to-consume format (JSON/XML/etc). If not properly designed and secured, Joe Developer can write a client that consumes your API. For public APIs, this is great! For APIs you just want to use for your application... not so great.

One of the things I'm not sure about is how site agnostic these defensive measures are and why it's so hard to find information on how to implement them. Can ASP.NET Web API bake these into the framework? Why or why not? Is this not so common as to warrant more real-world articles/tutorials on how to do it? Am I not searching correctly? Do most people not care?

> All I want is a not-so-public API my browser-based app can consume

## What about SignalR?

This is just me thinking out loud. Could a library like SignalR help with this? By using a single connection, no longer will I have to create easily consumable URLs like `/api/x`. Instead, I could create hubs with my API methods I want, and call them in my client code. Authorizing the requests would work as they usually have in the past.

On the surface it sounds like a better idea, but am I missing something? Does it help at all? I could see it providing other benefits, like being able to easily implement multi-user interaction if I wanted in the future. 

Would the problem just shift? Now as Joe Developer could I simply reference the generated SignalR scripts and write my own client app? Is it harder or easier to abuse SignalR vs. HTTP URLs?

I don't know yet but I'd like to research the topic more; learn more about REST security using tokens and session-based methods.

## Is this even an issue?

Let's say I implemented and took measures to reduce the likelihood of abuse in my exposed API. Is this even an issue anymore? From what I gather, general consensus is that you can't eliminate the ability for an undesirable user to abuse your API, you can only try to mitigate it as best you can. I still wish it was "that easy" to secure it. Am I missing something obvious here?