# IdentityServer4.Postgresql

`Install-Package IdentityServer4.Postgresql`

e.g AspNet Core
```
using IdentityServer4.Postgresql.Extensions;

public void ConfigureServices(IServiceCollection services)
{
   var builder = services.AddIdentityServer();
   builder.AddConfigurationStore().AddOperationalStore();
}
 ```
 This will register all the `IdentityServer` stores and optionally a Marten's `IDocumentSession` as well as `IDocumentStore` if you pass a connection string;
 
