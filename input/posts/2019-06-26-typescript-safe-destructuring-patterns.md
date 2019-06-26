---
Title: Handling Safe Destructuring in TypeScript
Published: 2019-06-26 08:00:00 -0500
Lead: It's common in JavaScript code to accept objects as parameters in functions and setting them to an empty object by default to allow safe destructuring. In this post I'll cover how to handle this pattern in TypeScript.
Tags: 
- TypeScript
- JavaScript
- Refactoring
- Software Quality
---

When migrating from JavaScript to TypeScript, you will often run into scenarios that seem difficult to statically type related to destructuring objects.

## Destructuring an empty object

This pattern is showcased using the following code snippet, which is not actual code but *does* reflect the semantics of the actual code we reviewed:

```js
function transformSearchResults(payload = {}, queryParams = {}) {
  const { results } = payload;
  const { searchTerm } = queryParams;

  if (!results) {
    return payload;
  }

  if (searchTerm && isSearchPayload(payload)) {
    payload.transformedResults = results.map(r => ({
      ...r,
      anotherProp: true
    }));
  }

  return payload;
}
```

This function essentially takes a payload, transforms it, and returns the modified payload object.

Specifically, notice in the function arguments the expressions `= {}` which in JavaScript will set a default value of `{}` for the parameter if it is `undefined`. This is done because on the next 2 lines, we attempt to destructure and pull out some key props of the objects. If `payload` or `queryParams` is `undefined` then the destructuring would blow up and *we don't want this code to blow up.* That is pretty reasonable!

## Converting to TypeScript

If we want to migrate this function to TypeScript, it would make sense to add a type annotation for each parameter like this:

```typescript
export interface ResponsePayload {
  results: SearchResult[] | OtherResult[];
}

function transformSearchResults(
  payload: ResponsePayload = {},
  queryParams: QueryStringParams = {}
)
```

We are trying to annotate our types properly by telling TypeScript what types we expect these parameters to be. We've declared our API type `ResponsePayload` with a `results` property.

But alas, TypeScript will throw an error immediately:

```
Property 'results' is missing in type '{}' but required in type 'ResponsePayload'. ts(2741)
```

While this might seem wrong at first on the surface, TypeScript is actually pointing out a *code smell* in this function.

The reason that TypeScript doesn't allow this is because the `ResponsePayload` type has a `results` property that is *required* (cannot be undefined). We know this because we typed it that way, presumably because the API doesn't return undefined. If it did, we could change `results` to be optional.

When converting destructuring code to TypeScript, you will be encouraged to consider the code more critically than before, otherwise you will run into type errors.

Here's what I ask in this situation:

1. Can the function be simplified to pass a specific primitive value for the parameter?

2. If I need to pass an object, can I refactor the function to avoid it?

These questions are dependent on the specific case but in the *majority* of cases, you can usually remove the need to destructure by asking these questions.

For example, let's take it step-by-step.

## Simplifying Function Parameters

> Can the function be simplified to pass a specific primitive value for the parameter?

Here's the code again:


```typescript
function transformSearchResults(
  payload: ResponsePayload = {}, 
  queryParams: QueryStringParams = {}) 
{
  const { results } = payload;
  const { searchTerm } = queryParams;

  if (!results) {
    return payload;
  }

  if (searchTerm && isSearchPayload(payload)) {
    payload.transformedResults = results.map(r => ({
      ...r,
      anotherProp: true
    }));
  }

  return payload;
}
```

A quick look will tell us that the answer is **No** for `payload` but **Yes** for `queryParams`. 

Only `results` in `payload` is used but `payload` is also being mutated, so the function needs the object passed, let's set that aside. 

For `queryParams`, only `searchTerm` is actually referenced, so let's simplify this function to pass only that:

```typescript
function transformSearchResults(
  payload: ResponsePayload = {}, 
  searchTerm?: string) 
{
  const { results } = payload;

  if (!results) {
    return payload;
  }

  if (searchTerm && isSearchPayload(payload)) {
    payload.transformedResults = results.map(r => ({
      ...r,
      anotherProp: true
    }));
  }

  return payload;
}
```

Great! The rest of the code remains the same and we eliminated one unnecessary destructuring.

## Approaches to Refactoring for Destructuring

> If I need to pass an object, can I refactor the function to avoid it?

