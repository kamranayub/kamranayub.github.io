Title: Store Binaries Next to Powershell DSC Using Git LFS
Published: 2017-02-04 09:11 -06:00
Lead: Keep your Powershell DSC scripts and dependent binaries together using Git Large File Storage (LFS)
Tags:
- Git
- Powershell
- DSC
- Automation
- DevOps
- TFS
---

A common need when configuring servers using [Powershell DSC](https://msdn.microsoft.com/en-us/powershell/dsc/overview) is installing software or copying binary files, for example, installing the Java SDK, Sysinternals, MSI installers, or what have you.

<!-- More -->

## Updates

- **6/14/2017**: My good friend and colleague [Erik](https://erikonarheim.com) was going through this exercise at work and let me know the code doesn't *actually* work. Whoops! Apparently when I tested, the binaries weren't actually in Git LFS since I had already checked them in. However, I did manage to solve the issue but for now you may need to use a custom Script resource until I release a DSC module. [Check out this post](/posts/2017-06-14-downloading-git-lfs-files-from-tfs-vsts) for an explanation and solution. I will update this post again with my new resource once available.

## Using a network share

Depending on the software, there are multiple ways to do the installation--using the [Package](https://msdn.microsoft.com/en-us/powershell/dsc/packageresource) resource, copying files/manipulating the registry, etc. Common to all cases is the fact you need a *source* of the installation. Typically this is an exe, msi, zip, or some other large binary file.

If I asked you where to store common installation source files that need to be accessible by any arbitrary server for DSC, you might (rightly) say, "A network share!" This is where most articles on using DSC stop.

You would do something like this, such as copying a zip file from the share to a target node:

```powershell
Configuration WebServer {
  Param($Credential)

  File EnsureSysinternalsIsPresentInInstallationSource {
    Ensure = "Present"
    DestinationPath = "C:\DSC\Sources\SysInternalsSuite-2016-11-18.zip"
    SourcePath = "\\contoso.com\DSC\Sources\SysInternalsSuite-2016-11-18.zip"
    Credential = $Credential
  }

  Archive EnsureSysInternalsIsInstalled {
    Ensure = "Present"
    Destination = "C:\Utilities\Sysinternals"
    Path = "C:\DSC\Sources\SysInternalsSuite-2016-11-18.zip"
  }

}
```

Here we are copying the Sysinternals source archive to the target node for extraction. The network share is secured to some credential we've passed in. 

In some cases, some DSC resources allow using UNC paths as their source but I've found this to be inconsistently implemented, finicky and riddled with permission issues, especially when doing complex installations that may require manual scripting. Behind the scenes Powershell has to mount the remote share and you can run into strange issues because of this--hours and days have been spent diagnosing network issues only to come down to black magic AD configuration. I've been burned too often to recommend this approach.

## Binaries are configuration too

There's another issue. As a developer who moved into a more operational role (DevOps!), I've made it a point to store all our DSC in source control (Git, specifically). So now I'm in a situation where all my configuration is versioned and in source control but my installation sources or other binaries are stored separately in a file share.

I don't like it. You should *store things that relate together next to each other.* If you don't, it introduces extra complexity:

- I need to ensure the entire team can access the file share
- Anyone could accidentally remove or change a file in the share and kill our DSC
- I need to document the sources somewhere in the repository
- The installation source files are not versioned or audited
- As mentioned above, your mileage may vary with access to network resources

The fact is installation binaries *are still configuration.* Changes to the installation sources go hand-in-hand with changes to your DSC because they are married together. In your Git history, you want a record **of all changes to any aspect of your configuration.**

## Using Git LFS to store binaries

Normally storing binaries in Git isn't a recommended approach. Git wasn't designed for gigabye repositories. However, there's an extension available called [Git Large File Storage (LFS)](https://git-lfs.github.com/). It supports gigabye files and reduces repository size by storing the binaries on the remote server.

Since we're using TFS, [Git LFS is supported](https://www.visualstudio.com/en-us/docs/git/manage-large-files). It's also supported on pretty much every major Git hosting provider. The only dependency is that local development requires you to install the Git LFS extension--something easily put into the README of the repository.

Once you configure Git LFS for a repository:

    git lfs track "*.zip"
    git lfs track "*.exe"
    git lfs track "*.msi"

You can now add these binary files and they'll be tracked in Git LFS (track whatever you need).

Let's say for our purposes our repository now looks like this:

    configuration/WebServer.ps1
    installers/sysinternals/SysInternalsSuite-2016-11-18.zip

Now you can efficiently store binary files alongside your configuration files!

## Downloading Installers During DSC Pull

We're using DSC pull, which is the recommended approach for using DSC if you want to scale it out across your clusters. How does our DSC change now that we're storing the installers side-by-side?

Presumably, you've managed to get your configs over to the pull server (which is a series of articles I still intend to write), but your installers are still stored in Git. What to do?

Instead of copying files from a share, we can use some APIs available to use in TFS (or GitHub or what have you) to **download the installers locally.**

The TFS Rest API is documented and you can see there's a way to [download a folder as a zip file or stream a file](https://www.visualstudio.com/en-us/docs/integrate/api/git/items#get-a-file)--which is perfect!

For TFS, the format of the URL is like this:

    GET https://{instance}/DefaultCollection/{project}/_apis/git/repositories/{repository}/items?api-version={version}&scopePath={itemPath}

To get a specific *branch* (in cases where different branches are for different environments), you can pass `&versionType=branch&version={branch}`.

This works for on-premise TFS and VSTS. There are [similar APIs](https://developer.github.com/v3/repos/contents/#get-archive-link) for GitHub.

Using the [xRemoteFile](https://github.com/PowerShell/xPSDesiredStateConfiguration#xremotefile) resource, we can bring down our dependencies to the local machine to use, so our configuration above would change:

```powershell
Configuration WebServer {
  Param($Credential)

  Import-DscResource -Name xPSDesiredStateConfiguration 

  xRemoteFile EnsureSysinternalsIsPresentInInstallationSource {
    DestinationPath = "C:\DSC\Sources\SysInternalsSuite-2016-11-18.zip"
    Uri = "https://tfs.contoso.com/tfs/DefaultCollection/TeamProject/_apis/git/repositories/DSC/items?api-version=1.0&scopePath=installers/sysinternals/SysInternalsSuite-2016-11-18.zip"
    Credential = $Credential
    MatchSource = $True
  }

  Archive EnsureSysInternalsIsInstalled {
    Ensure = "Present"
    Destination = "C:\Utilities\Sysinternals"
    Path = "C:\DSC\Sources\SysInternalsSuite-2016-11-18.zip"
  }

}
```

To use `xRemoteFile` we need to import the xPSDesiredStateConfiguration DSC module [which has to be stored](https://msdn.microsoft.com/en-us/powershell/dsc/pullserver#placing-configurations-and-resources) on the pull server. Then we can use the appropriate URL to download an individual installer, or we could have downloaded the zip of the **installers** folder and unzipped it locally. The `MatchSource` parameter should prevent re-downloading the file if it already exists (and we added the version to the zip file). We are still passing a `Credential` because your source control repository is probably still authenticated.

> **Attention Reader:** This actually doesn't work when using Git LFS. You have to [shave some yaks](/posts/2017-06-14-downloading-git-lfs-files-from-tfs-vsts) before you can get it to work.

If you needed something more sophisticated, you could wrap downloading into a custom resource or Script resource--e.g. [use the SHA1 hash](https://www.visualstudio.com/en-us/docs/integrate/api/git/items#get-item-metadata-for) to compare whether it needed to be downloaded again, etc. Where you go from here is your own choice!

We've been using this pattern with great success--it keeps everything together, it's simple, and it lets us track the versions/history of our installation sources.