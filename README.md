# IdentityServer4.Postgresql

[![Build status](https://ci.appveyor.com/api/projects/status/r4boyo3qkhbrmh8o/branch/master?svg=true)](https://ci.appveyor.com/project/sphiecoh/identityserver4-postgresql/branch/master)
[ ![Codeship Status for Sphiecoh/IdentityServer4.Postgresql](https://app.codeship.com/projects/325c07d0-1b78-0135-016a-3270df5f1418/status?branch=master)](https://app.codeship.com/projects/219607)
[![NuGet Version](http://img.shields.io/nuget/v/IdentityServer4.Postgresql.svg?style=flat)](https://www.nuget.org/packages/IdentityServer4.Postgresql/)

`Install-Package IdentityServer4.Postgresql`

e.g AspNet Core
```
using IdentityServer4.Postgresql.Extensions;

public void ConfigureServices(IServiceCollection services)
{
   var builder = services.AddIdentityServer();
   builder.AddConfigurationStore().AddOperationalStore();
}
public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
 {
    app.UseIdentityServer():
 }
 ```
 This will register all the `IdentityServer` stores and optionally a Marten's `IDocumentSession` as well as `IDocumentStore` if you pass a connection string;
 
