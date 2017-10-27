Title: Frontend Masters Workshop: Node API Design v2 with Scott Moss
Published: 2017-10-27 14:02:00 -0500
Lead: Scott taught us some Node.js API design approaches including using GraphQL
Tags:
- Workshop
- Frontend Masters
- Personal Development
---

On Oct 16 & 17 a fellow coworker and I attended a [Frontend Masters](https://frontendmasters.com) workshop in person on [Node.js API Design by Scott Moss](https://frontendmasters.com/workshops/api-design-in-node-v2/).

I love FEM workshops (this is my second time in-person) since they're pretty hands-on and you can interactively ask questions. It's awesome that they are FREE to attend in-person if you get chosen and I've been lucky enough to attend both times I applied. It's only a block away from my work and bus stop in downtown Minneapolis.

## What was it about?

The workshop was on how to build native Node.js APIs using Express with both REST-style endpoints *and* a single [GraphQL](https://graphql.org) endpoint.

The code for the workshop is up on GitHub: https://github.com/FrontendMasters/api-design-node-v2

## Why was it valuable?

I've been doing Node for awhile but it's only been since I started my new job I've been doing it full-time so I definitely wanted to brush up on the latest practices. That's why I find FEM workshops valuable--they bring in experts that have *practical* experience in the technologies so they can share their own approaches to the topic.

On my current project we use Node + Express stack heavily and I wanted to get a sense of how we were doing compared to the latest practices. It turns out there's some opportunities!

## What did I learn?

- Using Hot Module Replacement (HMR) with a vanilla Node.js server. Most of the documentation and examples around Webpack HMR are in the context of React but it turns out you can set it up to be used in a vanilla Express app too. See [server's index.js](https://github.com/FrontendMasters/api-design-node-v2/blob/master/src/index.js).

- I especially appreciated Scott's guidance on using multiple Express routers together. Right now our project configures a single router but the cool thing about Express is you can actually create specific Routers that handle a few things then *compose* them together through the middleware pipeline. [Example importing `restRouter`](https://github.com/FrontendMasters/api-design-node-v2/blob/lesson-5-solution/src/server.js).

- Seeing how to handle API errors via middleware was valuable. Right now we don't really have a catch all handler, so I've already proposed a solution using what I learned from the workshop. In our case, we're using `request-promise` package so our requests are async and we need to handle unhandled rejections from Bluebird. [Example API error handler middleware](https://github.com/FrontendMasters/api-design-node-v2/blob/lesson-5-solution/src/api/modules/errorHandler.js).

- Seeing `async/await` in action. I come from C# so I've been using `async` and `await` with .NET for a long time but it's *finally* available in JS. The project is using Babel to transform the async/await into generators. I liked seeing examples of how to use it with promise-based APIs. [See webpack config](https://github.com/FrontendMasters/api-design-node-v2/blob/master/webpack.config.js)

- **[GraphQL](https://graphql.org)**! I was most excited about learning some *actual* GraphQL. So there are a few things that started to make sense after going through the workshop:
    - GraphQL itself is not a *thing*, it's a specification. There are various implementations for different frameworks/languages.
    - GraphQL has a single endpoint. Versioning is done through the query. GraphQL makes you define a schema using Flow/Typescript-like "types" (see [example](https://github.com/FrontendMasters/api-design-node-v2/blob/master/src/api/resources/playlist/playlist.graphql)) using a query language (hence GraphQL) for all inputs and outputs, and inputs are treated separately as "mutations."
    - GraphQL does **not** dictate how the server handles associations; you handle it through functions called *resolvers.* If you've used Redux, it's a bit like reducers--they manage a single slice of the schema. So you might resolve an association through an external API, database join/include, in-memory cache, etc. It's totally up to you. The beauty is the consumer doesn't care and just requests what they need.

    After writing some GraphQL and getting hands-on, I'm a believer. I loved it. It feels really good to design an API using GraphQL types and way more natural than REST design did; I don't think I've ever seen a PURE RESTful API design--it's always been weird hybrid stuff, even my own APIs I design return view models, handle contextual responses, etc. As soon as I can make it work, I'm switching to [GraphQL.NET](https://github.com/graphql-dotnet/graphql-dotnet) for my own work. I'm pretty certain RavenDB will be a very good companion to a GraphQL backend. A **G**QL + **R**avenDB + **Rea**ct + .**N**ET Core stack sounds mighty fine to me (hmm, the GRREAN stack?).

    For my job, we do a lot of chatty API calls to retrieve data from various endpoints. The thing is that our endpoints are usually single-purpose which is *good* but because of that we have to make multiple calls or hit multiple endpoints for our UIs/backend to the data they need which isn't good. There are ways to help solve that using API gateways, proxies, etc. but GraphQL provides an easy-to-consume abstraction layer that lets you choose how to handle it within your business domain with standard rules. I love it.

    Part of me that likes the open web and HTTP standards wonders about the singular endpoint and lack of URL design but the other part of me that wants to get shit done is currently winning.
    
    Scott summed up GraphQL pretty succinctly, "I'm never making a REST API again."

- I now use [HTTPie](https://github.com/jakubroztocil/httpie) instead of curl

So that's that! I probably can't enumerate every little thing I learned but that's what stuck out to me as I reflected on it for this post.
