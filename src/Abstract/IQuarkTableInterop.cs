using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Quark;

/// <summary>
/// A Blazor interop library for the Quark Table component
/// </summary>
public interface IQuarkTableInterop : IAsyncDisposable
{
    /// <summary>
    /// Initializes the Quark Table interop by loading required scripts and styles.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the initialization operation.</param>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
    ValueTask Initialize(CancellationToken cancellationToken = default);
}