Right now the function requires `payload` to be provided directly. There are two approaches we could now take:

1. Specifying a default value for required properties
2. Removing the need to mutate an object so we can just pass `results`

### Providing default values for properties

If we intend to keep the logic in the function intact, we need to provide a value for all required properties of `ResponsePayload`.

We can do this inline within the function which works well for a small list of parameters:.

```typescript
function transformSearchResults(
  payload: ResponsePayload = { results: [] }, 
  searchTerm?: string)
```

All fixed; since we provide a default value for `results` which isn't `undefined` TypeScript is happy.

We could also choose to provide a default object for more complex situations:

```typescript
const defaultPayload: ResponsePayload = {
  results: []
};

function transformSearchResults(
  payload: ResponsePayload = defaultPayload, 
  searchTerm?: string)
```

This works just as well but introduces a dangling object that might not add a ton of value. Can we remove the need for providing `payload` in the first place?

### Removing the object altogether

The fact that the function mutates the original incoming object is a smell itself. There are cases where that makes sense but in this case, we are transforming results--we should just return the newly mapped results and *push the concern of mutating `payload`* to a higher-level.

```typescript
function transformSearchResults(
  results: SearchResults[] | OtherResults[], 
  searchTerm?: string) 
{
  if (searchTerm && isSearchPayload(payload)) {
    return results.map(r => ({
      ...r,
      anotherProp: true
    }));
  }

  return results;
}
```

*Nearly there.* We will now receive an error because `isSearchPayload` was testing the payload itself. We have two options:

1. Modify `isSearchPayload` to inspect results
2. Push the concern of checking payload higher

We could certainly do option 1 but I like option 2 better. It will end up simplifying the `results` typing even more because we can *know* the results are *search results*! This also makes the function name much more accurate:

```typescript
function transformSearchResults(
  results: SearchResults[], 
  searchTerm?: string) 
{
  if (searchTerm) {
    return results.map(r => ({
      ...r,
      anotherProp: true
    }));
  }

  return results;
}
```

There we go; we've totally removed the need for destructuring with this function by simplifying the requirements and avoiding mutation of objects.

### Where did the complexity go?

By refactoring the function we pushed two concerns higher in the call stack:

1. Is this a search payload (`isSearchPayload`)?
2. Handling `transformedResults` for `payload`

