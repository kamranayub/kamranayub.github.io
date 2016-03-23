---
layout: post
title: "Generating an Encryption Certificate for PowerShell DSC in WMF5"
date: 2016-03-23 18:44:00 -0600
comments: true
published: false
categories:
- PowerShell
- DSC
- Security
---

I'm currently building out a PowerShell DSC pull server cluster at work. If you aren't familiar with DSC, I'll talk more about it in an upcoming post that ties it all together. The long and short of it is that DSC is a way to store configuration as code and automate the configuration of many servers at once.

In the recent Windows Management Framework 5 release, Microsoft has improved its support and feature set for DSC but with a new release comes new surprises. The first surprise you may run into, as we did, was that your old WMF4 way of encrypting MOF files doesn't work. In WMF5, the requirements for the certificate used to secure MOF files is stricter. [Taken from MSDN](https://msdn.microsoft.com/en-us/powershell/dsc/securemof):

1. Key Usage:
  - Must contain: 'KeyEncipherment' and 'DataEncipherment'.
  - Should *not* contain: 'Digital Signature'.
2. Enhanced Key Usage:
  - Must contain: Document Encryption (1.3.6.1.4.1.311.80.1).
  - Should *not* contain: Client Authentication (1.3.6.1.5.5.7.3.2) and Server Authentication (1.3.6.1.5.5.7.3.1).

If you read my previous foray into certificates with Azure Key Vault, you know I'm pretty green when it comes to certificate management and terminology. I really didn't know what this stuff meant--I mean, I understand a certificate has key usages and enhanced key usages, but **how does it get them?**

It turns out Microsoft recommends obtaining a certificate from Active Directory Certificate Services. That's cool, but I'm just a developer who wants to work on DSC, I don't have an ADCS server to give me certificates during testing--that's a different team altogether and when they're primary guy is out of the office, I'm a bit stuck.

So I thought I could maybe use a self-signed certificate while I wait for a "for real" one later.

see: http://stackoverflow.com/a/36161490/109458
