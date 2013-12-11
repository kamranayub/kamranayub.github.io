---
layout: post
title: "What's the most efficient way to read config settings in .NET?"
date: 2011-10-20 20:33:09 -0500
comments: true
categories:
permalink: /blog/posts/22/whats-the-most-efficient-way-to-read-config-settin
disqus_identifier: 22
---

In a project I'm contributing to, I saw some code like this (simplified but boiling down to):

```c#
public static string ConfigValue {
    get {
        return System.Configuration.ConfigurationManager.AppSettings["ConfigValue"];
    }
}
```

Is there anything wrong with this? It's a static read-only property. That seems pretty reasonable!

### Storage matters

The issue that I saw was that there's no real reason to do this. You could do this just as easily:

```c#
public static readonly string ConfigValue = 
     System.Configuration.ConfigurationManager.AppSettings["ConfigValue"];
```

*So, who cares? It looks the same to me!* As far as I can tell, you should care, even a little bit. Check this out:

![Perf Timer](/blog/images/34.png)

Notice anything? The `static readonly` field is far and away faster than any other approach (400 *nanoseconds*). The other three approaches are pretty similar, ranging from 95ms to 104ms. I'll do the math for you: 

> **A `static readonly` field is upwards of 200x faster** than the equivalent property implementation.

The reason is simple: `get/set` are translated into methods by the compiler. A field is simply a field. Any property will incur some penalty for wrapping methods, so a field will always be faster! I use properties all the time but sometimes it might make sense to forgo a property in favor of a field.

### The test case

Will you ever notice differences this small? I doubt it (the test was 100000 iterations). I did this for a learning experiment and for future reference. <strike>However, reading a config file is I/O at its core. To me, that says you should care about minimizing possible calls to the disk. Under a substantial load test, perhaps this would incur a visible penalty to page load time depending on how many calls you had to your settings wrapper.</strike> (**Update:** One of my colleagues pointed out that app config is probably loaded on start-up, so there shouldn't any disk I/O happening.)

**Performance matters** but this is probably so picky that you really don't ever need to worry about it. As my co-worker said to me after reading this:

> "A single call to a suboptimal EF/NHibernate/SQL query is far more damaging than 100,000 calls to these simple properties/fields. If a query took 1 second it would be roughly equivalent to 1,000,000 calls to the most inefficient property implementation. I would say spend your time where you get the biggest bang for your buck."

Couldn't have said it better myself! If you're doing perf testing, make changes where it matters. This is only a microscopic change compared to some that would make more of a difference (such as how you handle `Regex`).

Here's the test case. I am not entirely positive if there's unseen overhead but the other three examples are common things I've seen or even done in regards to this type of pattern:

<script src="https://gist.github.com/1302236.js?file=PerfTestCaseBackingMethods.cs"></script>

Let me know if I made a mistake in the test case that's skewing results and I'll fix it.