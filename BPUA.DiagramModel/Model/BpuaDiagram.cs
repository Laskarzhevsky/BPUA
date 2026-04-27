using System.Collections.Generic;

namespace BPUA.DiagramModel.Model
{
    /// <summary>
    /// Root model for one BPUA use case state diagram.
    /// This model is intentionally UI-independent and can be edited by WPF, Blazor, WinUI, or tests.
    /// </summary>
    public sealed class BpuaDiagram
    {
        #region Constructors

        public BpuaDiagram()
        {
            Metadata = new BpuaDiagramMetadata();
            Nodes = new List<BpuaDiagramNode>();
            Transitions = new List<BpuaDiagramTransition>();
        }

        #endregion

        #region Properties

        public BpuaDiagramMetadata Metadata { get; set; }

        public List<BpuaDiagramNode> Nodes { get; set; }

        public List<BpuaDiagramTransition> Transitions { get; set; }

        #endregion
    }
}
