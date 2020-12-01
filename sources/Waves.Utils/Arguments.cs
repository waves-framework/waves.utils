using System;
using System.Collections.Generic;

namespace Waves.Utils
{
    /// <summary>
    /// Arguments helpers.
    /// </summary>
    public static class Arguments
    {
        /// <summary>
        /// Read arguments from array by pattern: [-KEY] [VALUE].
        /// </summary>
        /// <returns>Dictionary of arguments.</returns>
        public static Dictionary<string, string> ReadFromArray(string[] args)
        {
            var dictionary = new Dictionary<string, string>();
            
            if (args.Length % 2 != 0)
                throw new Exception("The number of arguments must be even.");

            for (var i = 0; i < args.Length; i+=2)
            {
                var key = args[i];
                var value = args[i+1];

                if (key.Substring(0, 1) != "-")
                    throw new Exception("The key must contain a hyphen at the beginning.");
                
                dictionary.Add(key.Replace("-", string.Empty), value);
            }

            return dictionary;
        }
    }
}