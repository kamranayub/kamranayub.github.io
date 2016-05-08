---
layout: post
title: "Adding Subresource Integrity support to Cassette .NET"
date: 2016-05-07 23:13:00 -0600
comments: true
published: true
categories:
- C#
- Nuget
- OSS
- Security
---

If you aren't familiar with [Subresource Integrity](https://developer.mozilla.org/en-US/docs/Web/Security/Subresource_Integrity), it's a browser-based security measure to protect embedded content like scripts and stylesheets using a file content hash to help protect against XSS attacks.

For example, let's say you're including a script from a CDN:

    <script src="https://mycdn.com/jquery/1.0/jquery.js"></script>
    
Then let's say the CDN is compromised and instead of returning jquery, the script returns some malicious code that could compromise your site. Even if you're using Content Security Policy (CSP), you won't be protected because you whitelisted the CDN.

Subresource Integrity allows you to put a hash of the file's contents in an attribute of the tag. The browser will then hash the contents of the response from the CDN and compare it against the attribute provided. If the hashes don't match, the browser won't include the response and will throw an error.

    <script src="https://mycdn.com/jquery/1.0/jquery.js" integrity="sha256-hfhsh02929fhgh303yg"></script>

## Integrating with Cassette

I use [Cassette](http://getcassette.net) to perform my bundling/minification and I also [host my assets on a CDN](http://kamranicus.com/blog/2015/10/10/azure-cdn-cassette/). Even though they are my own assets, I still want to ensure they are served securely and take advantage of SRI.

For third-party scripts, it is fairly easy to take advantage of SRI by [hashing the contents online](https://srihash.org/) and customizing the CDN reference in Cassette:

    bundles.AddUrl("http://mycdn.com/jquery/1.0/jquery.js", bundle =>
        bundle.HtmlAttributes.Add("integrity", "sha256-jquerysfilehash"));

But since my *own* files are dynamic, how can we still leverage Cassette *and* automatically hash the file contents when outputting to the page?

Luckily, Cassette is pretty extensible and includes a way to [customize the bundle pipeline](http://getcassette.net/documentation/v2/bundle-pipelines). So what we can do is essentially override the rendering of the HTML and add the `integrity` tag to the output.

To make this easy, I've created an open source Nuget package called [Cassette.SubresourceIntegrity](https://github.com/kamranayub/cassette-sri). All you do is install the package and **that's it.** Since Cassette automatically scans for bundle customizations, all I did was implement a class `InsertIntoPipelineSubresourceIntegrity` and modify the pipeline to replace a couple parts with SRI-aware code.

The meat of the change is this code here:

    string integrity;

    using (var stream = asset.OpenStream())
    {
        using (var sha256 = SHA256.Create())
        {
            integrity = $"integrity=\"sha256-{Convert.ToBase64String(sha256.ComputeHash(stream))}\"";
        }
    }

    return $"<script src=\"{_urlGenerator.CreateAssetUrl(asset)}\" " +
           $"type=\"text/javascript\" " +
           $"{integrity}{bundle.HtmlAttributes.ToAttributeString()}></script>";

I am just getting the asset stream and hashing the contents using SHA256, then adding the attribute to the output. You'll notice that the **URLs are not changed**, so Cassette will continue to use SHA1 hashes internally. It's *only when rendering* we use SHA256 because that's the only place we need it.

While the code is interesting, it's nothing too crazy--in fact, most of the code required is because Cassette doesn't expose certain needed classes used in the rendering pipeline so I had to basically copy/paste a lot of the helper classes.

## The end result

Now Cassette will automatically include SRI hashes for individual assets:

    <link href="cassette.axd/asset/Content/bootstrap/bootstrap-ext.css?cabc6264a89106c4b9021c293cfa5c2cae7a0549" 
    	integrity="sha256-sNfA6O5zvZPmMJ474pm2w6UyZbz5tfukxTEZXrsLm7Q=" type="text/css" rel="stylesheet"/>
    <link href="cassette.axd/asset/Content/modules/typeahead.css?00581b47ff3848da273d91c31adb8270e9ef8707" 
    	integrity="sha256-W6JAiwRw2ER1QoXjXL/YxsY/On1Y7MhW4TtoWY0XuH8=" type="text/css" rel="stylesheet"/>
    <link href="cassette.axd/asset/Content/modules/toastr.css?32e90a136e05728ac23995ff8fe33077df9f50ca" 
    	integrity="sha256-JT6UwDlczdRDx+9mnMCzvxwABJP0cSDgNLmT+WumJrQ=" type="text/css" rel="stylesheet"/>
    <link href="cassette.axd/asset/Content/hopscotch/hopscotch.css?58ea04e54df958c33cf9e6bfed9f39a166354e9c" 
    	integrity="sha256-Bq06LI6L0XMhxF+CoJo+4L12w2Orsbh2oRjOZ+fpmWc=" type="text/css" rel="stylesheet"/>
    <link href="cassette.axd/asset/Content/core.css?a3b4fcb8b7d9b0e8465a4fea29d60247ea47fd87" 
    	integrity="sha256-fAqyFLkOx1cFONu7NXX3c7/G1DSmXeHgtPtcWU72a4E=" type="text/css" rel="stylesheet"/>
    <link href="cassette.axd/asset/Content/library.css?2c2746a086737dc588e313c0cc2c5adf8b947605" 
    	integrity="sha256-SaP9kdYfbafIVes+qntAiDLPsi4JaXnit4eN6IfU9lA=" type="text/css" rel="stylesheet"/>
    	
*and* bundles:

    <link href="cassette.axd/stylesheet/ba58f2a04873e41b6a599274ea6768db1a61a650/Content/core" 
        integrity="sha256-thzkrIApz9dAI9nfJGleO1jbNFXXVT/BxoSynI2pEPw=" type="text/css" rel="stylesheet"/>
    <link href="cassette.axd/stylesheet/2c2746a086737dc588e313c0cc2c5adf8b947605/Content/library.css" 
        integrity="sha256-6LgYbxu4UwouRBqvUdHZAQc0lewdik6aZYpDgrtAWJ4=" type="text/css" rel="stylesheet"/>

Voila!
