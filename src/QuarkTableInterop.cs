using System.Threading;
using System.Threading.Tasks;
using Soenneker.Blazor.Utils.ResourceLoader.Abstract;
using Soenneker.Extensions.ValueTask;
using Soenneker.Quark.Table.Abstract;
using Soenneker.Utils.AsyncSingleton;

namespace Soenneker.Quark.Table;

/// <inheritdoc cref="IQuarkTableInterop"/>
public sealed class QuarkTableInterop : IQuarkTableInterop
{
    private readonly AsyncSingleton _styleInitializer;

    public QuarkTableInterop(IResourceLoader resourceLoader)
    {
        IResourceLoader resourceLoader1 = resourceLoader;

        _styleInitializer = new AsyncSingleton(async (token, _) =>
        {
            await resourceLoader1.LoadStyle("_content/Soenneker.Quark.Table/css/quarktable.css", cancellationToken: token).NoSync();

            return new object();
        });
    }

    public ValueTask Initialize( CancellationToken cancellationToken = default)
    {
        return _styleInitializer.Init(cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        await _styleInitializer.DisposeAsync().NoSync();
    }
}