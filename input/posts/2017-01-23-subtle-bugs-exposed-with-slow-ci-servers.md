Title: Exposing Subtle Timing Bugs With a Slow CI Server
Published: 2017-01-23 20:44:00 -0600
Tags:
- Continuous Integration
- Excalibur.js
- Testing
Lead: Test timing- or animation- related code at lower CPU thresholds to expose possible timing bugs
---

We ran into a subtle issue with [Excalibur.js](http://excaliburjs.com) recently, as documented in [this pull request](https://github.com/excaliburjs/Excalibur/pull/740).

Turns out one of the CI systems we use to test on Windows, [Appveyor](http://appveyor.com), exposed a long-standing bug in the draw code because it's **slower** than the other Linux-based CI system we use, [Travis CI](http://travis-ci.org). I don't know whether that is something that Appveyor should be proud of--but it does showcase the fact that when testing asynchronous or animation-based libraries, testing on slower systems can reveal timing issues.

We are using [PhantomJS](http://phantomjs.org/) and [js-imagediff](https://github.com/HumbleSoftware/js-imagediff) to do visual integration testing. We discovered the bug when writing a test that attached to our "post draw" event handler and tried to ensure the HTML canvas matched an expected image--an awesome way to do visual integration tests. The problem was that we were seeing the image wasn't being *drawn* on the first frame, even though the engine had "loaded" all the textures.

```
log: uiactor postdraw, rgba:, 0, 0, 100, 255
```

This is logging the top-left pixel of the canvas and it's the default pixel color of our background--in other words, even after the engine **said** it was loaded, the first frame had nothing drawn! 

[The issue](https://github.com/excaliburjs/Excalibur/issues/748) turned out to be setting `Image.src` and not handling the `load` event to ensure we had loaded all the pixels before drawing (because setting `src` is asynchronous--something we *knew* yet forgot in this particular instance to account for). On fast systems, the pixels were loaded in-between calls to `requestAnimationFrame` and we never noticed it. On slow systems, the pixels could take 1-2 frames before the pixels were available, something only a slow system would exhibit. To fix the issue, we ended up realizing we didn't need to rely on a backing `Image` element and could simply use [`CanvasRenderingContext2D.drawImage`](https://developer.mozilla.org/en-US/docs/Web/API/CanvasRenderingContext2D/drawImage) to draw the *backing Canvas* context instead--more efficient and faster overall.

Thanks Appveyor!
