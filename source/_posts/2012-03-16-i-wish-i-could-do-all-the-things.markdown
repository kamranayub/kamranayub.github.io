---
layout: post
title: "I wish I could do all the things"
date: 2012-03-16 16:29:32 -0500
comments: true
categories:
permalink: /blog/posts/34/i-wish-i-could-do-all-the-things
disqus_identifier: 34
---

Do you ever have the problem where you have so many "cool" ideas you want to work on, but don't have enough time? When I started out in web development, I had a ton of time, being a high school student. I ran out of ideas by the time I was in college, but now after some time working "in the real world," I've lately had a torrent of ideas that I wish I could spend time on. For some, I've started on them, but I find it hard to make time for them.

## The Ideas

Here's a rundown of all the ideas in my head that I wish I could spend time on:

* A web-based multiplayer game (I have started working on this)
* A web app related to coding (I started it a little)
* A web site (just ideas written down)
* A project for work (I started a little)
* A crazy idea for an app (IDE?) for writing code (yah, right, I'll never be able to do this... unless it was web-based)
* A GitHub-style front-end for TFS because TFS is not social
* [Keep Track of My Games](http://keeptrackofmygames.com) (v1 finished, but wish I could work on v2)
* [Cassette](http://getcassette.net) for Node.js
  - I have no idea if this makes sense for Node, but I've found existing asset management packages not as easy to use as Cassette

You can be sure there's more but these are off the top of my head.

## What's blocking?

Is it totally true I don't have time to work on these? Not exactly. My problem is that having two really big hobbies (gaming + development) and having to work in one hobby (development) conflict with each other. After coming home from developing all day, I am sometimes motivated to work hard on an idea for a few hours (or all night) but that quickly turns to me being burnt out and I have a hard time recovering from it to continue being motivated to work on an idea. More often, I want to relax and play some games or spend time with friends.

The other issue is that my idea list and what I *want* for each of these ideas quickly turns into a humongous wish list and I start to doubt I can even begin to finish the list. I've done enough "agile" development at work to realize I can prioritize and start small and then iterate, which is what I did with KTOMG but then I have so many other ideas, it still feels like too much.

Money is also an issue. I want to implement this and that, but it's going to cost *me* money to do it. I have loans to pay off (thanks, education), a car to payoff, and rent to pay, not to mention daily or life expenses. I already spend quite a bit and KTOMG still costs me a fair amount to keep running (especially now that AppHarbor has $10 custom domains and $20 SSL). Partly that is dictated by technology, right? I could write everything in PHP and pay for cheap hosting, but I like being a .NET developer and lately I like being a Node/JS developer. Furthermore, complex ideas sometimes need complex hosting. KTOMG has a scheduled job but because I'm cheap, it's not a background worker... it's a scheduled URL request to a "secret" URL that initiates a 2 minute job. My database is hosted at a different host, which makes the site slower because it's expensive to host SQL Server on AppHarbor.

If an idea I had were to generate revenue, and several of my ideas I'm working on *could*, expenses would be less of an issue. When I released TorrentTyphoon, hosting was cheap and the site paid for itself (I think during my 4 years of high school I made around $10,000 in freelance and ad revenue). The main issue is that I still need to pay for the idea while I'm developing it or testing it, and what happens if I release it and it goes nowhere? I'm the one who has to pay for it.

## So what's the solution?

I've found that if I *force myself to start* working on an idea, I will be carried off and will work on it continuously for awhile. The hard part is actually sitting down and opening up the project. For KTOMG, I said to myself that I'd spend an hour a day for a week working on the site. What happened was that "hour" turned into several hours and it was really fun. The reward was making an app that actually accomplished what I wanted. Then the ideas started getting bigger (make it AJAX, support multiple release dates, let me organize, let me tag, etc. etc.) and it's almost as if I became scared to work on it... for fear of it taking so much time; *plus* those features were not the core of what I wanted for myself, and if I don't really want to use them, what's the motivation for me to implement them? There are around 80 people that use the site... and it does *exactly what I need it to*. It's hard for me to justify spending time adding more features, *even though I realize deep down it'll attract more users.*

What I have had luck with is creating and working on tiny ideas. DotJson was a tiny idea. My latest UnderscoreKO was a tiny idea. This site was a tiny idea. These ideas are 1 week projects or less and they have a finite end, which I seem to do better on. They are spawned because *I* want to use them and I make them public so others can too.

I've also learned, as I related above, that "chunked" development works well for me. Spend an hour a day, or aim to spend 5 hours a week, if I make the goal small, I tend to have more fun which makes me more motivated. If I start to see a working prototype or actual result, it snowballs until I finish something consumable. The other thing that motivates me pretty well is doing something new; developing a game is not something I've done before, and getting a chance to learn Node.js, Socket.io, and other new stuff is pretty fun.

The other thing that seems to help is working with someone else. Even if it's bouncing ideas, or including them in my repository, it becomes fun to work on an idea. So far I haven't had a chance to work with someone *coding-wise* on an idea, but I've had the chance to do feature or mechanic work with other people. Everyone else is just as busy, or more busy, than I am.

Finally, one of the things I could do is to throw my ideas and all my documentation out there and let someone else run with it. The problem with that, is that I'd be worried I wouldn't have a say in what happens or worse, would lose control of the idea. The last thing I want is for someone else to take my idea, not tell me, and then profit off of it. Perhaps that's a cynical point of view but it is something I worry about. For things that I know I probably don't have enough knowledge to do or are just "Wouldn't it be nice?" ideas, I don't mind sharing them (like some ideas I listed above).

Does anyone else have this problem? Have you had it and solved it? Do you have it and wish you could solve it? I'm interested in knowing.