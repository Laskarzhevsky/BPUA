namespace BPUA.Application.BootTests.TestInfrastructure
{
    internal class Helpers
    {
        public static string EscapeJson(string value)
        {
            return value.Replace("\\", "\\\\");
        }
    }
}
