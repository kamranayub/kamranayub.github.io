---
layout: post
title: "Using Azure CDN Origin Pull With Cassette"
date: 2015-10-09 19:30:00 -0500
comments: true
published: true
categories:
- Azure
- .NET
- C#
- Keep Track of My Games
---

For the October update for [Keep Track of My Games](http://keeptrackofmygames.com) I wanted to offload my web assets to a CDN. Since I'm already using [Microsoft Azure](http://azure.com) to host the site, I decided to use [Azure CDN](https://azure.microsoft.com/en-us/services/cdn/).

I set it up for "Origin Pull" which means that instead of uploading my assets to the CDN (Azure Blob storage), you request a file from the CDN and Azure will go and get it from your website and then cache it on their servers.

So as an example:

```
User requests http://az888888.vo.msecnd.net/stylesheets/foo.png
|
|
CDN: have I cached "stylesheets/foo.png?"
  Yes: Serve content from edge cache (closest to user)
  No: Request http://yourwebsites.com/stylesheets/foo.png and serve
```

You can read more about [how to set up origin pull in Azure CDN](https://azure.microsoft.com/en-us/documentation/articles/cdn-create-new-endpoint/). In my case, I used "Custom Origin" of "http://keeptrackofmygames.com".

## Using CDN with Cassette

I use the .NET library [Cassette](http://getcassette.com) for bundling & minification for KTOMG--when I started KTOMG there was no Microsoft provided option and Cassette has been really stable.

It works pretty much as you'd expect:

* Define "bundles" which are sets of scripts/stylesheets
* Render bundles onto page(s)
* If debug mode, render individually otherwise minify and concatenate

By default, Cassette will render URLs like this in your source code:

In debug mode:

```
Bundle: ~/Content/core

- /cassette.axd/asset/Content/bootstrap.css?hash
- /cassette.axd/asset/Content/site.css?hash
- /cassette.axd/asset/Content/app.css?hash
```

And in production:

```
/cassette.axd/stylesheet/{hash}/Content/core
```

But if we want to serve assets over the CDN, we need to plug in our special CDN URL prefix--not only for script/stylesheet references but also references to images *in* those files.

Luckily, Cassette provides a facility to modify generated URLs by letting you register a `IUrlGenerator`. Here's my full implementation of this for my CDN:

<script src="https://gist.github.com/kamranayub/2da4ccfec3e7812c8367.js"></script>

As you can see, I register a custom `IUrlGenerator` and a custom `IUrlModifier`. The default `IUrlModifider` is Cassette's `VirtualDirectoryPrepender` and it just prepends "/" to the beginning of every URL but in our case we want to conditionally prepend the Azure CDN endpoint in production. 

In production, this will produce the following output:

```
https://az99999.vo.msecnd.net/cassette.axd/stylesheet/{hash}/Content/core
```

To allow local debugging and CDN in production I just use an app setting in the web.config. In Azure, I also add an application setting (`CdnUrl`) through the portal in my production slot with the correct CDN URL and voila--all my assets will now be served over CDN.

### Notes

- Azure CDN does not yet support HTTPS for custom origin domains. So if you want to serve content over http://static.yoursite.com you can't serve it over HTTPS because Azure doesn't allow you to upload or set a SSL certificate to use and insteads uses their own certificate which is not valid for your domain. [Vote up the UserVoice issue](http://feedback.azure.com/forums/169397-cdn/suggestions/1332683-allow-https-for-custom-cdn-domain-names) on this.

- Azure CDN origin pull does not seem to respect `Cache-Control: private` HTTP header. For example, by default MVC serves pages with private cache control which means browsers won't cache that page and neither *should* Azure CDN--but it does anyway. In my case, I really don't want a true mirror of my site, I just wants assets served over CDN and Cassette sets `Cache-Control: public` on them automatically. [You can upvote my feature request on UserVoice](http://feedback.azure.com/forums/169397-cdn/suggestions/10148280-respect-cache-control-private-for-origin-pull).

- I am choosing **not** to point my entire domain to the CDN. Some folks choose to serve their entire site over the CDN which is definitely something you can do. However, in my case, I didn't want to do that. If you instead chose to point your domain to the CDN endpoint, you don't need to do any of this--**everything** will be served over the CDN.
