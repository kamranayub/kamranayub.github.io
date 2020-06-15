Title: "Building a TypeDoc-powered Gatsby Documentation Site"
Published: false
Lead: "We recently migrated our user-facing documentation to Gatsby and it's still powered by TypeDoc symbols"
Tags:
- JavaScript
- TypeScript
- Excalibur
- Gatsby
---

I help maintain the [Excalibur.js](https://excaliburjs.com) web-based game engine. We started Excalibur back in 2011 and it was always written from the ground up in TypeScript. Since Excalibur hasn't yet reached 1.0, all of our documentation has been in the source code. Luckily, early on we started to adopt a tool called [Typedoc](https://typedoc.org) which could generate a rich documentation site for TypeScript-based projects.

## Linking to API Symbols

Using TypeDoc, we could build richer documentation with embedded Markdown documents. It also has a compelling feature to do [link to API symbols](http://typedoc.org/guides/doccomments/#symbol-references) using a `[[symbolName]]` or `{@link symbolName}` syntax. This was awesome because we could write "user-facing" documentation and easily create maintainable links to the raw API symbols.

Since we could separate user-facing documentation into separate `.md` files, TypeDoc allowed us to use `[[include:file.md]]` directives to keep our documentation separate from the source code.

This approach has been working well for us in the past years but there were several downsides:

- The "user" documentation was locked into the TypeDoc site and theme
- While TypeDoc was customizable, it was still hard to do more complex things like custom components (think MDX) or rendering
- It just wasn't cohesive--we had a main site and you'd expect to find docs hosted under there. It would take a lot of work to customize the TypeDoc theme enough to make it feel like a natural part of the site.

We needed to do something different and _ideally_ have these user docs hosted within the main site **but still maintain the ability to easily link to API docs**.

## Migrating to Gatsby.js

 Awhile back, I converted our site to be statically generated using [Gatsby.js](https://gatsbyjs.org) and this has proved to be a good decision. Gatsby allows us to customize all the aspects of the site including the way we generate documentation.

Gatsby uses a GraphQL-based sourcing architecture where you can add any kind of "source" of data--this could be Wordpress, the GitHub API, or basically _any_ external piece of data you wanted. These source plugins take the data from one place and _transform_ it into GraphQL nodes that Gatsby can understand and make available to your pages statically. This makes Gatsby incredibly versatile and customizable using a consistent architecture.

Gatsby also has the idea of _transformers_. Transformers take input, usually an Abstract Syntax Tree (AST) and run it through processors that make changes. For example, there's [gatsby-transformer-remark]([https://www.gatsbyjs.org/packages/gatsby-transformer-remark/](https://www.gatsbyjs.org/packages/gatsby-transformer-remark/)) that takes Markdown AST and parses it using Remark.

Using a combination of _sources_ and _transformers_ you can basically transform one source of data into whatever you want as the output.

## Creating a TypeDoc Gatsby Source

When we migrated our user documentation, we didn't want to lose the ability to link to the API symbols using the convenient `[[symbol]]` syntax. In order to maintain that, we needed a way to source the TypeDoc JSON (or AST) into our Gatsby site.

For that purpose, I made an npm package [gatsby-source-typedoc](https://npmjs.com/package/gatsby-source-typedoc). This will allow you to run TypeDoc against a TypeScript project and it will take the generated structure and store it as a GraphQL node for querying within your Gatsby app.

For example, here's what [Excalibur's Gatsby config looks like](https://github.com/excaliburjs/excaliburjs.github.io/blob/site/gatsby-config.js#L10):

```js
plugins: [
  {
    resolve: 'gatsby-source-typedoc',
    options: {
      src: [
        `${__dirname}/ex/edge/src/engine/index.ts`,
        `${__dirname}/ex/edge/src/engine/globals.d.ts`,
        `${__dirname}/ex/edge/src/engine/files.d.ts`,
        `${__dirname}/ex/edge/src/engine/excalibur.d.ts`,
      ],
      typedoc: {
        target: 'es5',
        mode: 'modules',
        experimentalDecorators: true,
        excludePrivate: true,
        tsconfig: `${__dirname}/ex/edge/src/engine/tsconfig.json`,
      },
    },
  },
]
```

This will then allow you to query for the TypeDoc JSON content in a Gatsby page:

```js
export const pageQuery = graphql`
	typedoc: allTypedoc {
		edges {
			node {
				id
				internal {
					content
				}
			}
		}
	}
`

export default function MyPage({ data: { typedoc } }) {
	const typedocContent = JSON.parse(typedoc?.edges?.[0].node.internal.content;
	
	// do something with that data...
}
```
<!--stackedit_data:
eyJoaXN0b3J5IjpbODUwNjU5ODY1LC04OTA2Mjk5NCwtOTcyOT
A4OTYyLC02NzkxNzkxMl19
-->