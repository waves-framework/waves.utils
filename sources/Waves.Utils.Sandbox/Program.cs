using System;
using Waves.Utils.Nuget;

namespace Waves.Utils.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            var manager = new Manager();
            var version = manager.GetLastPackageMinorVersion("Waves.Core").Result;
        }
    }
}