Title: "JavaScript heap out of memory with WebdriverIO"
Published: false
Lead: I recently spent a few hours wrestling with a JavaScript out-of-memory heap error when running WebdriverIO in a continuous integration environment
Tags:
- WebdriverIO
- JavaScript
- Testing
---

Throughout working on a course you usually run into bugs and issues that throw you for a loop for awhile. In this case, it was doubly frustrating because I had previously set up [webdriverio](https://webdriver.io) to run in my continuous integration environment (GitHub Actions) and _it was working fine_. Until it stopped working.

I kept getting this error:

```

<!--stackedit_data:
eyJoaXN0b3J5IjpbLTMzNDUwMzcwMF19
-->