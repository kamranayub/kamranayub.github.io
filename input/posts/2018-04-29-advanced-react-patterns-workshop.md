Title: "Frontend Masters: Advanced React Patterns Workshop"
Published: 2018-04-29 20:18:00 -0500
Lead: I attended the in-person workshop for Advanced React Patterns at Frontend Masters
Tags:
- Workshop
- Frontend Masters
- React
- JavaScript
---

A couple weeks ago I went to the in-person Frontend Masters workshop with Kent C. Dodds on Advanced React Patterns.

I was looking forward to it, I've followed Kent for awhile and knew his workshops were well-received. I'm doing React at work and it was perfect timing for our project.

## Takeaways

The gist of the workshop was state management and props passing patterns in React, to be able to build smarter, flexible components.

We centered around a toggle component that showcased each of the patterns, some isolated and some working in tandem.

**Context API**

I hadn't really been exposed to the context API before React 16.3 so this was brand new to me. They really have made it much easier to use now.

The Context API makes it straightforward to pass data around throughout your app without forcing you to pass props down through the component tree (called "props drilling").

**Render Props**

This pattern has come into fashion pretty recently I'd say. When I started learning React over a year ago I hadn't seen this pattern used much. Before attending the workshop I had used this pattern in practice in KTOMG to build analytics/social sharing providers that encapsulated some behavior like respecting Do Not Track across the site.

The pattern is useful for building flexible UI components where you can encapsulate behavior and state but delegate rendering (or parts of rendering) to the consumer.

Render props is powerful and in a lot of cases provides most of what you'd need in a flexible component.

**Control Props**

This is a pattern to delegate the setting of state to "control props." This provides flexibility to a component to delegate control of its state to a consuming component vs. setting internal state only.

Whenever the component is intending to use `setState`, it checks whether that state is *controlled* (meaning, it's being passed in as a prop). If it is, it just calls a callback with what *would* be the new state value. If it's not controlled, it goes ahead and sets state normally.

This is useful in situations where you as the caller want to control the state yourself--it means though that you basically have to know how the internal state works of the component you're controlling.

**State Reducer**

This is a more advanced pattern Kent showed. The goal of this is to enable a consuming component to manipulate the internal state of a component by basically intercepting state changes.

It's a more flexible version of control props basically allowing your entire state to be controlled by a consumer.

There's an [example in the workshop repo](https://github.com/kentcdodds/advanced-react-patterns-v2/blob/master/src/why/state-reducer.js).

## Rendux Exercise

I missed the last couple exercises going over Higher Order Components (HOCs) and this last exercise called [Rendux](https://github.com/kentcdodds/advanced-react-patterns-v2/blob/master/src/exercises-final/13.js).

The Rendux exercise uses the render props pattern to hook into a Redux store as a reusable component. Any time you need to access the store, you can use the `Rendux.Consumer` component or `withRendux` enhancer function. This is a different way to accomplish the similar Redux experience of using `Provider` and the `connect` enhancer.

## Putting into practice

The goal of the workshop was to learn each of these patterns on a small scale so we can apply them to our larger projects. There is certainly already opportunities on my project at work to leverage what I learned--I'm hoping to spend some more time building out our project and then writing about some useful patterns.
