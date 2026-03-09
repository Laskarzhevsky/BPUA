using System.Threading.Tasks;

namespace BPUA.Core
{
    #region Delegates
    /// <summary>
    /// Defines delegate for asynchronous events
    /// </summary>
    /// <typeparam name="TEventArgs">Event arguments type</typeparam>
    /// <param name="sender">Event source</param>
    /// <param name="args">Event arguments</param>
    public delegate Task AsyncEventHandler<TEventArgs>(object? sender, TEventArgs args);
    #endregion
}
