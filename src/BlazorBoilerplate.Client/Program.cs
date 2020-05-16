using BlazorBoilerplate.Application;
using BlazorBoilerplate.Application.Contracts;
using BlazorBoilerplate.Application.Implementations;
using BlazorBoilerplate.CommonUI;
using BlazorBoilerplate.Shared.AuthorizationDefinitions;
using MatBlazor;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace BlazorBoilerplate.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddAuthorizationServices();

            builder.Services.AddAppState();
            builder.Services.AddMatComponents();

            await builder
            .Build()
            .UseLoadingBar()            
            .RunAsync();
        }
    }
}
