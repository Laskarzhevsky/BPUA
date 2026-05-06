using System;
using System.Reflection;

using BPUA.Application.Orchestration;

namespace BPUA.Application.TestInfrastructure
{
    public static class BPUATestApplicationResetter
    {
        public static void ResetSingleton()
        {
            Type applicationType = typeof(BpuaApplication);
            FieldInfo? fieldInfo = applicationType.GetField("_bppApplication", BindingFlags.Static | BindingFlags.NonPublic);
            if (fieldInfo == null)
            {
                throw new InvalidOperationException("Unable to locate BpuaApplication singleton field.");
            }

            fieldInfo.SetValue(null, null);
        }
    }
}
