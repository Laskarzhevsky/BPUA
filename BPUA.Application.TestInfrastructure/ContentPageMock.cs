using BPUA.Application.Contracts;
using BPUA.Application.Extensions.Services;

using System.Threading.Tasks;

namespace BPUA.Application.TestInfrastructure
{
    public class ContentPageMock
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="currentPath">Current path</param>
        /// <param name="bpuaApplication">BPUA application instance</param>
        public ContentPageMock(string currentPath, IBPUAApplication bpuaApplication)
        {
            CurrentPath = currentPath;
            BpuaApplication = bpuaApplication;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets current path
        /// </summary>
        public string CurrentPath
        {
            get; private set;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Handles OnInitializedAsync event
        /// </summary>
        public async Task OnInitializedAsync()
        {
            object? locatedBPUAService = await BPUAServiceLocator.GetBPUAServiceAsync(CurrentPath);
            if (locatedBPUAService != null && locatedBPUAService is IStateHandler)
            {
                StateHandler = (IStateHandler)locatedBPUAService;
                await StateHandler.InitializeComponent(BpuaApplication);
            }
        }
        #endregion

        #region Private Properties
        /// <summary>
        /// Gets or sets the BPUA application instance
        /// </summary>
        IBPUAApplication BpuaApplication
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets view model
        /// </summary>
        protected IStateHandler? StateHandler
        {
            get;
            set;
        }
        #endregion
    }
}
