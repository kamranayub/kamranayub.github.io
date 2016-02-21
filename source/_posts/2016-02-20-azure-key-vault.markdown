---
layout: post
title: "Securing Secrets and Keys using Azure Key Vault"
date: 2016-02-20 20:30:00 -0600
comments: true
published: false
categories:
- C#
- Azure
- Security
---

Secrets. Where should you store them? I've seen many ways in my career:

1. In the source code, accessible via Reflection
2. In the app settings config, acessible via source control
3. In a database, accessible if you have permissions
4. In a managed portal interface, like Windows Azure, accessible if you have permissions

These are pretty much in order of least secure to pretty secure, but options 3 and 4 typically still require local secrets during development. It's hard to escape that need unless you force your app to rely on connectivity to retrieve secrets which makes it hard to work offline.

<!-- More -->

Is it possible to store secrets in a way that **no one** can see them, even if they have access to the database/portal *and* file system? Furthermore, even secrets are stored in memory--so what can we do about that? I haven't even mentioned **encryption keys**. If someone got ahold of them, it's game over--keys need to be protected at all costs.

Some of you might say, "It's okay if it's in the config, no one can see it on the web server." You'd think so, but I ran into an exploit last year where you could see files on the web server by passing in the path you wanted to see (the exploit has since been fixed by the vendor).

You might also (rightfully) say that if an attacker got access to your Azure portal, it's game over anyway--they could perform remote debugging, see files, etc. That's true but they can't download a signed certificate from the portal, so if we store secrets away in Key Vault and encrypt our Azure AD client ID and **don't store in the portal**, they would have a very hard time accessing the secrets. I'll talk about these approaches below.

# Encryption keys

The most important secret in your app is probably your **encryption key**. This is the skeleton key to your kingdom. If someone got ahold of it, they could unlock your user's data and tarnish your reputation. So how can you protect this key if none of the options above truly secure it? What if I told you... that **you don't need to know the key**. If you don't know it, no one can steal it! But how does that work exactly?

# Enter the vault

