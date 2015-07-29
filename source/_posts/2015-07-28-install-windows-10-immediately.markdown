---
layout: post
title: "[Updated] Install Windows 10 Immediately Before Rollout"
date: 2015-07-28 00:20:13 -0600
comments: true
published: true
categories:
- Windows 10
---

**Update (8:49pm)**: I adjusted my Windows 8 date/time to tomorrow and the progress of the update jumped and I'm completed now. I now see a Restart PC to finish installing updates.

**Update (9:00pm)**: Well it looks like it's a bust with Windows 8.1. My friend tested on Windows 7 and it worked but mine refuses to install--it just says I have it reserved and it's ready. I tried rebooting multiple times and running the /updatenow command again but no go.

![image](https://cloud.githubusercontent.com/assets/563819/8948430/c0f67f0e-356c-11e5-9daf-8cafb3521042.png)

**Update (10:00pm CST):** No luck on my other PC, same situation. Guess I'll just have to wait in line like everybody else!

---

This is only applicable for the next few hours until your machine gets Windows 10 rolled out. If you're impatient like me, a friend tipped me off that he was able to install Windows 10 prematurely by simply forcing Windows Update to download Windows 10 and then setting his system time forward a day (BIOS, I'm thinking).

It's kind of unbelievable but it's working so far. I'm at 95% complete downloading (you can view in Windows Update window).

![Progress](https://cloud.githubusercontent.com/assets/563819/8948016/ce3997f0-3567-11e5-8e1e-679fd5b54daa.png)

1. Hit Windows+R to bring up Run command
2. Type in `wuauclt.exe /updatenow` (**Works**)
3. Wait for the download to finish (Control Panel -> Windows Update) (**Works**)
4. When Windows Update says, "Preparing for installation...", set system time forward a day in Windows (**Works**)  
   ![image](https://cloud.githubusercontent.com/assets/563819/8948191/fa639e0a-3569-11e5-97c0-2b79d709c8cf.png)
5. When progress is done, reboot (**Untested**)
6. Windows 10 should install (**Untested**)

I will update this post with any new information.
