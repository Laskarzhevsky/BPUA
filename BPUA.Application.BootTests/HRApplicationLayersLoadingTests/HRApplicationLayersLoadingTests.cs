using BPUA.Application.TestInfrastructure;

namespace BPUA.Application.BootTests
{
    /// <summary>
    /// Groups hosted application layer loading tests and resets the BPUAApplication singleton
    /// before and after each test so bootstrap state does not leak across batch execution.
    /// </summary>
    public partial class HRApplicationLayersLoadingTests : IDisposable
    {
        /// <summary>
        /// Creates a new test instance and resets the singleton first.
        /// </summary>
        public HRApplicationLayersLoadingTests()
        {
            BPUATestApplicationResetter.ResetSingleton();
        }

        /// <summary>
        /// Performs test cleanup by resetting the singleton again.
        /// </summary>
        public void Dispose()
        {
            BPUATestApplicationResetter.ResetSingleton();
        }
    }
}
