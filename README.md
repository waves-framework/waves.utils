## ![logo](files/logo_very_small.png)  Waves Utils

![logo](http://teamcity.ambertape.ru/app/rest/builds/buildType:(id:WAVES_Utils_Release)/statusIcon) ![logo](https://img.shields.io/github/license/waves-framework/waves.utils) ![logo](https://img.shields.io/nuget/v/Waves.Utils.Serialization)

### 📚 About Waves

**Waves** is a small cross-platform framework designed for flexible developing of desktop applications. Its ideology was created by one developer for 3 years and hopes to find support in the community. Enjoy!



### 📒 About Waves.Utils

**Waves.Utils** is set of framework tools and CI/CD tools.



### ⌨️ Usage basics

##### Waves.Utils.Serialization

Simple serialization of objects in binary, XML and JSON formats for **Waves.Core**. 

**Waves.Utils.Props.Version.Updater**

Utility for updating version in *.props* files. Usage:

```bash
versionupdater.exe -PropsDirectory PATH_TO_PROPS_DIRECTORY -Version X.X.X.X
```

**Waves.Utils.Nuget.Packer**

Utility for packing nuget-packages from nuspec-files. Usage:

```bash
packer.exe -NuGetExePath PATH_TO_NuGet.exe -WorkingPath WORKING_DIRECTORY_PATH -OutputDirectory OUTPUT_DIRECTORY_PATH -Version X.X.X.X -Properties Configuration=Release
```



### 📋 Licence

Waves.Core is licenced under the [MIT licence](https://github.com/ambertape/waves.core/blob/master/license.md).
