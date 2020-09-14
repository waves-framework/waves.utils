using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Waves.Utils.Project
{
    /// <summary>
    /// Utils for .csproj files.
    /// </summary>
    public static class Csproj
    {
        /// <summary>
        /// Replaces versions in csproj file.
        /// </summary>
        public static void ReplaceDependencyVersions(string fileName, List<Package> packages)
        {
            var doc = XDocument.Load(fileName);
            var elements = doc
                .Element("Project")?
                .Elements("ItemGroup")
                .Elements("PackageReference");

            if (elements == null) return;
            
            foreach (var element in elements)
            {
                var id = string.Empty;
                foreach (var attribute in element.Attributes())
                {
                    var name = attribute.Name;
                    var value = attribute.Value;

                    if (name == "Include") 
                        id = value;
                }
                
                if (string.IsNullOrEmpty(id))
                    continue;

                Version version = null;
                foreach (var package in packages)
                {
                    if (package.Id == id)
                        version = package.MinVersion;
                }
                
                if (version == null) return;
                
                foreach (var attribute in element.Attributes())
                {
                    var name = attribute.Name;
                    var value = attribute.Value;

                    if (name == "Version") 
                        attribute.Value = version.ToString();
                }
            }

            doc.Save(fileName);
        }
    }
}