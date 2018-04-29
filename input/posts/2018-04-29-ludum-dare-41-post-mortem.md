Title: "Ludum Dare 41 Post-Mortem"
Published: 2018-04-29 08:00:00 -0500
Lead: The Excalibur.js team made another game for Ludum Dare
Tags:
- Games
- Ludum Dare
- Game Jam
- Post-Mortem
- Excalibur.js
---

Whoo! Another year, another opportunity to participate in Ludum Dare with the [Excalibur.js][excalibur] team. Last jam we made [I Just Wanted Groceries][ld38] (which I totally spaced on writing about). You can play our LD41 game [Office Daydream][ld41] right now.

## The jam

[Ludum Dare][ludum] is a global game jam that has two modes: the Compo and the Jam. The Compo is for individuals and strict rules on content creation, limited to 24 hours. The Jam is for teams and is a bit looser on content rules (you can use premade assets) and lasts 72 hours. Since we are a team, we always do the jam but it's always interested me to try doing a Compo by myself at some point.

## The theme

The theme this jam was not as great as we hoped--there were certainly some better ones in the running but it was a challenge and we made the most of it. The theme was "Combine 2 incompatible genres."

First, we didn't like the term "incompatible" because that already presupposes the two genres you choose can't have been considered compatible today and that they would never be compatible in the future. But that's nonsense because I'd DEFINITELY play [this typing FPS][jeff] for realsies if it ever made it into a production game.

Second, we like open-ended themes more than specifically scoped themes. For example, previous ones we participated in included "Under the Sea", "A Small World", and "Shapeshift." These all leave much more open interpretations.

Still. The theme forced people to come up with some interesting combinations like this basketball + Connect 4 game. Constraints breed creativity, even if there were some really effing weird ones.

## Brainstorming

We played with a lot of combinations. We started by listing game genres, then movie genres, then music genres. Some we combos we liked but knew we didn't have time for, mainly RPG-based or turn-based.

It kept coming back to a runner + idle game theme. The dichotomies of a fast-paced runner with a casual idle game was attractive. They were incompatible in terms of mechanics and style. We didn't want to "mix" two genres, we really wanted to have a distinct separation between the genres.

We landed on either a librarian dreaming of things she read in the books she was putting away or a bored office worker. The office theme was strong and we didn't have enough time to build out a full themed set of art for different book genres.

And so, Office Daydream was born. You were just a person doing menial office tasks dreaming of being a motorcycle stunt action hero.

## Coding

This jam was different from previous jams because we were at our teammate's cabin and were essentially on vacation. My wife and son were there, other spouses were present. We knew we'd only be working 8 hour days to spend evenings with family.

This turned out to work super well--and even when I had to leave halfway through on Sunday, the team managed to get everything done that we had scoped for. We ended up starting around 8-9am, taking an hour lunch break, and finishing around 5pm.

Some things we did ahead of time that ensured we met this schedule:

- Spend Friday after theme announcement *just* focusing on design and mechanics and idea generation to have a solid goal by the end of the night
- Scoped all mechanics/design to a minimal goal (MVP) with stretch goals
- Had all our tooling/setup in place beforehand so we could sit down and work immediately
- Used Trello like we have in the past to manage work

## Design

We initially had thought you'd control the runner via keyboard and the office minigames with your mouse. But then we decided to use the mouse for all interaction because it forced you to focus on one thing at a time, making it harder.

We also didn't really know what we'd do for an "endgame" until we realized the theme decided for us. You're in an office. You typically work an 8 hour day. So, each minigame will last essentially a portion of that time and when you finish them, you end your work day successfully daydreaming.

## Assets

I spent a little time and made the background for the runner portion. I went with a cyberpunk-ish/Bladerunner cityscape theme. It worked pretty well! Just simple fixed width skyscrapers that could be randomly tiled horizontally to make the background more dynamic.

I also found the motorcycle asset and designed the art for the paper collation minigame (the mug I did was replaced by Erik's better mug). I'm no artist and I'm very glad Erik took care of the rest of the art. If we need anyone else on the team, it's a dedicated artist.

## The end

Every game jam is a ton of fun and I always learn something new about game dev. It was nice not feeling the "crunch" of 12 hour days, I feel we might adopt the 8 hour cycle in future jams more often now that we know we can accomplish what we set out to build.

[projects]: /projects
[ludum]: http://ldjam.com
[excalibur]: http://excaliburjs.com
[ld38]: https://blog.excaliburjs.com/post/161334104662/ludum-dare-38-postmortem
[ld41]: http://excaliburjs.com/ludum-41
[jeff]: https://ldjam.com/events/ludum-dare/41/jeff-from-accounting