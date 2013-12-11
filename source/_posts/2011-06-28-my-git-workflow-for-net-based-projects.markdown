---
layout: post
title: "My Git workflow for .NET-based projects"
date: 2011-06-28 17:38:24 -0500
comments: true
categories:
permalink: /blog/posts/13/my-git-workflow-for-net-based-projects
disqus_identifier: 13
---

When I was new to Git, it was very confusing for me to get up and running with how to deal with the command-line interface (if you're fancy and use a GUI, leave now). Once you do get the hang of it, it's super nice and fluid. I used to do terminal CVS at my last job so I'm not out of touch with using version control from a console.

# Set up your .gitignore

Here's mine:

```
#OS junk files
[Tt]humbs.db
*.DS_Store

#Visual Studio files
*.[Oo]bj
*.exe
*.pdb
*.user
*.aps
*.pch
*.vspscc
*.vssscc
*_i.c
*_p.c
*.ncb
*.suo
*.tlb
*.tlh
*.bak
*.[Cc]ache
*.ilk
*.log
*.lib
*.sbr
*.sdf
*.docstates
ipch/
obj/
[Bb]in
[Dd]ebug*/
[Rr]elease*/
[Pp]ublish*/
Ankh.NoLoad

#Tooling
_ReSharper*/
*.resharper
[Tt]est[Rr]esult*

#Project files
[Bb]uild/

#Subversion files
.svn

# Office Temp Files
~$*

# Custom
*.private
readme.html
```

At the end, you can add whatever you want. For some projects, I store API keys in a `.private` file. You might want to as well.

# Init your repo

The first command you'll need to execute is `git init`. This will make whatever folder you're in a new Git repository (repo).

```bash
Kamran@Kamranicus /c/Projects/Contrib/.JSON
$ git init
```

You should then add all your files and commit them for an initial "base" version.

```bash
git add .
git commit -am "Initial project commit"
```

Because of your `.gitignore` file, Git will ignore all the unnecessary files .NET solutions tend to generate.

# Set up a remote

If you're doing stuff on AppHarbor or GitHub, you'll need to add a "remote." This is just a remote Git repository you can associate with your local repository. You can have lots of remotes. I tend to have one or two.

```bash
git remote add https://kamranayub@github.com/<some url>
```

This information is usually provided by your remote site (GitHub has it when you create a new repo on the site).

# Status, Add, Commit, Push

I do a lot of my projects on [AppHarbor](http://appharbor.com) now. It's just a real nice workflow and I'll explain why.

I make changes to my .NET solution and when I'm ready to commit (usually after a feature or bug fix) my console  command history looks like this:

```
git status
git add .
git commit -am "My commit message"
git push appharbor master
```

## git status

This command will display what things you currently have pending and what changes are untracked. Use this to determine if you should not track a certain change (using `git checkout -- <file>`).

## git add .

This command will add whatever "untracked" and modified changes you have currently pending to ready your commit. Deletes, adds, etc. usually are untracked. Git tracks modifications and renames. The `.` adds all files (relative to your current folder). You can be as specific or as generic as you want.

## git commit -am [message]

This command commits your changes. The `-am` switch is for all changes (a) with a message (m). You can type `-?` to get a list of switches you might want to use. If you don't use the `-a` switch, your untracked changes may not be committed. If someone wants to explain why this is the case, I'd love to know (or I guess I could look it up). Whatever the reason is, use `-am` unless you know otherwise.

## git push [remote] [branch]

This command will send a git `push` command to your remote server. In my case, it's appharbor. GitHub will usually tell you to add their remote as `origin` but you can name them whatever you want. I tend to have `appharbor` and `appharbor-d` for staging.

Once you push, your latest changes will be on the remote server. It is usually a good idea to wait to push until you have a stable release ready for your branch. It could be after one commit or after 10 commits.

# Git pushin'

So now you know what a typical workflow is for my .NET development. If you're interested in file structure, check out my [.JSON](http://github.com/kamranayub/.JSON) project.