using System;

namespace Waves.Utils.Project
{
    /// <summary>
    /// Package.
    /// </summary>
    public class Package
    {
        public Package(string id, Version minVersion, Version maxVersion)
        {
            Id = id;
            MinVersion = minVersion;
            MaxVersion = maxVersion;
        }
        
        /// <summary>
        /// Gets or sets Id of the package.
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// Gets or sets min version of package.
        /// </summary>
        public Version MinVersion { get; set; }
        
        /// <summary>
        /// Gets or sets max version of package.
        /// </summary>
        public Version MaxVersion { get; set; }
    }
}