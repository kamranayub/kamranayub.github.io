---
layout: post
title: "Tools of the Trade 2016"
date: 2016-02-08 21:09:00 -0600
comments: true
published: true
categories:
- Tools
- Visual Studio
- C#
- Development
---

Sometimes you get so caught up in the work you do on a daily basis that you forget what it was like to start your job on day one--not knowing anything about what tools, extensions, and general utilities you take for granted now, 6 years into your career. It seems like on a monthly basis I find a new extension or utility that is useful to me. I wanted to share my toolbelt, in case it contains something you've never heard of and causes you to exclaim in excitement about something awesome that you'll start using today.

<!-- More -->

This list is organized by function--i.e. what the tool contributes to for my work. If I use extensions for a tool, I will list them under the tool. I've definitely used more things than I list here but I use these on a day-by-day basis typically and are what I would consider essential to my workflow. Share any awesome tools you use that I missed in the comments! If I think of more, I'll add them below.

## Coding

I work with JavaScript/TypeScript, HTML, CSS, and C# on a daily basis. Here's what I use and for what.

### [Visual Studio 2015 Pro/Community](https://go.microsoft.com/fwlink/?LinkId=691978&clcid=0x409)

For primary .NET work, web app work, and work-work. I use Community edition at home, it's free!

**Extensions**

