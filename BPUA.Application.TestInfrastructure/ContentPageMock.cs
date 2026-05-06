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
        public ContentPageMock(string currentPath, IBpuaApplication bpuaApplication)
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
            object? locatedBpuaService = await BpuaServiceLocator.GetBpuaServiceAsync(CurrentPath);
            if (locatedBpuaService != null && locatedBpuaService is IStateHandler)
            {
                StateHandler = (IStateHandler)locatedBpuaService;
                await StateHandler.InitializeComponent(BpuaApplication);
            }
        }
        #endregion

        #region Private Properties
        /// <summary>
        /// Gets or sets the BPUA application instance
        /// </summary>
        IBpuaApplication BpuaApplication
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets view model
        /// </summary>
        public IStateHandler? StateHandler
        {
            get;
            set;
        }
        #endregion
    }
}
