using System;
using System.IO;
using LibGit2Sharp;
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
                "-VersionType",
                "Minor",
                "-WorkingPath",
                _workingDirectory
            };
            
            Nuget.Version.Updater.Program.Main(args);
            
            // args = new string[]
            // {
            //     "-Version",
            //     "0.0.0.0-test",
            //     "-NuGetExePath",
            //     Path.Combine(_workingDirectory, "files", "nuget.exe"),
            //     "-WorkingPath",
            //     _workingDirectory,
            //     "-OutputDirectory",
            //     Path.Combine(_workingDirectory, "bin", "packages"),
            //     "-Properties",
            //     "Configuration=Release"
            // };
            //
            // Nuget.Packer.Program.Main(args);
        }
    }
}