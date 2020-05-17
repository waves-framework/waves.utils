using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Fluid.Utils.Nuget.VersionUpdater
{
    class Program
    {
        private const string ProgramName = "[Fluid Nuget Packer]";

        private const string InformationKey = "[INFORMATION]";
        private const string WarningKey = "[WARNING]";
        private const string ErrorKey = "[ERROR]";

        private const string VersionKey = "-Version";
        private const string PropsDirectoryKey = "-PropsDirectory";

        /// <summary>
        /// Gets or sets nuget version.
        /// </summary>
        private static string Version { get; set; }

        /// <summary>
        /// Gets or sets .props directory.
        /// </summary>
        private static string PropsDirectory { get; set; }

        /// <summary>
        /// Main method.
        /// </summary>
        /// <param name="args">Arguments.</param>
        static void Main(string[] args)
        {
            Initialize(args);

            UpdateVersions();
        }

        /// <summary>
        /// Initialized program.
        /// </summary>
        /// <param name="args">Arguments.</param>
        private static void Initialize(string[] args)
        {
            try
            {
                const int argsCount = 4;

                if (args.Length != argsCount)
                {
                    throw new ArgumentException("Invalid arguments specified.");
                }

                // [0] props directory key
                if (!args[0].Equals(PropsDirectoryKey))
                {
                    throw new ArgumentException("Invalid arguments specified (-PropsDirectoryKey).");
                }

                // [1] props directory
                PropsDirectory = args[1];
                if (!Directory.Exists(PropsDirectory))
                {
                    Directory.CreateDirectory(PropsDirectory);
                    Console.WriteLine("{0} {1}: {2}", ProgramName, InformationKey, "Props directory created " + PropsDirectory);
                }

                // [2] version key
                if (!args[2].Equals(VersionKey))
                {
                    throw new ArgumentException("Invalid arguments specified (-Version).");
                }

                // [3] version
                Version = args[3];
                Console.WriteLine("{0} {1}: {2}", ProgramName, InformationKey, "Version initialized - " + Version);

                Console.WriteLine("{0} {1}: {2}", ProgramName, InformationKey, "Utility initialized successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} {1}: {2}", ProgramName, ErrorKey, "An error occurred while initializing the utility:\r\n" + e);

                Environment.ExitCode = 100;
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
                    var doc = XDocument.Load(file);

                    var elements = doc.Elements().Elements().Where(n => n.Name.LocalName == "PropertyGroup")
                        .Elements()
                        .Where(e => e.Name.LocalName == "Version");

                    foreach (var element in elements)
                    {
                        element.Value = Version;
                    }

                    doc.Save(file);
                }

                Console.WriteLine("{0} {1}: {2}", ProgramName, InformationKey, "Props versions updated successfully (" + Version + ")");
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} {1}: {2}", ProgramName, ErrorKey, "An error occurred while updating props versions:\r\n" + e);

                Environment.ExitCode = 100;
            }
        }
    }
}
