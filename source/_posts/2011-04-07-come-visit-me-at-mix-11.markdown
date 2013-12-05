---
layout: post
title: "Come Visit Me at MIX 11"
date: 2011-04-07 15:14:55 -0500
comments: true
categories:
permalink: /blog/posts/4/come-visit-me-at-mix-11
---

I am extremely excited (and thankful) to be visiting [MIX 11](http://live.visitmix.com/) next week. Hopefully I can garner some "cred" with the .NET community and I hope I can meet some of my .NET community idols as well.

## Come see me at Open Source Fest

I'll be standing at a cocktail table at the [Open Source Fest](http://live.visitmix.com/OpenSourceFest) (I am #37) presenting my tiny little "library" (what do you call one file?), [.JSON](http://github.com/kamranayub/.JSON).

## About .JSON

I know that there exists several open source libraries for dealing with JSON, including [Json.NET](http://james.newtonking.com/projects/json-net.aspx) and [JsonFx](https://github.com/jsonfx/jsonfx).

However, I wanted something a bit different. First, I didn't want a library... all I needed to do was consume JSON-based web services. Second, all I wanted to do was interact with the results as a dynamically typed object.

The output of that endeavor was .JSON, a single-file library containing about [~500 SLOC](https://github.com/kamranayub/.JSON/blob/master/DotJson.cs).

It supports:

- POST to JSON services
- GET from JSON services
- Converting strings to dynamic JSON objects
- Convert JSON objects to strings
- Convert dictionaries to JSON objects
- Supports LINQ to Objects if using `dynamic[]`
- Includes my [`PrettyJson`](/Home/PrettyJson) formatter

I only realized recently that JsonFx also supports dynamic typing, but as far as I know no other library offers a utility to access JSON services. I sent a message to JsonFx's owner about integrating my `JsonService` into JsonFx, as that seems like a good fit.

Part of my motivation to create the library was to learn about dynamics. Part of it was to prove you *could* do it in a single, reasonably small class. I thought it was pretty neat! I [was not disappointed to hear](http://www.hanselman.com/blog/NuGetPackageOfTheWeek4DeserializingJSONWithJsonNET.aspx) that JsonFx did the same thing, since I learned a lot in the process and I still think I added some value for people that didn't need a whole library to work with JSON.

At any rate, I am talking about it at the OSF. Maybe some kind souls will part with a poker chip or two for my "tip jar". For me, it's just to meet [all](http://twitter.com/eisenbergeffect) [the](http://twttier.com/bradmi) [other](http://twitter.com/gblock) [awesome](http://twitter.com/samsaffron) [developers](http://twitter.com/johnsheehan).