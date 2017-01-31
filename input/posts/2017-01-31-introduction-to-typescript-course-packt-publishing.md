Title: Introduction to TypeScript Course Now Available
Published: 2017-01-31 07:00 -0600
Lead: My Introduction to TypeScript course is available to purchase
Tags:
- TypeScript
- Courses
- JavaScript
---

I'm very excited to share that the course I've been working on the past few months, [Introduction to TypeScript][1] is now available to purchase through Packt Publishing! I should have some discount codes to share soon (will Tweet/update when available).

![Course cover](images/2017-01-31-course-cover.jpg)

<!-- More -->

## Goals

My goals while developing the course were pretty simple:

**Provide background on TypeScript**

The first couple sections of the course focus on *why TypeScript.* Not only trying to answer, "Why would I use TypeScript?" but also, "Why is TypeScript even a thing?" I think it's important to understand *why* you're using a language and to understand the context in which to use it.

**Learn by doing**

Throughout the course the majority of content is presented inside of Visual Studio Code. Why? Because that's where *you'll* be working (or in some other IDE). After the first few introductory sections, the amount of slides I use in the rest of the course are mainly explanatory/summary slides. The rest **is all in the code.** Furthermore, I've built the course sample code to allow following along--each section has its own folder and it just starts at the final state, so you can begin where I leave off.

The other motivation to focus so much on actual coding is the fact that **one of the primary reasons to use TypeScript is for productivity.** And you just can't *tell* someone how much time they will save, you need to *prove* it. I hope by the sheer amount of coding samples I walk through that it will be crystal clear how useful TypeScript can be.

**Focus on real-world samples**

The middle of the course introduces language concepts and features by building a sample application from scratch. I purposefully designed an example that didn't assume prior knowledge of any other JavaScript framework or library--I want people to "go in fresh." This is to allow the viewer to see TypeScript in a purely isolated context to learn the language and how to design their own applications from scratch. I cover environment setup, refactoring, and testing.

In the latter part of the course, I switch to migrating several "vanilla" JavaScript codebases. I intentionally chose *publically available projects.* Why? Because I knew if I wrote contrived samples to migrate I would be biased towards designing it with TypeScript in mind. Instead, I chose libraries that **other people wrote** so they could be more raw and real. The only thing I did was choose examples that would allow me to introduce more concepts and workflows that people might encounter in the real world *and* that could support migration in a reasonable time for a course.

## Post-mortem

This was my first course I developed myself. I was happy with the folks I worked with at Packt and I think I did a good job planning out work. I did *not* do a good job at estimating the amount of time up front--I think I had anticipated a 3 hour course and the final time is about 8 hours. I'm very happy with all the produced content but *man* that was a terrible estimate. Now I know for the future what to expect so I'm hoping future courses will have better up-front time estimates.

The editors I worked with remarked that I was a very good author to work with and wondered what tips I could share to help other authors keep to their schedule (we finished early! The course was expected to be released in February). Each section was due about 6-7 days apart and this is how I split the work to make my evenings and weekends manageable:

**Code first (1-2 days)**

Each section comprises of about 3-4 videos. I would tackle one section at a time, where I'd begin by *writing all the code examples first*. Why? Because even I didn't quite know what I'd learn when I started writing the code. As I wrote the code, I'd take down notes on what I wanted to highlight, what obstacles I ran into, and I'd mentally picture how I'd walk through the code to get to the end result.

This is by far the most time-consuming phase of making the course and I'd usually do this part on Sundays and Tuesdays (that worked best for my schedule, see Punchcard below). It's also where I learned the most and did the most research.

**Content next (Evening)**

Once the code was written, I had a clear picture of where to begin and where to end. Then I'd spend time on the slides--writing up any content that needed an introduction or explanation, the goals of the section, and a summary of everything the viewer should have learned.

In retrospect, this is the step I should have written all the required metadata for. The metadata is required for distribution of the course and I waited until the end to do it all, it would be have been more efficient to do it during this step.

Almost exclusively I worked on this during the evenings in the week. Some presentations took longer to make than others, depending on how much content I needed to cover.

**Record last (Evening)**

After the code and slides were done, I recorded. Using the software provided, I recorded each video in a single session, usually after dinner. My wife would go downstairs with the dog and I'd let her know when I was done. There were only a couple sections where I had to split recording into two days, otherwise I could usually do it in one night. I'm a perfectionist, so any part of the session I messed up I just started that part over. Each minute of produced material was about 2.3 minutes of raw material, so for a 20 minute video I'd spend about 46 minutes recording. Mainly this was because I didn't write a script beforehand or anything, I just tried to "present" and when I stumbled or made a mistake, I'd just start that bit over again. 

A few things I did to minimize headaches during editing:

- Don't move the mouse while speaking in case of mistakes/cutting
- Minimize or eliminate parts of the screen that change dynamically (file explorer, clocks, taskbar, notifications)
- Clear the terminal before each step. It would look weird to have different command history being shown between cuts
- Ensure the mic was positioned consistently between sessions (I have a boom arm)
- Ensure the mixer was always the same settings each session (I never touched the settings after I started recording the first video)

I think the end result turned out really well because of the steps I took to minimize how much of the screen/sound changed during recording. All in all, I don't think any section took more than 2-4 hours to record.

**Don't be afraid to change the outline**

The original outline of the course was a bit different than what ended up being final. As I worked through each section, sometimes topics that I had planned to cover later came up sooner than expected. Rather than trying to avoid it, I just modified the outline to compensate. Overall, there was only one case where I had to move/merge sections and the editors were able to work with me on it--it ended up being way better than the original plan I had.

## Stats

I can use these basic stats to help estimate any other courses I work on:

- **Raw unedited video:** 19 hours, 5 minutes
- **Final produced video:** 8 hours, 17 minutes
- **Days to produce:** Nov 11 - Jan 18 (68 days or 10 weeks)
- **Course sample code commits:** 78
- **Estimated time spent**: 60-80 hours (6-8 hours per week)

**Git Commit Punchcard**

![Git punchcard](images\2017-01-31-course-punchcard.jpg)

You can see clearly that the majority of time was spent in the evenings but hardly after midnight--there was one instance I had to stay up a bit late to finish. You can also see I took advantage of my holiday break (Thanksgiving and Christmas) to work during the day. Sun-Tue were coding days whereas Wed-Fri were mainly recording or content creation. Thu-Sat nights were almost always free, perfect for spending time with friends and getting [other work done](https://kamranicus.com/posts/2017-01-02-year-in-review).

Data is useful, especially to understand how much time goes into an endeavor like this--time is money, after all. For subsequent courses I plan to store *all* course material in Git (including slides/metadata) and also use something like [RescueTime](https://www.rescuetime.com/) to track my time spent in the background. That will give me a much clearer picture of time spent on the course.

## What's next?

While it would be ideal to do the follow-up course to this one immediately, I didn't know what my schedule would look like after my first kiddo arrives. The editors and I agreed that it didn't make sense for me to commit to doing another course so soon with such an unknown schedule. Once I have some idea of how much time I have, I'll be thinking about other courses. It's more important that I spend time with little Ayub first.

[1]: https://www.packtpub.com/application-development/introduction-typescript-video
