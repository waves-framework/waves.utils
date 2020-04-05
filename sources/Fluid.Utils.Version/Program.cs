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
                Console.WriteLine("[Fluid Version Tool] {0}", "Started.");
                
                if (args.Length != 2)
                    throw new Exception("[Fluid Version Tool]: Input arguments not valid.");
                
                var path = args[0];
                var version = args[1];

                var files = Directory.GetFiles(path, "*.nuspec", SearchOption.TopDirectoryOnly);

                foreach (var file in files)
                {
                    Console.WriteLine("[Fluid Version Tool] {0}: {1}", "Updating file" , file);
                
                    var text = File.ReadAllText(file);
                    text = text.Replace(NugetVersionKey, version);
                    File.WriteAllText(file, text);
                    
                    Console.WriteLine("[Fluid Version Tool] {0}: {1}", "File updated" , file);
                }
                
                Console.WriteLine("[Fluid Version Tool] {0}", "Success.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}