using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Waves.Core.Base;
using Waves.Core.Base.Enums;
using Waves.Utils.Project;

namespace Waves.Utils.Props.Version.Updater
{
    /// <summary>
    /// Update version utility.
    /// </summary>
    static class Program
    {
        private const string Name = "Version Updater";
        private const string VersionKey = "Version";
        private const string PropsDirectoryKey = "PropsDirectory";

        /// <summary>
        /// Gets or sets nuget version.
        /// </summary>
        private static string Version { get; set; }

        /// <summary>
        /// Gets or sets .props directory.
        /// </summary>
        private static string PropsDirectory { get; set; }
        
        /// <summary>
        /// Gets or sets core instance.
        /// </summary>
        private static Core.Core Core { get; set; }
        
        /// <summary>
        /// Gets or sets arguments dictionary.
        /// </summary>
        private static Dictionary<string, string> Args { get; set; }

        /// <summary>
        /// Main method.
        /// </summary>
        /// <param name="args">Arguments.</param>
        static void Main(string[] args)
        {
            Core = new Core.Core();
            Core.Start();
            
            try
            {
                var watch = new Stopwatch();
                watch.Start();
                
                Core.WriteLog(new Message(
                    "Initializing",
                    "Starting utility...",
                    Name,
                     MessageType.Information));

                Initialize(args);

                UpdateVersions();
            
                watch.Stop();
                var elapsed = Math.Round(watch.Elapsed.TotalSeconds, 1);
                
                Core.WriteLog(new Message(
                    "Cancelling utility",
                    "Time elapsed: " + elapsed + " Seconds",
                    Name,
                    MessageType.Information));
                
                Core.WriteLog(new Message(
                    "Cancelling utility",
                    "Versions successfully updated.",
                    Name,
                    MessageType.Success));

                Environment.ExitCode = 0;
            }
            catch (Exception e)
            {
                Environment.ExitCode = 1;
            }
            
            Core.Stop();
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
                
                if (!Args.ContainsKey(VersionKey))
                    throw new Exception("Version is not defined.");
                
                if (!Args.ContainsKey(PropsDirectoryKey))
                    throw new Exception("Props path is not defined.");
                
                if (!Directory.Exists(Args[PropsDirectoryKey]))
                    throw new DirectoryNotFoundException("Props directory not found.");

                PropsDirectory = Args[PropsDirectoryKey];
                
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
            try
            {
                var files = Directory.GetFiles(PropsDirectory);

                foreach (var file in files)
                {
                    Project.Props.SetVersion(file, Version);
                    
                    Core.WriteLog(new Message(
                        "Updating versions",
                        "Props file updated successfully (" + file + ")",
                        Name,
                        MessageType.Success));
                }
            }
            catch (Exception e)
            {
                Core.WriteLog(new Message(
                    "Updating versions",
                    "Error occured while updating versions:\r\n" + e,
                    Name,
                    MessageType.Fatal));

                throw;
            }
        }
    }
}
