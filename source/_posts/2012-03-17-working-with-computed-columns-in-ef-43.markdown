---
layout: post
title: "Working with Computed Columns in EF 4.3"
date: 2012-03-17 23:38:15 -0500
comments: true
categories:
permalink: /blog/posts/35/working-with-computed-columns-in-ef-43
disqus_identifier: 35
---

There's not a ton of information out there on how to handle computed columns in EF. It turns out it is pretty straightforward if you know what to do.

## Telling EF about your computed column

First, if all you want to do is manually create a function and use it in a column, you only need to decorate your model property with `[DatabaseGenerated(DatabaseGeneratedOption.Computed)]` as seen here:

```c#
public class Person {

    public string FirstName { get; set; }
    public string LastName { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public string FullName { get; set; }
}
```

You should know that **if possible, try to keep "computed" values in your application.** This is my opinion from an ease of use POV, not a design POV. I say it because unless you really *need* a computed column, you could just as easily create a read-only property that EF ignores.

However, in some cases, a computed column is a viable option such as when you need to sort (on the database-side) against a computed value. That was my own reason for using a computed.

## Manually adding the function and column

We've told EF to ignore our property and treat it as a computed column. Now how do we actually *create* the column? Well, what I did previous to EF Migrations was to just do it all manually through SQL Management Studio. This is entirely OK to do, you just need to make sure you save the script somewhere or remember to do it anytime you create a new database for your application.

You can now go into SSMS and delete the column EF generated (or add it if it's new). Make sure you create your function first:

```sql
CREATE FUNCTION [dbo].[GetFullName] 
(
    -- Add the parameters for the function here
    @FirstName varchar(50),
    @LastName varchar(50)
)
RETURNS varchar(255)
AS
BEGIN
    DECLARE @FullName varchar(255);

    SELECT @FullName = @FirstName + ' ' + @LastName;

    RETURN @FullName;
END
```

That approach worked fine for me, since I use Schema Compare (or used to!) to migrate my database between environments.

## Using EF Migrations

Now that EF 4.3 has been released, we can all enjoy the benefit of [Database Migrations](http://blogs.msdn.com/b/adonet/archive/2012/02/09/ef-4-3-automatic-migrations-walkthrough.aspx). What I like about this approach is that it's very deliberate and discoverable. I could clone my repository onto a new PC and run the `Update-Database` command to do *everything*, plus now we can migrate backwards if needed. It's excellent!

I am using automatic migrations for [KTOMG](http://keeptrackofmygames.com), so what I had to do first was manually create a new migration in the Package Manager console:

    $> Add-Migration CustomFunction

This will scaffold a new, empty migration:

```c#
public partial class CustomFunction : DbMigration
{
    public override void Up()
    {
    }
    
    public override void Down()
    {
    }
}
```

Now we can be really cool cats. In this migration, all we have to do is execute our `CREATE FUNCTION` SQL statement in the `Up` method, along with dropping and re-adding the column:

```c#
public override void Up()
{
    // Create function
    Sql(@"
CREATE FUNCTION [dbo].[GetFullName] 
(
-- Add the parameters for the function here
@FirstName varchar(50),
@LastName varchar(50)
)
RETURNS varchar(255)
AS
BEGIN
DECLARE @FullName varchar(255);

SELECT @FullName = @FirstName + ' ' + @LastName;

RETURN @FullName;
END
", true);

    // Add the computed column
    DropColumn("Person", "FullName");
    Sql(@"
ALTER TABLE [dbo].[Person]
ADD [FullName] AS ([dbo].[GetFullName]([FirstName],[LastName]));
");
}
```

Take note of the `true` parameter at the end of the `Sql()` call. This explicitly tells EF *not* to include the statement in the migration transaction. If you don't do this, SQL Server will complain that the CREATE FUNCTION statement must be the only one in the batch.

Now let's scaffold our tear down step. We should drop the column first, drop the function, and then finally re-add the basic column back.

```c#
public override void Down()
{
    DropColumn("Person", "FullName");
    Sql("DROP FUNCTION [dbo].[GetFullName]");
    AddColumn("Person", "FullName", c => c.String(nullable: true, maxLength: 255));    
}
```

I would take care to make sure the column is re-created using the original schema EF created.

Now we can tell EF to migrate to the latest version:

    $> Update-Database -Verbose (or -Script, to preview)

Voila, you should now have a brand spankin' new computed column.