using System;
using System.Collections.Generic;
using System.Composition;
using System.IO;
using Waves.Core.Base;
using Waves.Core.Base.Enums;
using Waves.Core.Base.Interfaces;
using Waves.Core.Base.Interfaces.Services;
using Waves.Utils.Nuget.Packer.Services.Interfaces;
using Waves.Utils.Project;

namespace Waves.Utils.Nuget.Packer.Services
{
    /// <summary>
    /// Nuspec files templates service.
    /// </summary>
    [Export(typeof(IService))]
    public class NuspecTemplatesService : Service, INuspecTemplatesService
    {
        /// <inheritdoc />
        public override Guid Id => Guid.Parse("B9BA28D8-E8BA-433E-A468-0A332533CB4E");

        /// <inheritdoc />
        public override string Name { get; set; } = "Nuspec Files Templates service";
        
        /// <inheritdoc />
        public List<string> Templates { get; private set; } = new List<string>();
        
        /// <inheritdoc />
        public List<string> Nuspecs { get; private set; } = new List<string>();
        
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
        public void LoadTemplates(string path)
        {
            Templates.Clear();
            foreach (var file in Directory.GetFiles(path))
            {
                try
                {
                    var templateFileInformation = new FileInfo(file);
                    if (templateFileInformation.Extension.Equals(".template"))
                    {
                        Templates.Add(file);
                    
                        OnMessageReceived(
                            this, 
                            new Message(
                                "Loading templates",
                                "Template file added " + file,
                                Name,
                                MessageType.Information
                            ));
                    }
                }
                catch (Exception e)
                {
                    OnMessageReceived(
                        this, 
                        new Message(
                            "Loading templates",
                            "Failed to load template file " + file + "\r\n" + e,
                            Name,
                            MessageType.Error
                        ));
                }
            }
        }

        /// <inheritdoc />
        public void CreateNuspecs(string outputPath, string versionKey, string version)
        {
            foreach (var fileName in Templates)
            {
                var templateFileInformation = new FileInfo(fileName);
                var nuspecFileName = templateFileInformation.Name.Replace(".template", string.Empty);
                var nuspecFileFullName = Path.Combine(outputPath, nuspecFileName);
                
                OnMessageReceived(
                    this, 
                    new Message(
                        "Copying templates",
                        "Copying template... (" + fileName + ").",
                        Name,
                        MessageType.Information
                    ));
                
                File.Copy(fileName, nuspecFileFullName, true);

                if (!File.Exists(nuspecFileFullName))
                {
                    throw new FileNotFoundException("Nuspec file not copied (" + fileName + ")", nuspecFileFullName);
                }
                
                OnMessageReceived(
                    this, 
                    new Message(
                        "Copying templates",
                        "Nuspec file was copied from template (" + nuspecFileFullName + ").",
                        Name,
                        MessageType.Information
                    ));
                
                OnMessageReceived(
                    this, 
                    new Message(
                        "Versioning",
                        "Replacing data... (" + fileName + ").",
                        Name,
                        MessageType.Information
                    ));
                
                var hasChanges = Nuspec.ReplaceVersionKey(nuspecFileFullName, versionKey, version);

                if (hasChanges)
                {
                    OnMessageReceived(
                        this, 
                        new Message(
                            "Versioning",
                            "The version set successfully - " + version,
                            Name,
                            MessageType.Success
                        ));
                }
                else
                {
                    OnMessageReceived(
                        this, 
                        new Message(
                            "Versioning",
                            "The version was not set - " + version,
                            Name,
                            MessageType.Success
                        ));
                }
            }
        }
    }
}