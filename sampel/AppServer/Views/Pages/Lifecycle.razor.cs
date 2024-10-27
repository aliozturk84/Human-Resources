
namespace AppServer.Views.Pages;

public partial class Lifecycle
{
    private int count;

    protected override async Task OnInitializedAsync()
    {
        count = 0; // Bileşen ilk kez oluşturulurken sayacı başlat
        await Task.CompletedTask;
    }

    protected override async Task OnParametersSetAsync()
    {
        // Parametreler değiştiğinde işlem yap
        await Task.CompletedTask;
    }

    private async Task OnClickAsync()
    {
        count++;
        StateHasChanged(); // Bileşeni yeniden render et
        await Task.CompletedTask;
    }


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // Bileşen ilk kez render edildikten sonra işlem yap
            await Task.CompletedTask;
        }
    }

    public void Dispose()
    {
        // Kaynakları temizle
    }
}