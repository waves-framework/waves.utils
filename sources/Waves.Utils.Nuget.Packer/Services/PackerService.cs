using System;
using System.Collections.Generic;
using System.Composition;
using System.Diagnostics;
using System.IO;
using System.Text;
using Waves.Core.Base;
using Waves.Core.Base.Enums;
using Waves.Core.Base.Interfaces;
using Waves.Core.Base.Interfaces.Services;
using Waves.Utils.Nuget.Packer.Services.Interfaces;

namespace Waves.Utils.Nuget.Packer.Services
{
    /// <summary>
    /// Packer service.
    /// </summary>
    [Export(typeof(IService))]
    public class PackerService : Service, IPackerService
    {
        private const string PackCommandKey = "pack";
        private const string NuGetExePathKey = "NuGetExePath";
        private const string OutputDirectoryKey = "OutputDirectory";
        private const string VersionKey = "Version";
        private const string PropertiesKey = "Properties";
        
        private StringBuilder _output;
        
        /// <inheritdoc />
        public override Guid Id => Guid.Parse("4EDF8A5A-21DB-4D2A-BA71-D3385746FAFF");

        /// <inheritdoc />
        public override string Name { get; set; } = "NuGet Packer Service";
        
        /// <inheritdoc />
        public override void Initialize(ICore core)
        {
            if (IsInitialized) return;

            Core = core;

            OnMessageReceived(this,
                new Message("Initialization", "Service has been initialized.", Name, MessageType.Information));

            IsInitialized = true;
        }

        /// <inheritdoc />
        public override void LoadConfiguration()
        {
            OnMessageReceived(this, new Message("Loading configuration", "There is nothing to load.",
                Name,
                MessageType.Information));
        }

        /// <inheritdoc />
        public override void SaveConfiguration()
        {
            OnMessageReceived(this, new Message("Saving configuration", "There is nothing to save.",
                Name,
                MessageType.Information));
        }
        
        /// <inheritdoc />
        public override void Dispose()
        {
        }

        /// <inheritdoc />
        public void Pack(List<string> fileNames, string outputPath, string version, string properties, string nugetExePath)
        {
            foreach (var fileName in fileNames)
            {
                Pack(fileName, outputPath, version,properties,nugetExePath);
            }
        }

        /// <summary>
        /// Creates package from nuspec file to <see cref="outputPath"/>.
        /// </summary>
        /// <param name="fileName">Nuspec file name.</param>
        /// <param name="outputPath">Output path.</param>
        /// <param name="version">Version.</param>
        /// <param name="properties">Properties.</param>
        /// <param name="nugetExePath">Path to nuget.exe</param>
        private void Pack(string fileName, string outputPath, string version, string properties, string nugetExePath)
        {
            // creating package
            var command = "pack" + " " +
                          fileName + " " +
                          "-OutputDirectory" + " " +
                          outputPath + " " +
                          "-Version" + " " +
                          version + " " +
                          "-Properties" + " " +
                          properties;
            
            OnMessageReceived(this, new Message("Packing", "Starting creating package from .nuspec... (" + fileName + ")",
                Name,
                MessageType.Information));

            var process = new Process
            {
                StartInfo =
                {
                    FileName = nugetExePath,
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
            
            OnMessageReceived(this, 
                new Message("Packing", 
                    "Information from nuget.exe:\r\n" + _output,
                Name,
                MessageType.Information));
            
            process.OutputDataReceived -= OnPackProcessOutputDataReceived;
            process.ErrorDataReceived -= OnProcessErrorDataReceived;

            if (process.ExitCode == 0)
            {
                OnMessageReceived(this, 
                    new Message("Packing", 
                        "Package created successfully.",
                        Name,
                        MessageType.Information));
            }
            else
            {
                var messageText = "Package was not created:\r\n" + error;
                
                OnMessageReceived(this, 
                    new Message("Packing", 
                        messageText,
                        Name,
                        MessageType.Error));
                
                throw new Exception(messageText);
            }
        }
        
        /// <summary>
        ///     Notifies when error data received.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Arguments.</param>
        private void OnProcessErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
                _output.AppendLine(e.Data.Replace("\r\n", string.Empty));
        }

        /// <summary>
        ///     Notifies when output data received.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Arguments.</param>
        private void OnPackProcessOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
                _output.AppendLine(e.Data.Replace("\r\n", string.Empty));
        }
    }
}