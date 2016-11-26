namespace RssReader.Tests
{
    using System.IO;
    using System.Reflection;

    internal sealed class TestData
    {
        public static string Get(string relativePath)
        {
            var location = typeof(TestData).GetTypeInfo().Assembly.Location;
            var dirPath = Path.GetDirectoryName(location);
            return Path.Combine(dirPath, "TestData", relativePath);
        }
    }
}
