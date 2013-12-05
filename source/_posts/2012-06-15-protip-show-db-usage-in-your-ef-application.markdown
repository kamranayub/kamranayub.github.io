---
layout: post
title: "Protip: Show DB usage in your EF application"
date: 2012-06-15 06:19:29 -0500
comments: true
categories:
permalink: /blog/posts/51/protip-show-db-usage-in-your-ef-application
---

My staging application is on [AppHarbor](http://appharbor.com) so I gave it the shared Yocto SQL database which has a limit of 20MB. That's not much, so I wanted a way to display my current usage level in staging when I test the site in case I want to flush it out. It was fairly easy (with a gotcha)!

**MVC Layout**

```html
@if (AppSettings.RuntimeEnvironment != RuntimeEnvironment.P)
{
    <p>DB Usage: @{ Html.RenderAction("DbUsage", "Stats"); }</p>
}
```

I am rendering an action since I need to call out to my service layer.

**StatsController**

```c#
[ChildActionOnly]
[GET("stats/dbusage")]
public ActionResult DbUsage()
{
    if (AppSettings.RuntimeEnvironment != RuntimeEnvironment.P)
    {
        return Content(SomeAbstraction.GetDatabaseUsageStat());
    }

    return new EmptyResult();
}
```

I am using [AttributeRouting](https://github.com/mccalltd/AttributeRouting), and if you use `RenderAction` you should know it requires a route to be made. I use `ChildActionOnly` to prevent browsing to the action. I also return an empty result if it's production (I only have 3 states).

**IRepository**

In my project, I have a application service layer that uses an injectable repository. I've found this makes integration testing pretty easy as my controllers are dumb as heck and leave all the logic up to the service layer where there's no need to mock any HTTP stuff. 

I'll skip to the repository since that's where the actual code is.

```c#
public string GetDbUsage()
{
    try
    {
        IEnumerable dt = _dbContext.Database.SqlQuery(typeof (DbUsage), "sp_spaceused");

        if (dt == null)
            return "Unknown";

        var usage = dt.OfType<DbUsage>().FirstOrDefault();

        return usage.database_size;
    } catch
    {
        return "Unknown";
    }
}

private class DbUsage
{
    public string database_name { get; set; }
    public string database_size { get; set; }
}
```

This just executes the stored proc `sp_spaceused` and coerces it into the `DbUsage` type. The gotcha is that there's another column, "unallocated space" which **for some ungodly, inconsistent reason** has a space in it. I cannot figure out how to get it without resorting to an ADO.NET data reader. I'd appreciate any advice on how to grab that additional column as well.

If you want to log an error, you should, but I don't really care if I can't access the size... so I just ignore it. It's only for debugging.

And as for the result? Handy!

![DB Usage](/blog/images/53.png)