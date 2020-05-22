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

Since we could separate user-facing documentation into separate `.md` files, TypeDoc allowed us to use `[[include: file.md]]` directives.

## Migrating to Gatsby.js

This approach has been working well for us in the past years but recently we have been preparing to level up our documentation for the 1.0 release. Awhile back, I converted our site to be statically generated using [Gatsby.js](https://gatsbyjs.org) and this has proved to be a good decision. Gatsby allows us to customize all the aspects of the site including the way we generate documentation.

<!--stackedit_data:
eyJoaXN0b3J5IjpbLTY3OTE3OTEyXX0=
-->