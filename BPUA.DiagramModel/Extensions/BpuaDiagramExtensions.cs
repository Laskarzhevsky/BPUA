using BPUA.DiagramModel.Model;

namespace BPUA.DiagramModel.Extensions
{
    /// <summary>
    /// Convenience methods for manipulating a diagram model.
    /// </summary>
    public static class BpuaDiagramExtensions
    {
        #region Public Methods

        public static void AddNode(this BpuaDiagram diagram, BpuaDiagramNode node)
        {
            if (diagram == null)
            {
                return;
            }

            if (node == null)
            {
                return;
            }

            if (diagram.Nodes == null)
            {
                diagram.Nodes = new System.Collections.Generic.List<BpuaDiagramNode>();
            }

            diagram.Nodes.Add(node);
        }

        public static void AddTransition(this BpuaDiagram diagram, BpuaDiagramTransition transition)
        {
            if (diagram == null)
            {
                return;
            }

            if (transition == null)
            {
                return;
            }

            if (diagram.Transitions == null)
            {
                diagram.Transitions = new System.Collections.Generic.List<BpuaDiagramTransition>();
            }

            diagram.Transitions.Add(transition);
        }

        public static BpuaDiagramNode FindNodeById(this BpuaDiagram diagram, string nodeId)
        {
            if (diagram == null)
            {
                return null;
            }

            if (diagram.Nodes == null)
            {
                return null;
            }

            int index = 0;
            while (index < diagram.Nodes.Count)
            {
                BpuaDiagramNode node = diagram.Nodes[index];
                if (node != null && node.Id == nodeId)
                {
                    return node;
                }

                index = index + 1;
            }

            return null;
        }

        #endregion
    }
}
