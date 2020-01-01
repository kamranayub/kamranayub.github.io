Title: "Migrating from disk drives to solid state drives"
Published: 9999-01-01 00:00:00 -0500
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

- Old Disk 0: 250GB SSD (Primary OS)
- Old Disk 1: 1TB HDD (Development Work)
- Old Disk 2: 750GB HDD (Games/Media)
- Old Disk 3: 750GB HDD (Games/Media)

These disks had varied amounts of free space but in total I think I had about 700GB free between all of them. The primary drive was 95% or so full, enough that I couldn't uninstall anything to get back more space.

My desired future state was this:

- New Disk 0: 1TB SSD (OS / Development)
- New Disk 1: 1TB SSD (Storage / Games)
- New Disk 3: 250GB SSD (Storage / Games)

I don't think I _really_ needed the extra 250GB but I didn't see a reason not to use a perfectly good drive.

## Playing Musical ~~Chairs~~ Files

Do you notice something about the desired end state above? It's actually 500GB less space than what I had before.

What this meant in practice was that I needed to move files around in order to properly clone the HDDs over to the new SSDs and then even after that, I had to move files manually over from the HDDs.

So here's what I did:

1. Identify the drive with the most storage space (e.g. **Old Disk 1** in my case)
2. Copy over all "loose" files from other drives over to **Old Disk 1**. In my case, this left **Old Disk 2** empty as there was nothing really installed there.
3. Format **Old Disk 2** completely so we can disconnect it
4. Open up the PC, disconnect Disk 2, connect SSD **New Disk 1**.
5. Boot up with Clonezilla Live USB
6. Clone device-to-device **Old Disk 1** to **New Disk 1**
7. **Old Disk 1** can now be formatted or disconnected!

Repeat the same steps for the remaining drives, leaving the OS drive to the last. We want to be sure our cloning process is working.

## Moving Installation Folders

Copying loose files is easy -- music, TV, videos, movies, backups, etc. But moving _installations_ of programs is a bit more work. For me, I had to reconfigure:

- [Steam Library](https://www.howtogeek.com/257472/how-to-painlessly-move-your-steam-library-to-another-folder-or-hard-drive/)
- [Windows Games](https://www.windowscentral.com/how-install-apps-separate-drive-windows-10) (installed via Xbox Game Pass)
- [Epic Games](https://www.howtogeek.com/404999/how-to-move-fortnite-to-another-folder-drive-or-pc/)

I followed the linked guides above to do it all, it took _forever_ to wait and wait for all the copying to finish but it did work.

> Note: I ran into an issue trying to move some games like Master Chief Collection. I got an error 0x80073d0b and [tried to follow some threads](https://answers.microsoft.com/en-us/windows/forum/apps_windows_10-winapps/windows-10-getting-error-0x80073d0b-when-moving/f42ecd3c-f386-4967-8df7-4887bd2f45e4?page=1&auth=1) on the problem but ended up simply uninstalling the games to re-download later.

## Cleaning up the last hard drive

I had **Old Disk 2** last to clean up; so I had to move more files around because **New Disk 0** had a lot more free space (750GB worth). I copied stuff from **New Disk 1** and **Old Disk 2** so I could finally format and erase **Old Disk 2**.

Once I finished wiping the last HDD, I disconnected it and reconnected **Old Disk 0** which I then wiped in Windows and created a new logical volume.

## The final state

After all was said and done, I reached the desired final state! Whew. It took about 4 days since I had to wait for a lot of copying to finish but it was worth it, my PC is _screaming_ fast now.