using BPUA.DiagramModel.Enums;
using BPUA.DiagramModel.Model;
using System.Collections.Generic;

namespace BPUA.DiagramModel.Validation
{
    /// <summary>
    /// Performs structural validation of a BPUA diagram model.
    /// This validator intentionally validates only model-level rules, not generated code correctness.
    /// </summary>
    public sealed class BpuaDiagramValidator
    {
        #region Public Methods

        public BpuaDiagramValidationResult Validate(BpuaDiagram diagram)
        {
            BpuaDiagramValidationResult result = new BpuaDiagramValidationResult();

            if (diagram == null)
            {
                result.AddMessage(BpuaValidationSeverity.Error, "DIAGRAM_NULL", "Diagram is missing.", string.Empty);
                return result;
            }

            ValidateMetadata(diagram, result);
            ValidateNodes(diagram, result);
            ValidateTransitions(diagram, result);

            return result;
        }

        #endregion

        #region Private Methods

        private void ValidateMetadata(BpuaDiagram diagram, BpuaDiagramValidationResult result)
        {
            if (diagram.Metadata == null)
            {
                result.AddMessage(BpuaValidationSeverity.Error, "METADATA_MISSING", "Diagram metadata is missing.", string.Empty);
                return;
            }

            if (string.IsNullOrWhiteSpace(diagram.Metadata.DomainName) == true)
            {
                result.AddMessage(BpuaValidationSeverity.Warning, "DOMAIN_NAME_MISSING", "Domain name is missing.", diagram.Metadata.DiagramId);
            }

            if (string.IsNullOrWhiteSpace(diagram.Metadata.UseCaseName) == true)
            {
                result.AddMessage(BpuaValidationSeverity.Warning, "USE_CASE_NAME_MISSING", "Use case name is missing.", diagram.Metadata.DiagramId);
            }
        }

        private void ValidateNodes(BpuaDiagram diagram, BpuaDiagramValidationResult result)
        {
            Dictionary<string, int> ids = new Dictionary<string, int>();
            Dictionary<string, int> names = new Dictionary<string, int>();
            int entryStateCount = 0;
            int index = 0;

            if (diagram.Nodes == null)
            {
                result.AddMessage(BpuaValidationSeverity.Error, "NODES_MISSING", "Diagram nodes collection is missing.", string.Empty);
                return;
            }

            while (index < diagram.Nodes.Count)
            {
                BpuaDiagramNode node = diagram.Nodes[index];

                if (node == null)
                {
                    result.AddMessage(BpuaValidationSeverity.Error, "NODE_NULL", "Node is missing.", string.Empty);
                    index = index + 1;
                    continue;
                }

                if (string.IsNullOrWhiteSpace(node.Id) == true)
                {
                    result.AddMessage(BpuaValidationSeverity.Error, "NODE_ID_MISSING", "Node id is missing.", node.Name);
                }
                else
                {
                    if (ids.ContainsKey(node.Id) == true)
                    {
                        result.AddMessage(BpuaValidationSeverity.Error, "NODE_ID_DUPLICATE", "Duplicate node id exists.", node.Id);
                    }
                    else
                    {
                        ids.Add(node.Id, 1);
                    }
                }

                if (string.IsNullOrWhiteSpace(node.Name) == true)
                {
                    result.AddMessage(BpuaValidationSeverity.Error, "NODE_NAME_MISSING", "Node name is missing.", node.Id);
                }
                else
                {
                    if (names.ContainsKey(node.Name) == true)
                    {
                        result.AddMessage(BpuaValidationSeverity.Error, "NODE_NAME_DUPLICATE", "Duplicate node name exists.", node.Id);
                    }
                    else
                    {
                        names.Add(node.Name, 1);
                    }
                }

                if (node.NodeType == BpuaDiagramNodeType.Unknown)
                {
                    result.AddMessage(BpuaValidationSeverity.Error, "NODE_TYPE_UNKNOWN", "Node type is unknown.", node.Id);
                }

                if (node.NodeType == BpuaDiagramNodeType.State && node.StateRole == BpuaStateRole.Entry)
                {
                    entryStateCount = entryStateCount + 1;
                }

                index = index + 1;
            }

            if (entryStateCount == 0)
            {
                result.AddMessage(BpuaValidationSeverity.Warning, "ENTRY_STATE_MISSING", "No entry state is defined.", string.Empty);
            }

            if (entryStateCount > 1)
            {
                result.AddMessage(BpuaValidationSeverity.Error, "ENTRY_STATE_MULTIPLE", "More than one entry state is defined.", string.Empty);
            }
        }

