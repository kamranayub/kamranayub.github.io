Title: "Finished My First Workshop at MinneWebCon"
Published: 2018-04-29 19:56:00 -0500
Lead: On April 24 I gave an Node.js Bots in Azure workshop at MinneWebCon
Tags:
- Workshop
- Azure
- Node.js
- Post-Mortem
- MinneWebCon
---

Last week I had the opportunity to [give a half-day workshop][workshop-post] at MinneWebCon. I had initially submitted the session as a 50 minute talk but the committee thought it would be better as a workshop.

The workshop outline and code is [available on GitHub][workshop]. I thought I'd write up a brief retrospective since it was my first workshop!

## What went well

- Everyone was able to deploy a live bot and finished the workshop (huzzah!)
- I was able to help answer some questions I hadn't anticipated
- People paired up if they didn't have everything they needed on their computers
- Azure Bot Service works pretty well and even when a couple people accidentally chose the wrong code template, they were able to redo the bot in a few minutes
- People said they liked it, which always feels good!

## What could change

- If it was a longer workshop, I'd rather build a custom sample bot rather than building off the pre-built Notes domain
- I walked through some of the LUIS grammar via whiteboard but it would have been better to visualize the relationships better in slide form I think
- At the end some folks wanted to see what a "real" bot looks like hooked up to real data. I showed what I did for [KTOMG][ktomg] but ideally in a full workshop we'd use a live data source as you would in the real world.
- The Azure Bot Service "web chat" blade was super finicky--it usually required people to sign out fully and sign back in to boot up the interface

## What I learned

- I learned I enjoy giving workshops. Helping people accomplish something in a hands-on way is pretty cool.
- Different people are on different levels and want different things out of the workshop. It was good I had some stretch goals.
- Professionals want a way to see how what they learned will apply to their day-to-day work. I'd flesh this out more if I gave the workshop again. Showing how I wrote my bot in C# for [KTOMG][ktomg] was helpful I think.

[workshop-post]: https://kamranicus.com/posts/2017-12-18-workshop-bots-javascript-node-minnewebcon
[workshop]: https://github.com/kamranayub/workshop-nodejs-azure-bots
[ktomg]: http://ktomg.com