Title: "Migrating from disk drives to solid state drives"
Published: False
Lead: This past week or so I finally sat down and migrated my PC from old spinning disk drives to solid state. In this post I'll share my process.
Tags:
- Hardware
---

My PC is not that old but I've had the same hard drives for about 8 years (since 2012 or so). I had already upgraded my primary drive to be a 250GB SSD which had helped a lot but these days 250GB is not a lot.

I was able to successfully migrate from one SSD and 3 spinning disk drives to 3 SSDs total, without re-imaging or re-installing anything. It's not hard to do but it can seem intimidating if you don't know the steps.

## Picking out the drives

At [Micro Center](https://microcenter.com) I picked out two Samsung 860 EVO SSDs 1TB each that were on clearance for about $200 total.

I figured 2TB + 250GB of SSD was enough for my PC since I mainly do development now and some limited gaming. I need enough for about 5-8 AAA games (90GB each) and a dozen or so smaller games plus some storage for media streaming. I figured the worst case is I stick the extra spinning disks in an enclosure and create a NAS.

One key thing to understand if you're looking at SSDs in 2020 and beyond is support for [NVMe](https://www.pcworld.com/article/2899351/everything-you-need-to-know-about-nvme.html). It's the latest storage bus for SSDs that provide the fastest possible speeds. Unfortunately, it's still fairly new so your motherboard needs to have support and mine does not, being about 8 years old. I wasn't at the point of doing a full upgrade again so I'm sticking with the older generation SSD, which is plenty fast for me.

## Going from multiple spinning disks

My previous state was as follows:

- Disk 0: 250GB SSD (Primary OS)
- Disk 1: 1TB HDD (Development Work)
- Disk 2: 750GB HDD (Games/Media)
- Disk 3: 750GB HDD (Games/Media)

These disks had varied amounts of free space but in total I think I had about 700GB free between all of them. The primary drive was 95% or so full, enough that I couldn't uninstall anything to get back more space.

My desired future state was this:

- Disk 0: 1TB SSD (OS / Development)
- Disk 1: 1TB SSD (Storage / Games)
- Disk 3: 250GB SSD (Storage / Games)

I don't think I _really_ needed the extra 250GB but I didn't see a reason not to use a perfectly good drive.

## 