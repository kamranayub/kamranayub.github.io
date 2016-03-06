---
layout: post
title: "Handling Multiple Origins in CORS Using URL Rewrite"
date: 2016-03-06 9:50:00 -0600
comments: true
published: true
categories:
- C#
- Azure
- Security
---

Here's a quick tip if you're trying to figure out how to handle [cross-origin requests (CORS)](https://developer.mozilla.org/en-US/docs/Web/HTTP/Access_control_CORS) when you have multiple origins (namely, HTTP and HTTPS). This works in IIS 8.0 and above, including Azure, as long as you have the [URL Rewrite module](http://www.iis.net/downloads/microsoft/url-rewrite) installed.

The CORS header looks like this:

```
Access-Control-Allow-Origin: http://mydomain.com
```

The spec is very strict. The header can only return a single value and it must be absolutely qualified, which means if you have a site that is served over HTTP and HTTPS (or multiple domains), you need to *dynamically* build this header in your response. Many tutorials and blog posts say to specify `*` as the value--**DO NOT DO THIS!** This means any origin (domain) can embed/request assets from your website. Unless you have hundreds of sites doing this (aka CDN), you should only whitelist the domains that can include resources from your site.

If you are sharing resources with a known number of hosts, the following method will help. If it's a *dynamic* list, you will need to programmatically add the `Access-Control-Allow-Origin` header depending on the incoming `Origin` header--something I won't cover here.

Rather than messing with C# and modifying outgoing responses what I ended up using was a simple URL rewrite rule, proposed by [this Stack Overflow answer](http://stackoverflow.com/a/31084390/109458). All it does is add a header to the outbound response when the regular expression matches--in this case, whitelisting only the HTTP and HTTPS version of my domain (or subdomain).

```xml
<system.webServer>
   <httpProtocol>
     <customHeaders>
         <add name="Access-Control-Allow-Headers" value="Origin, X-Requested-With, Content-Type, Accept" />
         <add name="Access-Control-Allow-Methods" value="POST,GET,OPTIONS,PUT,DELETE" />
     </customHeaders>
   </httpProtocol>
        <rewrite>            
            <outboundRules>
                <clear />                
                <rule name="AddCrossDomainHeader">
                    <match serverVariable="RESPONSE_Access_Control_Allow_Origin" pattern=".*" />
                    <conditions logicalGrouping="MatchAll" trackAllCaptures="true">
                        <add input="{HTTP_ORIGIN}" pattern="(http(s)?:\/\/((.+\.)?mydomain\.com))" />
                    </conditions>
                    <action type="Rewrite" value="{C:0}" />
                </rule>           
            </outboundRules>
        </rewrite>
 </system.webServer>
 ```
 
This is using special syntax of the URL Rewrite module (`RESPONSE_`) to add a outgoing response header (dashes replaced with underscores). Then it matches the *incoming* `Origin` header, compares the value, and if it matches includes the CORS header with the value of my domain.
 
 That was all I had to do!
 
 **Note:** Since I just converted over to always SSL, I no longer need this workaround but multiple origins is pretty common when dealing with CORS so this solution will come in handy.
