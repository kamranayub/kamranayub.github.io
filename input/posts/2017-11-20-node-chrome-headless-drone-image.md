Title: Here's a Drone Docker image with Node.js and Chrome headless
Published: 2017-11-20 15:52:00 -0500
Tags:
- Drone CI
- Node.js
- Puppeteer
- Chrome
- Testing
- Docker
---

I worked on a fun little aside this last week at work. We are using [Drone](https://github.com/drone/drone) to perform our CI builds (it's just like [Travis CI](http://travis-ci.org)). Well, the default Docker `node` image is built on Debian and when run on Drone 5 it does not contain all the packages needed to use Chrome Headless (as [documented in this GH issue](https://github.com/Googlechrome/puppeteer/issues/290)). We're using Puppeteer to run Chrome headless with Karma to run our Angular tests.

We were running a bash script to install them when the build started but I went ahead and just packaged it all as a Docker container. This is a general purpose container image but using it with Drone sped up our builds by an order of magnitude.

You can find it in [my Drone image repository](https://github.com/kamranayub/drone-images). I'll be sure to add anymore useful Drone images I end up making there too!

With Puppeteer, I also had to use these args:

    --no-sandbox --disable-setuid-sandbox
    
It's possible you don't need them but once I got it working I didn't revisit it in depth.

I could also publish this to the public registry, just let me know in the comments if you'd like me to--otherwise, feel free to do so.
