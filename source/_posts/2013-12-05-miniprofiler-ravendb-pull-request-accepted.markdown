---
layout: post
title: "MiniProfiler RavenDB Pull Request Accepted"
date: 2013-12-05 08:32:08 -0600
comments: true
categories:
- GitHub
- OSS
---

I just got a notification that my [pull request](https://github.com/MiniProfiler/dotnet/pull/15) for 
adding RavenDB support to MiniProfiler went through! It's always really cool to see your contributions
get pulled in. The MiniProfiler guys were really easy-going, [@yellis](https://github.com/yellis) was even kind enough to take on
the testing. That kind of acceptance for pull requests is great to see because it makes me feel good
about contributing and is a bit less intimidating.

<!-- More -->

The "official" MiniProfiler RavenDB plugin does several things:

- Integrates with all existing MiniProfiler settings
- Displays the query duration just like you'd expect in a separate raven column
	- This let's you see the raven percentage individually just like SQL
	- It matches the experience of using EF with MiniProfiler exactly
- Allows you to dig into the exact request details ala EF integration
- (not included) It's possible to also include the *results* of the request but in my pull request I don't because it was way too much
  overhead to include the full response in the web interface (displaying 50 complex objects in JSON slowed down the UI too much)
  - You can just copy the query to see the results in Raven Studio anyway
  - Possibly you could just display the query URL and open the request in a new tab

I have only just begun my exploration of RavenDB but one of the downsides from moving from EF was 
the excellent MiniProfiler integration. I was really surprised to find adding the same level 
of support to Raven was pretty straightforward in the latest iteration of MiniProfiler.

There are two packages I found that added profiling to Raven, the ["official" Raven profiler](https://github.com/ravendb/ravendb/tree/master/Raven.Client.MvcIntegration) and another
[MiniProfiler plugin](http://blog.csainty.com/MvcMiniProfiler.RavenDb/), but both of those are lacking. The official one didn't seem to handle AJAX requests
and the other only seemed to add profiling steps; it didn't provide any details about the request.
I wanted **full** integration with MiniProfiler on the same level as EF and I didn't want to enable *two* profilers at once. I was
already using MiniProfiler to profile my MVC app, I didn't want to enable Raven Profiler and MiniProfiler
at the same time.

There is [a RavenDb.Contrib project](https://github.com/ravendb/ravendb.contrib/tree/master/src/Raven.Client.Contrib.Profiling) that added MiniProfiler integration on the level I wanted, but unfortunately it was not integrated
tightly into the MiniProfiler codebase (it was using reflection and other hacks) and it also was not built against the latest version of the codebase
that simplified the steps to create a new custom timing. Still, credit is due, because some of the formatting logic and general direction of my pull
request was taken from that contrib project.

I do not know what the release schedule is like for the next version of MiniProfiler, but I have my
RavenDB-based branch compiled and am using it for my site currently. You can bring down my fork
and compile it yourself too, if you're really interested in using it right now.