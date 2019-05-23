Title: "Automating RavenDB 4 with Ansible"
Published: 2019-05-22 20:31:00 -0500
Lead: Useful tips for setting up RavenDB 4 on Docker using Ansible
Image: images/2019-05-22-masthead.jpg
Tags:
- RavenDB
- DevOps
- Ansible
---

I've finished migrating my database infrastructure for [Keep Track of My Games](https://ktomg.com) from [RavenDB](https://ravendb.net) 3.5 to 4.1 ([and soon, 4.2](https://ravendb.net/features/ravendb-42-features)!).

## Summary

I thought I'd take a moment to lay out how I used [Ansible](http://ansible.com), a popular configuration automation tool, to stand up my RavenDB servers with a bit less manual work and to maintain easy upgrades.

Since I [use Vultr for hosting as it's fast and cheap](https://www.vultr.com/?ref=7006849) I decided to "do it right" and plan for the future architecture of KTOMG which I hope to work towards this year, namely hosting on Linux only and cutting a ton of cost from my monthly hosting bill.

I believe it's important to treat your servers [like cattle, not pets](https://medium.com/@Joachim8675309/devops-concepts-pets-vs-cattle-2380b5aab313). Unfortunately, when I originally set up KTOMG on my Vultr VMs I didn't follow my own advice (remember dear reader: Do as I *say* not as I *do*). I had no automation scripts or configuration management, it was entirely bespoke (with documented steps, granted). I have used [Ansible](http://ansible.com) in the past and knew it would be a good fit for configuring my new RavenDB VMs.

## Preparing the VM

In order to execute Ansible in the first place, I needed to set up a fresh Ubuntu VM on Vultr. Typically, you don't want to just run as `root` for everything in Linux. Instead, you set up dedicated users with permissions to do the things you need. This is the [principle of least privilege](https://en.wikipedia.org/wiki/Principle_of_least_privilege). I followed the steps outlined in this [basic guide to securing Ubuntu 18.04](https://www.vultr.com/docs/initial-secure-server-configuration-of-ubuntu-18-04) to set up a deployment user (`deploy`) with appropriate permissions and lock down SSH.

In Vultr as well I created [a Firewall group](https://www.vultr.com/docs/vultr-firewall) for the databases that allows my custom SSH port through (from specific IPs) which provides another layer of security.

The last thing I did was [enable Canonical Live Patch](https://www.ubuntu.com/livepatch) to hotpatch the servers without rebooting, which is free for 3 servers. I did try to set up unattended upgrades with Ansible but was running into strange apt locking issues.

Much of this VM setup could be automated at least partially with VM Startup Scripts, a feature Vultr offers. Since it only takes a couple minutes and I only have a few VMs, I didn't bother with that step.

Once you test logging in via SSH with your new `deploy` user, you can configure Ansible to do the same.

## Setting Up Your Inventory

Ansible has the idea of an [inventory](https://docs.ansible.com/ansible/latest/user_guide/intro_inventory.html) which is the sets of servers to configure. You can group them and provide ansible variables to each one as-needed.

Ansible also supports [dynamic inventory](https://docs.ansible.com/ansible/latest/user_guide/intro_dynamic_inventory.html) and [even supports Vultr](https://docs.ansible.com/ansible/latest/scenario_guides/guide_vultr.html) as a source! I opted to not do this, again due to the limited number of VMs I needed to configure.

The inventory file looks something like this:

```
[dbservers]
10.0.0.1 
10.0.0.2

[dbservers:vars]
ansible_port=12345
ansible_user=deploy
ansible_become_user=ravensu
ansible_become_method=su
```

Each node is listed that you wish to configure followed by some shared "group" variables that you want to provide to Ansible (they are shared for each node, see [variable precedence](https://docs.ansible.com/ansible/latest/user_guide/playbooks_variables.html#ansible-variable-precedence)). In this case, we are connecting as the user `deploy` on a custom SSH port.

What is `ansible_become_user`? For the playbook we'll use, most if not all the tasks requires sudo permissions to configure. The way I accomplish this is by ensuring I *connect* via SSH with my approved user (`deploy`) who has low permissions, then at playbook runtime specify Ansible to [become](https://docs.ansible.com/ansible/latest/user_guide/become.html) a sudo user (`ravensu`) to execute any privileged tasks.

**Note:** The `ansible_become_method=su` was the only option that worked for me against my Vultr VMs and I'm unsure why. The default is `sudo` but that wasn't working!

## Creating the Playbook

In my example playbook, I only do a handful of tasks to set up a server. Possibly in the future, it'd be nice to even automate some of the cluster setup tasks you need to perform for Raven, scripted using the HTTP API. **Bonus points:** Make a formal Ansible role!

At a high-level, the playbook does the following:

1. Install Docker on Linux
2. Copy RavenDB settings and certificates to the remote machine
3. Start a RavenDB 4 container with the appropriate volume mounts, networking configuration and environment variables set

View the playbook in my [ravendb-devops](
https://github.com/kamranayub/ravendb-devops/blob/master/ansible/ravendb.yml) repository.

I want to touch on a few key configuration settings:

1. **Port Mapping:** The `published_ports` maps the VM host ports (8081, 38889) to the container ports Raven will bind to (8080, 38888). I don't *think* they need to be different but in experimenting with setting up Raven I ended up using different ports.
1. **Settings:** I am providing a custom `settings.json` file from the host machine. This contains pre-set config with IP addresses or DNS for what URLs to bind to and other server settings I wanted to customize.
1. **Certificates:** Raven uses two different kinds of certificates for server administration: a Cluster certificate and an Admin Client certificate. The cluster certificate is for communicating between nodes securely. The Admin Client cert is for you to connect to Studio securely via your browser. Raven needs the private key so it can authenticate you to the Studio interface using Client Certificate Authentication. Both certs need to be copied to the remote host and we map the host `/var/db_cert` to the container path `/opt/RavenDB/cert` where Raven looks for certificates.
1. **Raven Args:** The `RAVEN_ARGS` env variable sets up some config needed such as the path to the custom `settings.json` file we copy over, specifying no setup wizard experience, and what server URL to bind Raven to, in this case the private container address `172.17.0.2:8080`. 
1. **Volumes:** Containers stop and start, they are ephemeral. We don't want to lose our data if a container is deleted either (during an upgrade to a new image, say). Raven stores data within the container at `/opt/RavenDB/Server/RavenData` and we map that to our host path `/var/db` so we can take backups, persist to SSD or block storage, etc.

**Remember:** Raven is running within a container, so we bind to a private IP. We then *map* the container port (8080) to a host port (8081) to expose it. Why don't we bind to `10.0.0.1`, the host IP? Because Raven can't see that from within a container and it'll fail to boot up. Instead, the DNS `*.ravendb.community` can be used to set up a name that points to the private VM network IP and so you'll never need to expose Raven publicly outside your VM, besides the Studio interface to manage it (which you can lockdown to a limited IP range using Vultr).

**Note about DNS:** Notice I am copying certificates over. Where did I get them? I actually generated them all through Setup Wizard in a **fresh local RavenDB installation** so I could more easily set up a new cluster with certs and DNS already created. You can walk through the same steps with a local Docker container of RavenDB.

I found the following guides helpful when setting up RavenDB in a container:

- [RavenDB Installation: Setup Wizard](https://ravendb.net/docs/article-page/4.2/csharp/start/installation/setup-wizard)
- [RavenDB: Docker on AWS Linux VM](https://ravendb.net/docs/article-page/4.2/csharp/start/installation/setup-examples/aws-docker-linux-vm)
- [RavenDB: Running in a Docker Container](https://ravendb.net/docs/article-page/4.2/csharp/start/installation/running-in-docker-container)
- [DockerHub for ravendb/ravendb](https://hub.docker.com/r/ravendb/ravendb/)

## Running the Ansible Playbook

Now that we have our `ravendb.yml` playbook and `inventory` file, we can run `ansible-playbook`:

    ansible-playbook -i inventory -K ravendb.yml

The `-K` flag will prompt for the `ansible_become_user` password. This **should not** be anywhere in plaintext in source control or your host machine. It either needs to be provided when running the command interactively or via [Ansible Vault](https://docs.ansible.com/ansible/latest/user_guide/vault.html).

You'll be promoted first for the `become_user` password, then the SSH key phrase if necessary, and finally it will run through the playbook. 

## Finishing the Installation

If it was successful, everything will be `ok` and you can then browse to the Studio in your local browser against your VM's public IP address (and hopefully, only allowing your IP via a firewall rule).

If the Studio prompts for your certificate, choose it, and now you can finish setting up the cluster:

1. Enter your license to activate the cluster
2. [Set up backups](https://ravendb.net/docs/article-page/4.2/csharp/server/ongoing-tasks/backup-overview), which took all of 5 minutes to hook up to Azure Storage.
3. [Create client certs](https://ravendb.net/docs/article-page/4.2/csharp/server/security/authentication/certificate-management) for your applications to [connect to Raven programmatically](https://ravendb.net/docs/article-page/4.2/csharp/server/security/authentication/client-certificate-usage). When creating a client certificate in the Studio, a zip file will be downloaded automatically after generating containing your `pfx` file you can use in your app.

## Conclusion

While I've laid out what works for me if you deviate chances are you'll need to do a continuous trial-and-error process to successfully configure your Raven installation. But I hope this provides a nice starting point for configuring a production cluster! If you run into any issues, feel free to leave a comment so others can learn too.

> If you are still learning RavenDB 4, [check out my Getting Started with RavenDB 4](https://bit.ly/PSRavenDB4) Pluralsight course! It covers the basics like how to get around the Studio, connect with .NET, and more. Using my link won't cost you anything extra but I will get credit for your subscription which helps me out!