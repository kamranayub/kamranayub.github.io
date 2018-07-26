Title: "Showing Custom UI on React Router Transitions"
Published: 2018-07-26 08:00:00 -0500
Lead: Using the React Router `<Prompt />` component you can trigger custom UI to prevent navigation
Tags:
- React
- React Router
- JavaScript
---

We had a recent requirement on our project to prompt the user with a custom modal when they have some pending changes to save in our application. The intention was to ensure they saved their changes as it affected other parts of the data they're working on and we need to recompute business rules (fun, huh?).

Since we're leveraging [React Router v4][router], I looked into what was provided out of the box to see how customizable it was.

## The Prompt Component

React Router core has a component called [`Prompt`][prompt]. The purpose of this component is to [show a dialog to the user during a router transition][transition-docs]. The `when` prop can be set to enable/disable this functionality dynamically which is great. You can also pass a `message` that will get displayed or a function that takes a `location` and returns `true` to allow navigation or a message to prevent.

```js
<Prompt when={true} message="Please save your changes before proceeding" />
<Prompt when={true} message={location => location.pathname === "/foo" ? true : "Denied"} />
```

![The default prompt](https://user-images.githubusercontent.com/563819/43242206-4b62f540-9065-11e8-80bb-d4ca944cc9ea.png)

However, I noticed a few caveats with this component. First, the default prompt is the browser prompt and when you click 'Okay' it _allows navigation_ which is exactly the opposite of what I want. Second, you can't render custom UI directly through `Prompt`--it just takes a `message`. Why? If you [dive down][dive] into the code, it leverages the [history][history] npm package which just delegates to that browser prompt. The good news is, the `history` package allows you to override the UI generation via [`getUserConfirmation`][getuserconfirmation].

```js
import createHistory from "history/browserHistory";

const history = createHistory({
  getUserConfirmation(message, callback) {
    // Show some custom dialog to the user and call
    // callback(true) to continue the transiton, or
    // callback(false) to abort it.
  }
});
```

This is helpful, using this we can always prevent the transition if needed. So, given that we can't use `Prompt` directly to render custom React UI but we _can_ override the core handler for showing the prompt, is there a way to connect the two? Of course.

## Overriding getUserConfirmation

First, let's test our assumption out and see if we can log the message we get from `Prompt`.

Using the regular `BrowserRouter` component from `react-router-dom`, pass the `getUserConfirmation` prop:

```js
const getUserConfirmation = (message, callback) => {
  console.log(message);
  callback(false);
}

<BrowserRouter getUserConfirmation={getUserConfirmation}>
...
</BrowserRouter>
```

If like me you are using `connected-react-router`, we can still customize the `createHistory` call as shown above:

```js
...
import { createBrowserHistory } from 'history'
import { applyMiddleware, compose, createStore } from 'redux'
import { connectRouter, routerMiddleware } from 'connected-react-router'
...
const history = createBrowserHistory({
  getUserConfirmation(message, callback) {
    console.log(message);
    callback(false);
  }
})

const store = createStore(
  connectRouter(history)(rootReducer), // new root reducer with router state
  initialState,
  compose(
    applyMiddleware(
      routerMiddleware(history), // for dispatching history actions
      // ... other middlewares ...
    ),
  ),
)
```

Then our Prompt:

```js
// MyComponent.js
<Prompt when={true} message="Please save your changes before proceeding" />
```

If you set this up, when you try to navigate away from the page, you will not be able to and the message will be logged to the console.

Great! Now you may be thinking, "We can just pass anything and it'll get passed through!" But you'd be wrong because that's what I thought too.

```js
const MyCustomDialogComponent = () => <div />

<Prompt
  when={true}
  message={MyCustomDialogComponent}
/>
```

If you try this, nothing will be logged to the console and navigation will not be blocked. React Router uses prop-types to validate `message` is a string (phooey!).

So, we have to stick with strings. Is there another way besides doing a bunch of work to add support for this in `react-router` directly (which'd be neat!)?

## Using a HOC with a Global Symbol-based Trigger

I landed on this approach as it seemed to be the less hacky way to achieve this. Essentially, since we can pass a string only to the `getUserConfirmation`, I pass in the key for a global [Symbol][symbol] and store a global trigger to signal to the React dialog component to show.

If you haven't used `Symbol` before, it is a primitive type in JavaScript introduced in ES6. What's neat about them is you can "register" them globally and they will be able to be looked up from other modules loaded within the same "code realm" (roughly the execution context of the engine). Well-known global symbols are built-in like `Symbol.iterator`.

Why a `Symbol` vs. just a regular string? A `Symbol`-based property won't be [enumerable][enumerable] (like using `Object.keys`) so it's kind of a way to do basic "private" properties. They can still be enumerated with other methods like `Object.getOwnPropertySymbols`. It creates a nice barrier between things your app might care about and walls it off unless someone explictly asks for that property.

So what does this end up looking like? We create a HOC that has basic state and a method that will show the dialog when triggered (user attempts to navigate away). The `getUserConfirmation` handler will receive the Symbol key and invoke the callback on the global object (`window`) with that Symbol property.

```js
// PreventNavigationDialog.js
class PreventNavigationDialog extends React.Component {

  state = { open: false };

  constructor() {
    super();

    // NOTE: Don't actually use Date.now. In the example
    // repo, I use the `cuid` package
    this.__trigger = Symbol.for(`__PreventNavigationDialog_${Date.now()}`);
  }

  componentDidMount() {
    window[this.__trigger] = this.show;
  }

  componentWillUnmount() {
    delete window[this.__trigger];
  }

  render() {
    const { when } = this.props;
    const { open } = this.state;

    return (
      <React.Fragment>
        <Prompt when={when} message={Symbol.keyFor(this.__trigger)} />
        {open && <div onClick={this.close}>Test dialog</div>}
      </React.Fragment>
    );
  }

  show = allowTransitionCallback => {
    this.setState({ open: true }, () => allowTransitionCallback(false));
  };

  close = () => {
    this.setState({ open: false });
  };
}
```

```js
// index.js

const getUserConfirmation = (dialogKey, callback) => {

  // use "message" as Symbol-based key
  const dialogTrigger = window[Symbol.for(dialogKey)];

  if (dialogTrigger) {
    // delegate to dialog and pass callback through
    return dialogTrigger(callback);
  }

  // Fallback to allowing navigation
  callback(true);
}

<BrowserRouter getUserConfirmation={getUserConfirmation}>
...
</BrowserRouter>
```

Okay, a bit going on here!

1.  First, define our shared Symbol. Using `Symbol.for` will register the Symbol globally so it can be accessed across the page. We assign it a unique key for lookup (I recommend [`cuid`][cuid] for real world usage).
2.  Next, define our HOC with some basic state. It takes in a `when` prop just like `Prompt` (and passes it down). It also has a simple `open` flag.
3.  When our HOC mounts, it registers our `show` trigger globally. It cleans up in case it is unmounted.
4.  Modify `getUserConfirmation` to check and see if a dialog trigger callback exists and call it if so. Since we assign it when the HOC mounts, it will set the state of the dialog.

The nice thing about this design is that we're allowing for multiple potential dialog prompts across the app. We can even have multiple instances of this component without conflict using the uniqueness nature of Symbols. I'm not a huge fan of using `window` but it has its uses--it's tough to avoid _something_ due to how we need to access it in `getUserConfirmation` and we can't pass anything but a string.

## Demo and Code

![End result](https://user-images.githubusercontent.com/563819/43242522-2e47c664-9067-11e8-8799-0e2c9723bf9f.png)

You can play with the [fully working CRA-based demo][demo] with Material UI on CodeSandbox or the [corresponding GitHub repo][repo]. The demo has a few more features like passing `title` and `message` content as well as using the `(location: Location) => boolean | string` overload that `Prompt` `message` prop supports to decide whether to allow transitions based on target location (we use this for our app).

It wouldn't take much to change this HOC to leverage the render props pattern, for example, to show _whatever you want_ by passing down `show`. Reuse it across your app!

This was an interesting issue to solve and it wasn't as easy as I hoped initially. I hope this helps other people! This could probably be packaged up as a customizable HOC with a little work. Maybe when I have a spare moment I'll stream making a package out of this. Remember to follow me on [Twitch][twitch]!

[router]: https://reacttraining.com/react-router/web/guides/philosophy
[prompt]: https://reacttraining.com/react-router/core/api/Prompt
[transition-docs]: https://reacttraining.com/react-router/web/example/preventing-transitions
[history]: https://www.npmjs.com/package/history
[dive]: https://github.com/ReactTraining/react-router/blob/e6f9017c947b3ae49affa24cc320d0a86f765b55/packages/react-router/modules/Prompt.js#L30
[getuserconfirmation]: https://www.npmjs.com/package/history#customizing-the-confirm-dialog
[symbol]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Symbol
[enumerable]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Symbol#Symbols_and_for...in_iteration
[cuid]: https://www.npmjs.com/package/cuid
[demo]: https://codesandbox.io/s/myw173jyq8
[repo]: https://github.com/kamranayub/example-react-router-transition-ui/tree/master/
[twitch]: https://twitch.tv/kamranicus
