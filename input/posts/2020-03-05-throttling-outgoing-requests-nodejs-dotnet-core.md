Title: "Throttling Outgoing Requests in Node.js and .NET Core"
Published: 2020-03-05 09:00:00 -0500
Lead: "Rate limiting outgoing requests across a cluster using distributed counters and sliding time windows"
Tags:
- Programming
- Node.js
- .NET
- C#
- JavaScript
---

I have published two articles recently on a problem I was running into working on [Keep Track of My Games](https://keeptrackofmygames.com). In order to sync user's Steam collections, I have to call the [Steam Web API](https://developer.valvesoftware.com/wiki/Steam_Web_API).

The Steam Web API implements "rate limiting" meaning that if you call it too many times too quickly it returns a `HTTP 429 Too Many Requests` response. According to [the terms](https://steamcommunity.com/dev/apiterms) the rate limit is 100,000 requests per day, which is pretty generous. But if you're thinking of syncing 2000 users every 15 minutes, that puts you **two times** over the limit! So you need a throttling mechanism to defer processing once you reach the limit. In most scenarios like this, public APIs will return some useful HTTP headers that let you know what your current request count is but in this case, the Steam API does no such thing (it's a bit dated).

There are a few ways to rate limit or _throttle_ outgoing requests to an API like this but most approaches don't work with **clustering** meaning multiple isolated clients. Approaches like using [slim semaphore](https://codeburst.io/throttling-concurrent-outgoing-http-requests-in-net-core-404b5acd987b) or [limiter](https://www.npmjs.com/package/limiter) don't cut it because those only work **in-memory**. We need a backing store to coordinate counting requests across a cluster. [Bottleneck](https://npmjs.com/package/bottleneck) is one npm package that supports this but it can only use Redis. Since I don't use Redis (and I'm using C#) that wasn't an option for me. Instead I turned to RavenDB for the solution and it's been working out well!

I wrote up two guides on achieving this using RavenDB, [one for .NET](https://www.codeproject.com/Articles/5260137/Throttling-Outgoing-HTTP-Requests-in-a-Distributed) and [one for Node.js](https://www.codeproject.com/Articles/5260913/Throttling-Outgoing-Requests-in-Node-js), so if you're curious how I solved the problem then check them out!