---
layout: post
title: "UX applies to API design too"
date: 2012-04-03 15:05:22 -0500
comments: true
categories:
permalink: /blog/posts/40/ux-applies-to-api-design-too
disqus_identifier: 40
---

I'm working on enhancing a component we use for cryptography at work today. Previously it had a lot of useful methods, but no documentation. I was able to add XML documentation and now it's fully Intellisense-friendly:

![Intellisense](/blog/images/38.png)

That's one step for making your API friendly, even though it's a little more work.

What I want to point out is that I also made exceptions friendly. Cryptography is not an easy thing to understand and even I don't understand all of it. We have a method that lets you generate a new key for a given algorithm (the API defaults to `RijndaelManaged` as best practice), but it also lets you specify a key size.

I did a couple things:

1. The argument accepts key size in bits, as that is what the .NET API uses and is what you see online ("128 bit").

2. The API will tell you if the key size is invalid. Not only that, it will list out what the acceptable key sizes are.

Check it out:

```
Test method SymmetricEncryption_Should_Throw_Exception_For_Invalid_Key_Sizes threw exception:

System.ArgumentException: 56 bits is not a valid key size for RijndaelManaged. Valid key sizes: 128 bits, 192 bits, 256 bits

Parameter name: keySize
```

And it didn't even take much code:

<script src="https://gist.github.com/2292726.js?file=SymmetricEncryption.cs"></script>

Never forget:

> UX is just as important for API design

Put yourself in your consumer's shoes and design it with them in mind. They'll thank you for it, trust me.