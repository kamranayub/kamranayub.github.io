Title: "Two New Pluralsight Courses on Azure Storage Published"
Published: 2018-10-25 22:22:00 -0500
Lead: Excited to announce two brand new Pluralsight courses on using Azure Storage with Azure CDN and CORS.
Tags:
- Courses
- Pluralsight
- Azure
- Microsoft
- Cloud
---

The first week of October I released not one but **TWO** Pluralsight courses I've been hard at work on the past few months:

**Implementing Azure CDN with Azure Storage**

[![Azure CDN course link](https://user-images.githubusercontent.com/563819/47162935-95932a80-d2ba-11e8-8d95-a037ca745333.png)][cdncourse]

[Watch the course now][cdncourse]. This course covers a lot of how Azure CDN works and various ways to integrate it with Azure blob service. I walk through how Azure CDN works at a high-level, then dive down into ways to automate the creation of CDN profiles/endpoints and how to serve content from blob storage. I cover Azure PowerShell, Azure CLI, REST, and ARM templates. I also spend a whole module on serving content securely using Shared Access Signature (SAS) tokens in different ways including pass-through authentication, hidden SAS authentication, and CDN token authentication.

**Configuring CORS with Azure Storage**

[![Azure CORS course link](https://user-images.githubusercontent.com/563819/47162956-9f1c9280-d2ba-11e8-9109-b93dfe96dee5.png)][corscourse]

[Watch the course now][corscourse]. This course covers configuring Cross-Origin Resource Sharing (CORS) with Azure Storage accounts. First, we spend time on what CORS is, how it works, what it's **not** (hint: it's not a way to do authentication!), and then how Azure evaluates CORS rules internally. Then I walk through several automation scenarios in Azure PowerShell, Azure CLI, and REST. By the way, I've [open sourced a Postman collection][postman-collection] that handles signing Azure Storage REST requests, if you're interested.

## Building two courses at once

As you may know, I [released a RavenDB 4 course][ravendb-course-post] earlier this year so this was not my first experience making a course. But it **was** my first time doing two courses at once!

The genesis of these courses was sparked by meeting with a Pluralsight "author success manager" (aka ASM) at NDC MN on the speaker boat cruise (which reminds me, [register for NDC MN 2019][ndc-mn]!). I was meeting her to get some author swag! We got into a conversation about what courses I'd do in the future and I mentioned I was thinking of diversifying my next set. She asked, "Do you know anything about Microsoft Azure?" and I was like, "Yeah!" 😎

We decided early on that because these two courses were similar that I could potentially do both at once. I was really happy my ASM believed I could do it but I wasn't sure *I* could. I knew that I had a hard deadline of November as that's when the next baby comes along (it's *too* close now...). At the time we were discussing it, it was May/June. These courses were a little special in that they are considered "partner" courses with Microsoft so they involved back-and-forth with Microsoft that would extend the course timelines.

Still, the opportunity to do **two** courses was not something I'd be offered every time and it would be in-line with my [2018 stretch goal][2018-goals] of doing two additional courses. So I said yes. But I knew I had to plan these courses differently than my first due to the tighter timelines.

By the time the proposal process finished and after a vacation in August, the actual timeline I ended up working with was mid-August to October 8. **It was about 2 months less than I thought I'd have!**

## Tracking time

Let me start by telling you that building these two courses took exactly **112 hours**. I **know** this because I was religiously tracking time. I had also created a task plan for each course at the start, here's what my Trello cards looked like:

![Trello cards](https://user-images.githubusercontent.com/563819/47169134-553aa900-d2c8-11e8-8cb2-5608d42ab953.png)

You can see the duration for each card and the number of tasks I was tracking. I used the Trello Power-Up, [TimeCamp][timecamp], to track time by just clicking a button on the Trello card. I really liked how easy this was; I always had my cards up anytime I worked on the courses and I'd just flip between them tracking time.

Now, using this data, I can actually see what hours over time look like for each course (note the timescales differ):

**CDN With Storage**

![Hours over time](https://user-images.githubusercontent.com/563819/47540021-bd752600-d898-11e8-809f-1057dac567d3.png)

**CORS with Storage**

![Hours over time](https://user-images.githubusercontent.com/563819/47540071-ec8b9780-d898-11e8-84bc-cc8672ca1d71.png)

A few observations:

- **The red dotted line was my hours budget.** Based on the proposal and scope of work, I budgeted 60h for each course. You can see I went over my budget for the CDN course since there was a 3rd module but my CORS course was well under budget at 45h, so overall I feel really good about the planning that went into the courses.

- **Production ramped up in September.** If you notice, I did not start the CDN course actively until Aug 18 and the CORS course actively until the 4th of Sep. By looking at the flat lines, you can see when I submitted a module and waited for review. I didn't really start crunching until Sep 11. By the way, that last bit Oct 1 to Oct 9 was the final module for the CDN course. Phew. 😅

*Why did I even put a hours budget?* Pluralsight pays a course completion fee for every produced course and this fee varies by course. I wanted to see if I could target a specific hourly bill rate with the fee I agreed to. I ended up exceeding my target rate which is a good thing--it means even if these courses made zero royalties, I was already compensated for the work put into producing them. I **do** get royalties for these, though so that's just a bonus.

*Why were the first module reviews so long?* Both courses were subject to Microsoft Subject Matter Expert (SME) review and that took awhile to work through. After the first modules, it was a much faster turnaround.

## Planning tasks

The task planning was straightforward. I planned out each step of the production process:

1. Proposal tasks
1. **for each module**
   1. Content tasks
   2. Recording tasks
1. Post-production tasks

In practice, here's what that looks like:

![Trello tasks](https://user-images.githubusercontent.com/563819/47169804-173e8480-d2ca-11e8-954d-fa04cb8f8dad.png)

This was really helpful--not only to plan but also to track progress day-to-day.

Knowing these numbers will help me properly plan for courses down the road.

## Producing the course

### Content creation

As you maybe could guess from the time pattern above, I would work on one module for one course and once submitted, flip to the other course. This worked out well, in fact I finished some modules before feedback for the previous one came back to me.

And this time, I actually wrote script. I knew I couldn't wing it every single time I sat down to record with such a compressed timeline since that would make each recording session take 2-3x longer with my retakes. I'd write my script in the Notes section of the PowerPoint. Sometimes I'd vary slightly when recording from what I had written but since I write very conversationally it wasn't a big issue. It helped reduce the retakes **a lot**.

I started each module by finishing the content first, starting with a rough entire module slide deck, then filling out the content for each clip. The order and names of the clips would generally change as I discovered better ways to flow the content but the objectives almost always stayed the same (it definitely helps to be clear on your module objectives during the proposal process).

### Recording demos

The way I recorded demos also changed a bit. This time, I divided demos into sections (with slide-based script and screenshots which doubled as a transcript) and narrated first then ran through the visual steps (or vice versa). This made it much easier to cut together and keep me focused. If I didn't do this, it would be hard to maintain the position of the mouse or if I encountered an issue, backtracking and editing together two disparate parts. I had to let go a bit of some of my perfectionist nature--sometimes I wished I could just redo entire clips but I knew I didn't have much time. The peer reviews came back very positive so I feel pretty good about the end product regardless!

One thing I'd definitely do again is to [set up a custom VS Code settings file][settings] for the materials folder. It allowed me to always keep a consistent VS Code experience and turned off a bunch of stuff that would make it tougher to edit. I've found for all three of my courses, VS Code works for any code demos--it has the terminal, files, everything, it's super clean and super fast.

I just saw this tip today, too! Heck yeah!

<blockquote class="twitter-tweet" data-lang="en"><p lang="en" dir="ltr">One thing <a href="https://twitter.com/code?ref_src=twsrc%5Etfw">@code</a> has going for it is the ability to create &quot;profiles&quot; by supplying CLI args when you open it. Combine it with an alias and 💥 I can open VSCode with my screencast settings in an instant. Thanks for the tip <a href="https://twitter.com/avanslaars?ref_src=twsrc%5Etfw">@avanslaars</a> and <a href="https://twitter.com/jlengstorf?ref_src=twsrc%5Etfw">@jlengstorf</a> <a href="https://t.co/3RfdeSvHHO">pic.twitter.com/3RfdeSvHHO</a></p>&mdash; Kyle Shevlin (@kyleshevlin) <a href="https://twitter.com/kyleshevlin/status/1055666040401670145?ref_src=twsrc%5Etfw">October 26, 2018</a></blockquote>
<script async src="https://platform.twitter.com/widgets.js" charset="utf-8"></script>


### Working with a big monitor

There *was* an issue I needed to work through right away which was how I record on an Ultrawide screen. [I love my monitor][monitor] (affiliate link). But it makes it a huge pain to record applications and presentations because they need to be recorded at 1280x720.

What I ended up doing was two things:

- **Record PowerPoint using a secondary output to my PC.** Wait, what? Well, my monitor has multiple outputs including DVI, HDMI, and DisplayPort. I ended up plugging *both* my DisplayPort (primary) and HDMI (secondary) into my PC so that I could *extend* my desktop onto the HDMI output at 1280x720. This made it pretty straightforward to put PowerPoint presentations on that "monitor" and record. Because Presenter View in PowerPoint shows you the slides, it worked well. I *would* have preferred a proper chromeless borderless window that PowerPoint could present on but that isn't available. The windowed mode has a ton of chrome, has no presenter view, and is awkward to deal with.
- **Use a window resizer tool.** I would recommend the tool [Sizer][sizer] by Brian. It works with Windows 10 just fine and allows you to set presets for different window sizes and switch to them quickly for the active application. This allowed me to easily record applications wherever they were on my monitor and even overlay them on top of each other to quickly switch apps without stopping the recording.

## Would I do anything differently?

Well, it's clear with enough planning and ideally *3 months* of time, it's very reasonable for me to tackle two short courses simultaneously. Compressing down to 2 months was tough on my evenings though in retrospect I should have done a few more hours a week nearer the beginning to ease the load on the last bits.

Overall, I'm happy with the result and would do it again. A **huge** thanks to my supportive Pluralsight team who encouraged me to do it and provided everything I needed to succeed.

## What's next?

Course-wise, nothing for awhile, honestly. Producing two courses puts me at **3 Pluralsight courses** for *my first year* which I think is pretty amazing. The new baby is coming in the next couple weeks and I'll be busy with that for 6-8 months. The first months of having an infant will consist of sleepless nights, toddler wrangling, and extreme tiredness. Instead of committing to new courses during that turbulent time, I will be focusing on [Excalibur][excalibur] and [Keep Track of My Games][ktomg] work. Oh, and home improvements, which I'd like to write more about. I also plan to propose some talks for 2019! I want to say that any new coursework won't be planned until the back half of 2019. We'll see if I can stay put that long 😊

[2018-goals]: https://kamranicus.com/posts/2018-01-01-2018-a-new-year#goals
[cdncourse]: http://bit.ly/PSAzureStorageCDN
[corscourse]: http://bit.ly/PSAzureStorageCORS
[postman-collection]: https://github.com/kamranayub/azure-storage-rest-postman
[ravendb-course-post]: https://kamranicus.com/posts/2018-02-08-ravendb-4-course-live
[ndc-mn]: https://ndcminnesota.com/
[timecamp]: https://trello.com/power-ups/59cf411fe342369bca2565b9/timecamp
[ktomg]: https://keeptrackofmygames.com
[excalibur]: http://excaliburjs.com
[sizer]: http://www.brianapps.net/sizer/
[settings]: https://github.com/kamranayub/pluralsight-azure-cdn-with-storage/blob/master/.vscode/settings.json
[monitor]: https://www.amazon.com/gp/product/B01N6S1P2D?&_encoding=UTF8&tag=kamranicus-20&linkCode=ur2&linkId=5427a9094117f6bab8eaccbcc18af141&camp=1789&creative=9325