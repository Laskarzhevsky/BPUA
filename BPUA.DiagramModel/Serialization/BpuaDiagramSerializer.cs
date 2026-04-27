using BPUA.DiagramModel.Model;
using System.IO;
using System.Text.Json;

namespace BPUA.DiagramModel.Serialization
{
    /// <summary>
    /// Serializes and deserializes BPUA diagram models as JSON.
    /// </summary>
    public sealed class BpuaDiagramSerializer
    {
        #region Fields

        private readonly JsonSerializerOptions _options;

        #endregion

        #region Constructors

        public BpuaDiagramSerializer()
        {
            _options = new JsonSerializerOptions();
            _options.WriteIndented = true;
            _options.PropertyNameCaseInsensitive = true;
        }

        #endregion

        #region Public Methods

        public string ToJsonString(BpuaDiagram diagram)
        {
            if (diagram == null)
            {
                return string.Empty;
            }

            return JsonSerializer.Serialize(diagram, _options);
        }

        public BpuaDiagram FromJsonString(string json)
        {
            if (string.IsNullOrWhiteSpace(json) == true)
            {
                return new BpuaDiagram();
            }

            BpuaDiagram diagram = JsonSerializer.Deserialize<BpuaDiagram>(json, _options);
            if (diagram == null)
            {
                return new BpuaDiagram();
            }

            EnsureCollections(diagram);
            return diagram;
        }

        public void SaveToFile(BpuaDiagram diagram, string filePath)
        {
            string json = ToJsonString(diagram);
            File.WriteAllText(filePath, json);
        }

        public BpuaDiagram LoadFromFile(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return FromJsonString(json);
        }

        #endregion

        #region Private Methods

        private void EnsureCollections(BpuaDiagram diagram)
        {
            if (diagram.Metadata == null)
            {
                diagram.Metadata = new BpuaDiagramMetadata();
            }

            if (diagram.Nodes == null)
            {
                diagram.Nodes = new System.Collections.Generic.List<BpuaDiagramNode>();
            }

            if (diagram.Transitions == null)
            {
                diagram.Transitions = new System.Collections.Generic.List<BpuaDiagramTransition>();
            }
        }

        #endregion
    }
}
