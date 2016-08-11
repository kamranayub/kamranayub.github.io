---
layout: post
title: "OpenWeatherMap PowerShell Module"
date: 2016-08-11 18:15:00 -0600
comments: true
published: true
categories:
- PowerShell
- OSS
---

There's no better to way to learn something new than to make a thing. Yesterday I had a strong desire to know what the weather was like from my command prompt. I can't explain it--I just had to and I thought, "Hey, it can't be that bad, right?" I decided to try writing and releasing an open-source PowerShell module.

<!-- More -->

It turned out to be very easy, in fact. There's an awesome project called [OpenWeatherMap](http://openweathermap.org) where you can sign up for a free API key and go to down on their open data API--limited to 60 calls a minute which isn't a problem for my pet project.

So, what happens when you combine PowerShell and an open weather API? Weather in your command prompt!

![Weather](https://cloud.githubusercontent.com/assets/563819/17608181/4a908116-5ff0-11e6-8bb2-396c8f5a998e.png)

I've [released the module on GitHub](https://github.com/kamranayub/posh-openweathermap/) and the [PowerShell Gallery](http://www.powershellgallery.com/packages/OpenWeatherMap/) for your enjoyment. I've already added weather symbols (Unicode is cool) and I'm adding friendly forecasting next.

You can set up your PowerShell profile to get the weather each time you boot your prompt, [following the example on GitHub](https://github.com/kamranayub/posh-openweathermap/blob/master/profile.example.ps1).

This also sets up an alias for `weather` so you can ad-hoc get the weather too:

![weather command](https://cloud.githubusercontent.com/assets/563819/17608247/ba82117e-5ff0-11e6-92c3-b06e7216feb3.png)

I plan to add the ability to get a quick forecast, so you can know what to plan for the next morning before you get into work! There's probably more neat stuff we can do with coloring the weather conditions but maybe you want to open a PR then?

One interesting thing I noticed was that the Windows Command Prompt (and PowerShell) does not support some Unicode characters. For example, you can probably see this:

    ⛱

But if you try pasting that into command prompt you'll see:

![no symbols](https://cloud.githubusercontent.com/assets/563819/17608313/4e4c8b3c-5ff1-11e6-9300-200bc4604a39.png)

So that was a little disappointing--it meant I had to stick to the Unicode symbols that worked in the prompt, which is still a good amount but not as a granular as it could potentially be.

I am not sure why Windows doesn't support those characters but maybe in a future Windows 10 update they'll add support!
