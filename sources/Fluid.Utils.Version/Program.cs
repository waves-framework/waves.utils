using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Fluid.Utils.Version
{
    class Program
    {
        private const string NugetVersionKey = "{$NUGETVERSION}";

        static void Main(string[] args)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                Console.WriteLine("[Fluid Version Tool] {0}", "Started.");
                
                if (args.Length != 2)
                    throw new Exception("[Fluid Version Tool]: Input arguments not valid.");
                
                var path = args[0];
                var version = args[1];

                var files = Directory.GetFiles(path, "*.nuspec", SearchOption.TopDirectoryOnly);

                foreach (var file in files)
                {
                    Console.WriteLine("[Fluid Version Tool] {0}: {1}", "Updating file" , file);

                    var hasChanges = false;

                    var lines = File.ReadAllLines(file);

                    for (var i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].Contains(NugetVersionKey))
                        {
                            lines[i] = lines[i].Replace(NugetVersionKey, version);
                            hasChanges = true;
                        }
                    }

                    File.WriteAllLines(file, lines);
                    
                    if (hasChanges)
                        Console.WriteLine("[Fluid Version Tool] {0}: {1}", "File updated" , file);
                    else
                        Console.WriteLine("[Fluid Version Tool] {0}: {1}", "File not updated", file);
                }
                
                Console.WriteLine("[Fluid Version Tool] {0}", "Success.");

                watch.Stop();

                Console.WriteLine("[Fluid Version Tool] {0}", "Time ellapsed: " + watch.Elapsed.TotalSeconds + " sec.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}