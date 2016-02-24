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
- Encryption
- Cloud
---

Secrets. We all have them. I'm talking about secrets like your database connection strings, API keys and encryption keys. Where are you storing yours? Are you storing them...

1. In your application's source code?
2. In a config file (`appSettings` or otherwise) checked into source control?
3. In a database?
4. In a managed portal, like Azure?

I hope you aren't storing them hardcoded. You're probably doing option 2 or a hybrid of options 2-4. Even if you use an external data source, it's hard to escape the need for secrets in local development unless you force your app to rely on having connectivity which makes it hard to work offline.

In this post I'm going to provide some suggestions on how to store your secrets better using Azure Key Vault and config file encryption, specifically for cloud applications.

<!-- More -->

## Why bother?

If you are already storing secrets in the config or an external system, does this even matter? Let me ask you, can a human being find your encryption key? If the answer is Yes, it matters--because the answer should be **No.**

Some of you might say, "It's okay if my secret is in the config, no one but an admin can see it on the web server." You'd think so, but I ran into an exploit last year where you could see files on the web server using a custom file handler vulnerability (the exploit has since been fixed by the vendor). If your secrets are in your configs, are you checking them into source control? If you work in an organization and your code is on the web servers, anyone with access to those servers can see your config files.

You might also (rightfully) say that if an attacker got access to your Azure portal, it's game over anyway. Yes, absolutely. If an app is compromised at the filesystem level where an attacker can upload files, you're pretty much done for anyway. That's why your portal account should have a strong password and have Two-Factor Authentication enabled. If you're using source control integration, that also needs to be protected with the same amount of security--go and [enable TFA for GitHub](https://help.github.com/articles/about-two-factor-authentication/) if you haven't already.  The goal is that we want to avoid storing plaintext secrets on the filesystem and in the portal itself, instead opting to store them in a secure location so that only **our application** has access to them, no one else.

So then, what exactly *are* the benefits of moving secrets out of the portal then?

- Logging -- Azure Key Vault logs all operations, so if someone did compromise your application, you'd have the logs or could monitor them closely for strange actions
- Least privilege -- You can grant a service principal Read-only access so even if the app was compromised, an attacker couldn't change or delete anything (unless they had access to change the policies in Key Vault)
- As-needed access -- By storing secrets away from your application, you at *least* guarantee only the application can access secrets whereas anyone with Read access to the portal can see app settings
- Defense in depth -- You're just adding one more layer of security between an attacker and your data
- Shared storage -- If you have multiple apps or services, using a single vault is useful and you can grant access policies at the secret/key level
- Right thing to do -- You owe it to your users and to your business to protect their data to the best of your ability

[This article](http://blogs.msdn.com/b/data_insights_global_practice/archive/2015/09/24/protecting-sensitive-data-with-azure-key-vault.aspx) sums it up nicely:

> One of the key security principals that is implicitly being applied here is to compartmentalize management of privileged data to security domains for which this is appropriate. An instance of Key Vault is used to manage the Twitter keys as a shared resource in the customer's environment, with access granted by whomever manages the Twitter account on an as-needed basis to specific applications and users. Applications are then responsible for managing only their application-specific Key Vault access tokens.

With that in mind, let's move on!

## Encryption keys

The most important secret in your app is probably your **encryption key**. This is the skeleton key to your kingdom. If someone got ahold of it, they could unlock your user's data and tarnish your reputation. If your Azure or portal account was compromised (even after Two Factor Auth), would attackers have access to your keys? They would if you stored them in a config or in the portal. So how can you protect this key if none of the options above truly secure it? 

Well, what if I told you that **you don't need to know the key**. If nobody knows it, no one can steal it! But how does that work exactly? Magic? Not exactly...

## Enter the vault

Enter [Azure Key Vault](https://azure.microsoft.com/en-us/services/key-vault/) which is basically the most hardcore thing ever when you read about how they secure your keys. If you opt for the Premium service tier, your key is stored on **dedicated hardware** called a Hardware Security Module (HSM). I had never heard of these so let me clue you in: they are **[devices](https://en.wikipedia.org/wiki/Hardware_security_module)** where all they do is encrypt and decrypt data and never let the key leave their boundaries. That means, essentially, you present the data you want to encrypt to the device, it encrypts it using a key that **nobody knows**, and spits out the ciphertext for you to store in your system. Azure Key Vault also supports *software-protected* keys which can operate under the same conditions except they are not stored on a dedicated device. The HSM is validated to be [FIPS 140-2 Level 2](https://en.wikipedia.org/wiki/FIPS_140-2#Level_2) compliant (out of 4 levels). What does that mean exactly?

Well, here's Level 1 security:

> Level 1 provides the lowest level of security. Basic security requirements are specified for a cryptographic module (e.g., at least one Approved algorithm or Approved security function shall be used). No specific physical security mechanisms are required in a Security Level 1 cryptographic module beyond the basic requirement for production-grade components. An example of a Security Level 1 cryptographic module is a personal computer (PC) encryption board.

OK, so we're still talking **a dedicated encryption board** to secure keys... how about Level 2?

> Security Level 2 improves upon the physical security mechanisms of a Security Level 1 cryptographic module by requiring features that show evidence of tampering, including tamper-evident coatings or seals that must be broken to attain physical access to the plaintext cryptographic keys and critical security parameters (CSPs) within the module, or pick-resistant locks on covers or doors to protect against unauthorized physical access.

Jeez. That means there are *physical defenses* in place on the device to prevent intrusion. We're not even talking intrusion through the network, no, literally these devices are secured so a **human being** cannot access them. Even if it's not Level 4, that's still *way* more secure than in your App.config or your database or web portal. And even Wikipedia admits that "[very few](https://en.wikipedia.org/wiki/Hardware_security_module#Security)" HSMs are Level 4 validated.

You'd think this hardcore security would be pricey right? Not at all. I think the [$1/key/mo](https://azure.microsoft.com/en-us/pricing/details/key-vault/) price tag is pretty fair considering the security offered.

## What about secrets?

OK. So Azure Key Vault is a pretty good solution to our encryption key problem. What about generic secrets, stuff you will need to pass within your application or to external services? Azure Key Vault supports that without any trouble, though they won't be stored on dedicated hardware. They will still be stored separately from your application behind lock and key which is our ultimate goal.

## But even a safe needs a combination, won't the *vault* require a key?

Yes! And you are right to point out that it doesn't really solve the secrets problem if the key I need to use to unlock the vault is *also* stored in my app.config or portal or database. Luckily, there's a way to solve that!

## Certificates to the rescue

Instead of using the default authentication to Azure AD, a "client ID" and "secret token", we will actually provide a secure X.509 certificate that we'll upload to Azure. Since you can't download the certificate from Azure or access the private key, it will authenticate your application without exposing the key to your vault in a config or portal interface.

## Let's do it!

I followed these two guides for setting up Key Vault and authenticating using a certificate:

1. [Getting Started with Azure Key Vault](https://azure.microsoft.com/en-us/documentation/articles/key-vault-get-started/)
2. [Using Azure Key Vault from a Web Application](https://azure.microsoft.com/en-us/documentation/articles/key-vault-use-from-web-application)

Follow the appendix in guide 2 to generate a certificate to authenticate to Azure AD.

Here are some notes:

### PowerShell Cmdlet Changes

For guide 2, in Azure SDK 2.8+, the cmdlets have changed now:

- `New-AzureADApplication` is now `New-AzureRmADApplication`
- `New-AzureADServicePrincipal` is now `New-AzureRmADServicePrincipal`

When executing `Set-AzureKeyVaultAccessPolicy` make sure to add the switch `-PermissionsToSecrets all` to grant permissions to manage secrets.

**Note:** The article tells you to grant `all` permissions to both keys and secrets. In reality, for production, you may want to only grant specific rights. See [this MSDN article](https://msdn.microsoft.com/en-us/library/dn903607.aspx) for the different access policies.

### Certificates...?

If you're like me, you probably find certificates can be confusing. Are you making an SSL cert? Not exactly. *Most* SSL certs are X.509 certs (not all) but they also can be used to encrypt web traffic. "Plain" X.509 certs can be used to sign things or encrypt/authenticate, which is what we're doing. If you Google around, you'll see they can be called "client certificates" or "personal" certificates. There are two places a cert can be installed ("stores"), one is the "Local Machine" store and the other is the "Current User" store. The machine store can be accessed by *any* user account, the current user store can only be accessed by the user running the process (usually, you). A "cer" file is the **public key** for your certificate. You can distribute it to anyone. The "pfx" file contains **both the private AND public key**. **DO NOT GIVE IT TO ANYONE.** You want the PFX file for yourself only and to import into your PC and into Azure. The PFX file is protected by a password, I recommend a strong one and *don't lose it.*

### Self-signed vs. commercial certs

In guide 2, you create a self-signed certificate and in the C# code (`GetCertificate`), you tell .NET **not** to validate the Root CA, since it won't be valid. You can do this but it's probably better to use a signed certificate from a trusted authority. Comodo provides a "[Free Email Certificate](https://www.instantssl.com/ssl-certificate-products/free-email-certificate.html)" that is a signed X.509 cert. When you install it, you can Export it to a file (include the private key) and use it.

### Install the certificate

For guide 2, after generating the certificate you need to install it locally to test Azure Key Vault. If you run your app under IIS and the app pool is `ApplicationPoolIdentity`, it's best to just [change it](http://www.iis.net/learn/manage/configuring-security/application-pool-identities) to run under your account. Trust me, it'll be easier. Since Azure requires the certificate to be in the `CurrentUser` store, the default app pool runs under a different account (see [this StackOverflow post](http://stackoverflow.com/a/3176253/109458)), so you'd have to install the cert at the machine level.

In the folder where your cert was generated, right-click the `.pfx` file and select Install. Enter the password you chose. You can also [follow this guide](http://www.databasemart.com/howto/SQLoverssl/How_To_Import_Personal_Certificate_With_MMC.aspx) to do it from the MMC console.

## Alright, so what about local secrets?

We have the cloud secrets squared away. You *could* still use Key Vault locally, except you'd depend on connectivity (and pay for the usage). Instead, there's something we can do even if we don't end up using Azure Key Vault. We can **encrypt the settings** in the web.config. 

I started with [this guide](http://eren.ws/2014/02/04/encrypting-the-web-config-file-of-an-azure-cloud-service) to encrypting the configuration sections (I recommend `appSettings` and `connectionStrings`). You **cannot** use the same certificate you generated in the tutorial above (well, maybe you could but you need the `-exchange sky` switch to `makecert` and I didn't try that initially so I generated a separate certificate).

There's another thing. You also need to use a different `PKCS12ProtectedConfigurationProvider`. The one provided **only** searches the `LocalMachine` certificate store but in Azure, your cert is installed for the current user, so the provider fails to decrypt the config when you try to build your app on Azure because it cannot find the certificate. You need a provider that can specify the `StoreLocation` of where to load certificates from. For Azure, it must be the **CurrentUser** store.

Here's my modified version:

<script src="https://gist.github.com/kamranayub/eaf4c4e4983ecb2d0b37.js"></script>

I've also added it to [GitHub](https://github.com/kamranayub/PKCS12ProtectedConfigurationProvider). You can download the DLL directly from [GitHub](https://github.com/kamranayub/PKCS12ProtectedConfigurationProvider/releases/tag/v1.0.1). Once done, you can change the entry in the config to:

```
    <configProtectedData>
        <providers>
            <add name="CustomProvider"
                 thumbprint="xxx"
                 storeLocation="LocalMachine"
                 type="Pkcs12ProtectedConfigurationProvider.Pkcs12ProtectedConfigurationProvider, PKCS12ProtectedConfigurationProvider, Version=1.0.1.0, Culture=neutral, PublicKeyToken=455a6e7bdbdc9023" />
        </providers>
    </configProtectedData>
```

A few notes:

1. This **is not** the same certificate you generated for Azure AD and Key Vault. This is a separate RSA certificate for use with configuration encryption. You must *also* upload the PFX for this to Azure.
1. You will need to install the PKCS12ProtectedConfigurationProvider.dll to the GAC before running the `aspnet_regiis` command. Just run `gacutil -i PKCS12ProtectedConfigurationProvider.dll` beforehand.
2. You will need to reference the custom compiled DLL instead of the one in the guide
3. I found [this StackOverflow question](http://stackoverflow.com/questions/17189441/web-config-encryption-for-web-sites) but there was no answer. Now there is.

### Storing secrets outside `<appSettings>`

I ran into a major hurdle that caused me some grief. It turns out, **YOU CANNOT ENCRYPT THE `<appSettings>` SECTION!** See [this SO question](http://stackoverflow.com/questions/15067759/why-cant-i-encrypt-web-config-appsettings-using-a-custom-configprotectionprovid).

Other sections are just fine but for whatever reason, IIS just **requires** you to GAC the config provider for it to work. In Azure web apps, we cannot GAC. So what can we do? We can use our **own** config section!

Here's an implementation example of an `ISecretsProvider` contract and a `ConfigSecretsProvider` example implementation. You'd also create an `AzureKeyVaultSecretsProvider` probably to handle getting secrets from Azure Key Vault using the code from the guides above.

<script src="https://gist.github.com/kamranayub/eb6518356ac2b2f1a72a.js"></script>

The `ConfigSecretsProvider` will use environment variables defined in Azure *first* then fallback to the config. This mirrors how app settings work in Azure.

**Note:** Here I am deciding to use only one provider per environment. You might want an implementation that actually uses both. My Key Vault implementation actually uses the `ConfigSecretsProvider` to find the URL to load the secret for, so that in Azure, the app settings just specify the Key Vault secret URL to load:

![App settings in Azure](https://cloud.githubusercontent.com/assets/563819/13271735/9a0f600a-da5c-11e5-9ff0-106d5e009464.png)

This way, locally I can use the raw value (encrypted) and then in Azure, reference the URL for the secret.

To encrypt the `<appSecrets>` section, just run the the command (in the same directory as the web.config and using the Visual Studio Command Prompt):

```
aspnet_regiis -pef appSecrets . -prov CustomProvider
```

And to decrypt:

```
aspnet_regiis -pdf appSecrets .
```

Easy peasy!

## So where are we at?

If you followed all the guides I linked to and followed the notes, you should have the following:

1. An Azure Key Vault set up with a secret to test with
2. A certificate that authenticates against Azure AD
3. A certificate that can encrypt/decrypt your web.config
4. Both certificates uploaded to Azure through the portal
5. A `<appSecrets>` section in your config for local secrets that is encrypted

Phew! With all this in place, here's what this gets you:

1. Encryption keys are not known, therefore the **most** an attacker could do if they compromised the application is to decrypt every user through Key Vault which is an audited system and slows them down
2. Your production secrets are not stored anywhere in your application or source control, local secrets and connection strings are encrypted
3. No cleartext tokens are used to access Key Vault, instead a signed certificate is used

## Implementation notes

The article above for getting started with a web app is a good place to start but I did a few things to make it easy to test and work with locally.

1. I created an `ISecretsProvider` interface with two implementations: a config provider and a Key Vault provider. This also lets me mock for testability.
2. When I bind the `ISecretsProvider` for dependency injection, I inspect the current environment and use the appropriate provider (config locally, key vault otherwise)

Some other thoughts of what you might want to do:

- Add some logging/telemetry around calls to key vault, such as [App Insights' track dependency](https://azure.microsoft.com/en-us/documentation/articles/app-insights-api-custom-events-metrics/#track-dependency)
- When the Key Vault client supports returning `SecureStrings`, you could use that to protect secrets in memory
- Rotate encryption keys every so often (store the version of the key used on the entities), though this might be pricey for HSM keys
- Encrypt secrets before storing them and then decrypt them at runtime (might be overkill)

## Troubleshooting

I ran into a bunch of problems during the writing of this guide. Hopefully these help:

### When I run my app and try to get a secret from Key Vault I get a "Keyset does not exist" error

Your app pool/user running the app does not have access to the private key. Follow my advice above to change the app pool identity to your own user account.

I use an app setting to determine where my app is running.

### When I run my app, I get a "Bad Key" error from the config encryption provider

You are trying to use the same cert you made for Azure AD, you can't do this. Follow the guide I linked to above to make a new `azureconfig` cert and import it the same way you did before (to both certificate stores).

### When I build my app in Azure through Continuous Deployment, it's not able to decrypt the web.config

1. Ensure you uploaded the PFX file through the portal
2. Ensure you restarted the application (or Stop then Start)
  - You can use the Kudu console to run Powershell to check if your cert is uploaded.
  - `PS> Set-Location Cert:\CurrentUser\My`
  - `PS> Get-ChildItem`
3. Ensure the `storeLocation` attribute in the web.config is set to `CurrentUser`

I tried to figure out how to run with `LocalMachine` in dev and `CurrentUser` in Azure but I couldn't do it via config, because the web.config is decrypted *before* any transforms happen. You could probably roll your own provider that checks for certain environment configuration but it was easier to just run my app as my own user in IIS to work around it.