Enter [Azure Key Vault](https://azure.microsoft.com/en-us/services/key-vault/) which is basically the most hardcore thing ever when you read about how they secure your keys. If you opt for the Premium service tier, your key is stored on **dedicated hardware** called a Hardware Security Module (HSM). I had never heard of these so let me clue you in: they are **[devices](https://en.wikipedia.org/wiki/Hardware_security_module)** where all they do is encrypt and decrypt data and never let the key leave their boundaries. That means, essentially, you present the data you want to encrypt to the device, it encrypts it using a key that **nobody knows**, and spits out the ciphertext for you to store in your system. Azure Key Vault also supports *software-protected* keys which can operate under the same conditions except they are not stored on a dedicated device. The HSM is validated to be [FIPS 140-2 Level 2](https://en.wikipedia.org/wiki/FIPS_140-2#Level_2) compliant (out of 4 levels). What does that mean exactly?

Well, here's Level 1 security:

> Level 1 provides the lowest level of security. Basic security requirements are specified for a cryptographic module (e.g., at least one Approved algorithm or Approved security function shall be used). No specific physical security mechanisms are required in a Security Level 1 cryptographic module beyond the basic requirement for production-grade components. An example of a Security Level 1 cryptographic module is a personal computer (PC) encryption board.

OK, so we're still talking **a dedicated encryption board** to secure keys... how about Level 2?

> Security Level 2 improves upon the physical security mechanisms of a Security Level 1 cryptographic module by requiring features that show evidence of tampering, including tamper-evident coatings or seals that must be broken to attain physical access to the plaintext cryptographic keys and critical security parameters (CSPs) within the module, or pick-resistant locks on covers or doors to protect against unauthorized physical access.

Jeez. That means there are *physical defenses* in place on the device to prevent intrusion. We're not even talking intrusion through the network, no, literally these devices are secured so a **human being** cannot access them. Even if it's not Level 4, that's still *way* more secure than in your App.config or your database or web portal. And even Wikipedia admits that "[very few](https://en.wikipedia.org/wiki/Hardware_security_module#Security)" HSMs are Level 4 validated.

You'd think this hardcore security would be pricey right? Not at all. I think the [$1/key/mo](https://azure.microsoft.com/en-us/pricing/details/key-vault/) price tag is pretty fair considering the security offered.

# What about secrets?

OK. So Azure Key Vault is a pretty good solution to our encryption key problem. What about generic secrets, stuff you will need to pass within your application or to external services? Azure Key Vault supports that without any trouble, though they won't be stored on dedicated hardware. They will still be stored separately from your application behind lock and key which is our ultimate goal.

# But even a safe needs a combination, won't the *vault* require a key?

Yes! And you are right to point out that it doesn't really solve the secrets problem if the key I need to use to unlock the vault is *also* stored in my app.config or portal or database. Luckily, there's a way to solve that!

# Certificates to the rescue

Instead of using the default authentication to Azure AD, a "client ID" and "secret token", we will actually provide a secure X.509 certificate that we'll upload to Azure. Since you can't download the certificate from Azure or access the private key, it will authenticate your application without exposing the key to your vault.

I followed these two guides for setting up Key Vault and authenticating using a certificate:

1. [Getting Started with Azure Key Vault](https://azure.microsoft.com/en-us/documentation/articles/key-vault-get-started/)
2. [Using Azure Key Vault from a Web Application](https://azure.microsoft.com/en-us/documentation/articles/key-vault-use-from-web-application)

Follow the appendix in guide 2 to generate a certificate to authenticate to Azure AD.

Here are some notes:

## PowerShell Cmdlet Changes

For guide 2, in Azure SDK 2.8+, the cmdlets have changed now:

- `New-AzureADApplication` is now `New-AzureRmADApplication`
- `New-AzureADServicePrincipal` is now `New-AzureRmADServicePrincipal`

When executing `Set-AzureKeyVaultAccessPolicy` make sure to add the switch `-PermissionsToSecrets all` to grant permissions to manage secrets.

**Note:** The article tells you to grant `all` permissions to both keys and secrets. In reality, for production, you may want to only grant specific rights. See [this MSDN article](https://msdn.microsoft.com/en-us/library/dn903607.aspx) for the different access policies.

## Self-signed vs. commercial certs

In guide 2, you create a self-signed certificate and in the C# code, you tell .NET **not** to validate the Root CA, since it won't be valid. You can do this but it's probably better to use a signed certificate from a trusted authority. Comodo provides a "[Free Email Certificate](https://www.instantssl.com/ssl-certificate-products/free-email-certificate.html)" that is a signed X.509 cert. When you install it, you can Export it to a file (include the private key) and use it.

## Install the certificate

For guide 2, after generating the certificate you need to install it locally to test Azure Key Vault. If you run your app under IIS and the app pool is `ApplicationPoolIdentity`, you need to install the certificate at the machine-level, not just the current user as the article suggests because the app pool does not have permissions to use *your* certs. You also need to modify the code to look at `StoreLocation.Machine` locally instead of `CurrentUser` on Azure.

In the folder where your cert was generated, you can run the following PowerShell command to import:

    Import-PfxCertificate -filepath "mycert.pfx" -CertStoreLocation "cert:\LocalMachine\My" -Confirm

Otherwise, [follow this guide](http://www.databasemart.com/howto/SQLoverssl/How_To_Import_Personal_Certificate_With_MMC.aspx) to do it from the MMC console. I imported the `.cer` file first, then the `.pfx` but you can just import the PFX (I think) since it contains the public *and* private key.

Once imported, right-click the certificate, click All Tasks -> Manage Private Keys and then add `IIS_IUSRS` account to grant permissions. See [this StackOverflow post](http://stackoverflow.com/a/3176253/109458) for details.

If you don't do this correctly, your application will throw a `Keyset does not exist` error.

**Note:** Import the certificate to the current user AND local machine stores.

# Alright, so what about local secrets?

Ah you got me again! You could still use Key Vault locally, except you'd depend on connectivity. Instead, there's something we can do even if we don't end up using Azure Key Vault. We can **encrypt the settings** in the web.config. Follow [this guide](http://eren.ws/2014/02/04/encrypting-the-web-config-file-of-an-azure-cloud-service) to encrypting the configuration sections (I recommend `appSettings` and `connectionStrings`). You can use the same certificate we created above.

There is one major change though. You need to use a different `PKCS12ProtectedConfigurationProvider`, one that can specify the "StoreLocation" of where to load certificates from the **CurrentUser** store, not the LocalMachine since Azure certs are stored in the CurrentUser store. If you're running through IIS, you will need to set the config property to load certificate from the LocalMachine store.

Here's a modified version:

<script src="https://gist.github.com/kamranayub/eaf4c4e4983ecb2d0b37.js"></script>

You will need to compile it with a signature (in Release mode), generate one in the "Signing" tab of the project properties. You'll also need to get the `publicKeyToken` of the Release DLL, I use [ILSpy](http://ilspy.net/) to do that. Once done, you can add a new attribute to the config:

```
<add 
  name="CustomProvider" 
  thumbprint="xxx" 
  storeLocation="LocalMachine" 
  type="..." />
```

A few notes:

1. This **is not** the same certificate you generated for Azure AD and Key Vault. This is a separate RSA certificate for use with configuration encryption. You must *also* upload the PFX for this to Azure.
1. You will need to install the PKCS12ProtectedConfigurationProvider.dll to the GAC before running the `aspnet_regiis` command. Just run `gacutil -i PKCS12ProtectedConfigurationProvider.dll` beforehand.
2. You will need to reference your custom compiled DLL instead of the one in the guide (make sure to grab your new public key token).
3. Despite this StackOverflow answer, you **can** use encryption with configuration files using the PKCS12 provider.
4. You may need to add a `web.Release.config` transform to set the PKCS provider to use the CurrentUser when building on Azure

# So where are we at?

If you followed all the guides I linked to and followed the notes, you should have the following:

1. An Azure Key Vault set up
2. A certificate that authenticates against Azure AD
3. A certificate that can encrypt/decrypt your web.config
4. Both certificates uploaded to Azure through the portal

Phew! With all this in place, here's what this gets you:

1. Encryption keys are not known, therefore cannot be discovered unless by some Act of God (practically)
2. Your production secrets are not stored anywhere in your application or source control, local secrets and connection strings are encrypted
3. No tokens are used to access Key Vault, instead a signed certificate is used

# Troubleshooting

I ran into a bunch of problems during the writing of this guide. Hopefully these help:

## When I run my app and try to get a secret from Key Vault I get a "Keyset does not exist" error

Your app pool/user running the app does not have access to the private key. Follow my advice above to make sure the certificate is imported to **both** Current User and Local Machine stores and then **grant permissions to the private keys**. Then modify the Key Vault `GetCertificate` helper to use the appropriate store at runtime, like this:

```
StoreLocation storeLocation = AppSettings.RuntimeEnvironment >= RuntimeEnvironment.D
  ? StoreLocation.CurrentUser
  : StoreLocation.LocalMachine;

  X509Store store = new X509Store(StoreName.My, storeLocation);
```

I use an app setting to determine where my app is running.

## When I run my app, I get a "Bad Key" error from the config encryption provider

You are trying to use the same cert you made for Azure AD, you can't do this. Follow the guide I linked to above to make a new `azureconfig` cert and import it the same way you did before (to both certificate stores).

## When I build my app in Azure through Continuous Deployment, it's not able to decrypt the web.config

1. Ensure you uploaded the cert through the portal
2. Ensure you restarted the application (or Stop then Start)
3. Ensure the `storeLocation` attribute is set to `CurrentUser`. If you are using `LocalMachine` locally, you can set the store location at build time through a `web.Release.config` transform (see above)
