---
layout: post
title: "Easily consuming services in Windows 8 apps"
published: false
comments: true
categories:
permalink: /blog/posts/55/easily-consuming-services-in-windows-8-apps
disqus_identifier: 55
---

I've been dabbling a bit in Windows 8 application development, in the minimal amount of time I have. Moreso, I've been absorbing all the resources I can on the topic, going to dev camps around town, and generally just being a sponge. Like anything else, though, it helps even more when you just dive in and play.

If you can't think of what you could make for a Windows 8 app, here's an idea: create an app for a site that has an API. How many can you think of off the top of your head? I bet **StackOverflow** may have crossed your mind. If it hasn't, maybe you didn't realize there's [a bomb-ass API](http://api.stackexchange.com) available!

### The StackExchange Windows 8 App

I'm playing with creating a StackExchange Windows 8 app, even if I never plan to make it public. The reason is because the API is excellent and rich enough to create a fully-featured application. I also don't need to worry about a data source, as SE has it all covered including filtering, sorting, etc. It's much more about learning the UX and design of a Windows 8 application.

The source for this application is on GitHub, but for now it's going to remain private. That doesn't mean it won't be public at some point, just that I'm in the "playing around" stage.

### Basic scenario: show network of sites

Let's start out with a really basic requirement. The app, at least on startup, should probably display a list of sites we can get data from in the app, since that dictates how the rest of the experience proceeds. Whether or not this will *actually* be needed in the real-world application is questionable. After all, wouldn't it be nicer to default to StackOverflow (the most popular) and allow the user to switch sites anytime using that fancy header menu (and remember their choice for next time)?

Regardless, it's a good exercise and a good way to see how easy it is to grab data from a service and show it in a sexy way.

### Navigation template

One of the tips I've learned from attending the developer camps is that while the Grid and Split project templates are good to learn from, it is usually easier to start out with the Navigation template.

Create a new project using the JS Navigation template:

(image)

### Adding a data source

The next order of business is to create a data source, or at least some semblance of a data source, that we can develop against.

This post has a lot in common with [Quickstart: Adding a ListView](http://msdn.microsoft.com/en-us/library/windows/apps/hh465496) on the Dev Center, except we'll be changing the data source and the template a bit.

Add a new **stackexchange.js** file to the **js** folder:

```js
(function () {
  "use strict";

})();
```

This should start to become a familiar construct to you: creating a closure and self-executing function. *use strict* helps keep your Javascript code clean and less error-prone.

Next, let's define a namespace and an interface to expose:

```js
(function () {
  "use strict";

  WinJS.Namespace.define("StackExchange", {
    siteRepository: siteRepository
  });
})();
```

This will expose whatever we need (the second parameter) to outside consumers inside our app. In this case, we're going to expose a property called `sites` that points to our yet-to-be-created *sites* binding [List](http://msdn.microsoft.com/en-us/library/windows/apps/hh700774).


```js
(function () {
  "use strict";

  var siteList = new WinJS.Binding.List();
  
  // This executes async using a WinJS.Promise
  // When it's done, it updates the binding list
  // which is aware of any changes that happen.
  //
  // This is pretty similar to KnockoutJS.
  WinJS.xhr({
    url: 'http://api.stackexchange.com/2.1/sites'
  }).done(function (result) {
    var response = JSON.parse(result.responseText);

    if (response.items) {
        response.items.forEach(function (site) {
            siteList.push(site);
        });
    }
  });

  // Expose the namespace with public members
  WinJS.Namespace.define("StackExchange", {
    sites: siteList
  });

})();
```

To be honest, I definitely prefer jQuery's AJAX method to the WinJS equivalent but we'll play nice for now. What happens should be pretty obvious: we go out to StackExchange's API and [get a list of sites](http://api.stackexchange.com/docs/sites), then add them to the List.

### Implementing the UI

We will just be using a ListView control with a GridLayout to display these sites.

In your **pages/home/home.html** file, add a reference to your **stackexchange.js** data file (if you want this to be shared across all pages, add it to **default.html**):

```html
<!-- Using the UI Light theme -->
<link href="//Microsoft.WinJS.1.0/css/ui-light.css" rel="stylesheet" /> 

<!-- WinJS references -->
<script src="//Microsoft.WinJS.1.0/js/base.js"></script>
<script src="//Microsoft.WinJS.1.0/js/ui.js"></script>

<!-- App Global References -->
<script src="/js/stackexchange.js"></script>
```

Notice I also modified the reference to use the UI Light theme. I think it fits the theme of the app better.

Next, add a ListView control to the main section:

```html
<div id="sites-list-view" 
	data-win-control="WinJS.UI.ListView" 
 	data-win-options="{
		itemDataSource: StackExchange.sites.dataSource, 
		itemTemplate: select('#se-site-template'), 
		layout: { type: WinJS.UI.GridLayout }
 	}">
</div>
```

The cool thing about VS2012 and WinJS is that you get a ton of Intellisense help with these custom *data-win-* attributes. I was surprised to see it detected the *StackExchange* namespace we exposed and the *sites* member (and it's type!). Kudos VS team!

There are a few options we set:

* itemDataSource: this points to our List binding's dataSource property
* itemTemplate: this points to a template we'll define next by its selector
* layout: the type of layout we want to use, in this case a GridLayout

Next, at the top of **home.html** just underneath *body*, add the site item template:

```html
<div id="se-site-template" data-win-control="WinJS.Binding.Template">
  <div class="site-landing" data-win-bind="style.background: styling.link_color">
	<img data-win-bind="src: high_resolution_icon_url">
	<p class="win-type-xx-small" data-win-bind="innerText: name"></p>
  </div>
</div>
```

If you've used Knockout before, this should at least look a bit familiar. This template will be written for each item in our array. It mainly consists of a tile, icon, and label.

If you run the site right now, it's going to look a bit weird. We need to define styles to put the icing on the cake.

In **home.css*, add the following style rules above the media queries:

```css
/* overall ListView dimensions */
#sites-list-view {    
    height: 100%;
	width: calc(100% - 120px);
	z-index: 0;	
}
    /* individual item dimensions and grid placement */
    #sites-list-view .site-landing
    {
        width: 250px;
        height: 250px;
        overflow: hidden;
        display: -ms-grid;
        -ms-grid-columns: 1fr;
        -ms-grid-rows: 1fr;
    }

        /* image */
        #sites-list-view .site-landing img
        {
            width: auto;
            height: 100%;
            overflow: hidden;
        }

		#sites-list-view .site-landing p {
			-ms-grid-row-align: end;
			margin-left: 16px;			
		}       
```

This is based off of the [image grid template](http://msdn.microsoft.com/en-us/library/windows/apps/hh770116) from Dev Center but modified to suit our needs. Basically, we're display a tiled grid of sites that a user can clicktap on to view.

For added effect, I also bound the background of the tile to the link color in the *styling* property of the object.

If you run the app now, it looks pretty good!

![Metro Stack](/blog/images/74.png)

### So that was easy...

I was able to do this in a couple hours, which I thought was pretty impressive. Throughout that time, Blend became an essential part of my workflow. 