using System.Reflection;

using BPUA.Application.Orchestration;

namespace BPUA.Application.BootTests.TestInfrastructure
{
    internal static class BPUATestApplicationResetter
    {
        public static void ResetSingleton()
        {
            Type applicationType = typeof(BPUAApplication);
            FieldInfo? fieldInfo = applicationType.GetField("_bppApplication", BindingFlags.Static | BindingFlags.NonPublic);
            if (fieldInfo == null)
            {
                throw new InvalidOperationException("Unable to locate BPUAApplication singleton field.");
            }

            fieldInfo.SetValue(null, null);
        }
    }
}
