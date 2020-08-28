using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace Waves.Utils.Nuget
{
    /// <summary>
    /// Manager for operations with NuGet.
    /// </summary>
    public class NuGetManager
    {
        private string _defaultRepository = "https://api.nuget.org/v3/index.json";
        
        private ILogger _logger = NullLogger.Instance;
        private SourceCacheContext _cache = new SourceCacheContext();
        private SourceRepository _repository;

        /// <summary>
        /// Creates new instance of <see cref="NuGetManager"/>.
        /// </summary>
        public NuGetManager()
        {
            _repository = Repository.Factory.GetCoreV3(_defaultRepository);
        }

        /// <summary>
        /// Gets last minor version of package.
        /// </summary>
        /// <param name="packageName">Package name.</param>
        /// <returns>Version.</returns>
        public async Task<NuGetVersion> GetLastPackageMinorVersion(string packageName)
        { 
            var resource = await _repository.GetResourceAsync<FindPackageByIdResource>();
            
            var versions = await resource.GetAllVersionsAsync(
                packageName,
                _cache,
                _logger,
                CancellationToken.None);

            return versions.Last();
        }
    }
}