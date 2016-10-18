---
layout: post
title: "Thoughts on Upgrading to a Dedicated Raven Standard Instance"
date: 2016-10-17 18:11:00 -0600
comments: true
published: true
categories:
- RavenDB
- Databases
- Keep Track of My Games
---

As you may know, I use the NoSQL RavenDB database to power my hobby project [Keep Track of My Games](http://ktomg.com). I really like the development workflow and the way it has simplified a lot of my data access logic. Right now I use the hosted [RavenHQ](http://ravenhq.com) solution for my two databases (staging and production). I've been doing that for some time now for a few reasons:

- The regular yearly Standard RavenDB subscription of $700 is a bit much for a hobby project
- I realized too late you could pay quarterly/one-time. One-time licenses only grant upgrades for 18 months and since major version upgrades seem to happen beyond that window, it is hard to justify even the one-time payment long term.
- The Basic/Basic 2X pricing would have been perfect except the 2GB database size limit was a dealbreaker. Additionally, I'd still need to add that monthy cost to the monthly cost of a VM.
- When I started with HQ the price was low enough to warrant the latency. Now though, the $10/1GB overage is getting to me especially with the two replicated instances BOTH incurring the $10/1GB extra each.

## The Cloud is Expensive and Slow [for Me]

This last week though, I decided to see if I could figure out how to lower my database hosting costs and the number one way to do that was to get a Standard license and manage my own VM. 

The hard truth is that PaaS offers awesome managed services and convenience but you pay (out the nose) for that convenience and often performance comes at a high cost so you're stuck with the smallest possible pricing tier otherwise your costs double and triple easily. When you want to save money, you need to go IaaS but even the major cloud platforms like Azure, AWS, and Google aren't super affordable for a hobby project *when you want performance*. Anything above 1 core on most cloud platforms is a minimum of about $100. You can't really expect to run Raven on a single core, 1GB machine--you need *some* metal.

## I Can Do It Myself

Enter [Vultr.com](http://vultr.com). Have a look at [their pricing](https://www.vultr.com/pricing/) for a second and then come back...

...

Back? **I KNOW, RIGHT?!**

I discovered Vultr from reading [Rick Strahl's woes on Azure VM performance](https://weblog.west-wind.com/posts/2015/Feb/01/Azure-VM-Blues-Fighting-a-losing-Performance-Battle) (see a pattern?) and he switched to Vultr VMs and was a happy camper. I have a 2 Core, 2GB VM spun up right now and I'm not even hosting my site/DB yet but it still feels way faster than an Azure VM. 

If I could find a way to get a Standard Raven license I could move all my stuff over to much cheaper infrastructure and get 2-3x the performance for less price. Yes, I may sacrifice some high availability and it will be more management overhead but managing web sites is what I do and I know how to monitor them plus even if I got a $36 2 Core 2GB web instance AND a $56 4 Core 4GB DB instance it would **still be cheaper than my current hosting cost that has way less performance.** Not to mention I could migrate over to .NET Core-based site and host on an even cheaper Linux VM. Raven 4 will also be able to run on Linux. That is some nice cost cutting!

## The Magic Non-Commercial License

So I had to pony up $1000 for a Standard license if I wanted to pay it forward and reduce my long-term hosting cost. It's hard enough to justify this to myself let alone to my wife! Yes, I've paid more than that overall so far but that's a sunk cost--it's tough to pay a lump sum like that all at once. I was willing to swallow that pill after a month or two of saving up.

But, I wondered if there was **some other way**. I kept thinking that I'm not making money from KTOMG, I wish that would count as an OSS license.

So I went back to the RavenDB pricing page. At the bottom it says "Open Source? Apply". This is great--I love it when awesome products like Resharper, RavenDB, and others are free for OSS projects. The issue is that KTOMG is *not* an OSS project. It's non-commercial, but it's not OSS. So obviously I initially ignored the offer years ago to apply for an OSS license.

I decided to Google more about RavenDB licensing--OSS projects can use it under AGPL but everyone else needs a commercial license, right? Well, yes... but:

> **NC:** 
> Blah I hate OSS, I don't understand all this stuff. I mean if i create a community site thats not designed to make money other than advertising to cover server costs. I don't want to make my site open source to use OSS, does that mean I have to pay for a commercial license?

> **Ayende:**
> Yes.
> **Or, for your scenario, you could contact us and ask for a freebie license in return for something like a "Powered by RavenDB" logo.**

This was [in the comments](https://ayende.com/blog/4508/comments-on-ravendb-licensing) on Ayende's blog post on RavenDB licensing.

## Whaaaaat?!

It turns out that if you ask nicely and have a truly non-commercial project, you may qualify for a "freebie" license in exchange for some free press or a watermark (I filled out a testimonial and shared my thoughts with the team). I wish I knew that 3 years ago! I would have done that from the beginning had I known it was a possibility--and I bet more people out there like me didn't know it was a possibility. How could they? The pricing page doesn't mention this at all. It could be that they *don't* want to advertise it as more people would ask--but I think that's a good thing. 

It's true that RavenHQ was able to extract money out of me for awhile and that benefited them--but it wasn't going to sustain itself. My options were to: a) fork out for a Standard license in which I pay once or b) explore other options like [Marten for PostgreSQL](http://jasperfx.github.io/marten/). There may be reasons people would move to Postgres and all that but I *am* comfortable with Raven and what it gives me so I'm not exactly looking to migrate all my data access code (again) to another provider. That isn't value-added work to me, I want to focus on releasing features!

I think that the more folks that use Raven, the more word will spread around. If people are *not* using Raven because they think they have to pay $700/yr out-of-pocket for a non-commercial project, they could be attracted back if they knew they were still able to harness the power of Raven with a free non-commercial license. Perhaps not everyone will want to manage their own VM but people like me working on fun projects that want a robust NoSQL database for .NET would jump at the chance. 

If anyone at Hibernating Rhino's is reading this, consider adding the non-commercial license as an "official" available license like OSS. It might be good for business!
