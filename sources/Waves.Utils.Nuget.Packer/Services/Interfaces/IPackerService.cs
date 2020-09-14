using System.Collections.Generic;
using Waves.Core.Base.Interfaces.Services;

namespace Waves.Utils.Nuget.Packer.Services.Interfaces
{
    /// <summary>
    /// Interface for packer service.
    /// </summary>
    public interface IPackerService : IService
    {
        /// <summary>
        /// Creates packages from list of nuspec files to <see cref="outputPath"/>.
        /// </summary>
        /// <param name="fileNames">List of file names.</param>
        /// <param name="outputPath">Output path.</param>
        /// <param name="version">Version.</param>
        /// <param name="properties">Properties.</param>
        /// <param name="nugetExePath">Path to nuget exe.</param>
        void Pack(
            List<string> fileNames, 
            string outputPath, 
            string version, 
            string properties,
            string nugetExePath);
    }
}