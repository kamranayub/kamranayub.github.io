---
layout: post
title: "Impersonating a User During Automated Testing Scenarios"
date: 2015-11-06 11:00:00 -0500
comments: true
published: true
categories:
- .NET
- C#
- Testing
- SpecFlow
- Keep Track of My Games
---

I'm starting to introduce privacy controls to [Keep Track of My Games](http://keeptrackofmygames.com) and I ran into the following scenario when writing my tests:

```
Scenario: Anonymous user should be able to view a public custom list
	Given a user has a list
	And a user's list is public
	When I request access to the list
	Then I have read-only access
```

In this context, **I** am the anonymous user. This is the exact [SpecFlow](http://www.specflow.org/) scenario I wrote. Do you know why I may have run into issues?

Let's look at the first two steps:

```c#
[Given(@"a user has a list")]
public void GivenAUserHasAList() {
    _listResult = _context.ListService.CreateList(_newList)
}

[Given(@"a user's list is public")]
public void GivenAUsersListIsPublic() {
  _privacySettings.Level = PrivacyLevel.Public;
  _context.ListService.UpdateListPrivacy(_listResult.Id, _privacySettings);
}
```

Why would this cause a problem with my given scenario?

1. In the first step, I'm creating a new list.
2. In the second step, I'm taking the new list I just made from the first step and updating the privacy settings on it. 

The problem is that my service assumes the context is an authenticated user and will apply changes to the **current user's** list. Well, since I did not call my login helpers before these two steps, I am in an anonymous context so the service calls fail. That's good! But how can I tell my steps to call a service method *on behalf* of another user without having *every* step use the current user context?

You might say I should just create a new method that accepts a username and refactor my methods. I *could* do that but not only is my entire service designed around the current user context, my service layer is essentially the interface of my public API. I would never allow one user to create a list for another user (unless that was a feature). So the same way I wouldn't expose an API method to do something on behalf of someone, I won't add a public method in my service layer to do the same. I could choose to make the method private or internal and grant access to the assembly for testing--true, I *could* but that seems like a workaround where I need to expose special functionality just for testing.

The approach I ended up doing was simpler and more elegant and leveraged an existing pattern I was relying on: injecting an `IUserContext` into my service layer like this:

```c#
public ListService(IUserContext userContext) {
  _userContext = userContext;
}
```

This is using standard dependency injection (Ninject, in my case) to inject a context for the current user. That context gets created and maintained outside this class, so it doesn't care who provided it or where it came from, it just uses it to determine business logic.

So since I'm already injecting the current user context and mocking it in my tests, why not simply swap out the context when I need to?

## Creating an impersonation context

That's what I ended up doing. Here's my implementation of a `TestingImpersonationContext` (https://gist.github.com/kamranayub/9654d6581fbcf63cf481):

<script src="https://gist.github.com/kamranayub/9654d6581fbcf63cf481.js"></script>

It should be clear what's happening but let me explain further. Specifically in SpecFlow you can inject a context into your testing steps like so:

```c#
public class StepBase : TechTalk.SpecFlow.Steps {
    protected TestingContext _context;
    public StepBase(TestingContext context)
    {
        _context = context;
    }
}
```

As long as your step classes inherit that `StepBase`, you have access to a context. All I did was build a method off that context that swapped out my existing dependency that was injected for `IUserContext` with a temporary context that impersonated the requested user. Once it is disposed, it restores the original context. Easy as pie!

If you are **not** using SpecFlow which is probably the case, don't fret--all you really need is a class or helper method that you can access in your test classes. However you want to achieve that is up to you. Create a base class, don't even bother with dependency injection, etc. This is entirely doable without DI but since my app relies on it I also leverage it during testing.

Now given we have an impersonation context helper, here's how our two testing steps have changed:

```c#
[Given(@"a user has a list")]
public void GivenAUserHasAList() {
  using (_context.Impersonate("user") {
    _listResult = _context.ListService.CreateList(_newList);
  }
}

[Given(@"a user's list is public")]
public void GivenAUsersListIsPublic() {
    using (_context.Impersonate("user"))
    {
        _privacySettings.Level = PrivacyLevel.Public;
        _context.ListService.UpdateListPrivacy(_listResult.Id, _privacySettings);
    }
}
```

I could even update my scenario to be specific about **who's list** I'm accessing (so it's not ambiguous between logged in user vs. another user) but since I only have two users in my testing context, it doesn't really matter.

Now for the test results:

```
Given a user has a list
-> done: ListSteps.GivenAUserHasAList() (0.2s)
And a user's list is public
-> done: ListSteps.GivenAUsersListIsPublic() (0.0s)
When I request access to the list
-> done: ListSteps.WhenIRequestAccessToTheList() (0.1s)
Then I have read-only access
-> done: ListSteps.ThenIHaveReadAccess() (0.0s)
```

The tests are green and now I'm a happy coder. By the way, if you aren't using [SpecFlow](http://specflow.org) for .NET you should consider it, I love it.
