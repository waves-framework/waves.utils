using System;
using System.IO;
using Waves.Utils.Git;
using Waves.Utils.Nuget;
using Waves.Utils.Project;

namespace Waves.Utils.Sandbox
{
    class Program
    {
        private static string _workingDirectory;
        
        static void Main(string[] args)
        {
            
            var tmp = Directory
                .GetParent(Environment.CurrentDirectory)?
                .Parent?
                .Parent?
                .Parent?.FullName;
            
            if (tmp == null) return;
            
            _workingDirectory = Path.Combine(tmp, "tests", "data");
            
            args = new string[]
            {
                "-Version",
                "0.0.0.0-test",
                "-NuGetExePath",
                Path.Combine(_workingDirectory, "files", "nuget.exe"),
                "-WorkingPath",
                _workingDirectory,
                "-OutputDirectory",
                Path.Combine(_workingDirectory, "bin", "packages"),
                "-Properties",
                "Configuration=Release"
            };

            Nuget.Packer.Program.Main(args);

            //     var packages = Nuspec.GetDependencyList("/Users/egorkhindikaynen/repos/ambertape/waves/utils/tests/data/nuget/templates/Test.nuspec.template");
            //     var nugetManager = new NuGetManager();
            //     
            //     foreach (var package in packages)
            //     {
            //         var version = nugetManager.GetLastPackageMinorVersion(package.Id).Result.Version;
            //
            //         var maxVersion = new Version(
            //             version.Major, 
            //             version.Minor, 
            //             version.Build + 1, 
            //             0);
            //         
            //         package.MinVersion = version;
            //         package.MaxVersion = maxVersion;
            //     }
            //     
            //     Csproj.ReplaceDependencyVersions("/Users/egorkhindikaynen/repos/ambertape/waves/utils/tests/data/sources/Test/Test.csproj", packages);
            //     
            //     //GitManager.Reset("/Users/egorkhindikaynen/repos/ambertape/waves/utils/");
            //     
            //     GitManager.Commit("/Users/egorkhindikaynen/repos/ambertape/waves/utils/", "Test", "Test", "test");
            //     GitManager.Push( "/Users/egorkhindikaynen/repos/ambertape/waves/utils/", "origin","khek", "i90CanSee50This99");



        }
    }
}