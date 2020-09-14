using System;
using System.Linq;
using System.Xml.Linq;

namespace Waves.Utils.Project
{
    /// <summary>
    /// Utils for .props files.
    /// </summary>
    public static class Props
    {
        /// <summary>
        /// Sets project version in props file.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="version">Version.</param>
        public static void SetVersion(string fileName, string version)
        {
            var doc = XDocument.Load(fileName);

            var elements = doc
                .Elements()
                .Elements()
                .Where(n => n.Name.LocalName == "PropertyGroup")
                .Elements()
                .Where(e => e.Name.LocalName == "Version");

            foreach (var element in elements)
            {
                element.Value = version;
            }

            doc.Save(fileName);
        }
    }
}