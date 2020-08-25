Title: "New Course: Testing Progressive Web Apps on Pluralsight"
Published: false
Lead: Learn how to handle service workers, offline support, event simulation, and mocking native browser APIs in my Testing Progressive Web Apps course
Tags:
- Courses
- Cypress
- Testing
---

Today my new course [Testing Progressive Web Apps](https://bit.ly/PSPWATesting) has been published and I'm pretty excited for it (like usual!).

![course overview](https://user-images.githubusercontent.com/563819/90946059-f7f56e80-e3ee-11ea-8f47-3d57e091e600.png)

## What I Learned

I learned quite a bit while developing this course because in order to test a PWA, you need to *have* a PWA in the first place!

For the course I developed the [Carved Rock Fitness Order Tracker](https://bit.ly/PSPWATestingSample) which is an open source sample application for demo purposes.

![Course sample demo app](https://user-images.githubusercontent.com/563819/90946131-9c77b080-e3ef-11ea-8fe7-7198ce0fab0f.png)

The is a fully-developed PWA which features:

- Offline support
- Service worker caching
- Local notifications
- Responsive design
- Cypress and WebDriverIO tests
- Local and BrowserStack-powered tests

The course uses this app as the basis of walking through how to handle all the various scenarios you'd encounter when testing a PWA like how to bypass service worker caching, notification permissions, and more.

Since Cypress didn't offer a built-in way to control browser permissions in a standard way, I also released the [cypress-browser-permissions](https://github.com/kamranayub/cypress-browser-permissions) which lets you manage browser permissions in a standard way for Chromium (Google Chrome, MS Edge) and Firefox.

The sample was built using [Ionic Framework](https://ionicframework.com/) and it was my first time using the framework to build something (there's no better way to learn, right?). I was *extremely impressed* with how easy it was to build using the framework and all the features provided out-of-the-box. I think there's even more that could be done using [Capacitor](https://capacitorjs.com/), which I'm not currently using, to provide abstractions over some of the native APIs like notifications, geolocation, etc.

## How Much Work Was It?

This was the first time I used [Clockify](https://clockify.me/) in earnest to track the various aspects of the course production. This meant I could track each checklist item (which is a bit too granular) but using tags I can group activities like content creation, coding, recording, and editing together.

In total, I spent about 150 hours give or take (I didn't track assessment question quite as much) with a monthly breakdown like this:

![Time spent by month](https://user-images.githubusercontent.com/563819/91182463-d080f900-e6af-11ea-85ad-80ac16be1aa7.png)

More interesting perhaps is the breakdown of time spent on different aspects of course development:

![Breakdown of course development](https://user-images.githubusercontent.com/563819/91182905-67e64c00-e6b0-11ea-91cb-8db2bea7fcab.png)

![Chart of ti](https://user-images.githubusercontent.com/563819/91182923-6f0d5a00-e6b0-11ea-9bc3-826d98e103cb.png)
<!--stackedit_data:
eyJoaXN0b3J5IjpbLTE4NDY4MTM3MjcsLTE2MjU1NDY3NTUsMT
Q5MzIyNTkyOV19
-->