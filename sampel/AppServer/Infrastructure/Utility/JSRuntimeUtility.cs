using Microsoft.JSInterop;

namespace AppServer.Infrastructure.Utility;

public static class JSRuntimeUtility
{
    public static async Task ModalHideAsync(this IJSRuntime jsRuntime, string htmlId)
    {
        await jsRuntime.InvokeVoidAsync("eval", $"bootstrap.Modal.getInstance(document.getElementById('{htmlId}')).hide();");
    }
    public static async Task ModalShowAsync(this IJSRuntime jsRuntime, string htmlId)
    {
        await jsRuntime.InvokeVoidAsync("eval", $"new bootstrap.Modal(document.getElementById('{htmlId}')).show();");
    }
}