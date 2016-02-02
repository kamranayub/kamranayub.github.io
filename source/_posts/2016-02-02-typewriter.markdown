---
layout: post
title: "Using Typewriter to Strongly-Type Your Client-Side Models and Services"
date: 2016-02-02 07:00:00 -0600
comments: true
published: false
categories:
- Javascript
- Typescript
- Typewriter
- C#
- Knockout.js
---

I've recently discovered [Typewriter for Visual Studio](http://frhagn.github.io/Typewriter/index.html) and since then I've been using it in all my projects, at work and at home. It's just **so** good. Let me explain what Typewriter does and why it's so awesome.

## The setting

It's 2016. The web app you're working on is a mix of Javascript, C#, and controllers for MVC or Web API. Your solution looks something like this:

![]()

You've got a standard folder structure with a MVC controller and API controller. You want to leverage a client-side library to make it easier to have a dynamic and responsive interface, let's say [Knockout.js](http://knockoutjs.com). You start creating a Knockout view model and you want to bind it to your view. What do you do now at this point for binding the initial data to your view?

Do you...

A. Serialize the server model into JSON and pass it into your Knockout view model manually
B. Don't even bother and fetch the data via AJAX when the page loads

In either case, you're left with a realization: **I need to pass in my server model so I can use it in my client-side code.** You're left doing something like this:

```js
var MyViewModel = function (model) {
   var vm = {};
   
   vm.someProp1 = ko.observable(model.someProp1);
   vm.otherProp2 = ko.observableArray(model.otherProp2);
   
   return vm;
};
```

And then passing in your server model, serialized from JSON either via AJAX or embedded in the view:

```html
<script>
window.model = @Html.Raw(JsonConvert.SerializeObject(Model));

// or

$(function () {
  $.getJSON('/api/tasks', function (tasks) {
    var vm = MyViewModel(tasks);
    
    ko.applyBindings(vm);
  });
</script>
```

We've all done something like this because no matter what approach you choose, you have to map the models *somewhere*. You could use a mapping library like ko.mapping to help. But even with help, you still have the same problem:

> What happens when you change your model in C#?

The answer is, "I have to go and update all the references in my client-side Javascript." So what do we do? We try to leave it as much alone as we can, preferring not to change things so we can avoid Happy JS Refactoring Funtime.

## Enter Typescript, stage left

We can address one aspect of this problem using [Typescript](http://typescriptlang.org). My preferences for Typescript are [well-documented](http://kamranicus.com/presentations/demystifying-typescript). Here's one reason why: we can create interfaces that strongly-type our C# models.

```js
interface TaskList {
  tasks: Task[];
  name: string;
  author: string;
}

interface Task {
  todo: string;
  done: boolean;
  created: Date;
  modified: Date;
}
```

Now I've created an interface that mirrors my serialized C# model representation. So now with Typescript, **anytime** I use a server-side model, I can ensure I never have any problems with misspellings/refactoring or type changes (e.g. "author" changing from a string to a `User` model). At compile-time, Typescript will ensure my references are correct.

But we still have one problem: how can we avoid the headaches when our server model changes? We *still* need to update our TS models even though they're at least in one place.

## Enter Typewriter, stage right

Typewriter is a Visual Studio extension that does one thing and does it well: it lets you create a **Typescript Template** files. These are *basically* T4 templates but they're abstracted to the point where it's actually *easy* to use (sorry T4). When you save your C# files, Typewriter reflects over them and will run the template and generate corresponding Typescript files. This lets you do simple things like mirror types to crazy things like... generate an entire AJAX web service.

So, using Typewriter, what would the template file look like to mirror our models?

```
${
}
namespace MySite {
    $Classes(MyApp.Models.*)[
    export interface $Name {
		    // Interface for: $FullName

        $Properties[
        $name: $Type;]
    }]	
}
```

The syntax of the template file is pretty straightforward, as [explained in the documentation](http://frhagn.github.io/Typewriter/pages/getting-started.html).
