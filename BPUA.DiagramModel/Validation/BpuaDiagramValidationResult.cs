using BPUA.DiagramModel.Enums;
using System.Collections.Generic;

namespace BPUA.DiagramModel.Validation
{
    /// <summary>
    /// Contains validation messages for one diagram validation run.
    /// </summary>
    public sealed class BpuaDiagramValidationResult
    {
        #region Constructors

        public BpuaDiagramValidationResult()
        {
            Messages = new List<BpuaDiagramValidationMessage>();
        }

        #endregion

        #region Properties

        public List<BpuaDiagramValidationMessage> Messages { get; set; }

        public bool IsValid
        {
            get
            {
                int index = 0;
                while (index < Messages.Count)
                {
                    if (Messages[index].Severity == BpuaValidationSeverity.Error)
                    {
                        return false;
                    }

                    index = index + 1;
                }

                return true;
            }
        }

        #endregion

        #region Methods

        public void AddMessage(BpuaValidationSeverity severity, string code, string message, string objectId)
        {
            BpuaDiagramValidationMessage validationMessage = new BpuaDiagramValidationMessage();
            validationMessage.Severity = severity;
            validationMessage.Code = code;
            validationMessage.Message = message;
            validationMessage.ObjectId = objectId;

            Messages.Add(validationMessage);
        }

        #endregion
    }
}
