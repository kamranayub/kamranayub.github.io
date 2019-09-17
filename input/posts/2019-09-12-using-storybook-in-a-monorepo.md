Title: "Using Storybook in a Monorepo"
Published: 2019-09-17 07:00:00 -0500
Lead: Storybook can work across packages in a monorepo, you just need to know how to set it up
Tags:
- React
- Storybook
- JavaScript
---

I'm currently working in a small monorepo and we have multiple packages set up for shared components, like this:

```
packages/
  atoms/
  molecules/
  organisms/
```

Each package has React components under a `src` directory and since each directory _is_ a package, they also contain `node_modules` (more on why that's important below).

```
packages/
  atoms/
    node_modules/
    src/
      components/
        Hello.js
        Hello.stories.js
    package.json
  molecules/
  organisms/
```

This is different than other posts I've seen that use a root `stories/` directory -- that is not how we'd like to set up our repo, we'd prefer stories to live right next to the components they describe.

## Setting Up Storybook

You can follow the same steps on the [getting started page](https://storybook.js.org/docs/guides/guide-react/) for setting up Storybook in a monorepo.

Once it's done, you should have a new folder at the root:

```
.storybook/
  config.js
```

If not, make sure that gets created. We need to make some changes to `config.js` to read stories within each package in the monorepo.

## Configuring Storybook for a Monorepo

The key issue I ran into that prompted me to write about this is that we need to import stories using the [Webpack context](https://github.com/webpack/docs/wiki/context):

```js
require.context('../packages', true, /stories.jsx?$/);
```

This prompts Webpack to scan a `src` directory for paths containing `stories.js` or `stories.jsx` anywhere in the string.

Since Webpack _statically analyzes_ this code it means we **cannot** dynamically read the file system and iterate through each package directory (I tried that 😔).

There's another problem. Remember I mentioned each package directory has `node_modules` and possibly other directories which means that the following paths will match the regular expression:

```
# some node modules (like Storybook itself) use Storybook
./atoms/node_modules/@storybook__react/src/stories/blah.stories.js

# code coverage reports, uses same paths!
./molecules/lcov-report/_html/src/components/Hello.stories.js
```

And you can imagine more paths like that can match. Webpack will attempt to import these files into the bundle it generates but these will most likely cause build errors. That isn't what we want! We need to _exclude_ everything but the `src` directory in a package when matching files.

The solution, as it turns out is fairly straightforward, since we have a convention where each package has components within a `src` directory, we really just need to match that inner `src` after the package name, so we can write a regular expression like this:

```js
require.context('../packages', true, /^\.\/[^\/]+\/src\/.*stories\.jsx?$/);
```

Let's break it down:

```
^               # match beginning of path
\.\/            # the path will begin with a "./", like ./atoms
[^\/]+          # get the first path segment (match characters up to first /)
\/src\/         # ensure we match under the `src` directory
.*              # match any character
stories\.jsx?   # match anything with "stories.js(x)" in it
$               # match end of string
```

Here's an example using my favorite regex tool [RegExr](https://regexr.com) on which paths match and which don't:

![regex example](https://user-images.githubusercontent.com/563819/65041547-a4e08500-d91c-11e9-895d-7acaf4cc65a1.png)

Handy! 

## The Final Config

Here's our final `config.js`:

```js
import { configure } from '@storybook/react';

function loadStories() {
  const req = require.context('../packages', true, /^\.\/[^\/]+\/src\/.*stories\.jsx?$/);
  req.keys().forEach(filename => req(filename));
}

configure(loadStories, module);
```

I hope this helps someone else and saves them the few hours I spent performing the correct rites to get this to work!