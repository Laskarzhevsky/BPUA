namespace BPUA.DiagramModel.Model
{
    /// <summary>
    /// Describes the BPUA identity represented by a diagram.
    /// These values are intended to become generated constants, class names, namespaces, and registration metadata.
    /// </summary>
    public sealed class BpuaDiagramMetadata
    {
        #region Constructors

        public BpuaDiagramMetadata()
        {
            DiagramId = string.Empty;
            DiagramName = string.Empty;
            DomainName = string.Empty;
            UseCaseName = string.Empty;
            DefaultApplicationLayerName = string.Empty;
            RootNamespace = string.Empty;
            Description = string.Empty;
            Version = "1.0";
        }

        #endregion

        #region Properties

        public string DiagramId { get; set; }

        public string DiagramName { get; set; }

        public string DomainName { get; set; }

        public string UseCaseName { get; set; }

        public string DefaultApplicationLayerName { get; set; }

        public string RootNamespace { get; set; }

        public string Description { get; set; }

        public string Version { get; set; }

        #endregion
    }
}
