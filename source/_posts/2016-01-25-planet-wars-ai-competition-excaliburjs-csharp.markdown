---
layout: post
title: "Planet Wars AI Competition with C# and Excalibur.js"
date: 2016-01-25 12:30:00 -0600
comments: true
published: true
categories:
- Excalibur.js
- Javascript
- Typescript
- Typewriter
- C#
- AI
- Games
---

![Planet Wars](https://zippy.gfycat.com/BraveBlushingImpala.gif)

This past weekend [Erik](http://twitter.com/erikonarheim) and I built out a [Planet Wars](https://github.com/eonarheim/planet-wars-competition) server (written in C#) and an [Excalibur.js](http://excaliburjs.com)-powered visualization (written in TypeScript). Planet Wars is an AI competition where you build an AI that competes against another player to control a solar system. A map consists of several planets that have different growth rates and an initial number of ships. You have to send out a "fleet" of ships to colonize other planets and the player who controls the most planets and has destroyed their opponent's ships wins the game.

At work we are hosting our 6th Code Camp and recently we started hosting an AI competition internally. You can find past competition agents for [Ants](https://github.com/eonarheim/AntAICompetition) and [Elevators](https://github.com/eonarheim/BellTowerEscape), for example.

The [visualization for Planet Wars](https://github.com/eonarheim/planet-wars-competition/tree/master/PlanetWars/Scripts/game) is fairly simple, made even simpler using the power of [Excalibur.js](http://excaliburjs.com), the engine we work on during our spare time. We basically just use an Excalibur timer to query the status of the game state and update the state of all the actors in the game. For moving the fleets, we just use the [Actor Action API](http://excaliburjs.com/docs/api/edge/classes/ex.actioncontext.html).

For the [game server](https://github.com/eonarheim/planet-wars-competition/tree/master/PlanetWars/Server), we are using a [HighFrequencyTimer](https://github.com/eonarheim/planet-wars-competition/blob/master/PlanetWars/Server/HighFrequencyTimer.cs) to run a 30fps server and then clients just send commands via HTTP, so any kind of agent will work like Python, Perl, PowerShell, or whatever! Anything that speaks HTTP can be a client. The server runs in the context of a website so we can easily query the state using a singleton `GameManager`. This wouldn't work in a load-balanced environment but it doesn't matter since people develop agents locally and we run the simulations on one server at high-speed to produce the results. If you backed the server with a data store, you could replay games but right now there's only an in-memory implementation.

To keep the server and client models in-sync, we use [Typewriter for Visual Studio](http://frhagn.github.io/Typewriter/index.html) which is **amazing** and super useful not just for syncing client/server but also generating web clients, interfaces, etc. from C# code. I plan to write a separate post on some Typewriter tips for Knockout.js and Web API.
