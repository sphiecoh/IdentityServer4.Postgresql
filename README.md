# IdentityServer4.Postgresql

`Install-Package IdentityServer4.Postgresql -Pre`

e.g AspNet Core
```
public void ConfigureServices(IServiceCollection services)
{
   var builder = services.AddIdentityServer();
   builder.AddConfigurationStore().AddOperationalStore();
}
 ```
 This will register all the `IdentityServer` stores and optionally a Marten's `IDocumentSession` as well as `IDocumentStore` if you pass a connection string;
 
