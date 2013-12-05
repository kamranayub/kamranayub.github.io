---
layout: post
title: "A Smarter `checked` Binding for KnockoutJS"
date: 2012-12-31 09:48:04 -0600
comments: true
categories:
permalink: /blog/posts/61/a-smarter-checked-binding-for-knockoutjs
---

I love KnockoutJS. I've been using it for awhile now and I really enjoy it. That said, there are a few oddities with it, one such being the `checked` binding.

Let's say you have this scenario (as I do now):

You want to `foreach` a list of items, each one having a checkbox. You want to store the checked items in a list, for use throughout the view model (bulk updates, select all, deselect all, etc.). How do you go about doing this?

The "recommended" approach would be to add an observable property, `isSelected`, to each of your items. Then, in your markup, you can bind to that property:

```html
<ul data-bind="foreach: tasks">
  <li><input type="checkbox" data-bind="checked: isSelected"></li>
</ul>
```

That is OK. It's OK because what if you need to use that item (assuming you're using a "class" or other reusable object) somewhere else where you don't need that property. You could just attach it where you need it. It also makes it a bit tricky to manage the list of selected items (you'd probably end up creating a `computed` array), as well as making it difficult to dynamically bind a checkbox list to a computed array.

What you may decide to do (like me) is to try and bind the `checked` to your own observable array, and assign the `value` of the checkbox to a model property like this:

```html
<input type="checkbox" data-bind="checked: $root.selectedTaskIds, value: id">
```

However, if you do this, you'll soon discover a few oddities:

1. When you check the checkbox, the *string* value of your model property will be placed into the array.
2. Your model property *will change to this new string value* essentially changing your original model.

There are reasons for these two crazy happenings, which I discovered:

1. The `checked` binding bases its value on the *element's* value, which is stored as a string.
2. The `value` binding keeps the property you are bound to and the *element's value* in-sync, which is why your model's ID becomes a string.

It turns out, we're suffering from some mental model breakdown. If you look at the KO documentation for `checked`, it's easy to believe the above syntax should work, after all, it says: 

> Special consideration is given if your parameter resolves to an array. In this case, KO will set the element to be checked if the **value** matches an item in the array, and unchecked if it is not contained in the array.

If the *value* matches... "Cool!" so I can just bind the `value:` to my model property? NOPE. You'll run into the aforementioned issues if you try.

### Making a smarter `checked` binding

Since I didn't want to add `isSelected` properties everywhere, I decided to "monkey patch" the `checked` and `value` bindings to play nice together.

Essentially, all my patch does is two things:

1. In the `checked` binding, if it finds a `value` binding, it binds to that value rather than the element's value.
2. In the `value` binding, it completely ignores any changes if the `checked` binding is present, essentially nullifying the binding.

Here are the new binding definitions:

<script src="https://gist.github.com/4418574.js"></script>

And you can see it in action here:

<iframe style="width: 100%; height: 300px" src="http://jsfiddle.net/kamranayub/G8YZU/embedded/" allowfullscreen="allowfullscreen" frameborder="0"></iframe>

As you can see, now it works as you'd expect! You can bind the `id` property to the list. You can also bind `$data` and you'll have the full object in your selected list.