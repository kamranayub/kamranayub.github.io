Title: New Year, New Site Design Using Wyam Static Site Generator
Published: 2017-01-21 18:25 -0600
Lead: I've redesigned the site using the .NET-based Wyam static site generator
Tags:
- Design
- Wyam
- .NET
---

In preparation for the upcoming release of my new course, [Introduction to TypeScript][1] and because it's 2017, I decided it would be best to update my personal site with a more complete picture of myself.

<!-- More -->

When [Kamranicus began][2], I had created a custom MVC blog--complete with a control panel and web editor. This was overkill and I came to realize there was no real reason to have a *dynamic* site.

Three years ago I moved [onto Octopress][3] since Jekyll was and is still a popular static site generator. However, there were still some downsides to Octopress/Jekyll, namely:

- It requires a Ruby install and I'm on Windows, it's a pain
- It uses Liquid templates which I don't want to deal with
- Plugins are written in Ruby and I don't know Ruby (yet)

I have *no problem* with Ruby, I'd like to learn someday in fact. But today is not that day and I would rather like to be able to extend and customize my site with a tech stack I'm familiar with.

Could there be something better three years later?

## Wyam the .NET static site generator

I first heard about [Wyam][4] from [Scott Hanselman's blog post][11] and I was *very* interested--a site generator written in .NET that I can extend and customize! It is not just for blogs, it can generate static sites based on "Recipes" that you can write yourself and it uses a simple plug and play pipeline system that is fully customizable.

So finding this week to be the perfect opportunity after sending in my last work for my new course, I decided to jump right in and get this bad boy working.

I have to give huge props to [Dave Glick](https://daveaglick.com/), the creator. He was friendly and helpful as I was learning my way around, I even [contributed some bug fixes]([)https://github.com/Wyamio/Wyam/pull/397) to the themes.

## Migrating to Wyam

Migrating to Wyam from Octopress was not hard but it was a little tedious. The YAML front matter of the blog posts in Wyam are slightly different enough that I had to use some Powershell to do some file transformation (yay `Substring` and `Regex`!). For example, the Jekyll `date:` property needs to be `Published:`.

I also had to take care of redirects. In Jekyll, my posts were organized under `/blog/yyyy/mm/dd/post/` but in Wyam the default is simply `/posts/post`. You can enable using dates in the URL but frankly I liked the simpler URLs. You can use the `RedirectFrom:` front matter property to provide a path that Wyam will then generate a redirect for (using meta refresh). Sweet! Again, some simple Powershell allowed me to update all my old posts with the redirect URLs.

Finally, the old blog was using Disqus for commenting and it turns out the "identifier" Disqus was using was the *full URL to the post*. Not great. Luckily, any front matter properties are immediately available in the Razor templates, so I added a `disqus_identifier` property (hold over from Jekyll) and referenced it in my `_PostFooter.cshtml` template:

```html
<script type="text/javascript">
    var disqus_shortname = 'kamranicus';
    var disqus_identifier = '@(Model.String("disqus_identifier") ?? Model.FilePath(Keys.RelativeFilePath).FileNameWithoutExtension.FullPath)';
    var disqus_title = '@Model.String(BlogKeys.Title)';
    var disqus_url = '@Context.GetLink(Model, true)';

    // ... etc
</script>
```

Using the `Model.String` accessor, you can get at any declared document metadata. In the same fashion, `Context.String` accesses the global metadata declared in your `wyam.config`.

## Customizing Wyam

The default theme, CleanBlog, that is included with the [Wyam blog recipe](https://wyam.io/recipes/blog/overview) is pretty awesome--much better than the default themes in Jekyll or Octopress.

With my Octopress blog, I had always felt that I was doing myself a disservice using a pre-canned theme. I liked its simplicity but as a self-proclaimed "web developer", I felt I had to, you know, *design my own theme* and that's what you see now before you.

To customize the Wyam theme, it was very easy--you just copy the existing theme files to your **input** directory and customize them. The layouts are all Razor templates which was refreshingly familiar. Any non-underscore-prefixed `.cshtml` or `.md` files get made into pages. Everything was so simple! 

I ended up really liking the [Trianglify](http://qrohlf.com/trianglify/) JavaScript library so I kept that for the hero banners across the site. I removed some extra libraries I didn't need and upgraded the rest. I'm not 100% convinced I even need jQuery still since I'm not using any Semantic UI JavaScript plugins yet.

Having previously used [Semantic UI][5] on the [Excalibur.js][6] site redesign, I decided to use it again--I've been pretty happy with it over Bootstrap due to the extra flexibility and modular design. I ripped Bootstrap out of the default theme and replaced it with my own customized Semantic install.

I built the current design within two days and I'm pretty happy with how it turned out. I still intend to customize it further but for now it's "good enough" in time for the course release.

You can, of course, [see my entire site's source code][7] if you're interested. Getting the site to deploy via [AppVeyor](http://appveyor.com) was not too bad--just had some trial and error issues with the magic incantations to get my DOS commands to work.

## CloudFlare SSL

[CloudFlare][8] is amazing. I'm using it to provide SSL support for the site, since the site is hosted on GitHub Pages and they don't yet support SSL for custom domains. That's fine by me though, since CloudFlare is free and easy to set up.

Working in a "dev ops" role right now I've seen examples of how not using SSL can screw over sites. I'm a huge fan of SSL everywhere, even on static sites. I've already enabled [HSTS][9] via CloudFlare on [Keep Track of My Games][10] to ensure the entire site is served over SSL.

## Showcasing more of "me"

As you may have noticed at the top, I now have some new pages for the [projects I work on](/projects), [talks I've given](/speaking), and [links to my travel blogs](/travel).

This may all be things you didn't know about me--and I wouldn't blame you. Sure, I tend to mention things on Twitter but I have been wanting to have a place on the site where I can talk about and show the things I've been doing. I'm a busy guy!

I also include my social profiles at the bottom of every post and in the footer of the site so people know where to find me.

## Here's to a new year!

I'm excited for the next year. I'll be having my first child in a matter of weeks, the new course will be going live by the end of the month, and I'm exploring some fun new projects for summer and fall. 

Cheers!

[1]: https://www.packtpub.com/application-development/introduction-typescript-video
[2]: https://kamranicus.com/posts/2011-04-05-welcome-to-kamranicus-yaps
[3]: https://kamranicus.com/posts/2013-12-04-kamranicus-now-with-100-percent-more-octopress
[4]: https://wyam.io
[5]: http://semantic-ui.com
[6]: http://excaliburjs.com
[7]: https://github.com/kamranayub/kamranayub.github.io
[8]: http://cloudflare.com
[9]: https://blog.cloudflare.com/enforce-web-policy-with-hypertext-strict-transport-security-hsts/
[10]: http://ktomg.com
[11]: http://www.hanselman.com/blog/ExploringWyamANETStaticSiteContentGenerator.aspx