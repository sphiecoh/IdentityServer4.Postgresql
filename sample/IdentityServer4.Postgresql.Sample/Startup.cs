using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using IdentityServer4.Postgresql.Extensions;
using Marten;
using IdentityServer4.Models;
using IdentityServer4.Postgresql.Mappers;
using IdentityServer4.Postgresql.Entities;

namespace IdentityServer4.Postgresql.Sample
{
    public class Startup
    {
        private const string connection = "host=localhost;database=sample;user id=postgres; Password=skhokho";
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityServer().AddConfigurationStore(connection).AddOperationalStore().AddTemporarySigningCredential();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            InitData(app);

            loggerFactory.AddConsole();
            loggerFactory.AddFile("Logs/myapp-{Date}.txt");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseIdentityServer();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }

        private void InitData(IApplicationBuilder app)
        {
            var store = DocumentStore.For(connection);
            store.Advanced.Clean.CompletelyRemoveAll();
            using (var session = store.LightweightSession())
            {
                if (!session.Query<Entities.ApiResource>().Any())
                {
                    var resources = new List<Entities.ApiResource> {
                     new Entities.ApiResource{ Name = "api1" , Description = "Api" , DisplayName ="api1" , Scopes = new List<ApiScope> { new ApiScope { Name = "api1" , DisplayName ="api1"  } } },

                    };
                    session.StoreObjects(resources);
                }

                if (!session.Query<IdentityServer4.Postgresql.Entities.IdentityResource>().Any())
                {
                    var resources = new List<IdentityServer4.Postgresql.Entities.IdentityResource> {
                        new IdentityResources.OpenId().ToEntity(),
                        new IdentityResources.Profile().ToEntity(),
                        new IdentityResources.Email().ToEntity(),
                        new IdentityResources.Phone().ToEntity()
                    };
                    session.StoreObjects(resources);
                }
                if (!session.Query<IdentityServer4.Postgresql.Entities.Client>().Any())
                {
                    var clients = new List<IdentityServer4.Postgresql.Entities.Client>
                    {
                          new Entities.Client
                            {
                                Id = "ro.client",
                                ClientId ="ro.client",
                                ClientName = "mvc",
                                AllowedGrantTypes =  new List<ClientGrantType> { new ClientGrantType { GrantType = GrantType.Hybrid } , new ClientGrantType {  GrantType = GrantType.ClientCredentials} },
                                AllowedCorsOrigins =  new List<ClientCorsOrigin>  {new ClientCorsOrigin { Origin = "http://localhost:5003" } },
                                RequireClientSecret = true,
                                ClientSecrets = new List<ClientSecret> { new ClientSecret { Value = "secret".Sha256() }  },
                                RequireConsent = false,
                                AllowedScopes = new List<ClientScope>{
                                     new ClientScope { Scope = IdentityServer4.IdentityServerConstants.StandardScopes.OpenId },
                                     new ClientScope { Scope = IdentityServer4.IdentityServerConstants.StandardScopes.Profile },
                                     new ClientScope { Scope ="api1" }
                                },
                                RedirectUris = new List<ClientRedirectUri> { new ClientRedirectUri { RedirectUri ="http://localhost:5003/signin-oidc" } }
                            }

                        };
                    session.StoreObjects(clients);
                }
                session.SaveChanges();
            }
        }
    }
}
