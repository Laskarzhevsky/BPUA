using BPUA.DiagramModel.Enums;

namespace BPUA.DiagramModel.Validation
{
    /// <summary>
    /// Represents one validation message produced from diagram validation.
    /// </summary>
    public sealed class BpuaDiagramValidationMessage
    {
        #region Constructors

        public BpuaDiagramValidationMessage()
        {
            Severity = BpuaValidationSeverity.Information;
            Code = string.Empty;
            Message = string.Empty;
            ObjectId = string.Empty;
        }

        #endregion

        #region Properties

        public BpuaValidationSeverity Severity { get; set; }

        public string Code { get; set; }

        public string Message { get; set; }

        public string ObjectId { get; set; }

        #endregion
    }
}
