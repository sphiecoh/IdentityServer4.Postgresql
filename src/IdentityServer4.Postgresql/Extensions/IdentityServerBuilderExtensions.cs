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
    public static class IdentityServerBuilderExtensions
    {
        /// <summary>
        /// Registers the IdentityServer stores with the container
        /// If the <paramref name="connectionString"> is passed , a <see name="IDocumentStore"> and <see name="IDocumentSession"> are registered
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="connectionString">A postgres connectionstring</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddConfigurationStore(
        this IIdentityServerBuilder builder, string connectionString = null)
        {
            builder.Services.AddTransient<IClientStore, ClientStore>();
            builder.Services.AddTransient<IResourceStore, ResourceStore>();
            builder.Services.AddTransient<ICorsPolicyService, CorsPolicyService>();

            if (!string.IsNullOrEmpty(connectionString))
            {
                ConfigureMarten(builder, new MartenOptions { ConnectionString = connectionString });
            }

            return builder;
        }
        /// <summary>
        /// Registers the IdentityServer stores with the container
        /// If the <paramref name="connectionString"> is passed , a <see name="IDocumentStore"> and <see name="IDocumentSession"> are registered
        /// </summary>
        /// <param name="builder">IIdentityServerBuilder</param>
        /// <param name="options">MartenOptions</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddConfigurationStore(
        this IIdentityServerBuilder builder, MartenOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            builder.Services.AddTransient<IClientStore, ClientStore>();
            builder.Services.AddTransient<IResourceStore, ResourceStore>();
            builder.Services.AddTransient<ICorsPolicyService, CorsPolicyService>();
            ConfigureMarten(builder, options);
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
        private static void ConfigureMarten(IIdentityServerBuilder builder, MartenOptions options)
        {
            var store = DocumentStore.For(_ =>
            {
                _.DatabaseSchemaName = options.SchemaName;
                _.Connection(options.ConnectionString);
            });
            builder.Services.AddSingleton<IDocumentStore>(store);
            builder.Services.AddTransient(_ => store.LightweightSession());
        }

    }
}
