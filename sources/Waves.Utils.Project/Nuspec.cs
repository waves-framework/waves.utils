using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NuGet.Packaging;

namespace Waves.Utils.Project
{
    /// <summary>
    ///     Utils for .nuspec files.
    /// </summary>
    public static class Nuspec
    {
        /// <summary>
        ///     Replaces <see cref="key" /> in file with <see cref="version" />.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="key">Version key.</param>
        /// <param name="version">Current version.</param>
        public static void ReplaceVersionKey(string fileName, string key, string version)
        {
            var lines = File.ReadAllLines(fileName);

            for (var i = 0; i < lines.Length; i++)
            {
                if (!lines[i].Contains(key))
                    continue;

                lines[i] = lines[i].Replace(key, version);
            }

            File.WriteAllLines(fileName, lines);
        }

        /// <summary>
        ///     Gets list of project NuGet dependencies from nuspec file.
        /// </summary>
        /// <returns>List of project NuGet dependencies</returns>
        public static List<Package> GetDependencyList(string fileName)
        {
            var reader = new NuspecReader(fileName);
            var dependencies = reader.GetDependencyGroups();

            return (from dependency in dependencies 
                from package in dependency.Packages 
                select new Package(
                    package.Id, 
                    package.VersionRange.MinVersion?.Version, 
                    package.VersionRange.MaxVersion?.Version))
                .ToList();
        }
    }
}