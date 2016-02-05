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

<!-- more -->

## The setting

It's 2016. The web app you're working on is a mix of Javascript, C#, and controllers for MVC or Web API. Your solution looks something like this:

![Folder structure](https://cloud.githubusercontent.com/assets/563819/12835031/1f2c4cfc-cb72-11e5-8f99-d6b3a4af3e83.png)

You've got a standard folder structure with a MVC controller and API controller. You want to leverage a client-side library to make it easier to have a dynamic and responsive interface, let's say [Knockout.js](http://knockoutjs.com). You start creating a Knockout view model and you want to bind it to your view. What do you do now at this point for binding the initial data to your view?

Do you...

1. Serialize the server model into JSON and pass it into your Knockout view model manually
2. Don't even bother and fetch the data via AJAX when the page loads

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

We've all done something like this because no matter what approach you choose, you have to map the models *somewhere*. You could use a mapping library like [ko.mapping](http://knockoutjs.com/documentation/plugins-mapping.html) to help. But even with help, you still have the same problem:

> What happens when you change your model in C#?

The answer is, "I have to go and update all the references in my client-side Javascript." So what do we do? We try to leave it as much alone as we can, preferring not to change things so we can avoid Happy JS Refactoring Funtime.

## Enter Typescript, stage left

We can address one aspect of this problem using [Typescript](http://typescriptlang.org). My preferences for Typescript are [well-documented](http://kamranicus.com/presentations/demystifying-typescript). Here's one reason why: we can create interfaces that strongly-type our C# models.

```js
interface TaskListViewModel {
  id: number;
  name: string;
  author: string;
  created: Date;
  tasks: TaskViewModel[];
}

interface TaskViewModel {
   order: number;
   canMarkDone: boolean;
   task: Task;
}

interface Task {
  text: string;
  done: boolean;
  created: Date;
  modified: Date;
}
```

Now I've created an interface that mirrors my serialized C# model representation. So now with Typescript, **anytime** I use a server-side model, I can ensure I never have any problems with misspellings/refactoring or type changes (e.g. "author" changing from a string to a `User` model). At compile-time, Typescript will ensure my references are correct.

Using type information, we can strongly type our previous JS view model:

```js
var TaskListViewModel = function (model: TaskListViewModel) {
   var vm = {};
   
   vm.name = ko.observable<string>(model.name);
   vm.tasks = ko.observableArray<TaskViewModel>(model.tasks.map(function (t) { return TaskViewModel(t); });
   
   return vm;
};
```

But we still have one problem: how can we avoid the headaches when our server model changes? We *still* need to update our TS models manually.

## Enter Typewriter, stage right

[Typewriter](http://frhagn.github.io/Typewriter/index.html) is a Visual Studio extension that does one thing and does it well: it lets you create a **Typescript Template** files. These are *basically* T4 templates but they're abstracted to the point where it's actually *easy* to use (sorry T4). When you save your C# files, Typewriter reflects over them and will run the template and generate corresponding Typescript files. This lets you do simple things like mirror types to crazy things like... generate an entire AJAX web service.

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

Oh man! This one's a doozy. All we're really doing is ensuring we cascade-map KO view models for collections that contain other view models. We also added a couple convenient helper methods like `getModel()` that returns a JSON object with the current KO model values. `map$Name` allows us to customize how we map each collection, for example, to override what view model to use (such as a custom view model).

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

Awesome? You bet! So how would I use this in practice? I would just `extend` the auto-generated code with my custom code!

```js
namespace TypewriterBlogPost {

    export class ViewModel extends KnockoutTaskListViewModel {

        constructor(model: TaskListViewModel) {
            super(model);
        }

        addTask() {
            // todo call service
        }
    }

    // apply KO bindings and use JSON object from server
    $(() => ko.applyBindings(new ViewModel((<any>window).viewModel)));
}
```

## Strongly-typing your API controllers

Now that we've got our view models squared away, how can we leverage Typewriter to help us with our Web API methods? Well, Typewriter comes with an awesome Web API extension that makes it easy to generate strongly-typed service classes.

```js
${
    using Typewriter.Extensions.WebApi;

    string ReturnType(Method m) => m.Type.Name == "IHttpActionResult" ? "void" : m.Type.Name;
    string ServiceName(Class c) => c.Name.Replace("Controller", "Service");
    string ParentServiceName(Method m) => ServiceName((Class)m.Parent);
}

module TypewriterBlogPost {
    $Classes(:ApiController)[
    export class $ServiceName {
        $Methods[

        // $HttpMethod: $Url
        public static Route$Name = ($Parameters(p => p.Type.IsPrimitive)[$name: $Type][, ]) => `$Url`;
        public static $name($Parameters[$name: $Type][, ]): JQueryPromise<$ReturnType> {
            return $.ajax({
                url: $ParentServiceName.Route$Name($Parameters(p => p.Type.IsPrimitive)[$name][, ]),
                type: '$HttpMethod',
                data: $RequestData
            });
        }]
    }]
}
```

So, let's break it down:

1. Include the WebApi extensions
2. Create some helper methods to rename the controllers and provide the right return type
3. For all classes that inherit `ApiController`
   1. Create a service class
   2. For each method:
      1. Create a route helper function that returns a URL formatted with the right parameters
      2. Create a JQuery AJAX call that sends a request to the right URL and includes the right request information

The `TasksController` we have defined looks like this:

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TypewriterBlogPost.Models;
using TypewriterBlogPost.ViewModels;

namespace TypewriterBlogPost.Controllers
{
    public class TasksController : ApiController
    {
        private static IList<TaskListViewModel> _taskLists = new List<TaskListViewModel>()
        {
            new TaskListViewModel()
            {
                Name = "Todos",
                Author = "Kamranicus",
                Created = DateTime.Now,
                Id = 1,
                Tasks =
                {
                    new TaskViewModel() { Task = new Task() { Text = "Get milk from store" } },
                    new TaskViewModel() { Task = new Task() { Text = "Get deli meat", Done = true  } }
                }
            }
        };

        public IEnumerable<TaskListViewModel> GetAll()
        {
            return _taskLists;
        }

        public TaskListViewModel GetById(int id)
        {
            return _taskLists.First(t => t.Id == id);
        }

        public void Post(int id, Task task)
        {
            var t = GetById(id);

            t.Tasks.Add(new TaskViewModel() { Task = task });        
        }
    }
}
```

A few things to note:

1. To avoid name collisions, I use `getAll` and `getById`
2. To use with Typewriter, I return simple types--using `HttpResponseMessage` won't allow you to strongly-type the service. However you can still be flexible with errors by throwing `HttpExceptions` and Web API will serialize your response.

What gets generated is what you'd expect:

```js
module TypewriterBlogPost {
    
    export class TasksService {
        

        // get: api/tasks/
        public static RouteGetAll = () => `api/tasks/`;
        public static getAll(): JQueryPromise<TaskListViewModel[]> {
            return $.ajax({
                url: TasksService.RouteGetAll(),
                type: 'get',
                data: null
            });
        }

        // get: api/tasks/${id}
        public static RouteGetById = (id: number) => `api/tasks/${id}`;
        public static getById(id: number): JQueryPromise<TaskListViewModel> {
            return $.ajax({
                url: TasksService.RouteGetById(id),
                type: 'get',
                data: null
            });
        }

        // post: api/tasks/${id}
        public static RoutePost = (id: number) => `api/tasks/${id}`;
        public static post(id: number, task: Task): JQueryPromise<void> {
            return $.ajax({
                url: TasksService.RoutePost(id),
                type: 'post',
                data: task
            });
        }
    }
}
```

Man, *how sexy is that?* Not only have we ensured our models and view models stay in-sync, our API is also reflected on the client-side so we don't need to worry about hard-coding routes!

Now we can implement our view model method properly:

```js
addTask(id: number, task: Task) {
  return TasksService.post(id, task).then(
    () => toastr.success("Posted new task successfully"));
}
```

Obviously there's much more you can do such as automatically handling errors, customizing options, creating Angular services, etc.

## So, that's why Typewriter is awesome

I've walked through a simple use case of why Typewriter is super useful--as a developer I'm always interested in ways to make my life easier and not worrying about differences between my client and server is always helpful. That's why I love TypeScript and why I love Typewriter. Hope you found this helpful!
