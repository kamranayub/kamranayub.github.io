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

![Folder structure](https://cloud.githubusercontent.com/assets/563819/12795770/3d24a9a8-ca81-11e5-95be-0d86c18eb293.png)

You've got a standard folder structure with a MVC controller and API controller. You want to leverage a client-side library to make it easier to have a dynamic and responsive interface, let's say [Knockout.js](http://knockoutjs.com). You start creating a Knockout view model and you want to bind it to your view. What do you do now at this point for binding the initial data to your view?

Do you...

A. Serialize the server model into JSON and pass it into your Knockout view model manually
B. Don't even bother and fetch the data via AJAX when the page loads

In either case, you're left with a realization: **I need to pass in my server model so I can use it in my client-side code.** You're left doing something like this:

```js
var TaskListViewModel = function (model) {
   var vm = {};
   
   vm.name = ko.observable(model.name);
   vm.tasks = ko.observableArray(model.tasks.map(function (t) { return TaskViewModel(t); });
   
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
    var vm = TaskListViewModel(tasks);
    
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
  created: Date;
}

interface Task {
  text: string;
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
namespace TypewriterBlogPost {
    $Classes(TypewriterBlogPost.Models.*)[
    /**
     * Interface for: $FullName
     */
    export interface $Name {
        $Properties[
        $name: $Type;]
    }]
}
```

The syntax of the template file is pretty straightforward, as [explained in the documentation](http://frhagn.github.io/Typewriter/pages/getting-started.html). Let's walk through it.

```
$Classes(TypewriterBlogPost.Models.*)[
```

The `Classes` keyword tells Typewriter to search for all public classes in a file. In parenthesis, you can filter classes by FullName using wildcard syntax. Typewriter also supports Lambda functions to filter by a predicate:

```
$Classes(x => x.FullName.Length > 50)[
```

The open square bracket indicates a repeated block of code of Typescript. We declare an interface since we want to add type safety, not an implementation (although you could, which you'll see next!). You can append another square pair for a separator string if there are > 1 items that match (i.e. multiple classes in a file, multiple properties, multiple methods).

Next, we list the properties using the same syntax. By the way, Typewriter has full Intellisense for all these keywords and variable names.

![Intellisense](https://cloud.githubusercontent.com/assets/563819/12796507/baabed5c-ca84-11e5-99bf-2079d85dabf0.png)

## Customize Knockout View Models

Now that we have our models reflected and auto-syncing with our client-side code, we can do some extra fun stuff to *automatically generate Knockout view models.*

The goal here is to auto-generate a base view model that we can then extend with custom methods, properties, and computed observables.

```js
${
    string KnockoutType(Property p) {
        if (p.Type.IsEnumerable) {
            return p.Type.Name.TrimEnd('[',']');
        }

        return p.Type;
    }

    string KnockoutValue(Property property) {
        var type = KnockoutType(property);

        if (IsEnumerableViewModel(property)) {
            return $"ko.observableArray<Knockout{type}>([])";
        } else if (property.Type.IsEnumerable) {
            return $"ko.observableArray<{type}>([])";
        }

        return $"ko.observable<{type}>()";
    }    

    bool IsEnumerableViewModel(Property p) {
        string type = KnockoutType(p);

        return p.Type.IsEnumerable && type.EndsWith("ViewModel");
    }
}
namespace TypewriterBlogPost {
    $Classes(*ViewModel)[ 
    /**
     * Interface for: $FullName
     */
    export interface $Name {
        $Properties[
        $name: $Type;]
    }

    /**
     * Knockout base view model for $FullName
     */
    export class Knockout$Name {        
        $Properties[
        public $name = $KnockoutValue;]

        constructor(model: $Name) {
            this.map(model);
        }

        /**
         * Map $Name model to Knockout view model
         */
        public map(model: $Name) {
            $Properties(x => !IsEnumerableViewModel(x))[
            this.$name(model.$name);]
            $Properties(x => IsEnumerableViewModel(x))[
            this.$name(model.$name.map(this.map$Name));]
        }

        $Properties(x => IsEnumerableViewModel(x))[
        /**
         * Map $KnockoutType equivalent Knockout view model. Override to customize.
         */
        public map$Name(model: $KnockoutType) {
            return new Knockout$KnockoutType(model);
        }]

        /**
         * Returns a plain JSON object with current model properties
         */
        public getModel() {
            return {
                $Properties(x => !IsEnumerableViewModel(x))[
                $name: this.$name(),]
                $Properties(x => IsEnumerableViewModel(x))[
                $name: this.$name().map(x => x.getModel())][,]
            }
        }
    }]
}
```

Oh man! This one's a doozy. All we're really doing is ensuring we cascade creating KO view models for collections that contain other view models. We also added a couple convenient helper methods like `getModel()` that returns a JSON object with the current KO model values. `map$Name` allows us to customize how we map each collection, for example, to override what view model to use (such as a custom view model).

Typewriter allows you to create "helper" functions that you can then use in the template. We created ones for parsing out the Knockout types (trimming square brackets)

Here's an example of what this template will generate for `TaskListViewModel`:

```js
namespace TypewriterBlogPost {
     
    /**
     * Interface for: TypewriterBlogPost.ViewModels.TaskListViewModel
     */
    export interface TaskListViewModel {
        
        id: number;
        name: string;
        author: string;
        created: Date;
        tasks: TaskViewModel[];
    }

    /**
     * Knockout base view model for TypewriterBlogPost.ViewModels.TaskListViewModel
     */
    export class KnockoutTaskListViewModel {        
        
        public id = ko.observable<number>();
        public name = ko.observable<string>();
        public author = ko.observable<string>();
        public created = ko.observable<Date>();
        public tasks = ko.observableArray<KnockoutTaskViewModel>([]);

        constructor(model: TaskListViewModel) {
            this.map(model);
        }

        /**
         * Map TaskListViewModel model to Knockout view model
         */
        public map(model: TaskListViewModel) {
            
            this.id(model.id);
            this.name(model.name);
            this.author(model.author);
            this.created(model.created);
            
            this.tasks(model.tasks.map(this.mapTasks));
        }

        
        /**
         * Map TaskViewModel equivalent Knockout view model. Override to customize.
         */
        public mapTasks(model: TaskViewModel) {
            return new KnockoutTaskViewModel(model);
        }

        /**
         * Returns a plain JSON object with current model properties
         */
        public getModel() {
            return {
                
                id: this.id(),
                name: this.name(),
                author: this.author(),
                created: this.created(),
                
                tasks: this.tasks().map(x => x.getModel())
            }
        }
    }
}
```

Awesome? You bet! So how would I use this in practice? I just extend the auto-generated code with my custom code!

## Strongly-typing your API controllers

Now that we've got our view models squared away
