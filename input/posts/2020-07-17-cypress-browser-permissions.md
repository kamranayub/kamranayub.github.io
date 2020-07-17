Title: "Controlling Browser Permissions in Cypress End-to-End Tests"
Published: false
Lead: I've released an open source package, `cypress-browser-permissions` that handles managing browser permissions in Cypress
Tags:
- Open Source
- Cypress
- Testing
---

I am excited to release a new open source package [cypress-browser-permissions](https://npmjs.com/package/cypress-browser-permissions). 🎉 You can view it on GitHub at [kamranayub/cypress-browser-permissions](https://github.com/kamranayub/cypress-browser-permissions).

This package solves a real need when testing more sophisticated applications when using [Cypress](https://cypress.io), the end-to-end testing framework. It helps control the permission level of various browser features such as:

- Desktop Notifications
- Geolocation
- Images
- Camera
- Microphone
- etc.

![image](https://user-images.githubusercontent.com/563819/87500464-2169f000-c622-11ea-8dbb-a480a6f137ac.png)

## How to Use It

To get started, you'll need to install the package and you'll need Cypress installed already.

```bash
npm i cypress cypress-browser-permissions --save-dev
```

If this is your first time installing Cypress, you'll need to run it once to generate a project structure:

```bash
npx cypress open
```

Then, you need to initialize the plugin to hook it into Cypress' plugin pipeline. In `cypress/plugins/index.js`, modify it as follows:

```diff
+ const { cypressBrowserPermissionsPlugin } = require('cypress-browser-permissions')

/**
 * @type {Cypress.PluginConfig}
 */
module.exports = (on, config) => {
  // `on` is used to hook into various events Cypress emits
  // `config` is the resolved Cypress config
+ config = cypressBrowserPermissionsPlugin(on, config);
+ return config;
};
```

Now you will have the ability to control various permissions for Chrome, Edge, and Firefox using [Cypress environment variables](https://docs.cypress.io/guides/guides/environment-variables.html).

For example, if you want to just set permissions for your project you can do so in `cypress.json`:

```json
{
  "env": {
    "browserPermissions": {
      "notifications": "allow",
      "geolocation": "allow"
    }
  }
}
```

The plugin will read the permission settings and apply them when launching the browser. It will also reset between launches since modifying the browser profile is persisted across sessions.

You can read more about [supported permissions and values in the README](https://github.com/kamranayub/cypress-browser-permissions).

### Writing an End-to-End Notification Test

So let's try it out! Once I finish my **Testing Progressive Web Apps** [Pluralsight course](http://bit.ly/KamranOnPluralsight), it will come with an open source sample app. In the meantime, we can write a basic test to see if permissions are working. This same test is included in the repo.

First, we have an HTML file that uses `window.Notification` to display a desktop notification:

**cypress/html/notification.html**

```html
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Cypress Notification Test</title>
</head>
<body>
    <script type="text/javascript">
        const n = new window.Notification('test', { body: 'This is a test!' })
        n.addEventListener('show', (e) => {
            window.__CypressNotificationShown = e;
        })
    </script>
</body>
</html>
```

You can learn more about how the [Notification API](https://developer.mozilla.org/en-US/docs/Web/API/notification) works but what we are doing is immediately triggering a notification. Once the browser shows the toast, it triggers the `show` event on the `Notification` instance. Since Cypress is awesome and we can hook directly into the `window` object, we set a callback value globally that we can then inspect/wait for in our test.

If you have a blank Cypress project you do not even need a server as Cypress will automatically host the root of the project when there is no other configuration.

Save the `notification.html` file under `cypress/html` and then we can visit that page in the test.

We can create a test suite in `cypress/integration`:

**cypress/integration/notification.test.js**

```js
import { isPermissionAllowed } from 'cypress-browser-permissions';

describe("notifications", () => {
    it("should be enabled", () => {
        expect(isPermissionAllowed("notifications")).to.be.true;
    })

    // Only test notification showing in "headed" browsers, which also
    // works in CI :tada:
    Cypress.browser.isHeaded && it("should display desktop notification", () => {
    
        // Visit the page we created previously
        cy.visit('/cypress/html/notification.html')
        
        // Wait for the window callback to populate with the event data
        cy.window().its('__CypressNotificationShown').should('exist');
    })
})
```

Now we can run our tests:

```bash
npx cypress open
```

That's all! If `browserPermissions.notifications` is set to `allow` then our test should pass:

![test run](https://user-images.githubusercontent.com/563819/87737665-3620c200-c7a1-11ea-8429-73bd40c99bed.png)

And a notification will be shown!

![toast](https://user-images.githubusercontent.com/563819/87737706-52bcfa00-c7a1-11ea-893e-9f6f8dec1e2e.png)

### How It Works

In Cypress, [you have control over the launch preferences for browsers](https://docs.cypress.io/api/plugins/browser-launch-api.html#Modify-browser-launch-arguments-preferences-and-extensions), so the magic lies in _what preferences to pass to each browser._

This topic is not heavily documented [as evidenced by this open issue in the Cypress repo](https://github.com/cypress-io/cypress/issues/2671) I came across while researching this. It has been open since 2018 with no one mentioning the ability to control launch preferences.

Thanks to BrowserStack for [documenting some of these permissions](https://www.browserstack.com/automate/handle-popups-alerts-prompts-in-automated-tests) as well as these StackOverflow posts:

- [Selenium + Python Allow Firefox Notifications](https://stackoverflow.com/questions/55435198/selenium-python-allow-firefox-notifications)
- [How to allow or deny notification geo-location microphone camera pop up](https://stackoverflow.com/questions/48007699/how-to-allow-or-deny-notification-geo-location-microphone-camera-pop-up)

I was able to piece together the information needed to tackle this with a Cypress plugin. Since each browser family uses different preferences, I thought it would be best to abstract it.

### What's Next?

My hope is that this package is _actually short-lived_ and the Cypress team can incorporate these permission settings into the core of the product, since it's such an important feature especially when testing new, modern APIs.

There will be a **full sample** of using Cypress with this plugin (as well as other black magicks such as bypassing service workers and more!) in my _Testing Progressive Web Apps_ course soon on [Pluralsight](https://bit.ly/KamranOnPluralsight). It should be released in August, you can follow me there to get notified when it releases. The sample app will be open source on GitHub so you'll be able to reference it 👍 
<!--stackedit_data:
eyJoaXN0b3J5IjpbOTA3ODgxMjU0XX0=
-->