Title: "NDC MN 2019: Designing a Flexible UI Architecture with React and GraphQL"
Published: 2019-02-01 21:38:00 -0500
Lead: I was accepted once again to NDC Minnesota!
Tags:
- Architecture
- GraphQL
- TypeScript
- React
---

[previous]: /posts/2018-05-11-ndc-minnesota-typescript-react
[talk]: https://ndcminnesota.com/talk/designing-a-flexible-ui-architecture-with-react-and-graphql/
[cypress]: https://cypress.io
[apollo-bug]: https://kamranicus.com/posts/2018-03-06-graphql-apollo-object-caching
[pubconf]: https://pubconf.io/

I'm excited to [once again][previous] be invited to [speak at NDC Minnesota][talk]!

Here's the what I'm speaking about:

### Designing a Flexible UI Architecture with React and GraphQL

> Many line-of-business apps we build today are "forms over data" applications. That data has its own backend schema and business rules, used by systems across the company. As that data flexes and scales, so do the needs of our UIs. Trying to manually track and maintain those data type changes in our UIs is time consuming and inevitably leads to bugs that cause data quality issues and results in real business impact.
>
> Instead why don't we introduce our own UI schema that abstracts the myriad backend data sources with common sets of rules and metadata shared with both the client and server? I will show you how we designed and built a modern schema-driven UI architecture with React, Redux, TypeScript, and GraphQL that could manage business complexity and still scale to meet future data requirements. 
>
> I'll share some lessons learned and advice for your own applications so you can design your own data-driven flexible UI architecture that fits your business needs.

I'm real excited about this talk because it's the culmination of exactly a year's worth of learning and effort at my job--rewriting part of an application from AngularJS to React/TypeScript. 

In the process I became intimately familiar with how to build production React applications, using TypeScript with React more intensely, testing with Jest and [Cypress][cypress], and working with Apollo Server GraphQL (like fixing [weird bugs][apollo-bug]). We solved some really interesting business problems that I'll dive into during the talk and ultimately created an elegant solution that has scaled nicely in terms of maintenance and agility for our product.

Hope to see you there, please stop me and say hello! If you can, try to [attend PubConf][pubconf] too, it's a **ton** of fun.