That logic still has to exist somewhere but now it can be lifted higher. There is a principle at work here called the [Command-Query Separation](https://en.wikipedia.org/wiki/Command%E2%80%93query_separation) principle, which says:

> "every method should either be a command that performs an action, or a query that returns data to the caller, but not both. In other words, **Asking a question should not change the answer.**"

Before we refactored this function, it violated this principle because it both *returned new results* (a query) **and** had a side effect by mutating the object (a command). Now we've refactored it into a "query" function only.

If we had inspected the previous way this function was called, you would have seen this:

```js
function (payload?: ResponsePayload) {
  if (!payload) {
    throw new Error("Missing response payload!");
  }

  payload = transformSearchResults(payload, queryParams);

  return payload;
}
```

This should raise a yellow flag since it is returning the object again which implies it may be mutating it (in other words, "it smells").

Now that we've refactored the function above, some responsibilities have been pushed higher, which may result in a change like this:

```typescript
function (payload?: ResponsePayload) {
  if (!payload) {
    throw new Error("Missing response payload!");
  }

  if (isSearchPayload(payload)) {
    return {
      ...payload,
      transformedResults: transformSearchResults(payload.results)
    }
  }
}
```

We avoid mutating the payload directly instead opting to merge the new property in. We now have options to extract this logic, refactor it, etc. which you can decide!

## Handling Nested Destructuring

If we're able to remove the need to destructure by simplifying functions that is a win but what if we really need to safely destructure and more specifically, in a nested way?

```javascript
function handleData(data = {}) {
  const { meta: { field1, field2, field3 } = {} } = data;
}
```

This is slightly more complicated and subsequently harder to type *and it should be*. One thing I have come to appreciate about TypeScript is that it makes you feel the pain a little more which in turn encourages you to take a step back and question why the code is structured the way it is.

In this example, we can ask the same question we originally asked: *Do we really need `data` or can we be more specific?*

### Keeping `data`

Let's say we must keep `data` as a parameter. If `meta` and all the `field*` properties *can truly be `undefined`* and are marked as so, this code will work.

As soon as one of them is non-optional, TypeScript will throw an error since we haven't provided defaults.

For example, let's say `data` is typed as:

```typescript
interface Data {
  meta: Metadata
}

interface Metadata {
  field1: string;
  field2: boolean;
  field3: object;
}
```

`meta` is not optional, it must be provided so the code above will throw an error as no default `meta` value is provided and neither are the `field*` defaults. Yikes!

The best approach is to do a refactoring like we did above to simplify the parameters, if possible.

The next best thing is to *remove the default*:

```typescript
function handleData(data: Data) {
  const { meta: { field1, field2, field3 } } = data;
}
```

This matches how we've described the types -- that no property can be `undefined` so there's no *reason* to use a default parameter anymore. If this matches what you expect, there's nothing additional to do.

What if in practice, things could potentially be `undefined` but we still want to ensure the types remain "pure"?

Then we should push the "guarding" of potentially `undefined` values higher up the stack:

```typescript
function checkData(data: Data | undefined) {
  if (!data) {
    throw new Error("Data shouldn't be undefined");
  }

  return handleData(data); // No longer can be undefined
}

function handleData(data: Data) {

  // This is safe to do now
  const { meta: { field1, field2, field3 } } = data;
}
```

In general, you should guard as close to the source as possible. This keeps downstream functions simpler and with TypeScript, you can ensure they get passed values that don't *need* defaults to be provided.

### Supporting partial objects

Let's assume that `meta` fields *can* be partially available, we can simplify and statically type the function like this:

```typescript
interface Data {
  meta: Partial<Metadata>
}

interface Metadata {
  field1: string;
  field2: boolean;
  field3: object;
}

function (data: Data) {

  // This is safe now as `meta` can be partially undefined
  const { meta: { field1, field2, field3 } = {} } = data;
}
```

Now we've *explicitly* declared that `meta` can be partially defined. This does two things: 

1. it allows the safe destructuring without compiler errors and,
2. it tells the reader to *expect* to guard against partially defined fields.

Static types are documentation and now we've called out to expect that `meta` can have partial fields. It also allows us to safely set the default object to `{}` to avoid errors while destructuring.

## The last resort: type assertions

There is a hammer we can use as a last resort. If we can't effectively refactor out destructuring or if we really need to force this pattern to avoid changing code, we can use type assertions:

```typescript
function transformSearchResults(
  payload = {} as ResponsePayload,
  queryParams = {} as QueryStringParams
)
```

This will force TS to treat the empty object *as* the types we want--but this introduces potential for bugs as now we are opting out of the type safety. We are putting the responsibility of checking for `undefined` on the developer writing the function vs. the TypeScript compiler.

If you do this with ESLint rules enabled, you'll be greeted with an error:

```
Type assertion on object literals is forbidden, 
use a type annotation instead. 
eslint(@typescript-eslint/no-object-literal-type-assertion)
```

Again this is because it's best to *avoid this* in the first place. In order to get this to compile, you'll need to disable the error:

```typescript
/*  eslint-disable-next-line @typescript-eslint/no-object-literal-type-assertion */
function transformSearchResults(
  payload = {} as ResponsePayload,
  queryParams = {} as QueryStringParams
)
```

At least now, you can later search for these disabled errors as an indication to revisit and refactor later.

## Summary

When migrating to TypeScript, you will run into a lot of issues like this that on the surface feel like you're battling the type system but what I hoped to get across is that *TypeScript is forcing you to think more explicitly about how your code is structured and what your intent is.* It will force you to look *beyond* the scope of a single function and think more holistically about usage and context.

When you find yourself trying to disable rules or workaround TypeScript errors through type assertions, you need to take a step back and ask: 

> Is this the best way to structure this code or is TypeScript hinting that I may need to refactor it?

This type of mindset is a *shift in thinking* when working with TypeScript, especially when you've come from JavaScript. It may be painful at first but ultimately it will lead to better, hopefully simpler code.

If you're interested in furthering your understanding of principles like Command-Query Separation and refactoring code, I highly recommend the [Encapsulation and SOLID course](https://app.pluralsight.com/library/courses/encapsulation-solid/table-of-contents) by Mark Seeman from Pluralsight.

You can also check out my [collection of React and TypeScript content](https://dev.to/kamranayub/how-to-get-started-using-react-with-typescript-21c)!