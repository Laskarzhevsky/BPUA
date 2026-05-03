using BPUA.Application.Contracts;
using BPUA.Core;

using PocoDataSet.BpuaExtensions;
using PocoDataSet.IData;

using System.IO;
using System.Threading.Tasks;

namespace BPUA.Application.DataAccessLogic
{
    [RegisterAsBPUAService]
    public class InitializingApplicationTransitionHandler : DataAccessLogicTransitionHandler, IDataAccessLogicTransitionHandler
    {
        #region Identification
        public static string DomainName = BPUA.Application.Contracts.DomainNames.BPUA;
        public static string UseCaseName = BPUA.Application.Contracts.UseCaseNames.APPLICATION;
        public static string ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.DAL;
        public static string StateName = default!;
        public static string TransitionName = BPUA.Application.Contracts.TransitionsNames.INITIALIZING_APPLICATION;

        /// <summary>
        /// Gets service key
        /// </summary>
        public static string ServiceKey
        {
            get
            {
                return KeyCompiler.CompileTransitionHandlerKey(DomainName, UseCaseName, ApplicationLayerName, StateName, TransitionName);
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public InitializingApplicationTransitionHandler() : base(DomainName, UseCaseName, ApplicationLayerName, StateName, TransitionName)
        {
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Handles request
        /// </summary>
        /// <param name="requestTransitionContext">Request transition context</param>
        /// <returns>Response transition context</returns>
        public override async Task<IDataSet?> HandleRequestAsync(IDataSet? requestTransitionContext)
        {
            if (requestTransitionContext == null)
            {
                return requestTransitionContext;
            }

            IRequestMetadata? requestMetadata = requestTransitionContext.GetCurrentRequestMetadata();
            if (requestMetadata == null)
            {
                throw new System.ArgumentOutOfRangeException(nameof(requestMetadata));
            }

            string fullPath = BPUAApplication.PathToFolderWithDynamicAssemblies;
            if (!string.IsNullOrEmpty(requestMetadata.Breadcrumbs))
            {
                fullPath = Path.Combine(fullPath, requestMetadata.Breadcrumbs);
            }

            if (Directory.Exists(fullPath))
            {
                string[] subdirectories = Directory.GetDirectories(fullPath);
                for (int i = 0; i < subdirectories.Length; i++)
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(subdirectories[i]);
                    if (FolderContainsSubfolders(subdirectories[i]))
                    {
                        string breadcrumbs = Breadcrumbs.Append(requestMetadata.Breadcrumbs, directoryInfo.Name);
                        requestTransitionContext.AddRequestMetadata(requestMetadata.DomainName, requestMetadata.UseCaseName, requestMetadata.ApplicationLayerName, requestMetadata.StateName, requestMetadata.TransitionName, breadcrumbs);
                    }
                    else
                    {
                        string breadcrumbs = Breadcrumbs.Append(requestMetadata.Breadcrumbs, directoryInfo.Name);
                        requestTransitionContext.AddRequestMetadata(requestMetadata.DomainName, requestMetadata.UseCaseName, requestMetadata.ApplicationLayerName, requestMetadata.StateName, BPUA.Application.Contracts.TransitionsNames.SWITCHING_TO_USE_CASE, breadcrumbs);
                    }
                }
            }

            return requestTransitionContext;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets flag indicating whether folder contains subfolders
        /// </summary>
        /// <param name="fullPathToFolder">Full path to folder</param>
        /// <returns>Flag indicating whether folder contains subfolders</returns>
        bool FolderContainsSubfolders(string fullPathToFolder)
        {
            string[] subfolders = Directory.GetDirectories(fullPathToFolder);
            return subfolders.Length > 0;
        }
        #endregion
    }
}
