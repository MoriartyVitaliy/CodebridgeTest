namespace CodebridgeTest.Core.Common.Info
{
    public static class ApiInfo
    {
        public const string ServiceName = "Dogshouseservice";
        public const string Version = "1.0.1";

        public static string FullVersion => $"{ServiceName}.Version {Version}";
    }
}
