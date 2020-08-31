using System.Collections.Generic;
using Waves.Core.Base.Interfaces.Services;

namespace Waves.Utils.Nuget.Packer.Services.Interfaces
{
    /// <summary>
    /// Service for templates loading.
    /// </summary>
    public interface INuspecTemplatesService : IService
    {
        /// <summary>
        /// List of templates.
        /// </summary>
        List<string> Templates { get; }
        
        /// <summary>
        /// List of nuspecs.
        /// </summary>
        List<string> Nuspecs { get; }
        
        /// <summary>
        /// Loads templates from <see cref="path"/>.
        /// </summary>
        /// <param name="path">Templates path.</param>
        void LoadTemplates(string path);

        /// <summary>
        /// Creates nuspecs from templates and copy them to <see cref="outputPath"/>.
        /// </summary>
        /// <param name="outputPath">Output path.</param>
        void CreateNuspecs(string outputPath);
    }
}