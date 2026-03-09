using System.IO;
using System.Threading.Tasks;

using BPUA.Application.Contracts;
using BPUA.Application.RequestHandlers;
using BPUA.Core;
using PocoDataSet.BPUAExtensions;

using PocoDataSet.IData;

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
        /// <param name="requestDataSet">Request data set</param>
        /// <returns>Response data set</returns>
        public override async Task<IDataSet?> HandleRequestAsync(IDataSet? requestDataSet)
        {
            if (requestDataSet == null)
            {
                return requestDataSet;
            }

            IRequestMetadata requestMetadata = requestDataSet.GetRequestMetadata();
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
                        ITransitionMetadata transitionMetadata = requestDataSet.GetNewTransitionMetadataAsInterface();
                        transitionMetadata.DomainName = requestMetadata.DomainName;
                        transitionMetadata.UseCaseName = requestMetadata.UseCaseName;
                        transitionMetadata.StateName = requestMetadata.StateName;
                        transitionMetadata.TransitionName = requestMetadata.TransitionName;
                        transitionMetadata.Breadcrumbs = Breadcrumbs.Append(requestMetadata.Breadcrumbs, directoryInfo.Name);
                    }
                    else
                    {
                        ITransitionMetadata transitionMetadata = requestDataSet.GetNewTransitionMetadataAsInterface();
                        transitionMetadata.DomainName = requestMetadata.DomainName;
                        transitionMetadata.UseCaseName = requestMetadata.UseCaseName;
                        transitionMetadata.StateName = requestMetadata.StateName;
                        transitionMetadata.TransitionName = BPUA.Application.Contracts.TransitionsNames.SWITCHING_TO_USE_CASE;
                        transitionMetadata.Breadcrumbs = Breadcrumbs.Append(requestMetadata.Breadcrumbs, directoryInfo.Name);
                    }
                }
            }

            return requestDataSet;
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
