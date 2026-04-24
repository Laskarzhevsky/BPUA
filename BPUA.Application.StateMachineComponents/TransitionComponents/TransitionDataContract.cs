using BPUA.Application.Contracts;

using System.Collections.ObjectModel;

namespace BPUA.Application.StateMachineComponents
{
    /// <summary>
    /// Provides transition data contract functionality.
    /// </summary>
    public sealed class TransitionDataContract : Collection<ITransitionDataTableContract>, ITransitionDataContract
    {
    }
}
