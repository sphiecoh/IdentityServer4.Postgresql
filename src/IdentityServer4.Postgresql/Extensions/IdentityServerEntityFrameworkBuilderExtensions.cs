using IdentityServer4.Postgresql.Options;
using IdentityServer4.Postgresql.Services;
using IdentityServer4.Postgresql.Stores;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Marten;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace IdentityServer4.Postgresql.Extensions
{
    public static class IdentityServerEntityFrameworkBuilderExtensions
    {
        public static IIdentityServerBuilder AddConfigurationStore(
        this IIdentityServerBuilder builder, string connectionString = null)
        {
            builder.Services.AddTransient<IClientStore, ClientStore>();
            builder.Services.AddTransient<IResourceStore, ResourceStore>();
            builder.Services.AddTransient<ICorsPolicyService, CorsPolicyService>();

            if (!string.IsNullOrEmpty(connectionString))
            {
                var store = DocumentStore.For(connectionString);
                builder.Services.AddSingleton<IDocumentStore>(store);
                builder.Services.AddTransient(_ => store.LightweightSession());
            }

            return builder;
        }
        public static IIdentityServerBuilder AddConfigurationStoreCache(
           this IIdentityServerBuilder builder)
        {
            builder.AddInMemoryCaching();

            // these need to be registered as concrete classes in DI for
            // the caching decorators to work
            builder.Services.AddTransient<ClientStore>();
            builder.Services.AddTransient<ResourceStore>();

            // add the caching decorators
            builder.AddClientStoreCache<ClientStore>();
            builder.AddResourceStoreCache<ResourceStore>();

            return builder;
        }

        public static IIdentityServerBuilder AddOperationalStore(this IIdentityServerBuilder builder, Action<TokenCleanupOptions> tokenCleanUpOptions = null)
        {
            builder.Services.AddScoped<IPersistedGrantStore, PersistedGrantStore>();
            var tokenCleanupOptions = new TokenCleanupOptions();
            tokenCleanUpOptions?.Invoke(tokenCleanupOptions);
            builder.Services.AddSingleton(tokenCleanupOptions);
            builder.Services.AddSingleton<TokenCleanup>();
            return builder;
        }
        public static IApplicationBuilder UseIdentityServerTokenCleanup(this IApplicationBuilder app, IApplicationLifetime applicationLifetime)
        {
            var tokenCleanup = app.ApplicationServices.GetService<TokenCleanup>();
            if (tokenCleanup == null)
            {
                throw new InvalidOperationException("AddOperationalStore() must be called before calling this method.");
            }
            applicationLifetime.ApplicationStarted.Register(tokenCleanup.Start);
            applicationLifetime.ApplicationStopping.Register(tokenCleanup.Stop);

            return app;
        }

    }
}
