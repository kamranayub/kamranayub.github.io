Title: "New Pluralsight Courses on Azure Storage with CDN and CORS Published"
Published: 2099-10-01 09:00:00 -0500
Lead: Excited to announce two brand new Pluralsight courses on using Azure Storage with Azure CDN and CORS.
Tags:
- Azure
- Pluralsight
- Courses
---

This last week or so I've released not one but **TWO** Pluralsight courses I've been hard at work on the past few months:

**Implementing Azure CDN with Azure Storage**

![Azure CDN course link](https://user-images.githubusercontent.com/563819/47162935-95932a80-d2ba-11e8-8d95-a037ca745333.png)

[Watch the course now](http://bit.ly/PSAzureStorageCDN). This course covers a lot of how Azure CDN works and various ways to integrate it with Azure blob service. I walk through how Azure CDN works at a high-level, then dive down into ways to automate the creation of CDN profiles/endpoints and how to serve content from blob storage. I cover Azure PowerShell, Azure CLI, REST, and ARM templates. I also spend a whole module on serving content securely using Shared Access Signature (SAS) tokens in different ways including pass-through authentication, hidden SAS authentication, and CDN token authentication.

**Configuring CORS with Azure Storage**

![Azure CORS course link](https://user-images.githubusercontent.com/563819/47162956-9f1c9280-d2ba-11e8-9109-b93dfe96dee5.png)

[Watch the course now](http://bit.ly/PSAzureStorageCORS). This course covers configuring Cross-Origin Resource Sharing (CORS) with Azure Storage accounts. First, we spend time on what CORS is, how it works, what it's **not** (hint: it's not a way to do authentication!), and then how Azure evaluates CORS rules internally. Then I walk through several automation scenarios in Azure PowerShell, Azure CLI, and REST. By the way, I've [open sourced a Postman collection][postman-collection] that handles signing Azure Storage REST requests, if you're interested.

## Building two courses at once

As you may know, I [released a RavenDB 4 course][ravendb-course-post] earlier this year so this was not my first experience making a course. But it **was** my first time doing two courses at once!

I had initially started this process by meeting with a Pluralsight "author success manager" (aka ASM) at NDC MN on the speaker boat cruise (which reminds me, [register for NDC MN 2019][ndc-mn]!). We got into a conversation about what courses I'd do in the future and I mentioned I was thinking of diversifying my next set. She asked, "Do you know anything about Microsoft Azure?" and I was like, "Yeah!" and as they say, the rest is history.

We decided early on that because these two courses were similar that I could potentially do both at once. I was really happy my ASM believed I could do it but I wasn't sure I could. I knew that I had a hard deadline of November as that's when the next baby comes along (it's *too* close now...). At the time we were discussing it, it was May/June. These courses were a little special in that they are considered "partner" courses with Microsoft. So the course proposal process was different--the proposals had to be vetted by a Microsoft SME and then the peer review process would be replaced with a Microsoft SME review process. I knew this would slowdown the whole process and that it would take longer to do these courses. 

Still, the opportunity to do **two** courses was not something I'd be offered every time and it would be in-line with my 2018 stretch goal of doing two courses. So I said yes. But I knew I had to plan these courses differently than my first due to the tighter timelines.

## Task planning and time tracking

Let me start by telling you that building these two courses took exactly 112 hours. I **know** this because I was religiously tracking time. I had also created a task plan for each course at the start, here's what my Trello cards looked like:

![Trello cards](https://user-images.githubusercontent.com/563819/47169134-553aa900-d2c8-11e8-8cb2-5608d42ab953.png)

You can see the duration for each card and the number of tasks I was tracking. I used the Trello Power-Up, [TimeCamp][timecamp], to track time by just clicking a button on the Trello card. I really liked how easy this was; I always had my cards up anytime I worked on the courses and I'd just flip between them tracking time.

Now, using this data, I can actually see what the cadence and punch-cards look like for each course.

**TBD**

The task planning was straightforward. I planned out each step of the production process:

1. Proposal tasks
1. **for each module**
  1. Content creation for each clip, then tasks for assessments / materials
  2. Recording & editing for each clip
1. Post-production tasks

In practice, here's what that looks like:

![Trello tasks](https://user-images.githubusercontent.com/563819/47169804-173e8480-d2ca-11e8-954d-fa04cb8f8dad.png)

This was really helpful--not only to plan but also to track progress day-to-day.

[postman-collection]: https://github.com/kamranayub/azure-storage-rest-postman
[ravendb-course-post]: https://kamranicus.com/posts/2018-02-08-ravendb-4-course-live
[ndc-mn]:
[timecamp]: https://trello.com/power-ups/59cf411fe342369bca2565b9/timecamp