using System;
using Waves.Utils.Git;
using Waves.Utils.Nuget;
using Waves.Utils.Project;

namespace Waves.Utils.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            var packages = Nuspec.GetDependencyList("/Users/egorkhindikaynen/repos/ambertape/waves/utils/tests/data/nuget/templates/Test.nuspec.template");
            var nugetManager = new NuGetManager();
            
            foreach (var package in packages)
            {
                var version = nugetManager.GetLastPackageMinorVersion(package.Id).Result.Version;

                var maxVersion = new Version(
                    version.Major, 
                    version.Minor, 
                    version.Build + 1, 
                    0);
                
                package.MinVersion = version;
                package.MaxVersion = maxVersion;
            }
            
            Csproj.ReplaceDependencyVersions("/Users/egorkhindikaynen/repos/ambertape/waves/utils/tests/data/sources/Test/Test.csproj", packages);
            
            GitManager.Reset("/Users/egorkhindikaynen/repos/ambertape/waves/utils");
        }
    }
}