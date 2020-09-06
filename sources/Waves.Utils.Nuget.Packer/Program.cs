using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Waves.Core.Base;
using Waves.Core.Base.Enums;
using Waves.Core.Base.Interfaces;
using Waves.Utils.Nuget.Packer.Services;
using Waves.Utils.Nuget.Packer.Services.Interfaces;
using Waves.Utils.Project;

namespace Waves.Utils.Nuget.Packer
{
    /// <summary>
    ///     Waves nuget packer utility.
    /// </summary>
    public static class Program
    {
        private const string NugetVersionKey = "{$NUGETVERSION}";
        private const string NuGetExePathKey = "NuGetExePath";
        private const string WorkingPathKey = "WorkingPath";
        private const string OutputDirectoryKey = "OutputDirectory";
        private const string VersionKey = "Version";
        private const string PropertiesKey = "Properties";

        /// <summary>
        ///     Gets nuspec templates folder.
        /// </summary>
        private static string WorkingTemplatesPath => Path.Combine(Args[WorkingPathKey], "nuget", "templates");

        /// <summary>
        ///     Gets nuspec folder.
        /// </summary>
        private static string WorkingNuspecPath => Path.Combine(Args[WorkingPathKey], "nuget", "nuspec");

        /// <summary>
        /// Gets or sets arguments dictionary.
        /// </summary>
        private static Dictionary<string, string> Args { get; set; }

        /// <summary>
        ///     Gets or sets template files collection.
        /// </summary>
        private static List<string> Templates { get; } = new List<string>();
        
        /// <summary>
        /// Core.
        /// </summary>
        private static ICore Core { get; set; }

        /// <summary>
        ///     Main method.
        /// </summary>
        /// <param name="args">Arguments.</param>
        public static void Main(string[] args)
        {
            try
            {
                Core = new Core.Core();
                Core.Start();
            
                if (Initialize(args)) 
                    Pack();
            
                Core.Stop();

                Environment.ExitCode = 0;
            }
            catch (Exception e)
            {
                Environment.ExitCode = 1;
            }
            
        }

        /// <summary>
        ///     Initialized program.
        /// </summary>
        /// <param name="args">Arguments.</param>
        public static bool Initialize(string[] args)
        {
            try
            {
                Args = Arguments.ReadFromArray(args);
                
                if (!Args.ContainsKey(WorkingPathKey))
                    throw new Exception("Working path is not defined.");
                
                if (!Args.ContainsKey(OutputDirectoryKey))
                    throw new Exception("Output directory is not defined.");

                if (!Args.ContainsKey(NuGetExePathKey))
                    throw new Exception("NuGet.exe path is not defined.");

                if (!Args.ContainsKey(VersionKey))
                    throw new Exception("Version is not defined.");
                
                if (!Args.ContainsKey(PropertiesKey))
                    throw new Exception("Properties is not defined.");
                
                CheckDirectories();
                
                if (!Directory.Exists(Args[WorkingPathKey]))
                    throw new DirectoryNotFoundException("Working directory not found.");
                
                if (!Directory.Exists(Args[OutputDirectoryKey]))
                    throw new DirectoryNotFoundException("Output directory not found.");
                
                if (!File.Exists(Args[NuGetExePathKey]))
                    throw new FileNotFoundException("NuGet.exe not found.");
                
                InitializeTemplates();
                
                Core.WriteLog(new Message("Initializing", 
                    "Utility initialized successfully.",
                    "Packer",
                    MessageType.Success));
                
                return true;
            }
            catch (Exception e)
            {
                Core.WriteLog(new Message("Initializing", 
                    "Error occured while initializing utility:\r\n" + e,
                    "Packer",
                    MessageType.Fatal));
                
                Environment.ExitCode = 1;

                return false;
            }
        }
        
        /// <summary>
        /// Initializes templates.
        /// </summary>
        private static void InitializeTemplates()
        {
            var service = Core.GetInstance<INuspecTemplatesService>();
            if (service == null)
                throw new ArgumentNullException(nameof(service));
            
            service.LoadTemplates(WorkingTemplatesPath);
            service.CreateNuspecs(WorkingNuspecPath, NugetVersionKey, Args[VersionKey]);
        }
        
        /// <summary>
        ///     Creates packages.
        /// </summary>
        public static void Pack()
        {
            var templateService = Core.GetInstance<INuspecTemplatesService>();
            var packerService = Core.GetInstance<IPackerService>();
            
            packerService.Pack(templateService.Nuspecs, 
                Args[OutputDirectoryKey],
                Args[VersionKey],
                Args[PropertiesKey],
                Args[NuGetExePathKey]);
        }

        /// <summary>
        /// Checks directories that they created.
        /// </summary>
        private static void CheckDirectories()
        {
            if (!Directory.Exists(WorkingNuspecPath))
            {
                Directory.CreateDirectory(WorkingNuspecPath);
                    
                Core.WriteLog(
                    new Message(
                        "Directory",
                        "Directory created: " + WorkingNuspecPath,
                        "Program",
                        MessageType.Success
                    ));
            }
                
            if (!Directory.Exists(Args[OutputDirectoryKey]))
            {
                Directory.CreateDirectory(Args[OutputDirectoryKey]);
                    
                Core.WriteLog(
                    new Message(
                        "Directory",
                        "Directory created: " + Args[OutputDirectoryKey],
                        "Program",
                        MessageType.Success
                    ));
            }
        }
    }
}