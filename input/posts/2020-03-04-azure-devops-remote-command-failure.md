Title: "Catching Errors with External Commands in PowerShell and Azure DevOps"
Published: 2020-03-04 20:00:00 -0500
Lead: "When calling an external command with PowerShell, how do you capture a printed error message?"
Tags:
- PowerShell
- DevOps
- Azure
---

Here's a quick tip for a problem I ran into within my Azure DevOps pipeline. I have a job task that executes a PowerShell script (Inline) and that script invokes a `git push` command to an Azure Kudu Git endpoint that deploys my site:

```
git push -u kudu HEAD:$env:GITBRANCH
```

I have multiple stages in my pipeline where `GITBRANCH` environment variable gets set to `dev`, `master`, etc. depending on the target environment I'm deploying to.

The Git command will print this kind of output within the task log:

```
remote: Updating branch 'master'.
remote: Updating submodules.
remote: Preparing deployment for commit id 'xxx'.
remote: Running custom deployment command...
remote: Running deployment command...
remote: Handling .NET Web Application deployment.
```

The problem becomes that even when the remote build **fails**, the `git push` command technically executes successfully with exit code 0 so PowerShell's `$LASTEXITCODE` check doesn't fail.

```
##[debug]$LASTEXITCODE: 0
##[debug]Exit code: 0
```

Since the command executed successfully, Azure does not fail the pipeline and I don't get notified when my builds actually fail.

To fix this, we need to somehow capture the output (but still preserve the logs) and check for a specific error string in the output to manually fail the pipeline.

We're looking for a log message like this in the output:

```
remote: An error has occurred during web site deployment.
remote:
remote: Error - Changes committed to remote repository but deployment to website failed.
```

If the string `An error has occurred during web site deployment` is present in the command output, we can fail the build.

So in our PowerShell script, we can take advantage of a neat cmdlet called [Tee-Object](tee-object) which I had never heard of, inspired by [this StackOverflow answer](so-capture-output).

```powershell
cmd /c "git push" '2>&1' | Tee-Object -Variable pushOutput

if ($null -ne ($pushOutput | ? { $_ -match 'An error has occurred during web site deployment' })) {
  Write-Error 'Build failed'
} else {
  Write-Verbose 'Build succeeded'
}
```

What `Tee-Object` does is redirect output to _two places_ (like a T, get it? 🐺) so we get our output both logged _and_ stored in a variable (a PowerShell string array).

What we can then do is operate against our variable `$pushOutput` and match any lines that contain our target string. If there's a match, it will _not equal_ `$null` so the condition will pass and we can write an error.

> **⚠ HUGE ISSUE ALERT:** Normally you would not need to include the `cmd /c` and `'2>&1'` portions of the script to execute the `git push` but because life is unfair, you can actually run into [a specific edge case](gh-tee-object-issue) with git commands where `Tee-Object` does **not** create a variable when all the output directed to it is stderr output (which is unintuitively the case with a `git push` to Azure Kudu). It took me several hours of trial and error with different commands to stumble upon [this StackOverflow answer](so-tee-not-working) that mentioned the edge case. By using `cmd /c` and then utilizing its redirection with `2>&1` it turns it [all into stderr into stdout](so-capture-output) for PowerShell's consumption.

This will properly fail the Azure pipeline now if our remote build fails, hurray! 🤩

Hopefully this helps the one person running into the same issues I did!

### Links

- [Microsoft Docs: PowerShell - Tee-Object][tee-object]
- [StackOverflow: Tee-Object variable not sending to stdout][so-tee-not-working]
- [StackOverflow: How do I capture the output into a variable from an external process in PowerShell?][so-capture-output]
- [GitHub PowerShell/PowerShell Issue #5560: Tee-Object should clear the -Variable target variable if no success-stream input is received][gh-tee-object-issue]

[tee-object]: https://docs.microsoft.com/en-us/powershell/module/microsoft.powershell.utility/tee-object
[so-tee-not-working]: https://stackoverflow.com/questions/47523222/tee-object-variable-not-sending-to-stdout
[so-capture-output]: https://stackoverflow.com/a/35980675/109458
[gh-tee-object-issue]: https://github.com/PowerShell/PowerShell/issues/5560
