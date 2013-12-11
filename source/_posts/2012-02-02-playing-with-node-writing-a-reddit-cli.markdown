---
layout: post
title: "Playing with Node: Writing a Reddit CLI"
date: 2012-02-02 15:21:04 -0600
comments: true
categories:
permalink: /blog/posts/30/playing-with-node-writing-a-reddit-cli
disqus_identifier: 30
---

This is not a complete post about how to write a fully featured CLI application for Reddit, like [Cortex](http://cortex.glacicle.org/). However, I think it demonstrates the ease and simplicity in making such an application.

### The idea

I love [Reddit](http://reddit.com). Except for one thing: I don't have much time to browse or keep up with it due to life getting in the way. What I wanted was a simple app I could run that would simply show me the top stories at that moment so I could glance at them and visit them.

I spent about 20 mins (mostly looking for CLI npm packages) and came up with this:

![Reddit CLI](/blog/images/35.png)

Pretty simple. It goes to the front page, grabs the JSON results, and formats it all into a nice list (shortening titles if they get too uppity), displaying the information I mostly care about: subreddit, score, # of comments, and the poster.

Now, I haven't yet implemented a way to actually visit the URL of any of these, mainly because I'm short on time. But maybe you can!

### The code

The code is pretty straightforward. I'm using the [colors](http://search.npmjs.org/#/colors), [request](http://search.npmjs.org/#/request), and [cli](http://search.npmjs.org/#/cli) npm packages.

I wanted to do something really quick. The `cli` package is way more than I need, in fact, I wrote this originally without using `cli` but for future improvement I added it in. The steps I wanted to perform from the get-go were:

1. Get the JSON for the homepage
2. Parse it and build a colorized "row" (two rows, in fact)
3. Trim titles so they didn't mess up the flow (my console window is 140 columns)

Here it is, all 43 lines of it:

```js
var cli     = require('cli'),
    http    = require('http'),
    request = require('request'),
    colors  = require('colors');

cli.main(function (args, options) {
  console.log("***********".rainbow);
  console.log("Node Reddit".cyan);
  console.log("***********\n\n".rainbow);

  request('http://reddit.com/.json', function (err, res, body) {
    if (!err && res.statusCode === 200) {
      var reddit  = JSON.parse(body),
          stories = reddit.data.children.map(function (s) { 
                      return s.data; 
                    });
      
      // Descending score
      stories.sort(function (a, b) { return b.score - a.score; });

      stories.forEach(function (story) {
        var row = "",
          title = story.title.length > 100
                ? story.title.substr(0, 100) + "..." 
                : story.title;

        // Build row
        // [score] [title] [comments] [subreddit]
        // This sucks
        row += story.score.toString().green + "\t";
        row += title.bold
        row += " (" + story.domain + ")";
        row += (" /r/" + story.subreddit).cyan;
        row += "\n\t";
        row += story.author.grey;     
        row += " " + (story.num_comments + " comments").italic.yellow;
        row += "\n";

        console.log(row);
      });
    }
  });
});
```

As you can see, definitely pretty light. That's all there is to it. Now imagine adding in commands (via `cli.parse`) or even [growl](http://search.npmjs.org/#/growl) support (I forked it and added Growl for Windows support, hopefully I can get it working decently and commit it). Then letting it monitor your specific subreddits, perhaps once every 15 minutes or so. Then adding in support for viewing links (interactive CLI app).

That's all more than I have time for, but maybe some of you that want to learn more about the Power of Node have more time. If so, I welcome additions.

It's [available on GitHub](https://github.com/kamranayub/node-reddit). Feel free to dissect it and improve it, fork it and b0rk it. Have fun!