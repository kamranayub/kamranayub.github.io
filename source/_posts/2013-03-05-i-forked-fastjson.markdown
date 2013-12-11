---
layout: post
title: "I forked fastJSON"
date: 2013-03-05 17:00:00 -0600
comments: true
categories:
permalink: /blog/posts/65/i-forked-fastjson
disqus_identifier: 65
---

My Windows Phone app mainly deals with talking to a web API. I've previously had good experience using RestSharp but now with the new `async/await` features, I've been trying to only use `Task`-based stuff. I gotta say, I'm loving it.

RestSharp for WP does not natively support async/await but you can wrap it yourself. However, why not just use `HttpClient`? So that's what I did. I ripped out RestSharp and replaced it with [AsyncOAuth](https://github.com/neuecc/AsyncOAuth) (which depends on HttpClient).

Besides OAuth, the other issue you'll run into is what serializer/deserializer to use. You could go with JSON.NET, or ServiceStack, but those are two extremes... one being very flexible and other being very inflexible. I needed an in-between but also something very fast and that's exactly what [fastJSON](http://www.codeproject.com/Articles/159450/fastJSON) is.

Of course, it's fast because of the same reason SS is fast: it doesn't do any fancy property matching. When you're dealing with APIs, it's rare that the response they send back matches exactly with your object model and properties (especially C#). That's why the name variance feature of the larger libraries is awesome.

So, I did what any good developer would, I dug in and added it. I also added `rootElement` support (for contained responses).

You can find my changes in my [fastJSON fork](https://github.com/kamranayub/fastJSON) on GitHub. I also went ahead and organized the solution a bit, as the original source was a bit hard to get running. I also created a Nuget package for my changes and pushed that ([fastJSON-kayub](https://nuget.org/packages/fastJSON-kayub)).

I don't necessarily intend to keep my changes in-sync. It would be preferable if my changes were pulled into the original... and it wouldn't hurt to use my new organization (since it's easy for others to pull down and build), but if that never happens, I'm fine with that.

### Note on Name Variance

The name variance is interesting. In R#, the code was written to `yield return` each variant so as soon as one matches, it returns. In fastJSON, the matching is done in the opposite direction, fastJSON will try to match the JSON key to an object's properties... so it makes it difficult to use what R# had written originally. I modified the code to allow for optional variant matching to speed things up. Without that, the name variance increased deserialization time by **a ton**. However, in my case, I really only needed ProperCase matching... which only increases the time by about 2x.

That's why I made the decision to opt-out of name variance by default and when enabled, only enable ProperCase by default as that's probably the most common.