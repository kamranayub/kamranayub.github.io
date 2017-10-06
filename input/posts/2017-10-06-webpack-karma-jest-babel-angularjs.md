Title: Using AngularJS 1.5 with webpack, babel, and Karma/Jest
Published: 2017-10-06 13:00:00 -0500
Lead: Living in harmony with Karma/Jest, webpack, and babel with AngularJS 1.5
Tags:
- AngularJS
- Testing
- JavaScript
- Webpack
- Babel
- Karma
- Jest
---

This took me awhile to get right so I'm sharing my experience trying to get webpack 3, babel 6, and Karma to work together for testing Angular 1.5 apps. I also have an example using Jest, which I actually prefer over Karma.

## Some context

Why bother with these modern build tools and Karma/Angular 1.5? Why not upgrade to ng 4 or migrate to React? For our team, there isn't yet a compelling reason to go and rewrite several apps when the product is stable and in production. Since code is spread out across several repositories and the team works in multiple codebases at a time, consistency is key.

That said, we still want to be happy writing JavaScript and Babel+Webpack+ES2015 is a great combination.

**Note:** I was actually using [Jest](https://facebook.github.io/jest/) originally which worked great with Angular 1.5 *and* our Node.js code! I'd highly recommend it *instead* of Karma/Jasmine/Mocha/Chai because it's a single dependency with easy configuration. We switched back to Karma to maintain consistency with the rest of our codebases. Here's an [example repo](https://github.com/kamranayub/angularjs-sample-webpack-es6-jest).

## Show me the code

You probably are reading this because you're struggling right now to get the puzzle pieces to fit.

With that in mind, here's a fully working repository with some specs to help you get off the ground.

- Karma: https://github.com/kamranayub/angularjs-sample-webpack-es6-karma
- Jest: https://github.com/kamranayub/angularjs-sample-webpack-es6-jest

**This is not meant to be a "starter" but you can use it however you want.**

Read on if you need specifics on each part of the puzzle.

## Prior art

When I was trying to get my setup working, I referenced the following posts:

- [Tips on setting up Karma testing with webpack](http://mike-ward.net/2015/09/07/tips-on-setting-up-karma-testing-with-webpack/) - Mike Ward, Sep 2015
- [angular-starter-es6-webpack](https://github.com/TheLarkInn/angular-starter-es6-webpack) - Sean Larkin, Aug 2016

## Configuring webpack & Babel

There's nothing different with my webpack or babel configuration when using Karma. Follow the standard setup guide for [Babel + Webpack](https://github.com/babel/babel-loader). The one addition I have is the [babel-plugin-angularjs-annotate](https://www.npmjs.com/package/babel-plugin-angularjs-annotate) which is handy for dependency injection.

## Configuring karma

This was the time suck. No matter what, it seemed I couldn't get things to process/compile the way I wanted.

The root of my issue was that my webpack configuration was actually an *array* of configuration objects and I was passing the array to the `webpack` karma middleware and it expects a **single** configuration. My bad.

Here's the relevant snippet:

```js
    // This will be the new entry to webpack
    // so it should just be a single file
    files: ["src/index.tests.js"],

    // Preprocess test index and test files using
    // webpack (will run babel)
    preprocessors: {
      "src/index.tests.js": ["webpack"],
      "src/**/*.test.js": ["webpack"]
    },

    // Reference webpack config (single object)
    // and configure some middleware settings
    webpack: require("./webpack.config"),
    webpackMiddleware: {
      noInfo: true,
      stats: "errors-only"
    },
```

I seemed to run into some issues when I tried using a glob pattern for the `files` array, like `**/*.test.js` so I switched to using a single file and that seemed to calm webpack down.

That **index.tests.js** file looks like:

```js
import "angular";
import "angular-mocks";

// require all test files using special Webpack feature
// https://webpack.github.io/docs/context.html#require-context
const testsContext = require.context(".", true, /\.test$/);
testsContext.keys().forEach(testsContext);
```

Key here the initial imports to set up Angular and Angular mock globals. The rest uses Webpack's module API to require the test files.

## Writing an Angular test

With this in place, we can write a simple component controller test in ES6!

https://github.com/kamranayub/angularjs-sample-webpack-es6-karma/blob/master/src/dummy.component.test.js

For more information on the pattern I'm using, check out [Pablo's post on Angular 1.5 component testing](https://puigcerber.com/2016/02/07/how-to-test-angular-1-5-components/).