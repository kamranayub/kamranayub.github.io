Title: Building a Raspberry Pi 3 Baby Monitor
Published: 2017-02-21 13:25:00 -0600
Lead: I put together a guide on building a baby monitor using a Raspberry Pi 3
Tags:
- Raspberry Pi
- Hardware
- Linux
- Baby
---

Our little baby boy came into the world February 6 and we couldn't be happier! During the course of preparing for the birth and buying items, a baby monitor had been high on the list--but I wasn't super happy with the choices available. So I [built my own](/guides/raspberry-pi-3-baby-monitor)!

<!-- more -->

A friend had recommended the [Summer baby monitor](https://www.amazon.com/gp/product/B01M34SOIM/ref=as_li_tl?ie=UTF8&tag=kamranicus-20&camp=1789&creative=9325&linkCode=as2&creativeASIN=B01M34SOIM&linkId=5e4ae35731346f971eb4deaa3d321160) series. Another friend just recommended the [Nest Cam](https://www.amazon.com/gp/product/B00WBJGUA2/ref=as_li_tl?ie=UTF8&tag=kamranicus-20&camp=1789&creative=9325&linkCode=as2&creativeASIN=B00WBJGUA2&linkId=708b18eb1764f496eacac072f1d6e243) since it was easy to use and viewable on any device. Reading the reviews on the Summer monitor and looking at the example image resolution left a bit to be desired--certainly it would *work* but I sort of wanted higher quality and the battery life and range (125 ft) on the viewing tablet was god awful. The Nest Cam seemed promising, able to hook up to Wi-Fi but it was definitely at a pretty high price point, being about $200. The other thing being a techie was the whole security aspect of the IoT space--I trusted myself to build a more secure system rather than [chancing vulnerabilities/exploits in any monitor I bought](https://information.rapid7.com/iot-baby-monitor-research.html).

I knew there had to be a more affordable option that would still give me a lot of flexibility and options. The [Raspberry Pi](https://www.raspberrypi.org/) seemed like a perfect use case! If you haven't heard of a Pi, it's a small programmable computer with USB, Wi-Fi, Bluetooth, Ethernet, and SD card memory. It's cheap (a full kit is only $50), USB hardware is cheap, and it has a 1080p 30fps camera module for high-resolution streaming--I could configure it to simply start streaming at bootup and then view it on *any* device including console web browsers, tablets, or our phones. I could also properly secure the stream (and Pi) to ensure it wasn't available outside my home network.

You can [check out the full guide to building it](/guides/raspberry-pi-3-baby-monitor), it doesn't take long and all the hardware can be acquired for sub-$100 depending on what options you want.

The quality speaks for itself!

**720p, low light**

![Image quality, low light](/guides/images/raspberry-pi-3-baby-monitor/baby.png)

**720p, high light**

![Image quality, high light](/guides/images/raspberry-pi-3-baby-monitor/baby-day.png)

In addition you also have high quality audio via a USB condenser microphone! There seems to be a small issue with using 1080p resolution with the software package but hopefully I can sort that out soon and update the images to be 1080p.