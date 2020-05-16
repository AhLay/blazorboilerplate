using BlazorBoilerplate.Application;
using BlazorBoilerplate.Application.Contracts;
using BlazorBoilerplate.Application.Implementations;
using BlazorBoilerplate.Shared.AuthorizationDefinitions;
using MatBlazor;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace BlazorBoilerplate.Client
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAuthorizeApi(this IServiceCollection services)
        {
#if ServerSideBlazor
             services.AddScoped<IAuthorizeApi, AuthorizeServerApi>();
#endif

#if ClientSideBlazor
            services.AddScoped<IAuthorizeApi, AuthorizeClientApi>();
#endif            
            return services;
        }


        public static IServiceCollection AddAuthorizationServices(this IServiceCollection services)
        {
            services.AddAuthorizationCore(config =>
            {
                config.AddPolicy(Policies.IsAdmin, Policies.IsAdminPolicy());
                config.AddPolicy(Policies.IsUser, Policies.IsUserPolicy());
                config.AddPolicy(Policies.IsReadOnly, Policies.IsUserPolicy());
                // config.AddPolicy(Policies.IsMyDomain, Policies.IsMyDomainPolicy());  Only works on the server end
            });

            services.AddScoped<AuthenticationStateProvider, IdentityAuthStateService>();           
            services.AddScoped(sp => (IIdentityAuthStateService)sp.GetRequiredService<AuthenticationStateProvider>());

            services.AddAuthorizeApi();

            return services;
        }

        public static IServiceCollection AddMatComponents(this IServiceCollection services)
        {
            services.AddLoadingBar();
            services.AddMatToaster(config =>
            {
                config.Position = MatToastPosition.BottomRight;
                config.PreventDuplicates = true;
                config.NewestOnTop = true;
                config.ShowCloseButton = true;
                config.MaximumOpacity = 95;
                config.VisibleStateDuration = 3000;
            });

            return services;
        }

        public static IServiceCollection AddAppState(this IServiceCollection services)
        {
            services.Add(new ServiceDescriptor(typeof(IUserProfileApi), typeof(UserProfileApi), ServiceLifetime.Scoped));
            services.AddScoped<AppState>();

            return services;
        }
    }
}
