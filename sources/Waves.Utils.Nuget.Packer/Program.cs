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
        private const string ProgramName = "[Waves Nuget Packer]";
        private const string InformationKey = "[INFORMATION]";
        private const string WarningKey = "[WARNING]";
        private const string ErrorKey = "[ERROR]";
        private const string PackCommandKey = "pack";
        private const string NugetVersionKey = "{$NUGETVERSION}";
        private const string NuGetExePathKey = "NuGetExePath";
        private const string WorkingPathKey = "WorkingPath";
        private const string OutputDirectoryKey = "OutputDirectory";
        private const string VersionKey = "Version";
        private const string PropertiesKey = "Properties";
        
        private static StringBuilder _output;
        
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
        public static ICore Core { get; set; }

        /// <summary>
        ///     Main method.
        /// </summary>
        /// <param name="args">Arguments.</param>
        public static void Main(string[] args)
        {
            Core = new Core.Core();
            Core.Start();
            
            if (Initialize(args)) 
                Pack();
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
                
                Console.WriteLine("{0} {1}: {2}", ProgramName, InformationKey, "Utility initialized successfully.");

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} {1}: {2}", ProgramName, ErrorKey,
                    "An error occurred while initializing the utility:\r\n" + e);

                Environment.ExitCode = 1;

                return false;
            }
        }
        
        /// <summary>
        ///     Creates packages.
        /// </summary>
        public static void Pack()
        {
            foreach (var fileName in Templates)
            {
                try
                {
                    Pack(fileName);
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} {1}: {2}", ProgramName, ErrorKey,
                        "An error occurred while packing files:\r\n" + e);

                    Environment.ExitCode = 1;
                }
            }
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

        /// <summary>
        /// Initializes templates.
        /// </summary>
        private static void InitializeTemplates()
        {
            var service = Core.GetInstance<INuspecTemplatesService>();
            if (service == null)
                throw new ArgumentNullException(nameof(service));
            
            service.LoadTemplates(WorkingTemplatesPath);
            service.CreateNuspecs(WorkingNuspecPath, VersionKey, Args[VersionKey]);
        }

        /// <summary>
        ///     Creates package from current template.
        /// </summary>
        /// <param name="fileName">Nuspec template file name.</param>
        private static void Pack(string fileName)
        {
            // copy
            var templateFileInformation = new FileInfo(fileName);
            var nuspecFileName = templateFileInformation.Name.Replace(".template", string.Empty);
            var nuspecFileFullName = Path.Combine(WorkingNuspecPath, nuspecFileName);

            Console.WriteLine("{0} {1}: {2}", ProgramName, InformationKey, "Copying template... (" + fileName + ").");

            File.Copy(fileName, nuspecFileFullName, true);

            if (!File.Exists(nuspecFileFullName))
                throw new FileNotFoundException("Nuspec file not copied (" + fileName + ")", nuspecFileFullName);
            Console.WriteLine("{0} {1}: {2}", ProgramName, InformationKey,
                "Nuspec file was copied from template (" + nuspecFileFullName + ").");

            // replace data
            Console.WriteLine("{0} {1}: {2}", ProgramName, InformationKey, "Replacing data... (" + fileName + ").");

            var hasChanges = Nuspec.ReplaceVersionKey(nuspecFileFullName,NugetVersionKey, Args[VersionKey]);

            if (hasChanges)
                Console.WriteLine("{0} {1}: {2}", ProgramName, InformationKey,
                    "Nuspec file was changed (" + nuspecFileFullName + ").");
            else
                Console.WriteLine("{0} {1}: {2}", ProgramName, WarningKey,
                    "Nuspec file wasn't changed (" + nuspecFileFullName + ")");

            // creating package
            var command = PackCommandKey + " " +
                          nuspecFileFullName + " " +
                          OutputDirectoryKey + " " +
                          Args[OutputDirectoryKey] + " " +
                          VersionKey + " " +
                          Args[VersionKey] + " " +
                          PropertiesKey + " " +
                          Args[PropertiesKey];

            Console.WriteLine("{0} {1}: {2}", ProgramName, InformationKey,
                "Creating package... (" + nuspecFileFullName + ").");
            Console.WriteLine("{0} {1}: {2}", ProgramName, InformationKey, command);

            var process = new Process
            {
                StartInfo =
                {
                    FileName = Args[NuGetExePathKey],
                    Arguments = command,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                }
            };

            _output = new StringBuilder();

            process.OutputDataReceived += OnPackProcessOutputDataReceived;
            process.ErrorDataReceived += OnProcessErrorDataReceived;

            process.Start();
            process.BeginOutputReadLine();

            var error = process.StandardError.ReadToEnd().Replace("\r\n", string.Empty);

            process.WaitForExit();

            if (process.ExitCode == 0)
            {
                Console.WriteLine(_output);
                Console.WriteLine("{0} {1}: {2}", ProgramName, InformationKey,
                    "Package created from nuspec file (" + nuspecFileFullName + ").");
                Console.WriteLine("{0} {1}: {2}", ProgramName, InformationKey, "SUCCESS.");
                
                process.OutputDataReceived -= OnPackProcessOutputDataReceived;
                process.ErrorDataReceived -= OnProcessErrorDataReceived;
            }
            else
            {
                Console.WriteLine("{0} {1}: {2}", ProgramName, ErrorKey,
                    "Package not created from nuspec file (" + nuspecFileFullName + ").");
                Console.WriteLine("{0} {1}: {2}", ProgramName, ErrorKey, error);
            }
        }

        /// <summary>
        ///     Notifies when error data received.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Arguments.</param>
        private static void OnProcessErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
                _output.AppendLine(e.Data.Replace("\r\n", string.Empty));
        }

        /// <summary>
        ///     Notifies when output data received.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Arguments.</param>
        private static void OnPackProcessOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
                _output.AppendLine(e.Data.Replace("\r\n", string.Empty));
        }
    }
}