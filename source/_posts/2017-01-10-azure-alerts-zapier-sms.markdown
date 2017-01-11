---
layout: post
title: "Site Down Alerts using Azure App Insights and Zapier"
date: 2017-01-10 20:01:00 -0600
comments: true
published: true
categories:
- Azure
- Cloud
- DevOps
---

It's [happened again][1]. Even though I *do* get emailed whenever [KTOMG](http://ktomg.com) goes down, I don't pay attention to my email 24/7 and the site can be down for a bit until I realize it. In this case, I was cooking and wasn't at my computer or on my phone. The Twitter notification by a friendly user actually got me to check!

I know this is one of the headaches [I willingly signed up for][2] by moving to my own VM--and even though I get emailed when the site's down I hate not knowing that **immediately**.

<!-- More -->

## App Insights

The first thing you should do is get an [Azure App Insights account][3]. **It's free.** And you can even use it to [monitor your own VMs][4]! This is how I can still get this for KTOMG:

![Server Metrics](https://cloud.githubusercontent.com/assets/563819/21832425/afa2d62e-d770-11e6-864a-ef6dd5dc13c9.png)

It's awesome. 

The other thing it can do is handle availability (ping) tests. It's [very easy to set up][5] and then you can set an alert for it. I've done this already, where I've set it to email me whenever KTOMG is down at more than 3 locations over 3 minutes--in other words, my site has blown up.

## Using a Webhook with Zapier

I also use [Zapier](http://zapier.com), which is just a (fancier?) [IFTTT](http://ifttt.com) clone. When I had deployed via Azure, I had [set up a Zap to notify me][7] whenever my Azure sites were deployed. Since I'm on my own now, I disabled that but now I'll show you how to use it to notify you via SMS and webhooks.

1. [Sign up for an account](http://zapier.com)
2. Create a new Zap
3. Select "Webhook by Zapier" as the trigger
4. Go through the steps until you get a new webhook URL

![Webhook url](https://cloud.githubusercontent.com/assets/563819/21832880/e0233246-d773-11e6-8b83-f974fe706ccd.png)

Now you've got a webhook URL which is just an HTTP endpoint you can POST to.

In Azure App Insights, you'll want to paste this URL into your alert "Webhook" setting:

![Webhook settings](https://cloud.githubusercontent.com/assets/563819/21832621/fe82c604-d771-11e6-839c-df997b44b942.png)

Zapier will be asking you to test your hook. It's a bit hard to trigger an Azure alert, but all you really need to do is POST a JSON payload at the endpoint, so [going off this Azure guide][6], use something like `curl` or Postman to POST to the endpoint:

![Testing the hook](https://cloud.githubusercontent.com/assets/563819/21832689/7baa2492-d772-11e6-9b9b-d6be7161fc01.png)

Now that the hook is tested, Zapier has filled in a bunch of template fields for you, pretty cool! On the left side of the Zap, click the (+) icon and add a Filter:

![Add Zapier filter](https://cloud.githubusercontent.com/assets/563819/21832706/9f6736c2-d772-11e6-8437-bad812a60201.png)

We want to only send the Zap if the alert has been "Activated" (not if it gets Resolved). Chances are, you only care that the alert occurred and after that you can check your email/fix the issue to resolve it.

![Filter for Activated](https://cloud.githubusercontent.com/assets/563819/21832731/c9a53d94-d772-11e6-899a-a52a1efea106.png)

Now, add an Action to send an SMS (or really, do whatever you want--I just want a text message).

![SMS action](https://cloud.githubusercontent.com/assets/563819/21832757/ffe84ea0-d772-11e6-8dc5-9fc4eadd2023.png)

You can use the template fields if you want, but this webhook is customized for our alert so I don't really need any info from the alert. If you were sending *multiple* alerts to this single hook, you could use the fields of the JSON payload to customize the message here.

I am setting a "From number" because I can use an app like [FireAlarm2][8] to read my SMS and page me loudly (even if my phone is on vibrate).

That's it! You can test the Zap and it'll send you an SMS to verify you can receive messages. Now you'll always know when your site's down so you don't need to rely on your users.

[1]: https://twitter.com/jordan_belinsky/status/818967360132513792
[2]: https://kamranicus.com/blog/2016/10/18/ravendb-standard-non-commercial-license/
[3]: https://docs.microsoft.com/en-us/azure/application-insights/app-insights-overview
[4]: https://docs.microsoft.com/en-us/azure/application-insights/app-insights-monitor-performance-live-website-now
[5]: https://docs.microsoft.com/en-us/azure/application-insights/app-insights-monitor-web-app-availability
[6]: https://docs.microsoft.com/en-us/azure/monitoring-and-diagnostics/insights-webhooks-alerts
[7]: https://zapier.com/zapbook/windows-azure-web-sites/
[8]: https://play.google.com/store/apps/details?id=de.hoernchen.android.firealert2&hl=en