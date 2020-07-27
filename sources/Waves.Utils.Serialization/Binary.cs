using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Waves.Utils.Serialization
{
    /// <summary>
    /// Binary serialization.
    /// </summary>
    public static class Binary
    {
        /// <summary>
        ///     Writes object to binary file.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="filePath">Path.</param>
        /// <param name="objectToWrite">Object.</param>
        /// <param name="append">Append.</param>
        public static void WriteToFile<T>(string filePath, T objectToWrite, bool append = false)
        {
            using (var stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }

        /// <summary>
        ///     Reads object from binary file.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="filePath">Path.</param>
        /// <returns>Object.</returns>
        public static T ReadFile<T>(string filePath)
        {
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                var binaryFormatter = new BinaryFormatter();
                return (T)binaryFormatter.Deserialize(stream);
            }
        }
    }
}