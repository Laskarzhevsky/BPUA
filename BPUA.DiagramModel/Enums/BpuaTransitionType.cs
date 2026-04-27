namespace BPUA.DiagramModel.Enums
{
    /// <summary>
    /// Defines the business meaning of a transition.
    /// </summary>
    public enum BpuaTransitionType
    {
        Unknown = 0,
        Business = 1,
        Navigation = 2,
        Validation = 3,
        Initialization = 4,
        Completion = 5
    }
}
