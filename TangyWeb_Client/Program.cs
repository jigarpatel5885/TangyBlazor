using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TangyWeb_Client;
using TangyWeb_Client.Services;
using TangyWeb_Client.Services.IService;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration.GetValue<string> ("BaseAPIUrl")) });

builder.Services.AddScoped<IProductService, ProductService>();

await builder.Build().RunAsync();
