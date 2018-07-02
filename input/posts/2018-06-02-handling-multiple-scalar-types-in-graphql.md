Title: "Handling Multiple Scalar Types in GraphQL"
Published: 2018-06-02 11:55:00 -0500
Lead: How to handle values that can be one of multiple scalar types in GraphQL
Tags:
- GraphQL
- JavaScript
- Apollo
- TypeScript
---

I've been using [GraphQL][graphql] for awhile now at work and I think it's pretty wonderful. I like how it has a consistent way to describe API contracts and has mechanisms to handle lots of different data fetching scenarios. Similar to other static type systems like TypeScript or Flow, in GraphQL you add type annotations to schemas for the server to enforce. If you need to describe a type as "this or that" like "string or integer", this is what's called a **type union.** The GraphQL spec supports type unions for objects but [it doesn't yet support scalar type unions][ghissue] (since 2016).

This is unfortunate and since scalar type unions are not yet supported, here's an approach that works well with my preferred GraphQL server, [Apollo][apollo].

## Describing Generic Values

In my scenario, we have multiple typeahead search fields that fetch values and pass them through our application untouched. There are half a dozen backend APIs that handle different kinds of data to search from and we use GraphQL to provide a consistent querying experience for the UI. Specifically, the raw `value` represented by these APIs can be literally anything--we have no idea what it could be and also, **we don't care.** This is key: the UI just passes this value through and eventually it gets saved back to these APIs, so we don't touch it.

The schema of this scenario looks something like:

```
query search($text: String!, $start: Int!, $resourceId: String!): Options

type Options {
  total: Int!
  start: Int!
  page_size: Int!
  options: [Option]!
}

type Option {
  display_value: String!
  value: ???
}
```

Imagine a dropdown where the `display_value` is what's shown to the user and `value` is the backing data of the option. The user never sees it, it gets stored in state somewhere, and eventually when the user saves it gets passed back up.

Now, since we use TypeScript in the UI, I can easily describe the `value` property like this on the client:

```typescript
interface Option {
  display_value: string;
  value: any;
}
```

To tie this back to unions, if I had more concrete information about what values get returned by these APIs, I could be more specific using a **type union**:

```typescript
interface Option {
  display_value: string;
  value: string | number | boolean;
}
```

On the GraphQL side to describe the `Option.value` property, what would be **ideal** is to do this:

```
type Option {
  display_value: String!
  value: String | Int | Boolean
}
```

or even:

```
type Option {
  display_value: String!
  value: Any
}
```

Unfortunately, neither of these type annotations are possible with the default GraphQL language specification. Many folks give up and just use `String` but that actually causes bugs with our scenario (numbers get converted to strings and so on) so we need to blaze our own trail.

## Custom Scalar Types

In Apollo and I assume most GraphQL implementations, [you can define your own custom scalar types][customtypes]. A scalar type is just a primitive type meaning it has no additional properties like a regular "object" would in GraphQL. One custom scalar we use often is the [JSON scalar type][json] for generic payloads but this doesn't work with primitive types like we need here.

What we need to do is define our own custom type that can resolve our value. In my case, I need an `Any` type that really just passes the value through. This is *frowned upon* in the GraphQL world because everything *should* be able to be described statically. I agree with the philosophy but I'm also pragmatic and the real world always has exceptions to these guidelines. So let's make it work.

In your GraphQL schema, define your new scalar type:

```
type Any

type Option {
  display_value: String!
  value: Any
}
```

Now we've declared a new type, so we need to implement it using what's called a [type resolver][resolver]. The implementation for our `Any` type is dead simple:

```js
// resolvers.js
import { GraphQLScalarType } from "graphql";

export default {
  Query: {
    search() {
      return ...
    }
  },
  Any: new GraphQLScalarType({
    name: "Any",
    description: "Literally anything",
    serialize(value) {
      return value;
    },
    parseValue(value) {
      return value;
    },
    parseLiteral(ast) {
      return ast.value;
    }
  })
}
```

**Note:** `ast.value` is always a string but I haven't seen a case in consuming a GQL API where `parseLiteral` comes into play--it may with the Introspection API. For our purposes, `serialize` and `parseValue` are the most important.

That's it! You can now pass any value for `value` and Apollo will happily consume or return it. For context I'm showing it next to the query resolver.

## Custom Type Union Resolver

Great! Now, can you imagine what this would look like if we needed to support a type union? We need to implement the three methods appropriately for the union. Let's make a `StringOrInt` type and resolver:

```
type StringOrInt

type Option {
  display_value: String!
  value: StringOrInt
}
```

```js
// resolvers.js
import { GraphQLScalarType, Kind } from "graphql";

export default {
  Query: {
    search() {
      return ...
    }
  },
  StringOrInt: new GraphQLScalarType({
    name: "StringOrInt",
    description: "A String or an Int union type",
    serialize(value) {
      if (typeof value !== "string" && typeof value !== "number") {
        throw new Error("Value must be either a String or an Int");
      }

      if (typeof value === "number" && !Number.isInteger(value)) {
        throw new Error("Number value must be an Int");
      }

      return value;
    },
    parseValue(value) {
      if (typeof value !== "string" && typeof value !== "number") {
        throw new Error("Value must be either a String or an Int");
      }
      
      if (typeof value === "number" && !Number.isInteger(value)) {
        throw new Error("Number value must be an Int");
      }

      return value;
    },
    parseLiteral(ast) {

      // Kinds: http://facebook.github.io/graphql/June2018/#sec-Type-Kinds
      // ast.value is always a string
      switch (ast.kind) {
        case Kind.INT: return parseInt(ast.value, 10);
        case Kind.STRING: return ast.value;
        default:
          throw new Error("Value must be either a String or an Int");
      }
    }
  })
}
```

This supports either a `String` or an `Int` scalar type union. There could be more validation cases you need to handle, I'll let you explore that. You can reference the [spec][kinds] on what kinds of tokens there are to parse.

The downside with this approach is that you'll need to define a resolver for each kind of type union but that's better than having no option at all.

[graphql]: https://graphql.org/
[jargon]: https://phinze.github.io/2014/05/24/useful-tech-terms-part-1.html
[ghissue]: https://github.com/facebook/graphql/issues/215
[apollo]: https://www.apollographql.com/
[customtypes]: https://www.apollographql.com/docs/apollo-server/v2/features/scalars-enums.html
[resolver]: https://www.apollographql.com/docs/apollo-server/v2/features/scalars-enums.html#graphqlscalartype
[kinds]: http://facebook.github.io/graphql/June2018/#sec-Type-Kinds
[json]: https://github.com/taion/graphql-type-json