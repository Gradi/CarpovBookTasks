using System.IO;
using System.Reflection;

namespace TestMilan
{
    public static class Sources
    {
        public static readonly string GreatestCommonDivisor;

        static Sources()
        {
            var sourceDir = Path.Combine(Path.GetDirectoryName(Assembly.GetAssembly(typeof(Sources))!.Location)!, "Sources");

            GreatestCommonDivisor = File.ReadAllText(Path.Combine(sourceDir, "gcd.mil"));
        }
    }
}
