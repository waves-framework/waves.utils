using System;
using System.IO;

namespace Fluid.Utils.Version
{
    class Program
    {
        private const string NugetVersionKey = "{$NUGETVERSION}";
        
        static void Main(string[] args)
        {
            try
            {
                if (args.Length != 2)
                    throw new Exception("Fluid Version Tool: Input arguments not valid.");
                
                var path = args[0];
                var version = args[1];

                var files = Directory.GetFiles(path, "*.nuspec", SearchOption.TopDirectoryOnly);

                foreach (var file in files)
                {
                    var text = File.ReadAllText(file);
                    text = text.Replace(NugetVersionKey, version);
                    File.WriteAllText(file, text);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}