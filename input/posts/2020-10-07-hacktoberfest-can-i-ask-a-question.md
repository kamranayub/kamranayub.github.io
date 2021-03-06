
Title:  "Hacktoberfest:  Can  I  ask  a  question?"
Published:  2020-10-08 08:00:00  -0500
Lead:  A  friend  contacted  me  about  how  they  should  approach  asking  questions  for  their  very  first  Hacktoberfest  /  open  source  contribution.
Tags:
-  Hacktoberfest
-  Open  Source
---

I have a friend who is doing their first open source contribution through [Hacktoberfest](https://hacktoberfest.digitalocean.com/) this year, which is a yearly event to encourage open source participation.

In their email, they asked:

> Is there any etiquette or expectation for how much coding help one can ask for when taking up an issue and contributing?

What a good question! Imagine yourself feeling pretty psyched to take on a Hacktoberfest issue and when you start to dig in to understand the codebase, you start to feel intimidated. 

Is it OK to ask for help? Would that be rude to the maintainers? 

The short answers are **YES** and **NO** (*hopefully*). Here is my longer answer!

## Hacktoberfest projects expect newcomers

There was a little bit of drama this year because historically projects did not necessarily opt into Hacktoberfest and were being spammed with low-effort PRs (or outright useless PRs). However, the [rules have changed](https://hacktoberfest.digitalocean.com/hacktoberfest-update) and now repositories can opt-in in several ways:

They can have a `hacktoberfest` topic in their GitHub project, like this:

![Screenshot of a repository with "hacktoberfest" topic](https://user-images.githubusercontent.com/563819/95412517-bd885800-08ee-11eb-883a-6620f19d0c0e.png)

Or, maintainers can mark accepted PRs with the `hacktoberfest-accepted`, like this:

![hacktoberfest-accepted label](https://user-images.githubusercontent.com/563819/95412776-5919c880-08ef-11eb-8a63-f58f6bc4e3e4.png)

If one of these is true, the project is considered opted-in to Hacktoberfest.

My expectation as a contributor would be that if a project opted into Hacktoberfest, especially with the **topic** approach, they are *expecting* to help you with making quality contributions.

Now since we at [Excalibur](https://github.com/excaliburjs/Excalibur) have always opted-in each year, I can tell you that **we expect people to ask questions!** We try to triage and choose potential Hacktoberfest issues ahead of time (heh, in "Preptember") but we know that people will likely have questions.

After all, first-time contributors can't be expected to have answers to everything, of course you will need some help along the way. This is especially true for anything beyond mere typos.

## But will my question be welcomed?

Now this is getting at a different issue, unrelated to Hacktoberfest specifically and more related to toxicity in the open source world. And let's be real: open source can be toxic.

Here's what I recommend to help avoid a toxic project until it's too late: "scout" a potential project beforehand. Does it have a Code of Conduct? Do the maintainers seem to follow it? How do they react to external contributors asking questions? Are they helpful? How do they review their own code?

Doing some prep work on your part beforehand I think will help having to ask this question in the first place.

## Where should I ask?

Start with the CONTRIBUTING document of the project or the CODE_OF_CONDUCT document. An actively maintained project that works with external contributors will typically lay out the expectations for issues and pull requests and may tell you where the best place to get help is. With the recent [introduction of GitHub Discussions](https://github.blog/2020-05-06-new-from-satellite-2020-github-codespaces-github-discussions-securing-code-in-private-repositories-and-more/), that may also be the first place to look if the project is opted-into the beta.

![Screenshot of GitHub Discussions](https://user-images.githubusercontent.com/563819/95463793-d6b8f500-093e-11eb-9f77-2e3997cd5dc0.png)
<figcaption>GitHub Discussions is slowly being made available on a opt-in basis</figcaption>

If there's no clear place to start, I recommend first asking within issues, assuming there's a Hacktoberfest issue you're taking. This is also where you'd volunteer to take the issue (like, "🙋‍♂️ Hey, I'd love to try and take this on!").

At that point, maintainers should see your interest and expect that you'll have questions as you get farther into it. If you have questions right away, like about the design, proposal, or a general question, the issue is where I'd ask.

If you have a draft pull request (and I always recommend starting with a draft!) then that is where I'd start asking questions about the approach, feedback on your current solution, etc. A draft pull request is like your writing drafts -- do you send them to your friends to read or an editor? Of course! So get feedback early in the process.

## Time to explore the cave, don't go it alone!

I like to think of exploring a codebase like exploring a cave system (or "spelunking" if you're a fancypants). You reveal one area, then reveal the next and that might have any number of offshoot paths or trails back to previous areas, etc. You can get lost in a codebase just as you would a cave system.

![Photo of a cave by Joshua Sortino (unsplash)](https://user-images.githubusercontent.com/563819/95415964-3ee3e880-08f7-11eb-8290-4987f6d85952.jpg)
<figcaption>Photo by <a href="https://unsplash.com/@sortino?utm_source=unsplash&amp;utm_medium=referral&amp;utm_content=creditCopyText">Joshua Sortino</a> on <a href="https://unsplash.com/wallpapers/nature/cave?utm_source=unsplash&amp;utm_medium=referral&amp;utm_content=creditCopyText">Unsplash</a></figcaption>

A maintainer is the cave guide. Or maybe just someone who's really been in and out of the cave a ton. Or maybe **really, really** likes this one area of the cave and knows it blindfolded. The metaphor sort of falls apart because who made the cave, anyway? Nevermind. 🤫

When you ask questions while you are working on a codebase (of any size), it's like you are wandering into the cave and _then_ asking for the maintainer to fill in parts of the cave map, throw you some new flashlight batteries, and tell you, "This is the safe path through. Avoid this one, though!"

Being a newcomer though brings fresh perspective. You might find parts of the cave no one has been through in awhile that could use some dusting. Maybe there's a new pathway the cave has opened up that could be faster than an existing pathway that's well-trodden. You get the picture -- when you ask questions, you may inspire new and better solutions to existing problems maintainers haven't considered.

It's definitely fun to explore a cave on your own at first but it won't be long until your flashlight dies. Ask for help and recharge those batteries! It results in a more agreeable outcome for everyone. Huzzah.

## What to avoid

I can't speak for all maintainers but I think it's safe to say that generally maintainers welcome quality contributions (of any kind!).

When you don't ask questions you may end up going down a road that could lead to maintainbility or design issues you haven't foreseen. Remember: this might be your first (and only) time in the cave, it's better to go in prepared!

## Always Be Asking

I'd be **surprised** if you didn't have a question about a new codebase. Actually, it would be unbelievable. Of course you will.

I get it, though. You might be listening to that voice in your head, "You can't ask a question like this, it's a silly question, I will look like a fool, and what will they think of me?" **Don't listen!** 

Listen, **it is not a sign of weakness to ask questions, it is how we learn.** Doesn't matter if you're brand new to open source, new in your role, or a Principal Engineer -- we ask questions to understand better so **Always Be Asking**. If anyone (person, company, etc.) expresses to you that asking a question is some sign of weakness, run the other way.

If this is your first time contributing to open source, first, how exciting! Second, I lay out everything I just mentioned in more detail in my course on [Contributing to an Open Source Project on GitHub](https://bit.ly/PSContributingToOpenSource). It's an A-to-Z guide geared toward new open source contributors with practical scenarios, workflows, and a guided process on choosing the right project for you to work on.
<!--stackedit_data:
eyJoaXN0b3J5IjpbLTM4MjIwMTUzMSwtNzE3MjkwNDg3LC02OT
kwMTYxOTZdfQ==
-->