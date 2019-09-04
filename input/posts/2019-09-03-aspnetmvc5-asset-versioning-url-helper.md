Title: "Versioning Content URLs in ASP.NET MVC 5"
Published: 2019-09-03 20:37:00 -0500
Lead: ASP.NET Core lets you add automatic versioning to JS and CSS files and I wanted to do the same in MVC 5.
Tags:
- ASP.NET
- MVC
- C#
- Nuget
- Open Source
---

I just published a new Nuget package, [AspNet.Mvc.AssetVersioning](https://www.nuget.org/packages/AspNet.Mvc.AssetVersioning). This package extends the MVC5 `@Url` Razor helper with a new `VersionedContent` method
which appends a Base64-encoded SHA256 hash to the end of the URL for cache-busting.

## Versioning in ASP.NET Core

In ASP.NET Core, you can version URLs using the [`asp-append-version` tag helper](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/tag-helpers/built-in/image-tag-helper?view=aspnetcore-2.2#asp-append-version):

```html
<script src="~/scripts/foo.js" type="text/javascript" asp-append-version="true"></script>
```

Which will output:

```html
<script src="/scripts/foo.js?v=hash" type="text/javascript"></script>
```

## Versioning in ASP.NET MVC 5

Since I have not yet migrated to ASP.NET Core for [Keep Track of My Games](https://ktomg.com), I needed the same functionality in order to remove some old libraries that are not available in .NET Core. Now you can achieve the same thing using my [package](https://www.nuget.org/packages/AspNet.Mvc.AssetVersioning):

```html
@using System.Web.Mvc.AssetVersioning;

<script src="@Url.VersionedContent("~/scripts/foo.js")" type="text/javascript"></script>
```

This will output the following:

```html
<script src="/scripts/foo?base64-encoded-hash" type="text/javascript"></script>
```

The helper will automatically cache hashes for files for the lifetime of the `HttpContext`. So, basically, for the lifetime of the application pool. Restart the site/app pool to refresh the cache, which should happen on any new deployments.

You can simplify the `@using` statement usage by adding the namespace to the `/configuration/system.web.webPages.razor/pages/namespaces` section in the `Views/web.config` file, as outlined in the GitHub repo [README](https://github.com/kamranayub/aspnetmvc5-asset-versioning).

Hope this helps someone else facing the same issue!