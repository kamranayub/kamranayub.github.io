---
layout: post
title: "Designing Hexshaper, a game for the Ludum Dare 35 Game Jam"
date: 2016-05-07 20:10:00 -0600
comments: true
published: true
categories:
- Games
- Javascript
- TypeScript
- Excalibur.js
- Ludum Dare
- Game Jams
---

This last weekend I took part in the global [Ludum Dare 35](http://ludumdare.com) game jam. If you've been following me for awhile, you know I've participated in the past too. This time we [made a game called Hexshaper](http://excaliburjs.com/ludum-35)--where the goal is to fly around, absorbing magic to seal portals to another dimension to prevent monsters from overtaking the world. The backstory, while not communicated directly, informed our design--but it wasn't like that at first.

<!-- more -->

It's interesting to look at where the game was 24 hours into the competition because **it looks nothing like the final game**:

![Ship in space](https://cloud.githubusercontent.com/assets/563819/14938110/522d02f8-0edf-11e6-9329-7af08f8f9ecc.png)

A ship shooting bullets at vampire bats in space? That is *nothing* like a witch flying around inside a castle closing portals! 

![Witchcraft](https://cloud.githubusercontent.com/assets/563819/14938116/935df5ca-0edf-11e6-8694-507052c8f6cf.png)

How did that come to be?

## Let's make a shmup

This being our fourth LD jam, we have a process for how we do things now. Friday is spent brainstorming and coming up with ideas and then Saturday-Monday we build out our idea. This jam was a little different though--by the end of the night the group agreed we wanted to make a shoot 'em up ("shmup") but we didn't quite know what the game would be about. All the ideas seemed to be too involved for a jam:

- "Let's make an Ikaruga style shoot 'em up with enemies and levels"
- "Let's have different kinds of shooting styles"
- "Let's make different shields that absorb certain bullets"
- "Let's do a Geometry Wars style arena shooter"

The most important thing you can do during a jam is to scope your idea to something that makes sense. We know from experience we never make more than one level or develop different enemy designs--all of that is stuff you do **after** the game proves fun and interesting, which usually ends up being the last day :)

We ended up landing on the last two ideas Friday night but I don't think any of us really saw a coherent design in our heads. We knew the mechanics we wanted but that's about it.

## Designing yourself into a corner

We began working on the game Saturday morning. We made a space ship, we made monsters, we had shooting and we had an arena... but by the end of the night, I was frustrated and I think several other team members were too. There were too many open questions in my mind:

1. What was interesting about the game? 
2. How will shields work?
3. Why are there vampire bats in space?
4. Why are you shooting them?
5. It feels wrong to constrain *space*, space is open and vast, why are you trapped?

The problem was clear in hindsight and something that was nagging at me all throughout the day: **we did not have an internally consistent idea.** Our idea was too abstract and didn't tie together the reasons why the mechanics existed. There was no point of reference for the mechanics, the *why* of it all. Why were you a spaceship shooting bats in outer space? I don't know! None of us could explain why anything was happening.

So as I drove home that night I was frustrated and afraid--we just spent the entire day working on a game but I have no idea to what end. It just didn't feel right. We had a spaceship, we had a space theme, how the heck could this make any sense? I felt like we designed ourselves into a corner.

## Creativity needs some downtime

I find some of my best ideas, in game jams and outside of them, come when I give my brain time to think--driving home late at night, right before bed, etc. [This is a pretty well-known phenomena](http://www.bbc.com/future/story/20131205-how-sleep-makes-you-more-creative) and also why it's SO IMPORTANT to sleep during a game jam. Give your brain a rest. Your brain can pull together random stuff and tie it together when you push everything out and the time that *usually* can occur is before bed.

So it was like a brick in the face, when on my way home, I had an idea. An idea that I was so excited about, I couldn't sleep until I had made sure to note it all down. In the morning, I was pretty excited:

![image](https://cloud.githubusercontent.com/assets/563819/14938261/683c1bc0-0ee3-11e6-9da2-8e6481cdd1aa.png)

## The importance of an internally consistent theme

When the team assembled, I told them I had a way to tie it all together--with minimal changes to existing mechanics. I was a little hesitant, would they like it? I was excited but started to doubt myself a little--was it a bit too farfetched?

> "OK, what if you're a witch or a wizard and you're trying to make some kind of crazy potion. But then your dumb assistant bumps the ingredients and they all fall and mix together, creating interdimensional portals! The potions correspond to the shield types and you are trying to close the portals by killing the monsters. The arena is closed because you're actually INSIDE on a broomstick and you're in a castle. So the goal is to close all the portals to save the world!"

I had nothing to worry about. Everyone loved it and then **everyone** started to contribute their ideas:

- "What if the monsters and the portals are colored to show their association?"
- "What if we use shapes to denote the bullets and shield types?"
- "What if we did waves, so like the first wave introduces the player to the idea and then have more waves until you win?"
- "What if we showed an opening sequence where you bump all the potions into a pile and a HUGE PILLAR OF LIGHT AND ENERGY SHOOTS UP FROM THE GROUND AND THEN THE SCREEN FLASHES WHITE AND WHEN IT COMES BACK THE CASTLE IS IN RUINS!?" (that was how I saw the intro in my head)

You see, **when you have an idea that is internally consistent you can answer all these kinds of questions easily and things just make sense.** *Why are you flying around?* Because you're a witch! *Why are their monsters?* Because there are interdimensional portals! *Why do you have a shield?* They are hexes! *How do you close portals?* Absorb the magical energy to close them!

And then what happens is the theme helps get rid of mechanics you *thought* you wanted but turned out to just hinder the goal:

- Q: "Why are you shooting?"
- A: "I don't know, what if we get rid of shooting?"

And sure enough when we did the game felt more natural--instead of shooting working against the primary mechanic of changing shields, now the only thing you can do is absorb magic and run into monsters. That felt much better! The new theme brought together the rest of the design ideas: you're in a castle, there are portals, it should be fantasy-themed, etc. It all just tied together nicely and all it took was re-skinning what we had Saturday night.

An internally consistent theme is extremely important in any game for obvious reasons:

- It explains the internal lore of the game
- It energizes team members
- It gets the creative juices flowing
- It defines what the game will be

Think of even the most basic game and try to find the theme--you'll see that the mechanics and design all play off each other, even if the theme is never communicated formally. This realization isn't anything new--I've read the [Art of Game Design](http://www.amazon.com/Art-Game-Design-book-lenses/dp/0123694965) and many of the "lenses" discussed help you design a coherent theme. It's just something that can be hard to land on within 72 hours and easy to lose sight of in the excitement of a game jam.

## Think and work in the abstract until the theme reveals itself

This experience also reinforces the value of *focusing on the mechanics first* rather than theming right away. One of the first things we did Saturday was design the spaceship! Instead, we should have focused on the mechanical ideas abstractly and *then* let the theme reveal itself. By using abstract design first, our minds could have been able to make connections faster to think of a consistent theme. When you're looking and playing with grey boxes, it becomes easier to imagine what the game might be like. When we decided to do art first, we pigeon-holed **our brains** into believing the final game needed to be space-themed rather than it being open-ended.

If we had decided on a more consistent theme before we started development, we could have probably done even more by the end of the jam. When we designed [Sweep Stacks](http://playsweepstacks.com), we knew exactly what the mechanics would be--so the theme was evident and we had scoped it to what we could finish in a jam. Sometimes what can help (and we didn't necessarily do this in this jam) is to write on a board the mechanics you want, then try to come up with creative ways they tie together. Don't spend too much time on one idea--someone should say let's move on and the group discusses a different idea entirely. This way, you can kind of avoid designing yourself into a corner by exploring all sorts of other ideas and how they relate to the mechanics you want.

Only by a lucky burst of creativity, "Ship Shape" became Hexshaper and turned out to be even better than we first thought.
