using System;
using Waves.Utils.Nuget;
using Waves.Utils.Project;

namespace Waves.Utils.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = Nuspec.GetDependencyList("/Users/egorkhindikaynen/repos/ambertape/waves/utils/tests/data/nuget/templates/Test.nuspec.template");
        }
    }
}