        private void ValidateTransitions(BpuaDiagram diagram, BpuaDiagramValidationResult result)
        {
            Dictionary<string, int> nodeIds = new Dictionary<string, int>();
            Dictionary<string, int> transitionIds = new Dictionary<string, int>();
            Dictionary<string, int> transitionNames = new Dictionary<string, int>();
            int index = 0;

            if (diagram.Transitions == null)
            {
                result.AddMessage(BpuaValidationSeverity.Error, "TRANSITIONS_MISSING", "Diagram transitions collection is missing.", string.Empty);
                return;
            }

            if (diagram.Nodes != null)
            {
                while (index < diagram.Nodes.Count)
                {
                    BpuaDiagramNode node = diagram.Nodes[index];
                    if (node != null && string.IsNullOrWhiteSpace(node.Id) == false && nodeIds.ContainsKey(node.Id) == false)
                    {
                        nodeIds.Add(node.Id, 1);
                    }

                    index = index + 1;
                }
            }

            index = 0;
            while (index < diagram.Transitions.Count)
            {
                BpuaDiagramTransition transition = diagram.Transitions[index];

                if (transition == null)
                {
                    result.AddMessage(BpuaValidationSeverity.Error, "TRANSITION_NULL", "Transition is missing.", string.Empty);
                    index = index + 1;
                    continue;
                }

                if (string.IsNullOrWhiteSpace(transition.Id) == true)
                {
                    result.AddMessage(BpuaValidationSeverity.Error, "TRANSITION_ID_MISSING", "Transition id is missing.", transition.Name);
                }
                else
                {
                    if (transitionIds.ContainsKey(transition.Id) == true)
                    {
                        result.AddMessage(BpuaValidationSeverity.Error, "TRANSITION_ID_DUPLICATE", "Duplicate transition id exists.", transition.Id);
                    }
                    else
                    {
                        transitionIds.Add(transition.Id, 1);
                    }
                }

                if (string.IsNullOrWhiteSpace(transition.Name) == true)
                {
                    result.AddMessage(BpuaValidationSeverity.Error, "TRANSITION_NAME_MISSING", "Transition name is missing.", transition.Id);
                }
                else
                {
                    if (transitionNames.ContainsKey(transition.Name) == true)
                    {
                        result.AddMessage(BpuaValidationSeverity.Error, "TRANSITION_NAME_DUPLICATE", "Duplicate transition name exists.", transition.Id);
                    }
                    else
                    {
                        transitionNames.Add(transition.Name, 1);
                    }
                }

                if (string.IsNullOrWhiteSpace(transition.FromNodeId) == true)
                {
                    result.AddMessage(BpuaValidationSeverity.Error, "TRANSITION_FROM_MISSING", "Transition source node id is missing.", transition.Id);
                }
                else
                {
                    if (nodeIds.ContainsKey(transition.FromNodeId) == false)
                    {
                        result.AddMessage(BpuaValidationSeverity.Error, "TRANSITION_FROM_NOT_FOUND", "Transition source node does not exist.", transition.Id);
                    }
                }

                if (string.IsNullOrWhiteSpace(transition.ToNodeId) == true)
                {
                    result.AddMessage(BpuaValidationSeverity.Error, "TRANSITION_TO_MISSING", "Transition target node id is missing.", transition.Id);
                }
                else
                {
                    if (nodeIds.ContainsKey(transition.ToNodeId) == false)
                    {
                        result.AddMessage(BpuaValidationSeverity.Error, "TRANSITION_TO_NOT_FOUND", "Transition target node does not exist.", transition.Id);
                    }
                }

                if (transition.TransitionType == BpuaTransitionType.Unknown)
                {
                    result.AddMessage(BpuaValidationSeverity.Warning, "TRANSITION_TYPE_UNKNOWN", "Transition type is unknown.", transition.Id);
                }

                index = index + 1;
            }
        }

        #endregion
    }
}
