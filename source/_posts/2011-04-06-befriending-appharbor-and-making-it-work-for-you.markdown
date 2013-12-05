---
layout: post
title: "Befriending AppHarbor and Making it Work for You"
date: 2011-04-06 16:11:44 -0500
comments: true
categories:
permalink: /blog/posts/2/befriending-appharbor-and-making-it-work-for-you
---

I chose to partially host my site on [AppHarbor](http://appharbor.com) because it affords several luxuries for **free**:

- Load balancing
- Automatic builds and deploys
- Fast and easy setup/maintenance
- Private Git hosting

However, it does fall short in a couple areas:

1. Native support for a staging environment
2. Free database is only 20MB and it's $10/mo for a 10GB database
3. Any more instances incur about $36/mo each

There are workarounds for the first two issues, which I will cover here. The third is a judgement call on your part, whether you think it's worth the cost. For me, I don't need another instance.

## Setting up a Staging Environment

You *can* set up a staging environment in AppHarbor. I needed several things for my staging environment:

1. Is a sub-domain
2. Is protected so only I can access it

Here is what I did:

1. In AppHarbor, create a new application (e.g. *Kamranicus-Dev*)
2. Locally, add this new application as a new remote to your Git:
   - `git remote add appharbor-stage {your new app Git url}`
3. In the **Configuration Variables** section of your AppHarbor application, add a new variable:
   - `RuntimeEnvironment` with a value of `Staging`
4. In your **web.config**, you can now add a new `appSetting`:
   - `<add key="RuntimeEnvironment" value="Local" />`

Now, when you push to your new `appharbor-stage` remote, AppHarbor will automatically replace that app setting with the value `Staging`.

At this point, you're all done. You can now add a host name like **stage.kamranicus.com** to point to your staging site.

To push to your staging environment:

    git push appharbor-stage master

If you're wondering about connection strings, you will need to test for the current `RuntimeEnvironment` and use one of your choosing:

    var runtimeEnvironment = ConfigurationManager.AppSettings["RuntimeEnvironment"].ToString();

### Adding protection

If you're like me, you don't want everyone looking at your new changes before they're ready and you sure as hell don't want people adding stuff to your development database.

I am using Forms authentication in conjunction with OpenID for Kamranicus. Thus, I needed a to make a special attribute for my `BaseController` that programmatically decided whether or not to restrict access to the controller based on the runtime environment.

```c#
using System;
using System.Configuration;
using System.Web.Mvc;

namespace System.Web.Mvc {
	public class AuthorizeLocalAttribute : AuthorizeAttribute {
		private const string LOCAL = "Local";
		private const string STAGING = "Staging";

		protected override bool AuthorizeCore(HttpContextBase httpContext) {
			// Ignore all Account controller calls (Anonymous)
			if (httpContext.Request.RequestContext.RouteData.Values["controller"].ToString() == "Account")
				return true;

			// Get runtime environment
			string runtimeEnvironment = ConfigurationManager.AppSettings["RuntimeEnvironment"];

			if (runtimeEnvironment == STAGING) {
				return base.AuthorizeCore(httpContext);
			}
			else {
				return true;
			}
		}
	}
}
```

You can then apply this attribute to your `BaseController`:

```c#
    [AuthorizeLocal(Users = "MyAdminID")]
    public abstract class BaseController : Controller {
        // yada yada yada
    }
```

## Does it work?

This is the method I use and it seems to work great. Note that you can create any number of environments like this and programmatically do different things based on what gets injected in by AppHarbor.

## Taking care of storage

For my blog I store all my images in the database. This means that I would quickly run over the 20MB in the free database allotted by AppHarbor. I also don't want to pay an extra $10/mo for a database.

Instead, I already have an account with [Arvixe](http://arvixe.com) and as far as I could tell, it is not against the TOS of AppHarbor to host your database externally. So that's what I did. I get to use my Arvixe account for not only DB hosting, but also the mail server and other hosting provider options for a measley $10/mo (it also hosts some other sites I own).