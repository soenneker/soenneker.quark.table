using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Quark.Table.Abstract;

/// <summary>
/// A Blazor interop library for the select user control library, Tom Select
/// </summary>
public interface IQuarkTableInterop :  IAsyncDisposable
{
    /// <summary>
    /// Initializes the TomSelect interop by loading required scripts and styles.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the initialization operation.</param>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
    ValueTask Initialize(CancellationToken cancellationToken = default);
}