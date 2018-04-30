Title: "Follow-Up to .NET Rocks episode on RavenDB"
Published: 2018-05-01 06:00:00 -0500
Lead: This last week I was honored to have the opportunity to be on one of my favorite podcasts, .NET Rocks
Tags:
- Podcast
- .NET Rocks
- RavenDB
- Post-Mortem
---

At the end of April I got the opportunity to record a show with .NET celebs Carl Franklin and Richard Campbell, hosts of the [.NET Rocks][dotnetrocks] podcast. It's [live now][episode]. 

I tweeted about it afterwards:

<blockquote class="twitter-tweet" data-lang="en"><p lang="en" dir="ltr">There really isn&#39;t anything more surreal than being on a podcast you&#39;ve listened to almost your entire career and then having to try to make sense and articulate your thoughts on said podcast. MAN, what a ride. 😆</p>&mdash; Kamran Ayub (@kamranayub) <a href="https://twitter.com/kamranayub/status/989605608541376513?ref_src=twsrc%5Etfw">April 26, 2018</a></blockquote>
<script async src="https://platform.twitter.com/widgets.js" charset="utf-8"></script>

It was fun and surreal for sure but also nerve-wracking--it was my first podcast appearance!

## How it came about

A few months ago I emailed Carl and Richard about interest in doing an episode on RavenDB 4, this was right after I [published my course][ravendb4-course].

I asked them if they'd have any appetite for a Raven 4 episode, that I just finished a course on it, and I've been a user since the 2.5 days. I suggested they could get Ayende or someone from the team on but that I'd be open to it, too. I didn't *really* expect to be invited, after all...

After I sent it, I didn't think about it much. Then, a week ago, Richard emails back and says they'd like to do an episode with someone who's been a user, which means they invited me!

At this point, I'm honestly freaking out a little. Me? On .NET Rocks? Oh yes! Oh no. Oh man.

We recorded the episode a few days later and you [can listen now][episode].

## Hindsight is 20/20

By far my least favorite thing about speaking in public, or really speaking in general, is the analyzing that happens inevitably afterwards. Even when you know a subject, in the moment it can be tough to bring forward everything you've learned. I'm sure it gets better with practice.

But you *can* follow-up with those things you missed. So I wanted to provide some thoughts and append to my answers on the show.

## Raven 4 Performance

On the show I was asked about performance and I offered that in some testing I did, I saw time to rebuild a full index go from 60 seconds in 3.5 to just only 3 seconds in Raven 4. That blew my mind.

I mentioned that I was seeing similar query times but that the "warm up" time on 3.5 was much larger. Here's some query performance tests I ran:

    Query                             Raven 3.5.4     Raven 4.0.3
    ---                               ---             ---
    Get game by name (skyrim)         0ms (126ms)     1ms (20ms)
    Get game by name (mario)          2ms (716ms)     1ms    
    Get game by search (search)       32ms (470ms)    1ms
    Get game by genre (Array)         1ms (635ms)     1ms
    Indexing 45,000 entries           60 seconds      3 seconds

All times are for query duration (i.e. no network latency). 

- The times in parenthesis are "warm up" (first query) times. You can see for the Raven 4 tests, there was little to zero measurable warm up time.
- The first test showcases response time when there's only one game.
- The second has multiple results
- The third is a "search" operation which is a Lucene search
- The fourth test is searching an array property for an exact match

Other observations I saw:

- Network latency locally was about 7ms in 4.0. Measured in Chrome dev tools within the Studio.
- Querying while another index was rebuilding in 3.5 added significant overhead to all queries above (400-700ms). It had no visible impact on Raven 4.

So from what I can gather on a limited set of tests is that Raven 4 is indeed *very fast*.

## Memory usage

The guys asked me a good question: what is the memory performance of Raven 4 compared to its predecessors? I didn't have any hard data but I offered the anecdote that my current VM host only has 4GB of RAM and runs the website alongside the DB. This is a poor model for redundancy but I'm penny pinching until I move to .NET Core on Linux. Anyway, Raven 3.5 runs okay on low-end hardware and I expected Raven 4 to be even better.

After the show I just did some simple checks. Turns out, Raven 4 **is** better. 

The following numbers are sitting idle after having done several queries and tests mentioned above. Both have just have a dev version of my KTOMG database running:

    Raven Version      Commit       Working Set
    3.5.4              1.035GB      90MB
    4.0.3              607MB        260MB

So at a glance it looks like Raven 4 is better at managing physical memory (Commit) than 3.5.4 is. To do that it looks like it has a higher working set. Even totaled, 4.0 still allocates 22% less. For someone like me who wants to make the most out of a small cloud VM, this is good.

## On Raven vs. Mongo

I prepped for the show and in doing so I tried to study up on Mongo a bit. Like I said in the show, I'm definitely not an expert in it nor do I use it extensively. I've written some queries against it, modified documents, and that's about it!

It's easy for this question to turn into a war, people have strong opinions. I haven't had an opportunity to use Mongo much in practice. We use it for our app at work; it's used for some configuration, user sessions, and preferences. It works really well. I don't even think we have any indexes, it was designed before I joined the team. 

Since I came from a .NET background, I tried Raven first and it met my needs. Raven 4 really has been impressing me as I use it more. When choosing between technologies I find it's best to just try each technology to see if it fits your use case. Ask yourself what you value most and measure that when testing.

## Challenges moving to a document database

On the show I talked about a couple challenges people might have when moving from a relational to document-based database--document modeling and indexes. Those are both real challenges but I was wracking my brain to think of more.

After the recording I immediately realized there was a lot more to this I should have mentioned (dumb brain!). I think one thing I struggled with a lot when starting out with Raven was just patterns and practices. How do I do this one thing I used to do with EF? 

One example I go over in [my course][ravendb4-course] is unique constraints. In EF, you'd just issue a query asking if an entity with a property equal to some value exists:

    context.DbSet<User>().Any(u => u.Username == "kamranicus")

Great, all set. When I moved to Raven, I didn't understand the total implications at first about indexing and eventual consistency. This type of query in Raven **is** possible, like this:

    session.Query<User>(x => x.WaitForNonStaleResults())
      .Any(u => u.Username == "kamranicus")

The problem is that from the beginning the guidance is to avoid `WaitForNonStaleResults()` at all costs since it has performance implications (see my course, I explain it). Well, if you *don't* do it, then your query isn't guaranteed to bring back up-to-date results (i.e. eventually consistent) and you will not get the result you expect.

Instead, you have to lean on the fact that document operations are ACID-compliant and hack your brain to think of unique constraints as *actual documents.* This is what the [unique constraints bundle](https://ravendb.net/docs/article-page/3.5/Csharp/server/bundles/unique-constraints) was for in Raven 3.5 and below. Now in Raven 4, you'd use an [interlocked distributed operation](https://ayende.com/blog/180067/ravendb-4-0-interlocked-distributed-operations), which I've yet to try in practice.

I'm hoping to address more of these patterns/practices in a follow-up course.

## Enjoy the show

Like I said in that tweet, it was completely surreal to be on the show. I'm still in shock I think and I'm not sure what to expect for response. If you listened, hopefully you learned something new. By next winter, I'll get that snowblower.

[dotnetrocks]: http://dotnetrocks.com
[episode]: https://dotnetrocks.com/?show=1541
[ravendb4-course]: https://kamranicus.com/posts/2018-02-08-ravendb-4-course-live