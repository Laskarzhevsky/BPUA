namespace BPUA.Application.BootTests
{
    internal class Helpers
    {
        public static string EscapeJson(string value)
        {
            return value.Replace("\\", "\\\\");
        }

        public static string FindBuildFolder()
        {
            string current = AppContext.BaseDirectory;

            while (!string.IsNullOrWhiteSpace(current))
            {
                string candidate = Path.Combine(current, "Build");
                if (Directory.Exists(candidate))
                {
                    return candidate;
                }

                DirectoryInfo? parent = Directory.GetParent(current);
                current = parent == null ? string.Empty : parent.FullName;
            }

            throw new DirectoryNotFoundException("Build folder not found.");
        }
    }
}
