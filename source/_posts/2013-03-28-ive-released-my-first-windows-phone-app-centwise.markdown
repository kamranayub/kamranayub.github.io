---
layout: post
title: "I've released my first Windows Phone app: Centwise"
date: 2013-04-11 22:47:42 -0500
comments: true
categories:
permalink: /blog/posts/67/ive-released-my-first-windows-phone-app-centwise
---

I'm excited to talk about my latest project that just went live a couple weeks ago: [Centwise](http://centwiseapp.com). Centwise is a Windows Phone 7 & 8 app client for a service that I use regularly, [Splitwise](http://splitwise.com).

You can download the app to your Windows Phone below:

[![Get the app](http://media.tumblr.com/410b39b16d0c759db1cbda923e968f91/tumblr_inline_mjvfbsUlMF1qz4rgp.png)](http://bit.ly/getcentwise)

## What's Splitwise?

[Splitwise](http://splitwise.com) is a web app that lets you manage IOUs and bill splitting. I use it day-to-day during outings with friends but mainly I use it to keep track of bills and household expenses between me, my fiancee, and our roommate. It's a great tool and they provide native apps for iOS and Android. When I started Centwise, they didn't have a WP app and I figured as an avid user of the service, my first app could be a Splitwise client.

I really want to thank the Splitwise team (Jon & Ryan!) for helping me out, answering questions, and for mentioning Centwise on their blog.

## First app experience

I bought a HTC 8X when it was announced on Verizon day one. I knew I wanted to leave my old Droid Incredible in the dust and move on to greener pastures. Being a Microsoft stack guy, I figured Windows Phone would be good not only because I like the interface and experience but also because I could make some apps for the platform.

In the past, I've worked with WPF and XAML before, so learning the ins and outs of the pared-down Silverlight for Windows Phone wasn't too terrible but there were certainly roadblocks. I've tried to bookmark every article or post I came across that helped me out, which I'll share at the end. Coming from a primarily web-based focus to a native XAML/C# platform was certainly jarring and there were times I slammed my hands on the keyboard exclaiming, "THIS WOULD BE WAY EASIER IN CSS AND HTML!" Like making a transparent button, for example.

I hope to go more in detail with certain issues as well as share some tips and tricks I learned along my journey to building my first app. There can be a lot of crazy things you wouldn't know XAML can do if you didn't spend time deconstructing bigger open-source libraries (which I *highly* recommend, I often kept [Phone Toolkit](http://phone.codeplex.com) open alongside my own app to see how they built controls).

## The beginnings

All apps have a beginning. Mine began on paper using the [excellent paper templates](http://www.flickr.com/photos/michaeldorian/5071511857/) provided by Michael Bach.

Here are the paper sketches I made that helped me form the screens for the app. I spent most of my time sketching the more complex pages, the ones that needed to do or show a lot of stuff but where I wanted to keep it as simple as possible.

<iframe src="https://skydrive.live.com/embed?cid=0E21D632E0E57AEE&resid=E21D632E0E57AEE%2136892&authkey=ACGYGiJBJeURBfk" width="320" height="180" frameborder="0" scrolling="no"></iframe>

<iframe src="https://skydrive.live.com/embed?cid=0E21D632E0E57AEE&resid=E21D632E0E57AEE%2136893&authkey=AEr0Ed2bQzMJx54" width="320" height="180" frameborder="0" scrolling="no"></iframe>

<iframe src="https://skydrive.live.com/embed?cid=0E21D632E0E57AEE&resid=E21D632E0E57AEE%2136894&authkey=APrH1Wua6J8fLJU" width="320" height="180" frameborder="0" scrolling="no"></iframe>

I've found it helpful to do these types of sketches to help me visualize the UI. It also helps me see immediately whether or not my idea makes sense.

From there, I could start working on the XAML and the screens to put my ideas into the app. However, as you'll soon see, my first ideas usually weren't the best.

## Optimizing the UX

<iframe src="https://skydrive.live.com/embed?cid=0E21D632E0E57AEE&resid=E21D632E0E57AEE%2136892&authkey=ACGYGiJBJeURBfk" width="320" height="180" frameborder="0" scrolling="no"></iframe>

If you look at the first image, you can see that my initial idea when creating expenses was to display a list of people to choose from. In fact, pre-1.0 that is exactly what I had done. The flow looked like this:

1. User specifies "Create an IOU"
2. User chooses other person or chooses from address book
3. User is shown IOU screen

This was pretty straightforward... after all, there are only 3 steps. But that was one step too many. It also ended up complicating my code quite a bit due to managing the back stack (i.e. what happens when a user cancels or hits 'Back').

In the final 1.0 release, I changed it to be easier and faster:

1. User chooses to create an IOU
2. User is shown screen and either a) the other person is already populated or b) user can tap the avatar placeholder to choose someone

That's just one example of a change I had made that was different from my first implementation.

If you take a look at the second set of sketches, you can see I drastically changed how the IOU (and other) screens were laid out.

<iframe src="https://skydrive.live.com/embed?cid=0E21D632E0E57AEE&resid=E21D632E0E57AEE%2136893&authkey=AEr0Ed2bQzMJx54" width="320" height="180" frameborder="0" scrolling="no"></iframe>

<iframe src="https://skydrive.live.com/embed?cid=0E21D632E0E57AEE&resid=E21D632E0E57AEE%2136609&authkey=AOWlC2tQqkFVW_A" width="180" height="320" frameborder="0" scrolling="no"></iframe>

Here are just some of the changes I made:

1. Instead of a button, I created a `CalculatorTextBox` that combines a calculator and amount textbox into one.
  - This was huge because it also greatly simplified the UI of the Divide a Bill screen
2. Moved the currency button to the appbar menu items because it's not something a user will typically use often.
3. Made the static arrow shown between avatars a button to easily swap people in case the user made a mistake

## Easy but not simple

Someone wise once said:

> Making things easy is *hard*.

That's been true with everything I've ever had the pleasure to work on. Easy is hard.

I didn't have any formal beta testers, mainly because the Windows Phone Dev Center didn't let me (*sigh*), but that didn't stop me from showing it to friends and family to have them test it out.

The core implementation of the app is pretty simple... it's all the extra work I had to put in to make the *app* simple that wasn't easy. Not only was it my first time trying to learn Windows Phone development, I also have a tendency to spend a lot of time on little things.

<iframe src="https://skydrive.live.com/embed?cid=0E21D632E0E57AEE&resid=E21D632E0E57AEE%2136736&authkey=AI7_LBxzEW-9MkM" width="180" height="320" frameborder="0" scrolling="no"></iframe>

For example, let's consider the "people picker" functionality. When a user wants to choose people to add to the expense, there's more logic than simply "show the user's friends". The control actually does several things:

1. If the user adds an expense type in the context of a group, then the picker shows the people in that group first.
2. If the user divides a bill from the home screen, the picker shows available groups which:
  - Automatically adds all the people in that group to the bill
  - Sets the "filed in" value to the group they chose, regardless of the context the user was in (home screen vs. group)
3. Makes sure not to show people already added to the expense
4. Let's the user choose someone from the address book (but not for payments!)

I use this people picker anywhere I need to, including when creating a group. By reusing the same experience, the user never has to learn a new way of picking people.

There's just a ton of little things like that throughout the app that the user probably won't ever notice but I hope surfaces in the form of, "Hey, this is easy to use!". I didn't want the app to get in the way of the user doing whatever they needed to do.

I cannot count the amount of hours I've spent so far, although I tried to keep track at first but once I hit 60+ hours in less than a week, I stopped. Luckily, GitHub has a great visualization of how often I worked the app:

**Commit Activity**

![I am insane](/blog/images/72.png)

Let me break it down for you: I am insane.

![Seriously, insane](/blog/images/73.png)

You can forgive me if I plan to take it a bit slower with updates. I was hoping to finish in time for the Next App Star challenge but unfortunately even spending countless hours during the week, the app just wasn't in a good state in time for that deadline.

## What's next?

I hope to go into more technical detail around some of the things I did with my app at a later point. I was really excited to learn how to build a Windows Phone app and am very pleased with the result. I didn't build it expecting to make thousands of dollars; I'll be lucky (and happy) if I break $500 but making it a paid-for app motivates me as a single developer to keep the app up-to-date and full of useful features. I have personal projects and open projects that eat up time that I need to make sure I can support each one of them.

As far as what I have planned, you can keep tabs on [CentwiseApp.com](http://centwiseapp.com) or follow Centwise on [Twitter](http://twitter.com/centwiseapp) or [Facebook](http://facebook.com/centwise). I hope to do a UI refresh soon and I have about 20+ enhancements to incrementally add. I already released the [1.1 update](http://centwiseapp.com/post/46850512483/update-v1-1-will-be-available-today) that added several new features, so that's a good start!

## Resources

Here are all my bookmarks currently under my Windows Phone folder:

- [Centwise | Windows Phone Apps+Games Store (United States)](http://www.windowsphone.com/en-us/store/app/centwise/faeb5f7b-fbb6-41c6-8fac-17d81dae703d)
- [The art of simplicity: Caliburn.Micro: Design time support](http://bartwullems.blogspot.com/2012/08/caliburnmicro-design-time-support.html)
- [Elegant Code » WPF String.Format in XAML with the StringFormat attribute](http://elegantcode.com/2009/04/07/wpf-stringformat-in-xaml-with-the-stringformat-attribute/)
- [Processing Sequences of Asynchronous Operations with Tasks - .NET Parallel Programming - Site Home - MSDN Blogs](http://blogs.msdn.com/b/pfxteam/archive/2010/11/21/10094564.aspx)
- [Implementing Windows Phone 7 DataTemplateSelector and CustomDataTemplateSelector | WindowsPhoneGeek](http://www.windowsphonegeek.com/articles/Implementing-Windows-Phone-7-DataTemplateSelector-and-CustomDataTemplateSelector)
- [Jeff Wilcox – Creating a global ProgressIndicator experience using the Windows Phone 7.1 SDK Beta 2](http://www.jeff.wilcox.name/2011/07/creating-a-global-progressindicator-experience-using-the-windows-phone-7-1-sdk-beta-2/)
- [Daniel Vaughan | Binding the WP7 ProgressIndicator in XAML](http://danielvaughan.orpius.com/post/Binding-the-WP7-ProgressIndicator-in-XAML.aspx)
- [Adding/Removing AppBar Buttons on the fly « Cyberherbalist&#39;s Blog](http://cyberherbalist.wordpress.com/2011/03/20/addingremoving-appbar-buttons-on-the-fly/)
- [Silverlight BringIntoView() extension method (with OnGotFocus behavior) - Josh Schwartzberg high-fives the CLR](http://weblogs.asp.net/dotjosh/archive/2009/11/05/bringintoview-extension-method-for-silverlight-with-behavior.aspx) 
- [Working with GIF images in Windows Phone - Jaime Rodriguez - Site Home - MSDN Blogs](http://blogs.msdn.com/b/jaimer/archive/2010/11/23/working-with-gif-images-in-windows-phone.aspx)
- [WP7 Context Menus with Caliburn Micro - Compiled Experience - Freelance Windows 8 and Windows Phone Application Development](http://compiledexperience.com/blog/posts/wp7-context-menus-with-caliburn-micro)
- [Caliburn Micro: WPF, Silverlight, WP7 and WinRT/Metro made easy. - Home](http://caliburnmicro.codeplex.com/)
- [Silverlight ListBox Add and Remove Animations](http://blog.falafel.com/blogs/josh-eastburn/2011/09/16/silverlight_listbox_add_and_remove_animations)
- [Using async/await without .NET Framework 4.5 - BCL Team Blog - Site Home - MSDN Blogs](http://blogs.msdn.com/b/bclteam/archive/2012/10/22/using-async-await-without-net-framework-4-5.aspx)
- [Metro In Motion Part #4 – Tilt Effect | Colin Eberhardt&#39;s Technology Adventures](http://www.scottlogic.co.uk/blog/colin/2011/05/metro-in-motion-part-4-tilt-effect/)
- [Task Exception Handling in .NET 4.5 - .NET Parallel Programming - Site Home - MSDN Blogs](http://blogs.msdn.com/b/pfxteam/archive/2011/09/28/10217876.aspx)
- [good way to cancel async method](http://social.msdn.microsoft.com/Forums/en-US/async/thread/b536336f-812a-4227-8abe-fd123df49fc1)
- [How to: Cancel a Task and Its Children](http://msdn.microsoft.com/en-us/library/dd537607.aspx)
- [Cancellation in Managed Threads](http://msdn.microsoft.com/en-us/library/dd997364.aspx)
- [Task Cancellation](http://msdn.microsoft.com/en-us/library/dd997396.aspx)
- [Fiddler and the Windows Phone 7 Emulator - Fiddler Web Debugger - Site Home - MSDN Blogs](http://blogs.msdn.com/b/fiddler/archive/2010/10/15/fiddler-and-the-windows-phone-emulator.aspx)
- [Silverlight 3.0 RTW: The CollectionViewSource](http://www.silverlightplayground.org/post/2009/07/10/Silverlight-30-RTW-The-CollectionViewSource.aspx)
- [How to: Search for a Contact and get Contact Picture in Windows Phone | WindowsPhoneGeek](http://windowsphonegeek.com/tips/how-to-search-for-a-contact-and-get-contact-picture-in-windows-phone)
- [Windows Phone 7 - Part #3: Understanding navigation](http://www.silverlightshow.net/items/Windows-Phone-7-Part-3-Understanding-navigation.aspx)
- [Redirecting an initial navigation - Peter Torr&#39;s Blog - Site Home - MSDN Blogs](http://blogs.msdn.com/b/ptorr/archive/2010/08/28/redirecting-an-initial-navigation.aspx)
- [Clarity on Windows Phone 7](http://blogs.claritycon.com/windowsphone7/)
- [How to: Encrypt Data in a Windows Phone Application](http://msdn.microsoft.com/en-us/library/hh487164(v=VS.92).aspx)
- [Windows Phone, Silverlight and WPF Multi-Touch Manipulations - Home](http://multitouch.codeplex.com/)
- [Dynamic Layout and Transitions in Expression Blend 4](http://msdn.microsoft.com/en-us/expression/ff624123.aspx)
- [Silverlight 4 Property Triggers | Clarity Blogs](http://blogs.claritycon.com/blog/2011/02/silverlight-4-property-triggers/)
- [Expression Blend Samples - Home](http://expressionblend.codeplex.com/)
- [Jacob Gable: Silverlight 4 Property Triggers](http://jacob4u2.blogspot.com/2011/02/silverlight-4-property-triggers.html)
- [Loading Data when the User Scrolls to the End of a List in Windows Phone 7 - CodeProject](http://www.codeproject.com/Articles/150166/Loading-Data-when-the-User-Scrolls-to-the-End-of-a)
- [Dynamic Layout and Transitions in Expression Blend 4 - Expression Blend and Design - Site Home - MSDN Blogs](http://blogs.msdn.com/b/expression/archive/2010/03/16/dynamic-layout-and-transitions-in-expression-blend-4.aspx)
- [How to target multiple versions with your app for Windows Phone](http://msdn.microsoft.com/id-id/library/windowsphone/develop/jj206997(v=vs.105).aspx#linked_projects)
- [Tile design guidelines for Windows Phone](http://msdn.microsoft.com/library/windowsphone/design/jj662929(v=vs.105).aspx)
- [Multi-resolution apps for Windows Phone 8](http://msdn.microsoft.com/en-US/library/windowsphone/develop/jj206974(v=vs.105).aspx)
- [Logging exceptions of Windows Phone applications to a central server with the help of FogBugz - CodeProject](http://www.codeproject.com/Articles/412435/Logging-exceptions-of-Windows-Phone-applications-t)
- [Creating Progress Dialog for WP7. - Alex Yakhnin&#39;s Blog - Site Home - MSDN Blogs](http://blogs.msdn.com/b/priozersk/archive/2010/09/20/creating-progress-dialog-for-wp7.aspx)
- [How to fix the standard header of a pivot control using a style and how to fix - ge using the TitleControl » .NET App](http://dotnetapp.com/blog/2012/05/21/how-to-fix-the-standard-header-of-a-pivot-control-using-a-style-and-how-to-fix-a-page-using-the-titlecontrol/)
- [How to crash other WP7 applications? | Niko Vrdoljak&#39;s Blog](http://nikovrdoljak.wordpress.com/2012/02/20/how-to-crash-other-wp7-applications/)
- [WP7: Splitting Application to Multiple Assemblies when using Caliburn.Micro - Mikael Koskinen](http://mikaelkoskinen.net/wp7-splitting-application-to-multiple-assemblies-when-using-caliburn-micro)
- [Background Agents - Part 1 of 3 - Peter Torr&#39;s Blog - Site Home - MSDN Blogs](http://blogs.msdn.com/b/ptorr/archive/2011/07/11/background-agents-part-1-of-3.aspx)
- [Oren Nachman » Talks](http://www.nachmore.com/talks/)
- [Sterling NoSQL OODB for .NET 4.0, Silverlight 4 and 5, and Windows Phone 7 - Home](http://sterling.codeplex.com/)
- [MSDN Magazine: Windows Phone 7 - Sterling for Isolated Storage on Windows Phone 7](http://msdn.microsoft.com/en-us/magazine/hh205658.aspx)
- [Modern UI Icons](http://modernuiicons.com/)
- [Nicolas Humann | Save disk space with IsolatedStorageGZipFileStream](http://blog.humann.info/post/2011/08/28/Save-disk-space-with-IsolatedStorageGZipFileStream.aspx)
- [Using Isolated Storage on the Phone](http://msdn.microsoft.com/en-us/library/hh821020.aspx)
- [Windows Phone 8: Critical Developer Practices for Delivering Outstanding Apps | Build 2012 | Channel 9](http://channel9.msdn.com/Events/Build/2012/3-045)
- [» Wicked Wireframes: WP7 Vector UX Kit on Windows Phone 7](http://blogs.claritycon.com/windowsphone7/2011/01/wicked-wireframes-wp7-vector-ux-kit-3/)
- [Windows Phone 7 Wireframe Paper Sketching Template | Flickr - Photo Sharing!](http://www.flickr.com/photos/michaeldorian/5071511857/)
- [Jeff Wilcox – Displaying static maps on the Windows Phone for performance and scenario wins](http://www.jeff.wilcox.name/2012/01/jeffwilcox-maps/)
- [Launchers and Choosers for Windows Phone](http://msdn.microsoft.com/en-us/library/windowsphone/develop/ff769542(v=vs.105).aspx)
- [People Hosting Open Office Hours](http://usdpe.ohours.org/)
- [iconmonstr - Free simple icons for your next project](http://iconmonstr.com/)