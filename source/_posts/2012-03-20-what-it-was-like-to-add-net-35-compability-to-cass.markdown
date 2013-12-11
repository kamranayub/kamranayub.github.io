---
layout: post
title: "What it was like to add .NET 3.5 compability to Cassette"
published: false
comments: true
categories:
permalink: /blog/posts/36/what-it-was-like-to-add-net-35-compability-to-cass
disqus_identifier: 36
---

As you may have heard, [Cassette](http://getcassette.net) now supports .NET 3.5. What you may not know is that [I had a pretty big hand](https://github.com/andrewdavey/cassette/pull/191) in making that happen (with the help of Andrew, of course!). I wanted to talk about what that experience was like since it was my first time making a *big* contribution to an open source project. I've sent bug fixes or small enhancements to projects I've used before, but never took on anything this big.

If you're thinking about contributing to a big project or wondering what it's like, I hope I can provide some insight into the process.

## Finding the motivation

First, let me confess something: I didn't help Cassette *totally* from the goodness of my heart. There was an underlying motive which was that we wanted to use Cassette at work with .NET 3.5, since we weren't planning to upgrade to .NET 4 for a few months. **Don't get me wrong**, I am really glad my contribution is going to help a lot of folks but I can tell you I wouldn't have taken this beast on without that kind of motivation, since I don't use .NET 3.5 in any of my personal projects (I live *on the edge*).

I think whatever your motivation is, it's good to have one when making a contribution. It can be selfish (like mine) or it could be "just because." Overall, I don't think there's been a time where I've literally thought, "I'm going to submit a patch because it's an open issue and no one's taken it." There's always been a reason, like I'm running into the bug myself and I know how to fix it. As a project maintainer myself, I don't expect anyone to work on my project "just because." I make projects public to share with the expectation that I'm the maintainer and while donation of time is awesome, it isn't expected.

## Doing your research

Whatever your underlying motivation, I believe it's important to do your homework before taking on a big contribution. For Cassette, that meant toying with the idea before committing to it. What I did was fork the repository and create a new branch. I explored the codebase, trying to get a feel for what it would take. I got the project built, I ran the tests, and *more importantly* I let Andrew know what I was thinking. [I created an issue](https://github.com/andrewdavey/cassette/issues/168) for it so I could have a decent record of activity and how I was doing.

The other thing that I felt was important was maintaining the same style and conventions the existing codebase had. I wasn't there to re-architect Cassette, I was playing in someone else's ball field. The cool thing about Cassette was that it jived really well with my existing style of coding; not only that, but Andrew did a *really* good job keeping it clean and easy to use *from a contributor perspective.* All dependencies were contained within the repository, there was no crazy environment set up. The only thing I had to do was download the XUnit extension for Resharper.

## Communicating with the maintainer

You've done your homework, you have the motivation, and you've decided you can take it on. Great! This is the point where you should communicate your intention to the maintainer(s). There were at least a dozen emails exchanged between Andrew and I about how adding backwards compatibility should work.

I think it's important to realize that you shouldn't just go out on your own and do your thing in a silo. Big decisions that might affect how the project is maintained should go through the maintainer or the team in charge. Furthermore, **the maintainer(s) is going to be the one who supports the code you're contributing.** When everything's said and done, no matter how awesome your contribution is, chances are you're not sticking around to support it. I made it a priority to make sure that whatever I was doing was communicated to Andrew so he was in the loop. I also tried to make my changes as transparent as possible, to not disrupt the codebase. In the end, [a lot had changed](https://github.com/andrewdavey/cassette/pull/191) but I believe it was a lot less than it *could* have been, had I not been religious on making sure the codebase was always up-to-date with whatever Andrew was doing.

## Be a good citizen

When I talk about communicating with the maintainer and doing your homework, I think this all can be lumped into the category of just "being a good citizen." I have some other pointers I followed that seemed to help me (and Andrew) during the contribution process:

* **Keep your changes in-sync**
  
    As a project maintainer myself, the last thing I want from a contributor is a huge set of changes that are against a codebase 5 versions ago. This is especially important in a project like Cassette where there are literally changes every week, and not small ones either, *big ones*. I think this was a difficult part for me but turned out to be very beneficial when I submitted my pull request. I could have simply *not* merged Andrew's changes into my branch and basically said, "You take care of it." I didn't think that was a good approach, and even downright rude. The maintainer shouldn't need to drop everything or "slow down" because you can't keep up with the codebase changing; this is why it's important to keep the communication channel open (for both sides). Every time there was a major change (like adding SASS support or rewriting the CoffeeScript engine), I sent updates with my concerns or questions (.NET 3.5 doesn't support the `dynamic` keyword, so what do we do?).

* **Document your changes or special steps you took**

    This goes along with the idea that the maintainer is going to be the one supporting your code once you're done contributing. I made sure that I kept a running document explaining any gotchas or special things I had to do to achieve backwards compatibility. This is not only for your benefit (to keep track of what you did) but also for the maintainer to look at and see what you did that might have thrown red flags. It was also useful because Cassette doesn't have any contributor documentation, so why not help create some as I go?

* **Make things easier to maintain, when you can**

    One of the issues I foresaw when working on Cassette was how to handle Nuget packaging. The way it was being done was fine but had the potential to be really difficult to maintain in the long run; there were at least 5 packages being generated for Cassette and they all had [mostly] the same metadata. Why should Andrew, or anyone, have to keep tons of Nuspec files in sync when there's a better way? My solution was [leveraging XML transformations to do Nuspec inheritance](/Blog/Posts/32/using-nuspec-inheritance-to-reduce-nuget-maintenan). I think it was really helpful and maybe other projects can benefit from it.

    If you see something you think will help make maintaining the project easier, I'd say let the maintainer know your thoughts (in case something was done a specific way on purpose) and if you get the green light, make your changes. If it's a big change, I'd suggest separating it into a different branch and creating a separate pull request. For the Nuspec inheritance, it made sense to keep it in my single pull request since it required build file changes.

* **Test everything**

    Testing is something a lot of people say you should do, but you never really *understand* the benefit until you come into an existing project. At work, we don't write many tests. It's encouraged... but too often we rely on the people who worked in the codebase to explain and help new people. It's different on a public project or a project you expect external people to help on. Testing is really, *really* important. That's one thing I loved about Cassette: if it didn't have any tests, I'd still be working through integration issues. I made it a point to make sure as many tests as possible worked perfectly in .NET 3.5 mode. In fact, I only had to disable 1-3 tests that dealt with SASS/CoffeeScript and was able to successfully add backwards compatibility to the test projects.

    One approach I could have taken was to disable the test projects when building in .NET 3.5 mode, but that's not being a good citizen; how could Andrew be confident changes he makes work in 3.5? Getting the tests working and converted took a bulk of the time but paid off in spades by letting me run all 800+ tests every time I made a change or merged in the latest codebase.

    Since I was only converting existing functionality, I didn't need to write any *new* tests, but I'd fully expect anyone contributing new functionality to do so, if that was the current approach that maintainer was taking.

## Have fun

Once I got going, it was hard to stop; it was a lot of fun learning how to do multi-targeting in .NET (I'll have a future post about tips, tricks, and gotchas). Sure, there were a lot of challenges to work through, sacrifices had to be made (sorry SASS/Nuget users!), but overall it turned out way better than I expected. It was a great experience and I learned a lot, I can see why some people have a passion for open source projects. I can tell you it's a really cool feeling [to see your name](http://getcassette.net/donate) on a project and see people using something you helped contribute to. It's also not bad for your coding "street cred."