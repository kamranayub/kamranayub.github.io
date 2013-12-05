---
layout: post
title: "A Better MVC Label Helper"
date: 2011-04-07 13:54:05 -0500
comments: true
categories:
permalink: /blog/posts/3/a-better-mvc-label-helper
---

I have high standards for web-based forms, after having spent the last 7 months ankle deep in them at work.

I have decided on a standard way for defining my forms:

```html
<form>
    <fieldset>
        <legend>A form area</legend>
        
        <div class="field text">
            <label for="firstName">
                First Name
                <span class="note">Enter your first name</span>
            </label>
            
            <input type="text" id="firstName" />
        </div>
    </fieldset>
</form>
```

In a future post, I will more formally present a form solution and my stylesheet that goes with it. For now, let's talk about generating that `<label>` above from metadata alone.

### First, the end result

Here is what my Razor view looks like for that form field above:

```html
<div class="field text">
    @Html.FieldLabelFor(m => m.FirstName)
    @Html.EditorFor(m => m.FirstName)
    @Html.ValidationMessageFor(m => m.FirstName)
</div>
```

Based off of:

```c#
public class Person {

    [DisplayName("First Name")]
    [Description("Please enter a first name")]
    [Required] // leave off to add '(optional)' to label
    public string FirstName { get; set; }
}
```

That's it.

### Building a better LabelFor

My helper is called `FieldLabelFor` but you can call it whatever you want. Here's what it does:

 - Is entirely based off metadata attributes
 - Label text is based off of `[DisplayName]` attribute (via ModelMetadata)
 - Adds an *(optional)* if a field isn't required
 - Adds a `<span class="note">` to the label if a `[Description()]` attribute is found
 - Supports both attributes decorating the model and via `[MetadataType]` attribute

It outputs the `<label>` markup found above.

I've made it a Gist, so you can hack and slash it to your needs:

<script src="https://gist.github.com/907780.js?file=FieldLabelFor.cs"></script>

Remember to add `@using YourProject.Library.Extensions;` to the top of your razor file (or add the namespace to your `Views\web.config`).

I've styled my forms so that a full field will look something like:

![Field label style](/blog/images/5.png)

As you can probably tell, I subscribe to the notion that all fields are required unless otherwise marked. I think for a user filling out a form, this is what they assume to be the case and more often that not, more fields are required than optional.

### Off you go!

Start building better forms with MVC. Look for a post at a later date that will delve deeper into my opinions on web forms (hint: they suck!).