using Microsoft.AspNetCore.Components;

namespace AppServer.Infrastructure.Razor;

public abstract class AppComponentBase : ComponentBase
{
    protected async Task BlockAsync(Func<bool> condition, int pollDelay = 25, CancellationToken ct = default)
    {
        try
        {
            while (condition())
            {
                await Task.Delay(pollDelay, ct).ConfigureAwait(true);
            }
        }
        catch (TaskCanceledException) { }
    }
}