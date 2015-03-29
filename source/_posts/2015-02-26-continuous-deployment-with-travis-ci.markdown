---
layout: post
title: "Continuous Deployment With Travis-CI"
date: 2015-02-26 17:18:22 +0100
comments: true
categories:
  - CI
  - Tips
  - Automation
  - Travis-CI
---

One of the fun things we did for [Sweep Stacks][1] (our Ludum Dare entry) 
during development was to setup continuous deployment. Since our GitHub
repository was public, we could leverage the excellent Travis CI build tool.

This allowed us to work on the game and have our dedicated tester play every new build
hot off the press. It reduced the amount of bugs we shipped with and created a very
fast feedback loop for QA. In the end it allowed us to push a polished game an hour
or so before the deadline hit. After the voting began, it also allowed us to quickly
push bug fixes that were reported by players.

If you're new to Travis CI, you can learn more about what it is and 
how to get started by reading [my Tech.pro tutorial][2].

Once you have your project building, you can then write a little bash script to
deploy it! By "deploying", what I really mean is pushing your built code to your production branch on GitHub for final deployment. For example, we use GitHub pages to host Sweep Stacks.

Let's look at the process.

## Customizing Travis Config

You will need to execute a script to deploy your site, so you will need to
customize your **_travis.yml** file.

Here is what mine looks like for this blog (built using Octopress):

	language: ruby
	rvm:
	  - 1.9.3
	branches:
	  only:
	    - source
	env:
	  global:
	  - GH_REF: github.com/kamranayub/kamranayub.github.io.git
	script:
	  - bundle exec rake generate
	  - chmod ugo+x deploy.sh
	  - '[ "${TRAVIS_PULL_REQUEST}" != "false" ] || ./deploy.sh'

Let's break it down:

    language: ruby

Easy, since I use [Octopress][4] (built on Jekyll) for my blog, we need to use Ruby.

    rvm:
	  - 1.9.3

The version of Octopress I use requires Ruby 1.9.3.

    branches:
	  only:
	    - source

We only want Travis to initiate builds for the `source` branch, since that is where
the source files are for my blog.

	env:
	  global:
	  - GH_REF: github.com/kamranayub/kamranayub.github.io.git

We will need to be able to clone and push to our repository, so we store it in an
environment variable for easy access. `GH_REF` will be available to our bash script.

	script:
	  - bundle exec rake generate
	  - chmod ugo+x deploy.sh
	  - '[ "${TRAVIS_PULL_REQUEST}" != "false" ] || ./deploy.sh''

In our Travis script, we build the blog (`rake generate`). We then mark our `deploy.sh` file
as executable (since I'm on Windows). After that we execute our deploy as long as this isn't
a pull request (don't want to build other people's changes!). Disabling pull request builds 
can also be set in your Travis project settings.

## Deploy script

Here's the script I use to do the deployment:

<script src="https://gist.github.com/kamranayub/ca7b6866ab43771d9da8.js"></script>

As you can see it's fairly simple. One thing to make sure of is hiding your access token (`GH_TOKEN`),
we use to authenticate which I'll show you how to generate and use.

## The access token

The whole reason this works is because we are authenticating to GitHub using a *Personal
Access Token* (stored in `GH_TOKEN`). This is an OAuth token that you can create when
using Two-Factor Auth to authenticate 3rd party tools like Visual Studio, or in our case, authenticating in a script.

You can generate a token by:

1. Going to [Applications][3] in your Settings
2. Clicking "Generate New Token"
3. Selecting your permissions (for Travis, all you need is `public_repo`)
4. Once created, copy the token to your clipboard (you *must* regenerate it if you lose it)

Now we need to configure Travis to expose this token securely to our script.

1. On Travis, go to the Settings of your project (top-right)
2. Click the "Environment Variables" tab (and ensure Build Pull Requests is 'Off')
3. Create a new environment variable called `GH_TOKEN`
4. In the Value field, paste in your token, being sure to remove any trailing whitespace
5. Ensure "Display value in build logs" is Off

Once created, you're all set! `GH_TOKEN` will now be available to your **deploy.sh** script.

## Testing it out

You should be ready to test your deployment. Commit any changes you have to your repository
and you should start seeing the build output once it starts. If there are any errors, now
you can fix them until your build passes.

Happy deploying!

[1]: http://playsweepstacks.com
[2]: http://tech.pro/tutorial/1749/get-your-ci-on-with-travis-ci
[3]: https://github.com/settings/applications
[4]: http://octopress.org