---
layout: post
title: "Getting WebSockets to Work on Windows Azure"
date: 2014-02-08 21:52:48 -0600
comments: true
categories:
- Azure
- WebSockets
- Node.js
---

I was banging my head against the wall for the past hour or so wondering why I was falling back to XHR polling when I deployed my Node.js application to Azure. I'm using socket.io and everything looks like it's in order, works locally, etc. It was failing with a WebSocket handshake error.

What I saw in the Chrome developer console was something like:

	Error during WebSocket handshake: Unexpected response code: 502

In my Azure Node.js console (`azure site log tail SITENAME`), I was seeing `EPIPE` errors.

It turns out, this little tidbit from the [original Windows Azure blog post][1] on Web Sockets did the trick.

Modify your **web.config** and add:

	<webSocket enabled="false" />

To your `system.webServer` configuration. Also, another good point in that blog post is to use SSL, since you get SSL for free with a `*.azurewebsites.net` site.

Hope this helps someone else out there. This should be added to the [official Azure tutorial][2] on using web sockets with Node.js.

  [1]: http://blogs.msdn.com/b/windowsazure/archive/2013/11/14/introduction-to-websockets-on-windows-azure-web-sites.aspx
  [2]: http://www.windowsazure.com/en-us/documentation/articles/web-sites-nodejs-chat-app-socketio/