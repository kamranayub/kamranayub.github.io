Title: "Frontend Masters Workshop: React Native Recap"
Published: 2020-03-06 21:20:00 -0500
Lead: "I attended the Frontend Masters workshop on React Native with Kadi Kraman and here's what I learned"
Tags:
- React Native
- React
- JavaScript
- TypeScript
- Workshops
---

Today I attended the local Frontend Masters [in-person workshop](https://frontendmasters.com/workshops/react-native-v2/) for React Native by [Kadi Kraman](https://twitter.com/kadikraman). First, I have to point out her last name is my first name with a couple letters swapped. Too funny. Next, it was great! 🎉 I had a lot of fun.

I've [posted my lab repository](https://github.com/kamranayub/sample-react-native-workshop-expo-ts) on GitHub for anyone else to peruse, notably I followed along in TypeScript as a way to better explore the APIs and see how the TypeScript experience was.

### The workshop

Kadi works at Formidable Labs as an engineer and she specializes in React Native applications. Her workshop was focused on getting started with React Native and we built a "color theme" app that allows you to view color schemes and add a new one. Since I use React daily, it definitely felt familiar but Kadi went over a lot of the foundational concepts just in case attendees weren't as familiar.

### What I took away

Using React day-to-day I feel like I know it pretty well but I had never sat down to actually look into React Native. What I appreciate about workshops like this is the ability to take dedicated time and go end-to-end with something, with an instructor who knows their subject and can help you not waste time.

As far as the _motivation_ to learn it, first, I'm always interested in learning new things. I'm a lifelong learner. But second, I was interested in comparing it to my experience building native Windows Phone apps [back in the day](https://kamranicus.com/projects) as well as some passing familiarity with [Ionic](https://ionicframework.com/), [Xamarin](https://dotnet.microsoft.com/apps/xamarin) and [Cordova](https://cordova.apache.org/) apps. 

I was _very impressed_ by how quick it was to run an app using the Expo CLI. I had the app running on my phone within a few minutes. That's awesome! Installing traditional React Native was definitely more of a chore, even when I had Xcode installed previously.

The workshop covered a lot of topics, enough to build out simple applications and I especially liked the familiar `fetch` examples without having to bring in complicated dependencies. In other words, coding in React Native was very familiar for a traditional React developer. I felt like the workshop taught me enough where I could fumble around and build out the app more without feeling like I was overwhelmed.

I think I would attend an Advanced React Native workshop in the future because of course my mind went to all sorts of questions that were beyond the scope of what time we had:

1. How do you change the UI in response to the running platform? e.g. show the "Floating Action Button" in Android vs. a button in iOS
1. How hard _is it_ to add a native module?
1. How do you handle device storage? That was a huge pain for me in Windows Phone (multithreading + I/O, ugh).
1. How do you write tests? React Native Testing Library and Detox were mentioned but we didn't have time to cover them
1. How do you bring in a design system that can work well cross-platform? I was glad she mentioned `styled-components` is supported and I also see [Paper](https://github.com/callstack/react-native-paper) seems well-established.

So Marc, if you're reading this, invite Kadi again next year for some Advanced React Native 😎

### Any improvements or feedback?

I appreciated Kadi's attention to realism -- she did go ahead and run through the installation steps as a new developer would and there *were* roadblocks she ran into that she showed how to resolve. At the same time, it _would_ have been nice to get to testing in a one-day workshop and the installation process took time away from other subjects so it's a balancing act there. I could see this being a two-day workshop in the future, to cover animations, testing, storage, and deployment on day two.

The exercises were just right I think -- they were helpful in reinforcing what we learned and were at just the right spots. I enjoyed the stretch goals!

I love the trend of instructors providing a full workshop site (the [Python Fundamentals workshop](https://www.learnpython.dev/) was also like this) and I actually really liked how Kadi simply presented from the site itself versus slides. It meant we could follow along, read ahead, or work at our own pace if we needed to without missing anything. Everything was in one place. It was easy to copy code samples and click links for further reading. Her pace was perfect when going through the demos. It was at the right level for someone with previous React experience but I could see by the hook section where it could lose folks who haven't done a lot of hooks-based development. There were a lot of questions pertaining to hook semantics that came up! I can relate since even I get tripped up by them.

### Would I use React Native?

I was keenly interested in what it might look like to build a React Native app for [Keep Track of My Games](https://keeptrackofmygames.com) and after taking the workshop, I feel more confident it could work out well. I can't say I'm 100% on board yet though since it's still true that when you need to get down to the nitty-gritty, you might be writing Objective-C/Swift or Java/Kotlin and _that's_ what sends shivers down my back. I love learning! _But I also have limited time!_ 

The biggest question going into the workshop was, **how much could I try to share between my React web code and React Native code**? I think the answer is, not much, unless I changed my web code to use something like [React Native for Web](https://github.com/necolas/react-native-web). What I _could_ share are logic modules though but you couldn't share 3rd party React modules unless they _also_ targeted React Native.

One thing to consider about Xamarin in particular is that when using [Xamarin.Forms](https://docs.microsoft.com/en-us/xamarin/xamarin-forms/) it's C# and .NET up and down _the entire stack including the UI_. In talking with my friend who has done a lot of Xamarin development you don't get to ignore XCode or Android Studio but at the same time you don't need to go and learn 2-4 languages to write native modules.

At the end of the day, it'll probably come down to how complicated I think the KTOMG app would get (I'd say, not that complicated...). I think overall the fact is _building native mobile apps is hard_. Overall the React Native developer experience felt really productive to build out this app. That's an important factor for me, with limited time to spend wrestling with cross-platform tooling. 

The closest parallel I can draw for mobile development was my experience building my Gatsby-based [savings tracker app](https://reachfi.app). Since that is a Progressive Web App (PWA) using traditional React and Material UI, I _loved_ the experience. I had a fully-working app in a weekend. But the [deployment of PWAs to app stores](https://www.simicart.com/blog/pwa-app-stores/) is still in a sub-optimal state. Honestly, I hope to rewrite the KTOMG frontend to be a PWA anyway and maybe by the time I do that, PWAs will be first-class citizens on mobile platforms and my decision will be made for me. Always bet on the web... right?