Title: "Flattening Deeply Nested Components in React"
Published: 2018-07-07 10:33:00 -0500
Lead: JSX is neat until you start getting into situations where things have to be nested deeply
Tags:
- React
- TypeScript
- JavaScript
---

Here's a quick tip. Often in entry points of React applications, you have to do some bootstrapping with various providers from dependent modules like Material UI, Redux, JSS, and React Router. That's exactly the stack I'm using so that's what my example is based on.

Normally, you would do this to bootstrap your `App` component:

```js
React.render(
  <Provider store={store}>
    <ConnectedRouter history={history}>
      <JssProvider jss={jss} generateClassName={generateClassName}>
        <MuiThemeProvider theme={theme}>
          <MuiPickersUtilsProvider utils={MomentUtils}>
            <App /> {/* FINALLY */}
          </MuiPickersUtilsProvider>
        </MuiThemeProvider>
      </JssProvider>
    </ConnectedRouter>
  </Provider>
)
```

It feels like I'm in Dante's Inferno creating the 9 circles of hell. I'm being dramatic of course--this is readable and probably fine. But there are situations with nesting even worse than this. If you're interested in how to flatten this out, read on.

## Recompose

I like the [recompose][recompose] utility library, it adds some functional helpers to React apps. One helper that we'll leverage here is called `nest`:

```js
import { nest } from "recompose";

const AppProviders = nest(
  Provider,
  ConnectedRouter,
  JssProvider,
  MuiThemeProvider,
  MuiPickersUtilsProvider
);

ReactDOM.render(
  <AppProviders 
    store={store} 
    history={history} 
    jss={jss} 
    generateClassName={generateClassName} 
    theme={theme} 
    utils={MomentUtils}
  >    
    <App /> {/* That's a bit better */}
  </AppProviders>,
  document.getElementById('root')
)
```

This actually works. What's happening here is that `nest` will take each component you give it and nest one after the other, in order of top to bottom. What you can do then is pass props that **get passed to each nested component**. This is the important bit and as it turns out will cause warnings in the console because some components have prop validation that will get angry at you if you pass them props they don't expect.

```
index.js:2178 Warning: Failed prop type: The following properties are not supported: `store`, `history`, `jss`, `generateClassName`, `utils`. Please remove them.
    in MuiThemeProvider (created by nest(Provider, ConnectedRouter, JssProvider, MuiThemeProvider, MuiPickersUtilsProvider))
    in nest(Provider, ConnectedRouter, JssProvider, MuiThemeProvider, MuiPickersUtilsProvider)
```

Oopsie. `nest` does accept stateless components as arguments, though. Let's do that!

## Nest with Stateless Components

```js
import { nest } from "recompose";

// Nest providers in this order (top to bottom)
const withProviders = wrap(
  ({ children }) => <Provider store={store}>{children}</Provider>,
  ({ children }) => <ConnectedRouter history={history}>{children}</ConnectedRouter>,
  ({ children }) => (
    <JssProvider jss={jss} generateClassName={generateClassName}>
      {children}
    </JssProvider>
  ),
  ({ children }) => <MuiThemeProvider theme={theme}>{children}</MuiThemeProvider>,
  ({ children }) => (
    <MuiPickersUtilsProvider utils={MomentUtils}>{children}</MuiPickersUtilsProvider>
  )
);

ReactDOM.render(
  React.createElement(withProviders(App)), 
  document.getElementById('root')
);
```

Alright. This also works. Buuuuut, I mean, c'mon, this somehow seems *worse*, right?

Is there a middle-ground of readability and less duplicated code? Of course! Let's just write a little helper to take pairs of components and their props and pass that to `nest`.

## Nesting with Component/Prop Pairs

```js
// Nest components in this order (top to bottom)
const AppProviders = nestPairs(
  [Provider, { store }],
  [ConnectedRouter, { history }],
  [JssProvider, { jss, generateClassName }],
  [MuiThemeProvider, { theme }],
  [MuiPickersUtilsProvider, { utils: MomentUtils }]
);

ReactDOM.render(
  <AppProviders>
    <App />
  </AppProviders>, 
  document.getElementById('root'));
```

Hey! That looks more readable *and* solves the prop passing issue. This is what `nestPairs` looks like:

```js
function nestPairs(...componentPropPairs) {
  return nest.apply(
    this,
    componentPropPairs.map(([ComponentClass, props]) => 
      ({ children }) => 
        React.createElement(ComponentClass, { ...props, children })
    )
  );
}
```

And in TypeScript (what I'm using):

```typescript
function nestPairs(this: any, ...componentPropPairs: Array<[React.ComponentType, any]>) {
  return nest.apply(
    this,
    componentPropPairs.map(([ComponentClass, props]): React.StatelessComponent<
      any
    > => ({ children }) => React.createElement<any>(ComponentClass, { ...props, children }))
  );
}
```

We are taking the array pairs and mapping them to stateless components with all the blanks filled in. This keeps the input
relatively simple while still allowing custom props per component. In TypeScript, the downside is that you lose the strong
typing compared to the previous version but I'm willing to do that since in my case, this is only used in the entrypoint of
our app so it'll be real apparent if things don't work.

[recompose]: https://github.com/acdlite/recompose