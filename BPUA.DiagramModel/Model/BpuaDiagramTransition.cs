using BPUA.DiagramModel.Enums;
using System.Collections.Generic;

namespace BPUA.DiagramModel.Model
{
    /// <summary>
    /// Represents a directed transition between two diagram nodes.
    /// </summary>
    public sealed class BpuaDiagramTransition
    {
        #region Constructors

        public BpuaDiagramTransition()
        {
            Id = string.Empty;
            Name = string.Empty;
            DisplayText = string.Empty;
            FromNodeId = string.Empty;
            ToNodeId = string.Empty;
            ApplicationLayerName = string.Empty;
            TransitionClassName = string.Empty;
            HandlerClassName = string.Empty;
            GuardName = string.Empty;
            Description = string.Empty;
            TransitionType = BpuaTransitionType.Unknown;
            Properties = new Dictionary<string, string>();
        }

        #endregion

        #region Identity

        public string Id { get; set; }

        public string Name { get; set; }

        public string DisplayText { get; set; }

        #endregion

        #region Endpoints

        public string FromNodeId { get; set; }

        public string ToNodeId { get; set; }

        #endregion

        #region BPUA Semantics

        public BpuaTransitionType TransitionType { get; set; }

        public string ApplicationLayerName { get; set; }

        public string TransitionClassName { get; set; }

        public string HandlerClassName { get; set; }

        public string GuardName { get; set; }

        public string Description { get; set; }

        #endregion

        #region Extensibility

        public Dictionary<string, string> Properties { get; set; }

        #endregion
    }
}
