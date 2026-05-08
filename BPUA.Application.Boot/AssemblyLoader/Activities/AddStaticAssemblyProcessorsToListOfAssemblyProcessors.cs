namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides dynamic assemblies loader functionality
    /// </summary>
    public partial class DynamicAssembliesLoader
    {
        #region Methods
        /// <summary>
        /// Adds static assembly processors to list of assembly processors
        /// </summary>
        void AddStaticAssemblyProcessorsToListOfAssemblyProcessors()
        {
            AddBpuaServiceAssemblyProcessor();
            AddBpuaTransitionAssemblyProcessor();
        }

        /// <summary>
        /// Adds BPUA service assembly processor if it was not added yet
        /// </summary>
        void AddBpuaServiceAssemblyProcessor()
        {
            bool exists = false;

            for (int i = 0; i < ListOfAssemblyProcessors.Count; i++)
            {
                if (ListOfAssemblyProcessors[i] is BpuaServiceAssemblyProcessor)
                {
                    exists = true;
                    break;
                }
            }

            if (!exists)
            {
                ListOfAssemblyProcessors.Add(new BpuaServiceAssemblyProcessor());
            }
        }

        /// <summary>
        /// Adds BPUA transition assembly processor if it was not added yet
        /// </summary>
        void AddBpuaTransitionAssemblyProcessor()
        {
            bool exists = false;

            for (int i = 0; i < ListOfAssemblyProcessors.Count; i++)
            {
                if (ListOfAssemblyProcessors[i] is BpuaTransitionAssemblyProcessor)
                {
                    exists = true;
                    break;
                }
            }

            if (!exists)
            {
                ListOfAssemblyProcessors.Add(new BpuaTransitionAssemblyProcessor());
            }
        }
        #endregion
    }
}