# MSBuild.Microsoft.VisualStudio.Web.targets

MSBuild targets for Web and WebApplications that come with Visual Studio. Useful for build servers that do not have Visual Studio installed.

Includes `.targets` files from Visual Studio 2015 that can be found in the `C:\Program Files (x86)\MSBuild\Microsoft\VisualStudio\v14.0\{Web,WebApplications}` directories.

The files are in the `tools\VSToolsPath` directory.

## Install

### NuGet

Package name is `MSBuild.Microsoft.VisualStudio.Web.targets`
https://www.nuget.org/packages/MSBuild.Microsoft.VisualStudio.Web.targets/

### Local NuGet feed

If you can't wait for a new release, you can build your own NuGet package and use a local feed
http://docs.nuget.org/docs/creating-packages/hosting-your-own-nuget-feeds

## Use

Just install the NuGet package. The package automatically sets the `$(VSToolsPath)` property to use the targets file in the tools folder.

Or include the `.targets` files in your `.csproj`

```xml
<Import Project="$(VSToolsPath)\WebApplications\Microsoft.WebApplication.targets" Condition="'$(VSToolsPath)' != '' And Exists('$(VSToolsPath)\WebApplications\Microsoft.WebApplication.targets')" />
<Import Project="$(SolutionDir)\packages\MSBuild.Microsoft.VisualStudio.Web.targets.14.0.0\tools\VSToolsPath\WebApplications\Microsoft.WebApplication.targets" Condition="('$(VSToolsPath)' == '' Or !Exists('$(VSToolsPath)\WebApplications\Microsoft.WebApplication.targets')) And Exists('$(SolutionDir)\packages\MSBuild.Microsoft.VisualStudio.Web.targets.14.0.0\tools\VSToolsPath\WebApplications\Microsoft.WebApplication.targets')" />
```

## Build

In command prompt

    nuget.exe pack

## License

Copyright (C) Microsoft Corporation. All rights reserved.
