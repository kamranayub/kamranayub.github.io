---
layout: post
title: "UnderscoreKO - Want some lumps of Underscore.js sugar in your Knockout java?"
date: 2012-03-13 14:33:19 -0500
comments: true
categories:
permalink: /blog/posts/33/underscoreko-want-some-lumps-of-underscorejs-sugar
---

I've been using [Knockout](http://knockoutjs.com/) at work for a MVC3 project and I have also been using [Backbone.js](http://documentcloud.github.com/backbone/) in some personal projects at home. I really love [Underscore](http://documentcloud.github.com/underscore/) and the array manipulation it offers for Node and the browser. I also liked how it was integrated into Backbone collections.

Yesterday evening as I drove home from work, I had a crazy idea. An idea so simple, I was really surprised [only one other person](https://github.com/thelinuxlich/knockout.underscore.plugin) (that a 1 second Google search turned up) made an attempt to do. Maybe *because* it's so simple, no one bothered to make their code public.

Can you guess what my idea was? C'mon, isn't it obvious? *Why not marry Underscore's array methods to Knockout's observable arrays?* And **BAM**, in one evening, **a tiny library named [UnderscoreKO](https://github.com/kamranayub/UnderscoreKO) was born.** It's really very cute. It took me maybe 20 minutes to write the library and then 3 hours to write the tests and package it up. Go figure.

## Get UnderscoreKO

You can download the [UnderscoreKO package off of Nuget](http://nuget.org/packages/UnderscoreKO), since I know that's how a lot of you roll.

The source is [also available on GitHub](https://github.com/kamranayub/UnderscoreKO).

## Let's see some code

First, let's see what life was like before UKO:

```js
vm.computedArray = ko.computed(function () {
    // marry the two arrays
    return _.union(this.someArray(), [0, 1, 2, 3, 4]);
}, vm);
```

So what problem have I "solved"? A small one, but I hope small enough that you'll miss it when you can't do it (those are the best, right?):

```js
vm.computedArray = ko.computed(function () {
    // marry the two arrays
    return this.someArray.union([0, 1, 2, 3, 4]);
}, vm);
```

Do you see it? Here, let me zoom in:

```js
return this.someArray.union([0, 1, 2, 3, 4]);
```

That's right! All of Underscore's array and collection methods are now available off of observable arrays! You didn't even know you wanted this, right?

## Mutators

[thelinuxlich](https://github.com/thelinuxlich/knockout.underscore.plugin) had a neat feature in her plugin which was the idea of "destructive" methods that were basically convenience methods to manipulate the observable array directly.

In other words, this:

```js
vm.myObservableArray(vm.myObservableArray.union([0, 1, 2, 3]));
```

Becomes this:

```js
vm.myObservableArray.union_([0, 1, 2, 3]);
```

See? Isn't that a little nicer? I added "mutator" methods (*methodName\_*) for anything I felt you'd want to use to manipulate the array. And of course, they still use Knockout underneath so change notifications still occur.

## Features

Um... what you'd expect? <strike>Actually, there are a couple issues I'd like to address tonight in another update (such as adding `chain` which I forgot about and taking care of passing in `observableArrays` to some functions like `union` so you don't have to do this: `vm.myObservableArray.union_(vm.array1(), vm.array2(), ...)`).</strike> I've released v1.1 which fixes a mutator bug, adds `chain()` support, and invokes KO arrays when passed to certain functions like *union*.

## Live Demo

Want to try before you buy? Go ahead!

<iframe style="width: 100%; height: 300px" src="http://jsfiddle.net/kamranayub/exnqe/embedded/" allowfullscreen="allowfullscreen" frameborder="0"></iframe>