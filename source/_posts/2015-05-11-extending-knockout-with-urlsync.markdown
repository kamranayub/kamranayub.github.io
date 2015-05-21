---
layout: post
title: "Extending Knockout Observables to Sync with Address Bar"
date: 2015-05-11 18:20:00 +0200
comments: true
categories:
- Knockout
- Javascript
- OSS
- Gists
---

I made a tiny Knockout extender called `urlSync` that syncs an observable with the URL.

<!-- More -->

For [Keep Track of My Games](http://keeptrackofmygames.com) I've been adding filtering to the game library. 
I'm really proud of what I have, here's what it looks like in the UI:

![Filtering UI](/blog/images/2015-05-11-filtering.png)

A modal allows the user to drill down and pick what filters to apply, the modal updates to show what filters are applied and what's available with the current resultset.

![Filtering Modal](/blog/images/2015-05-11-filtering-modal.png)

When you're outside the modal, I show any filters being applied allowing one-click removal or toggling the filtering method between AND/OR.

To make it easy to serialize/deserialize filters, I created a custom filtering expression that is pretty standard around town:

    {facetName}:{facetValue},{facetValue},...|...

For example, representing the screenshot above, the filtering expression would be:

    Lists:6|Status:8|My+Platforms:1

If you muck with the expression, there shouldn't be any issues. Facets are validated against whatever the original unfiltered resultset is, so you cannot add arbitrary expressions--if you do, they will not show up in the server response. Strings are special because potentially they can include characters used to parse the expression, the simplest way to deal with it is by encoding them and then decoding the value.

    Tags:my%20awesome%2ctag,tag2|Status:4

Now, I wanted to talk about the filtering expression because it makes it really easy to do a bunch of things:

1. Reading/writing from querystring or hash
2. Create a command textbox that can parse and autocomplete expressions
3. Allow the user to save "views" of their games

Items 2 and 3 might be on the agenda for some later date but item 1 is required for being able to create pre-filtered routes or create links. Since the library is loaded through Knockout and Web API, we need to be able to pass any filter or parameters when the page is loaded and also keep track of what the current values are for filtering.

To achieve this, I created a really simple [Knockout Extender](http://knockoutjs.com/documentation/extenders.html) called **urlSync.** All it does is initially load an observable from the hash (if found) or the querystring (if found). Then it observes the observable and keeps the `window.location.hash` updated.

This could easily be extended to use [HTML5 `pushState`](https://developer.mozilla.org/en-US/docs/Web/Guide/API/DOM/Manipulating_the_browser_history), if you wished. For now, managing the hash is "good enough" for my use case.

Here is a Gist of the extender:

<script src="https://gist.github.com/kamranayub/3feba45dd2da3262b872.js"></script>

It depends on [URI.js](http://medialize.github.io/URI.js) and the [URI.fragmentQuery](http://medialize.github.io/URI.js/docs.html#fragment-abuse) component. It also uses [Underscore.js](http://underscorejs.org), but that's just because I have it in my project already.

It's really simple to use, for example here's a snippet from my collection view model:


```
// Flattened facets
ViewModel.facets = ko.observableArray([]);

// Selected (applied) facets
ViewModel.selectedFacets = ko.observableArray([]).extend({
    urlSync: {
	    param: "filterBy",
	    read: function(value) {
	       return kt.utils.facetsFromString(value, ViewModel.facets);
	    },
	    write: kt.utils.facetsToString
    }
});

// Filter AND toggle
ViewModel.filterAnd = ko.observable(false).extend({ urlSync: "filterAnd" });
```

As you can see, the extender allows you to intercept read/write so you can perform any custom transformations (e.g. filtering objects to/from string). Because we're deserializing from a string and because the `checkedValue` binding is by reference, I pass in the existing facets collection to my utility, in order to preserve object references when required. On initial page load, this isn't required as the games haven't been fetched from the API yet. Once they are, my fetch method updates the observables. I could also modify the way I handle binding the checkboxes but this works.

The `urlSync` extender will then bind the observable values to the URL like this:

    users/kamran.ayub/lists/all#?
        sortBy=Name&
        filterAnd=true&
        filterBy=Status%3A8%7CLists%3A6%7CMy+Platforms%3A1

Since the extender can also fallback to reading from the querystring, we can create URLs that filter a user's collection:

    users/somebody/lists/all?filterBy=Platforms:2,5,40

Pretty neat and now allows anyone to copy/paste the URL and preserve any needed filtering state!