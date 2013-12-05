---
layout: post
title: "Use Enums to Reduce Markup Maintenance [MVC]"
date: 2012-06-08 04:49:33 -0500
comments: true
categories:
permalink: /blog/posts/49/use-enums-to-reduce-markup-maintenance-mvc
---

Have you ever done this before?

```html
<!-- Decide what CSS class to use for layout -->
@if (Model.Format == Formats.Table) {
	<div class="foo foo-layout-table">
} else if (Model.Format == Formats.Grid) {
	<div class="foo foo-layout-grid">
} else if (Model.Format == Formats.Paragraph) {
	<div class="foo foo-layout-paragraph">
}

	<!-- Markup -->

</div>
```

Or some other variant of the same idea (trying to assign a CSS class based on some branch logic)?

If you find yourself doing this, I have a pro tip for you: wherever you can, utilize your enum directly in the CSS class name. Why? Because in 2 weeks when you add another layout, you will need to go back into this if statement and add another branch. The other reason is, frankly, this is ugly and not very Razor-ish.

Instead, I'd advocate doing it this way:

```html
<div class="foo foo-layout-@Model.Format">
	<!-- Markup -->
</div>
```

Simple! I found that to be much more intuitive and is the way I've always approached this. You can use this approach with an Enum, string, or other type where a `.ToString()` outputs a simple value (booleans!).

*But Kamran, my logic is much more complex than that.* This is when I'd recommend pulling out that logic into a view model, creating a `@helper`, or as a last resort, inline.

```html
<!-- Helper -->
@helper LayoutCss() {
	if (Model.Format == Formats.Table) {
		@:foo-layout-table
	} else if (Model.Format == Formats.Grid) {
		@:foo-layout-grid
	} else if (Model.Format == Formats.Paragraph) {
		@:foo-layout-paragraph
	}
}

<div class="foo @LayoutCss()">

<!-- Inline -->
@{
	var layoutCssClass = "";

	if (Model.Format == Formats.Table) {
		layoutCssClass = "foo-layout-table";
	} else if (Model.Format == Formats.Grid) {
		layoutCssClass = "foo-layout-grid";
	} else if (Model.Format == Formats.Paragraph) {
		layoutCssClass = "foo-layout-paragraph";
	}
}

<div class="foo @layoutCssClass">
```

The point is to decouple your CSS class logic from your markup as much as you can, so your markup isn't crufted up. What other tips do you have for reducing CSS class headaches?