using BPUA.DiagramModel.Enums;
using BPUA.DiagramModel.Model;
using System;

namespace BPUA.DiagramModel.Factories
{
    /// <summary>
    /// Creates diagram objects with reasonable defaults.
    /// </summary>
    public sealed class BpuaDiagramFactory
    {
        #region Public Methods

        public BpuaDiagram CreateDiagram(string diagramName, string domainName, string useCaseName, string rootNamespace)
        {
            BpuaDiagram diagram = new BpuaDiagram();
            diagram.Metadata.DiagramId = CreateId("diagram");
            diagram.Metadata.DiagramName = diagramName;
            diagram.Metadata.DomainName = domainName;
            diagram.Metadata.UseCaseName = useCaseName;
            diagram.Metadata.RootNamespace = rootNamespace;
            return diagram;
        }

        public BpuaDiagramNode CreateState(string name, BpuaStateRole stateRole, double x, double y)
        {
            BpuaDiagramNode node = new BpuaDiagramNode();
            node.Id = CreateId("state");
            node.Name = name;
            node.DisplayText = name;
            node.NodeType = BpuaDiagramNodeType.State;
            node.StateRole = stateRole;
            node.X = x;
            node.Y = y;
            return node;
        }

        public BpuaDiagramNode CreateDecision(string name, double x, double y)
        {
            BpuaDiagramNode node = new BpuaDiagramNode();
            node.Id = CreateId("decision");
            node.Name = name;
            node.DisplayText = name;
            node.NodeType = BpuaDiagramNodeType.Decision;
            node.StateRole = BpuaStateRole.Regular;
            node.Width = 140;
            node.Height = 100;
            node.X = x;
            node.Y = y;
            return node;
        }

        public BpuaDiagramTransition CreateTransition(string name, string fromNodeId, string toNodeId, BpuaTransitionType transitionType)
        {
            BpuaDiagramTransition transition = new BpuaDiagramTransition();
            transition.Id = CreateId("transition");
            transition.Name = name;
            transition.DisplayText = name;
            transition.FromNodeId = fromNodeId;
            transition.ToNodeId = toNodeId;
            transition.TransitionType = transitionType;
            return transition;
        }

        #endregion

        #region Private Methods

        private string CreateId(string prefix)
        {
            return prefix + "-" + Guid.NewGuid().ToString("N");
        }

        #endregion
    }
}
