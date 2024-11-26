using Abdt.Loyal.NoteSaver.Web.Client;
using Abdt.Loyal.NoteSaver.Web.Client.Http;
using Abdt.Loyal.NoteSaver.Web.Shared;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();

builder.Services.AddHttpClient<INoteClient<Note>, NoteHttpClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:57692/api/v1/notes/");
});

await builder.Build().RunAsync();
