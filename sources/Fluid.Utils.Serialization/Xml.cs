using System.IO;
using System.Xml.Serialization;

namespace Fluid.Utils.Serialization
{
    /// <summary>
    /// XML Serialization.
    /// </summary>
    public static class Xml
    {
        /// <summary>
        ///     Writes object to XML file.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="filePath">Path.</param>
        /// <param name="objectToWrite">Object.</param>
        /// <param name="append">Append.</param>
        public static void WriteToFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                writer = new StreamWriter(filePath, append);
                serializer.Serialize(writer, objectToWrite);
            }
            finally
            {
                writer?.Close();
            }
        }

        /// <summary>
        ///     Reads object from XML file.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="filePath">Path.</param>
        /// <returns>Object.</returns>
        public static T ReadFile<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                reader = new StreamReader(filePath);
                return (T) serializer.Deserialize(reader);
            }
            finally
            {
                reader?.Close();
            }
        }
    }
}