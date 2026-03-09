using System;

namespace BPUA.Core
{
    /// <summary>
    /// Defines assembly facets
    /// </summary>
    [Flags]
    public enum AssemblyFacet
    {
        /// <summary>
        /// No facet
        /// </summary>
        None = 0,

        /// <summary>
        /// Services facet
        /// </summary>
        Services = 1,

        /// <summary>
        /// Renderer mappings
        /// </summary>
        RendererMappings = 2,

        /// <summary>
        /// Page assemblies
        /// </summary>
        PageAssemblies = 4
    }
}
