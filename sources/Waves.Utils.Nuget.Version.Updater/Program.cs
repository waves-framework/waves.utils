using System;
using System.Collections.Generic;
using System.IO;
using Waves.Core.Base;
using Waves.Core.Base.Enums;
using Waves.Utils.Project;

namespace Waves.Utils.Nuget.Version.Updater
{
    /// <summary>
    /// NuGet version updater.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Application name.
        /// </summary>
        private const string Name = "NuGet Version Updater";
        
        /// <summary>
        /// Version type key.
        /// </summary>
        private const string VersionTypeKey = "VersionType";
        
        /// <summary>
        /// Working path key.
        /// </summary>
        private const string WorkingPathKey = "WorkingPath";

        /// <summary>
        /// Sources path.
        /// </summary>
        private static string SourcePath => Path.Combine(Args[WorkingPathKey], "sources");
        
        /// <summary>
        /// Nuspec templates path.
        /// </summary>
        private static string NuspecTemplatesPath => Path.Combine(Args[WorkingPathKey], "nuget", "templates");
        
        /// <summary>
        /// Gets or sets core instance.
        /// </summary>
        private static Core.Core Core { get; set; }
        
        /// <summary>
        /// Gets list of projects.
        /// </summary>
        private static Dictionary<string, string> Projects { get; } = new Dictionary<string, string> ();
        
        /// <summary>
        /// Gets dependencies list.
        /// </summary>
        private static Dictionary<string, List<Package>> Dependencies { get; } = new Dictionary<string, List<Package>>();
        
        /// <summary>
        /// Gets or sets arguments dictionary.
        /// </summary>
        private static Dictionary<string, string> Args { get; set; }
        
        /// <summary>
        /// Main.
        /// </summary>
        /// <param name="args">Arguments.</param>
        public static void Main(string[] args)
        {
            Core = new Core.Core();
            Core.Start();
            
            Initialize(args);
            UpdateVersions();
        }
        
        /// <summary>
        /// Initializes program.
        /// </summary>
        /// <param name="args">Arguments.</param>
        private static void Initialize(string[] args)
        {
            try
            {
                Args = Arguments.ReadFromArray(args);
                
                if (!Args.ContainsKey(VersionTypeKey))
                    throw new Exception("Version type is not defined.");
                
                if (!Args.ContainsKey(WorkingPathKey))
                    throw new Exception("Working directory is not defined.");
                
                if (!Directory.Exists(Args[WorkingPathKey]))
                    throw new DirectoryNotFoundException("Working directory not found.");
                
                if (!Directory.Exists(SourcePath))
                    throw new DirectoryNotFoundException("Sources directory not found.");
                
                if (!Directory.Exists(NuspecTemplatesPath))
                    throw new DirectoryNotFoundException("Nuspec templates directory not found.");

                Core.WriteLog(new Message(
                    "Initializing",
                    "Arguments initialized successfully.",
                    Name,
                    MessageType.Success));
            }
            catch (Exception e)
            {
                Core.WriteLog(new Message(
                    "Initializing",
                    "Error occured while initializing utility:\r\n" + e,
                    Name,
                    MessageType.Fatal));

                throw;
            }
        }

        /// <summary>
        /// Updates versions.
        /// </summary>
        private static void UpdateVersions()
        {
            GetDependencies();
            GetProjects();
        }

        /// <summary>
        /// Gets dependencies.
        /// </summary>
        private static void GetDependencies()
        {
            var info = new DirectoryInfo(NuspecTemplatesPath);
            var files = info.GetFiles();

            foreach (var file in files)
            {
                var name = file.Name
                    .Replace(".nuspec", string.Empty)
                    .Replace(".template", string.Empty);

                var packages = Nuspec.GetDependencyList(file.FullName);
                
                Dependencies.Add(name, packages);

                foreach (var package in packages)
                {
                    Core.WriteLog(new Message(
                        "Dependencies",
                        "Dependency found for project " + name + ": " + package.Id + " (Min: " + package.MinVersion + ", Max: " + package.MaxVersion + ")",
                        Name,
                        MessageType.Information));
                }
            }
        }

        /// <summary>
        /// Gets projects.
        /// </summary>
        private static void GetProjects()
        {
            var info = new DirectoryInfo(SourcePath);
            var directories = info.GetDirectories();

            foreach (var directory in directories)
            {
                var files = directory.GetFiles();
                foreach (var file in files)
                {
                    if (file.Extension == ".csproj")
                    {
                        Projects.Add(file.Name.Replace(".csproj", string.Empty), file.FullName);
                        
                        Core.WriteLog(new Message(
                            "Dependencies",
                            "Project found: " + file.Name.Replace(".csproj", string.Empty) + " (" + file.FullName + ")",
                            Name,
                            MessageType.Information));
                    }
                }
            }
        }

        /// <summary>
        /// Gets newer versions for projects.
        /// </summary>
        private static void GetNewerVersions()
        {
            
        }
    }
}