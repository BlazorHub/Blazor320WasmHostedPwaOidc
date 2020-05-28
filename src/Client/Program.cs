using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Threading;

namespace Blazor320WasmHostedPwaOidc.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            //@inject HttpClient Http
            builder.Services.AddHttpClient("HttpClient.LocalNoAuth", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
            builder.Services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("HttpClient.LocalNoAuth"));

            //Oidc: DemoApiService, MgrApiService
            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("Oidc", options.ProviderOptions);
                options.ProviderOptions.DefaultScopes.Add("openid");
                options.ProviderOptions.DefaultScopes.Add("profile");
                options.ProviderOptions.DefaultScopes.Add("ourauth.mgrapi");
                options.ProviderOptions.DefaultScopes.Add("ourauth.demoapi");
            });
            builder.Services.AddTransient<OidcAuthorizationMessageHandler>();
            builder.Services.AddHttpClient<DemoApiService>(client => client.BaseAddress = new Uri(builder.Configuration["DemoApi:BaseAddress"]))
                .AddHttpMessageHandler<OidcAuthorizationMessageHandler>();
            builder.Services.AddHttpClient<MgrApiService>(client => client.BaseAddress = new Uri(builder.Configuration["MgrApi:BaseAddress"]))
                .AddHttpMessageHandler<OidcAuthorizationMessageHandler>();

            await builder.Build().RunAsync();
        }
    }

    public class OidcAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public OidcAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigation, IConfiguration configuration) : base(provider, navigation)
        {
            ConfigureHandler(new[] {
                configuration["DemoApi:BaseAddress"],
                configuration["MgrApi:BaseAddress"]
            });
        }
    }

    public class ApiService
    {
        public HttpClient Http { get; }
        public ApiService(HttpClient client) => Http = client;
        public Task<TValue> GetFromJsonAsync<TValue>(string requestUri, CancellationToken cancellationToken = default)
            => Http.GetFromJsonAsync<TValue>(requestUri, cancellationToken);
    }

    public class DemoApiService : ApiService
    {
        public DemoApiService(HttpClient client) : base(client) { }
    }

    public class MgrApiService : ApiService
    {
        public MgrApiService(HttpClient client) : base(client) { }
    }
}
