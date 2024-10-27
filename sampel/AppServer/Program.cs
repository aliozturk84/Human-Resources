using AppServer.Infrastructure.Container;
using AppServer.Views.Layout;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<FileStateContainer>();

builder.Services.AddDbContextFactory<PspDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Crud")));

builder.Services.AddQuickGridEntityFrameworkAdapter();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddServerSideBlazor();

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();


var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();

}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();



app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();



app.Run();
