---
layout: post
title: "Generating an Encryption Certificate for PowerShell DSC in WMF5"
date: 2016-04-04 21:43:00 -0600
comments: true
published: true
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

If you read my [previous foray into certificates with Azure Key Vault](http://kamranicus.com/blog/2016/02/24/azure-key-vault-config-encryption-azure/), you know I'm pretty green when it comes to certificate management and terminology. I really didn't know what this stuff meant--I mean, I understand a certificate has key usages and enhanced key usages, but **how does it get them?** It has to do with the certificate request and the template used to provision your certificate.

It turns out Microsoft recommends obtaining a certificate from Active Directory Certificate Services. That's cool, but I'm just a developer who wants to work on DSC, I don't have an ADCS server to give me certificates during testing--that's a different team altogether and when they're primary guy is out of the office, I'm a bit stuck.

**Update (4/13)**: [TechNet](https://msdn.microsoft.com/en-us/powershell/dsc/securemof) now has a guide on how to generate certificates for WMF5. I'm leaving the rest of this post as-is for posterity.

---

I thought I could maybe use a self-signed certificate while I wait for a "for real" one later. After searching around for a method to create a certificate with the required KU and EKU specs, I found a lot of answers suggesting using OpenSSL. I've never used OpenSSL before so I thought I'd give it a try and I found it a bit confusing--I think I could have gotten it to work but instead I came across a random PowerShell article (unrelated to anything) using a utility called `certreq` that could handle providing custom key usages, problem solved!

You just need to create a file to define your certificate settings, **MyCert.inf**:

```
[Version]
Signature = "$Windows NT$"

[Strings]
szOID_ENHANCED_KEY_USAGE = "2.5.29.37"
szOID_DOCUMENT_ENCRYPTION = "1.3.6.1.4.1.311.80.1"

[NewRequest]
Subject = "cn=me@example.com"
MachineKeySet = false
KeyLength = 2048
KeySpec = AT_KEYEXCHANGE
HashAlgorithm = Sha1
Exportable = true
RequestType = Cert

KeyUsage = "CERT_KEY_ENCIPHERMENT_KEY_USAGE | CERT_DATA_ENCIPHERMENT_KEY_USAGE"
ValidityPeriod = "Years"
ValidityPeriodUnits = "1000"

[Extensions]
%szOID_ENHANCED_KEY_USAGE% = "{text}%szOID_DOCUMENT_ENCRYPTION%"
```

Just change the `Subject` line to whatever you need in your case.

Then execute `certreq` using the input file:

    certreq -new MyCert.inf MyCert.cer
    
Certreq should be available if you have Makecert--if you aren't finding it in the default command prompt, try using the Visual Studio Command Prompt. Once you execute the command it will generate a public key file and install the private/public key pair into your `CurrentUser` personal certificate store:

    PS> dir Cert:\CurrentUser\My
    
From there, you can export the private/public keys and install it on your DSC nodes.

![Example screenshot](https://cloud.githubusercontent.com/assets/563819/14269791/dac55acc-fa9c-11e5-8352-55881c3150ed.png)

Until you get a signed certificate from your CA, this should work. Hope that helps!