- [ReSharper 10](https://www.jetbrains.com/resharper/download/) - Oodles of time-saving refactoring helpers and code analysis
- [Web Compiler](https://visualstudiogallery.msdn.microsoft.com/3b329021-cd7a-4a01-86fc-714c2d05bb6c) - for LESS, SASS compiling
- [Typewriter](https://visualstudiogallery.msdn.microsoft.com/e1d68248-f30e-4a5d-bf18-31399a0bcfa6) (see my recent [blog post](http://kamranicus.com/blog/2016/02/04/typewriter/)) - for T4-style TypeScript codegen
- [Web Essentials 2015](https://visualstudiogallery.msdn.microsoft.com/ee6e6d8c-c837-41fb-886a-6b50ae2d06a2) - for web dev
- [Razor Generator](https://visualstudiogallery.msdn.microsoft.com/1f6ec6ff-e89b-4c47-8e79-d2d68df894ec) - for Razor templates for emails
- [Node.js Tools for Visual Studio](https://visualstudiogallery.msdn.microsoft.com/dd1dc8a5-d627-48a2-a19d-df4fe0c47f19) - for Node.js projects
- [PowerShell Tools for Visual Studio](https://visualstudiogallery.msdn.microsoft.com/c9eb3ba8-0c59-4944-9a62-6eee37294597) - for interactive PowerShell prompt and editing
- [Rebracer](https://visualstudiogallery.msdn.microsoft.com/410e9b9f-65f3-4495-b68e-15567e543c58) - save formatting settings per solution

### [Visual Studio Code](http://code.visualstudio.com)

For working on lots of my JS/TS-based OSS projects like [Excalibur.js](http://excaliburjs.com). The cross-platform, Git-integrated nature of the IDE is awesome along with per-project user settings to keep everyone in-sync.

**Extensions**

- ReStructured Text - for [Excalibur docs](http://docs.excaliburjs.com)
- PowerShell - for scripts

### [Sublime Text 3](https://www.sublimetext.com)

I use Sublime for note-taking (auto-save) and quick file editing since it's so fast and has a context-menu shortcut to edit files.

### [GitHub](http://github.com)

I pay for a plan at GitHub for private source code hosting but I also use it for all my [OSS development](http://github.com/kamranayub). It's a staple of my coding workflow.

### [GitHub Desktop](http://desktop.github.com)

For working with GitHub projects and local Git repositories, I also like that launches posh-git for the shell.

### [Linqpad 5](http://linqpad.com)

For quick C# script testing, database queries, etc.

### [PowerShell & ISE](https://technet.microsoft.com/en-us/scriptcenter/dd742419.aspx)

I recently [became a believer](http://kamranicus.com/blog/2015/09/17/powershell-html5-offline-manifest/) in PowerShell, for automation and scripting it's awesome. Just [take the few hours](https://mva.microsoft.com/en-US/training-courses/getting-started-with-powershell-3-0-jump-start-8276) and learn it, you won't regret it. ISE is the scripting editor built into Windows.

### [posh-git](https://github.com/dahlbyk/posh-git)

The default shell for GH Desktop (above), posh-git is a PowerShell prompt with Git integration.

## Multimedia

### [Adobe Creative Cloud](http://www.adobe.com/creativecloud.html)

The subscription-based model softens the blow of owning Adobe products and, perhaps, costs more over time but the benefits outweigh the negatives--namely, I own the full suite of Adobe products (*cough* legally) and they're **always** up-to-date with new versions so I don't need to pay up-the-nose every year. I also really like TypeKit for syncing new fonts.

### [aseprite Editor](http://www.aseprite.org/)

This is for pixel graphics and sprites, ASE is great for pixel-perfect drawings and animations.

### [Tiled Map Editor](http://www.mapeditor.org/)

For creating game maps using the spritesheets and tilesets I made from ASE/Photoshop (or purchased). Tiled also exports to JSON, making it easy to [integrate with game engines](http://github.com/excaliburjs/excalibur-tiled).

### [Audacity](http://www.audacityteam.org/)

I use Audacity for audio editing since it's easy to use and very lightweight.

### [VirtualDub](http://www.virtualdub.org/)

Simple video editor and great for transencoding video formats.

### [Open Broadcasting Studio](https://obsproject.com/download#mp)

For streaming and screen recording, you can't beat the FOSS OBS Studio. The new version is hot stuff and is a total rewrite of the "Classic" version.

## Productivity

### [TeamViewer](http://www.teamviewer.com/en-us/)

I use TeamViewer because it's dead simple to set up and manage remote access to my machines without fiddling with firewalls or port forwarding. They also have native mobile clients for on-the-go RDP.

### [OneNote / Office 365](https://products.office.com/en-us/office-365-home)

I use OneNote for password-protected information (it's actually encrypted) and for cross-device note syncing. O365 is great for the cross-platform Office and syncing via OneDrive.

### [OneDrive](http://onedrive.com)

I use OneDrive for its cross-platform syncing (PC/Android/iPhone), cloud storage, and PC Windows-explorer integration. It just works. It also means my OneNote notebooks are available everywhere.

### [LastPass](http://lastpass.com)

I use LastPass Password Manager for its browser integration, always available cloud vault, and cross-device syncing (to my Android).

### [Trello](http://trello.com)

My wife and I use Trello to manage our household information--events, shopping, to-do lists, restaurants to eat at, blog posts to write, etc. My wife loves how she can use it easily on her iPhone and get notifications whenever someone changes/adds something. We both like the flexibility it offers and its ease of use. At work, we use it to manage our tasks and work for the team alongside TFS (because, you know, TFS).

### [Doggcatcher](https://play.google.com/store/apps/details?id=com.snoggdoggler.android.applications.doggcatcher.v1_0&hl=en), [Audible](http://audible.com), [iHeartRadio](http://iheartradio.com), [Spotify](http://spotify.com)

You have to listen to something while you work, right? Do you just listen to the local radio on your commutes? Podcasts are invaluable for staying current with tech news and listening to books makes it easy to be "literate" on the go. 

**Podcasts**

I paid for [Doggcatcher](https://play.google.com/store/apps/details?id=com.snoggdoggler.android.applications.doggcatcher.v1_0&hl=en) and it's money well spent. It works flawlessly and I listen to podcasts in the car on the way to and from work everyday. Usually I do one day podcasts, one day book, to keep it sane. I also pick and choose the episodes I listen to.

- [.NET Rocks](https://www.dotnetrocks.com/)
- [This Week in Tech](https://twit.tv/shows/this-week-in-tech)
- [Polygon Minimap](http://www.polygon.com/minimap)
- [Giant Bombcast](http://www.giantbomb.com/podcasts/)
- [My Brother, My Brother, and Me](http://maximumfun.org/shows/my-brother-my-brother-and-me)
- [This American Life](http://www.thisamericanlife.org/)

I listen to [Audible](http://audible.com) for books on commutes. I've been a member for over 6 years and through it own over 100 books and probably have saved myself hundreds of dollars on books. PS. Check out the [Matthew Corbett series](http://www.audible.com/series/ref=a_search_c4_1_1_1srSrs_sa?asin=B0085NK3SS), Edoardo Ballerini is a fucking awesome narrator.

For radio at home, I hooked up my old Android Moto G to a Bluetooth stereo and use [iHeartRadio](http://iheartradio.com). 

For streaming music, I subscribe to [Spotify](http://spotify.com) that my wife and I share on our devices. I can also use Spotify/iHeartRadio in the basement on my PS4.

### OK Google

I use OK Google on my phone (Cortana before, on my Windows Phone) to add reminders and to-dos on-the-go.

## Misc

### Chrome

I use [supervised user profiles](http://kamranicus.com/blog/2015/05/21/chrome-multi-user/) to keep my work separated.

### [ConEmu](https://conemu.github.io/)

An awesome multi-tabbed customizable command prompt host--I use it to create shortcuts for Azure Powershell SDK, Visual Studio CMD prompt, Posh-Git, CMD prompt, etc.

### [RegExr](http://regexr.com/)

An awesome Regular Expression engine in the browser, my go-to Regex reference/tester.

### [Stitches](http://draeton.github.io/stitches/)

An HTML5-based sprite sheet generator.

### [Emby](https://emby.media/)

Not work-related but I use Emby (it's free!) to stream media to my consoles and other devices. It has a great web interface for remote viewing too!
