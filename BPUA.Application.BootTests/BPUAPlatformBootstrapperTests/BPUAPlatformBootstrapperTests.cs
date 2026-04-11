using BPUA.Application.TestInfrastructure;

namespace BPUA.Application.BootTests
{
    /// <summary>
    /// Groups bootstrapper tests that verify how <c>BPUAPlatformBootstrapper</c>
    /// loads configuration, calculates plugin paths, initializes the singleton application,
    /// and registers the minimal services required for the BPUA platform to start.
    /// </summary>
    public partial class BPUAPlatformBootstrapperTests : IDisposable
    {
        /// <summary>
        /// Creates a new test instance and resets the <c>BPUAApplication</c> singleton first.
        /// This keeps every test isolated so that registrations performed by one bootstrap run
        /// do not leak into the next test and produce false positives or hidden coupling.
        /// </summary>
        public BPUAPlatformBootstrapperTests()
        {
            BPUATestApplicationResetter.ResetSingleton();
        }

        /// <summary>
        /// Performs test cleanup by resetting the <c>BPUAApplication</c> singleton again.
        /// The bootstrapper initializes global runtime state, so explicit teardown is required
        /// to keep the test suite deterministic and safe for repeated execution.
        /// </summary>
        public void Dispose()
        {
            BPUATestApplicationResetter.ResetSingleton();
        }
    }
}
