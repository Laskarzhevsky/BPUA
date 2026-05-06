using BPUA.Application.TestInfrastructure;

namespace BPUA.Application.RoutingTests
{
    /// <summary>
    /// Groups routing and use-case activation tests that verify dynamic layer loading
    /// through the bootstrapped platform runtime. These tests reset the application
    /// singleton before and after execution because activation mutates global process state.
    /// </summary>
    public partial class InfrastructureServerTests : IDisposable
    {
        /// <summary>
        /// Creates a new routing test instance and resets the <c>BpuaApplication</c> singleton first.
        /// This keeps every routing test isolated from previous bootstrap or activation activity.
        /// </summary>
        public InfrastructureServerTests()
        {
            BPUATestApplicationResetter.ResetSingleton();
        }

        /// <summary>
        /// Performs routing test cleanup by resetting the <c>BpuaApplication</c> singleton again.
        /// </summary>
        public void Dispose()
        {
            BPUATestApplicationResetter.ResetSingleton();
        }
    }
}
