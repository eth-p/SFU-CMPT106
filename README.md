# CMPT106
Fall 2017, Group Zeta

# Group Zeta

|Contributor|GitHub Account|Role|
|:--|:--|:--|
|Ethan P.|[eth-p](https://github.com/eth-p)|Programmer|
|Jack M.|[741o](https://github.com/741o)|Programmer, Level Designer|
|Leo L.||Programmer, Composer|
|Linh T.|[jkt637](https://github.com/jkt637)|Artist|
|Matt D.|[MatthewWDoyle](https://github.com/MatthewWDoyle)|Producer|
|Matthew Z.|[mzegar](https://github.com/mzegar)|Artist, Level Designer|
|Sukhdeep P.|[spa111](https://github.com/spa111)|Producer|
|William B.|[wiblwibl](https://github.com/wiblwibl)|Level Designer|

[Discord](https://discord.gg/jfUhzBg)

---

# Contributing
**You will need git-lfs in order to work on this project.**  
An installation guide is available [here](https://help.github.com/articles/installing-git-large-file-storage/).

---

# Unity Quirks
Unfortunately, Unity isn't perfect. If any bugs in the Unity Editor are severe enough to impede our development, it's neccessary to document them here.

## Mac OS X: "Application Not Responding" on Project Open.
Sometimes when you go to load the project, Unity will *permanently* hang for no apparent reason.  
If you check the `~/Library/Logs/Unity/editor.log` file, it will say:

```text
Task failed: Downloader Task
Canceling tasks, domain is going down
```

This is because Unity is failing to connect back to a server for some reason or another.  
There **is no known fix** to this, but a workaround **is** available:

1. Disconnect from the internet by turning off Wi-Fi and/or unplugging the ethernet cable.
2. Start unity.
3. Press the "work offline" button.
4. Load the project.
5. Wait for the editor to show up.
6. Reconnect to the internet.
