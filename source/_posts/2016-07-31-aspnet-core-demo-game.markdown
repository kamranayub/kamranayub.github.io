---
layout: post
title: "ASP.NET Core Sample Demo Game"
date: 2016-07-31 12:05:00 -0600
comments: true
published: true
categories:
- C#
- ASP.NET
- .NET Core
- MVC
- SignalR
---

At General Mills we do semi-annual code camps where the developer organization gets together for a half-day of talks and fun. This past code camp myself and my partner in crime, [Erik Onarheim](http://twitter.com/erikonarheim) gave a talk around ASP.NET Core. It's part of our roadmap to be familiar with hosting ASP.NET Core so we wanted to build something and showcase to developers what's changed in Core vs. the typical Framework application.

We made a trivial demo game built on top of SignalR and .NET Core while also showing off other new features of the stack, including:

- Depdendency Injection
- Custom Tag Helpers
- Cascading Configuration
- Multi-Environment Support
- Strongly-Typed Options
- Injecting Services into Views
- Injected MVC Filters
- Bundling & Minification
- Publishing to Azure

We had the demo running on Azure during the talk so people could join the game and we even attempted showing Linux support, though web sockets were not behaving nicely behind nginx. The game is not really a game but more of a showcase of using web sockets to allow some real-time multiplayer server action. It is definitely *not* how you'd implement a "real" multiplayer game but it's a fun demo.

You can check out the source code on GitHub: [https://github.com/eonarheim/aspnet-core-demogame](https://github.com/eonarheim/aspnet-core-demogame). It's commented pretty heavily to help understand the different parts, it's based on the default out-of-the-box web template (`dotnet new -t web`).

We may or may not upload the talk onto YouTube since there wasn't really anything specific about our work until the very end, which we can strip out without losing any important bits.
