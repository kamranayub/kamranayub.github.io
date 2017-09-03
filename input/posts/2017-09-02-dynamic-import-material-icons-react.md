Title: Dynamically Importing React Material Icons Using Webpack
Published: 2017-09-02 22:10:00 -0500
Lead: Quick tip on dynamically importing icons using Webpack
Tags:
- JavaScript
- React
- Webpack
---

The sample code for this post is [available on my GitHub](https://github.com/kamranayub/example-webpack-dynamic-import).

One of my team members was working on our app's navigation and for the icons we are using the [material-ui-icons](https://www.npmjs.com/package/material-ui-icons) package. This is an npm package that provides the Material UI icons as React components. We're also using webpack for bundling and babel for transpiling.

Their question was around how to dynamically reference these icon components within the navigation component. We dynamically build navigation from a JSON structure and need to load the appropriate icon for each item.

Imagine something like this to represent some settings menu:

```js
{
  "name": "Settings",
  "children": [
    {
      "name": "Manage Alarms",
      "icon": "AccessAlarmIcon"
    }
  ]
}
```

Somehow we need to import the appropriate icon from `material-ui-icons` and reference it based on the `icon` parameter:

```js
const icon = (icon) => {
  
  // somehow resolve this icon
  let resolvedIcon = // ...

  return React.createElement(resolvedIcon)
}

const Navigation = (props) =>
  <nav>
    {props.children.map(item => 
      <li>{item.icon} {item.name}</li>
    )}
  </nav>
```

The initial solution was to just hardcode the logic:

```js
import AccessAlarmIcon from 'material-ui-icons/AccessAlarm'

const icon = (icon) => {
  switch (icon) {
    case 'AccessAlarmIcon': return <AccessAlarmIcon />
    default: return null
  }
}
```

But we knew this was a sub-optimal solution, it requires updating this file each time we want to support a new icon. There must be a better way!

## Referencing the scope

At first, we tried referencing `AccessAlarmIcon` imported variable directly since it should be in the scope of the module.

```js
import AccessAlarmIcon from 'material-ui-icons/AccessAlarm'

const icon = (icon) => {
  return React.createElement(eval(icon))
}
```

Yes, `eval` is **evil** but we wanted to see if it worked before trying a safer solution. It didn't work.

    Uncaught ReferenceError: AccessAlarmIcon is not defined

Hmm, it should be, right? Wrong. Webpack is doing some bundling magic for us. The compiled code looks like this:

```js
var _AccessAlarm = __webpack_require__(70);
var _AccessAlarm2 = _interopRequireDefault(_AccessAlarm);
```

So first, it's not even named `AccessAlarmIcon` at runtime and second, we have some webpack magic going on with variable naming and such.

## Using the Webpack API

I decided to look into this over the weekend since it was an interesting problem. After playing around for a bit, I decided to look and see what webpack could offer us via [its API](https://webpack.js.org/api/module-methods/).

It turns out we can leverage `require` to just use a convention-based method of dynamically importing the icons. Within a webpack context, it will handle resolving the dependency for us internally.

```js
const icon = (icon) => {
    let iconName = icon.replace(/Icon$/, '')
    let resolved = require(`material-ui-icons/${iconName}`).default
    
    if (!resolved) {
        throw Error(`Could not find material-ui-icons/${iconName}`)
    }

    return React.createElement(resolved)
}
```

Eyyy, look at that!

![Screenshot of icon](https://user-images.githubusercontent.com/563819/30000066-cacbb048-9024-11e7-9770-ae750984fb59.png)

Turned out to be a simple, straightforward solution but it wasn't immediately apparent.

### Making it async

You could also asynchronously load the icon using `import()` which returns a native `Promise` object. To make that work with React, we can use [react-async-component](https://github.com/ctrlplusb/react-async-component) and create an async factory component:

```js
import { asyncComponent } from 'react-async-component'

const icon = (icon) => {
    let iconName = icon.replace(/Icon$/, '')
    return React.createElement(asyncComponent({
        resolve: () => import(
            /* webpackMode: "eager" */
            `material-ui-icons/${iconName}`)
    }))
}
```

I am using the [*eager* fetch mode](https://webpack.js.org/api/module-methods/#import-) to include all the Material icons vs. performing network requests to fetch them.

**Note:** When using Babel with the new dynamic `import` syntax, you'll need to install [syntax-dynamic-import](https://babeljs.io/docs/plugins/syntax-dynamic-import/) preset and enable it.