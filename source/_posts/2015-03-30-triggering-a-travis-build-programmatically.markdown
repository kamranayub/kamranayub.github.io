---
layout: post
title: "Triggering a Travis Build Programmatically"
date: 2015-03-30 00:45:30 +0200
comments: true
categories:
  - CI
  - Tips
  - Travis-CI
  - Automation
  - Node.js
  - Javascript
---

For [Exalibur.js][1] we wanted to be able to keep our `master` branch documentation up-to-date on the website. The website is built using [Assemble.io](http://assemble.io) and GitHub pages and [after successfully automating my blog][2], naturally I turned to [Travis CI](http://travis-ci.org) to set up automated documentation generation.

<!-- More -->

Travis normally sets up a hook in your GitHub repository to automatically trigger a build after you push a commit (or pull request). This is real nice but unfortunately that's where it stops. If you want to trigger a build **for another repository**, you have to do it manually.

I asked Travis about this and their response is promising (Twitter can be nice sometimes):

<blockquote class="twitter-tweet" data-partner="tweetdeck"><p><a href="https://twitter.com/kamranayub">@kamranayub</a> it&#39;s on the roadmap, and hopefully landing soon &lt;3</p>&mdash; Travis CI (@travisci) <a href="https://twitter.com/travisci/status/582311038772723713">March 29, 2015</a></blockquote>
<script async src="//platform.twitter.com/widgets.js" charset="utf-8"></script>

Until that time, though, something has to be done.

One approach I saw was a [small Ruby script][7] to forge a webhook POST message. This is actually a nice idea but since Excalibur uses Node to build, I needed something else and didn't want to port it over.

Instead I borrowed some code from [@patrickketner][3] that uses the [node-travis-ci][4] npm package to submit a build through the public API. I modified Patrick's code since I need to use GitHub personal access tokens like I did [previously][2].

You still need to follow the same steps to set up the `GH_TOKEN` environment variable but once you do, all you have to do is execute this Node.js script (changing the `repo`).

<script src="https://gist.github.com/kamranayub/88f963a9ac3d5bf6114d.js"></script>

So for example, in your `.travis.yml` file, you just need:

```
install:
  - npm install travis-ci
after_success:
  - node trigger-build.js
```

All set. Now when you commit to one repository, you can trigger a build for another one. This allows us to automatically [keep our docs up-to-date][8] with whatever `master` has.

You can [reference Excalibur][5] to see how we execute the script. Additionally, you can reference [excaliburjs.com's Travis configuration][6] to see how we use [TypeDoc](http://typedoc.io) to generate documentation for the latest version of `master`.

[1]: http://excaliburjs.com
[2]: http://kamranicus.com/blog/2015/02/26/continuous-deployment-with-travis-ci/
[3]: https://github.com/patrickkettner/travis-ping
[4]: https://github.com/pwmckenna/node-travis-ci
[5]: https://github.com/excaliburjs/Excalibur/blob/master/deploy-docs.js
[6]: https://github.com/excaliburjs/excaliburjs.github.io/blob/site/.travis.yml
[7]: https://github.com/metaodi/travis-ping
[8]: http://excaliburjs.com/docs/api/edge