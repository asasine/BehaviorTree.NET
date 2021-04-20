# BehaviorTree.NET
A behavior tree library in .NET and Unity.

## Installation
This package can be installed using NPM, directly from GitHub, or using a local clone of the repository.

### Install from NPM
Installing with NPM enables easier updates through the Unity Package Manager and is the recommended approach.

#### Add a scoped registry
1. Open the [Project Settings](https://docs.unity3d.com/Manual/comp-ManagerGroup.html) window
1. In the project settings window, open the [Package Manager settings](https://docs.unity3d.com/Manual/class-PackageManager.html).
1. In the package manager settings, [add a new scoped registry](https://docs.unity3d.com/Manual/class-PackageManager.html#scoped_add)
    - For `Name`, use `NPM Registry`
    - For `URL`, use `https://registry.npmjs.org`
    - For `Scope(s)`, add at least one item `com.asasine.behaviortree`
        - You can specify additional scopes for additional packages from the NPM Registry
        - You can specify broader scopes to make additional package available. Adding a scope `com.example` will make all packages that begin with `com.example` available from this registry.
    - Click `Apply`

#### Install the package
1. Open the [Unity Package Manager](https://docs.unity3d.com/Manual/upm-ui.html) window
1. In the top bar, change the packages to `Packages: My Registries`
1. Select `BehaviorTree.NET`
1. In the bottom bar, click `Install`

#### Update the package
1. Open the [Unity Package Manager](https://docs.unity3d.com/Manual/upm-ui.html) window
1. In the top bar, change the packages to `Packages: In Project`
1. Select `BehaviorTree.NET`
1. In the bottom bar, click `Update to v0.1.1`

### Install from Git URL
Installing from a Git URL allows you to fully customize which version of the package you use. If you are forking the package repository and not releasing on NPM, or are developing smaller features and bug fixes, you may find this approach useful.

1. Open the [Unity Package Manager](https://docs.unity3d.com/Manual/upm-ui.html) window
1. In the top bar, click the `+` button
1. In the dropdown, select `Add package from git URL` from the add menu
1. In the text box, enter the GitHub URL of this package: `https://github.com/asasine/BehaviorTree.NET.git`
    - You may also specify releases using tags
        - For example, targeting v0.1.1 would use the URL `https://github.com/asasine/BehaviorTree.NET.git#v0.1.1`
    - You can find all releases listed [here](https://github.com/asasine/BehaviorTree.NET/releases)
    - More information on specifying git references is available in the [Unity Manual](https://docs.unity3d.com/Manual/upm-git.html#revision)
1. In the text box, click `Add`

### Install from disk
Installing from disk lets you use a clone of the repo. This is useful if you are actively developing additional features for the package.
1. Clone the repo to your file system
1. Open the [Unity Package Manager](https://docs.unity3d.com/Manual/upm-ui.html) window
1. In the top bar, click the `+` button
1. In the dropdown, select `Add package from disk` from the add menu
1. In the file explorer, navigate to the cloned repository and open the `package.json` file

Install the package into Unity as a local or git package by following the steps at https://docs.unity3d.com/Manual/upm-concepts.html#Sources.

