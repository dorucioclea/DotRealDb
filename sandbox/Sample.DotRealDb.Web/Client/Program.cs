using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DotRealDb.Client;
using System.Collections.ObjectModel;
using Sample.DotRealDb.Web.Shared;

namespace Sample.DotRealDb.Blazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddDotRealDbClient(opts => opts.ServerBaseUrl = "https://localhost:5001");

            await builder.Build().RunAsync();
        }
    }

    public class MyViewModel
    {
        private readonly IDotRealChangeHandler changeHandler;

        public MyViewModel(IDotRealChangeHandler changeHandler)
        {
            this.changeHandler = changeHandler;
            FetchAndStartTracking();
        }

        public ObservableCollection<WeatherForecast> Items { get; set; } = new ObservableCollection<WeatherForecast>();
        private async void FetchAndStartTracking()
        {
            await changeHandler.StartTrackingAsync(this.Items, "SampleDbContext");
        }
    }
}
