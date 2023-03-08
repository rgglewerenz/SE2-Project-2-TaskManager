using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ElectronNET.API;
using ElectronNET.API.Entities;
using TaskManagerGUI.Authentication;
using TaskManagerGUI.Interface;
using TaskManagerGUI.Services;
using Blazored.LocalStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddElectron();
builder.WebHost.UseElectron(args);

builder.Services.AddHttpClient();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddScoped<IAuthProvider, AuthProvider>();
builder.Services.AddSingleton<ITaskService, TaskService>();
builder.Services.AddSingleton<IOpenAIService, OpenAIService>();


if (HybridSupport.IsElectronActive)
{
    // Open the Electron-Window here
    Task.Run(async () => {
        var options = new BrowserWindowOptions();
        options.AutoHideMenuBar = true;
        options.Width = 1200;
        options.Height = 1000;
        var window = await Electron.WindowManager.CreateWindowAsync(options);
        window.OnClosed += () => {
            Electron.App.Quit();
        };
    });
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();




app.MapBlazorHub();
app.MapFallbackToPage("/_Host");


app.Run();
