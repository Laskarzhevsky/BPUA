using BPUA.DiagramModel.Enums;
using System.Collections.Generic;

namespace BPUA.DiagramModel.Model
{
    /// <summary>
    /// Represents a drawable node on the BPUA diagram surface.
    /// State and Decision are represented by the same node type to keep the surface simple.
    /// </summary>
    public sealed class BpuaDiagramNode
    {
        #region Constructors

        public BpuaDiagramNode()
        {
            Id = string.Empty;
            Name = string.Empty;
            DisplayText = string.Empty;
            ApplicationLayerName = string.Empty;
            HandlerClassName = string.Empty;
            Description = string.Empty;
            NodeType = BpuaDiagramNodeType.Unknown;
            StateRole = BpuaStateRole.Regular;
            X = 0;
            Y = 0;
            Width = 180;
            Height = 80;
            Properties = new Dictionary<string, string>();
        }

        #endregion

        #region Identity

        public string Id { get; set; }

        public string Name { get; set; }

        public string DisplayText { get; set; }

        #endregion

        #region BPUA Semantics

        public BpuaDiagramNodeType NodeType { get; set; }

        public BpuaStateRole StateRole { get; set; }

        public string ApplicationLayerName { get; set; }

        public string HandlerClassName { get; set; }

        public string Description { get; set; }

        #endregion

        #region Layout

        public double X { get; set; }

        public double Y { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        #endregion

        #region Extensibility

        public Dictionary<string, string> Properties { get; set; }

        #endregion
    }
}
