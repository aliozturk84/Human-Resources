using AppServer.Models;
using AppServer.Views.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.JSInterop;

namespace AppServer.Views.Pages;

public partial class Crud
{
    private UserCreateModel CreateModel { get; set; } = new();
    private UserUpdateModel UpdateModel { get; set; } = new();
    private QuickGrid<User> RefGrid { get; set; }



    public async ValueTask DisposeAsync() => await dbContext.DisposeAsync();
    private PspDbContext dbContext = default;
    private PspConfirm confirm = default;



    protected override void OnInitialized()
    {
        dbContext = DbFactory.CreateDbContext();
    }



    private async Task OnDeleteAsync(Guid id)
    {
        if (await confirm.OpenAsync())
        {
            var user = await dbContext.Users.FindAsync(id);
            if (user is not null)
            {
                dbContext.Users.Remove(user);

                await dbContext.SaveChangesAsync();
                await RefGrid.RefreshDataAsync();
            }
        }
    }



    private async Task OnCreateAsync()
    {
        var entity = new User
        {
            Name = CreateModel.Name,
            Surname = CreateModel.Surname,
            Mail = CreateModel.Mail,
            Country = CreateModel.Country
        };

        dbContext.Users.Add(entity);
        CreateModel = new();

        await dbContext.SaveChangesAsync();
        await RefGrid.RefreshDataAsync();
        await jsRuntime.InvokeVoidAsync("eval", "bootstrap.Modal.getInstance(document.getElementById('create')).hide();");
    }



    private async Task OnShowAsync(Guid id)
    {
        var user = await dbContext.Users.FindAsync(id);
        if (user is not null)
        {
            UpdateModel.Id = user.Id;
            UpdateModel.Name = user.Name;
            UpdateModel.Surname = user.Surname;
            UpdateModel.Mail = user.Mail;
            UpdateModel.Country = user.Country;

            await jsRuntime.InvokeVoidAsync("eval", "new bootstrap.Modal(document.getElementById('update')).show();");
        }
    }
    private async Task OnUpdateAsync()
    {
        var user = await dbContext.Users.FindAsync(UpdateModel.Id);
        if (user is not null)
        {
            user.Name = UpdateModel.Name;
            user.Surname = UpdateModel.Surname;
            user.Mail = UpdateModel.Mail;
            user.Country = UpdateModel.Country;
            UpdateModel = new();

            await dbContext.SaveChangesAsync();
            await RefGrid.RefreshDataAsync();
            await jsRuntime.InvokeVoidAsync("eval", "bootstrap.Modal.getInstance(document.getElementById('update')).hide();");
        }
    }
}

// SQL
//CREATE TABLE dbo.Users(
//    Id uniqueidentifier NOT NULL,
//    Name nvarchar(50) NOT NULL,
//    Surname nvarchar(50) NOT NULL,
//    Mail nvarchar(150) NOT NULL,
//    Country nvarchar(75) NULL,
//    CONSTRAINT PK_Users PRIMARY KEY(Id)
//)