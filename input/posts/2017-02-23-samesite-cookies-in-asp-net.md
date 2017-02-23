Title: Adding SameSite Cookie Support In ASP.NET
Published: 2017-02-23 17:18 -06:00
Tags:
- ASP.NET
- MVC
- IIS
- Security
---

I was reading Scott Helme's post on how [CSRF is Dead](https://scotthelme.co.uk/csrf-is-dead/) because of the new Same Site cookie spec (which is supported in [Chrome and soon FF](http://caniuse.com/#feat=same-site-cookie-attribute)).

I wanted to add support into [KTOMG](http://ktomg.com) so I was trying to figure out how to modify my authentication flow to add the attribute. However, [HttpCookie](https://msdn.microsoft.com/en-us/library/system.web.httpcookie(v=vs.110).aspx) is sealed and can't be modified so what's a well meaning security citizen supposed to do?!

Well, thanks to this StackOverflow answer, it's actually pretty simple and can be done on any IIS website using URL rewrite!

```xml
<system.webServer>
  <rewrite>
    <outboundRules>
        <clear />
        <rule name="Add SameSite" preCondition="No SameSite">
          <match serverVariable="RESPONSE_Set_Cookie" pattern=".*" negate="false" />
          <action type="Rewrite" value="{R:0}; SameSite=lax" />
          <conditions>
          </conditions>
        </rule>
        <preConditions>
          <preCondition name="No SameSite">
            <add input="{RESPONSE_Set_Cookie}" pattern="." />
            <add input="{RESPONSE_Set_Cookie}" pattern="; SameSite=lax" negate="true" />
          </preCondition>
        </preConditions>
      </outboundRules>
    </rewrite>
</system.webServer>
```

These outbound rules will add `SameSite=lax` to any Set-Cookie header in responses from your site (that are not already marked SameSite), so all cookies effectively set by your site become SameSite cookies.

In this case, I'm using Lax security (see Scott's post above for a good explanation of Lax vs. Strict) because I don't quite have the dual cookie authentication suggested by Scott (e.g. one to make yourself "known" and logged-in, the other that MUST be present on any secure page such as your Account page). I do plan to implement dual cookie auth whenever I get around to migrating to .NET Core.