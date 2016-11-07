# IdentityServer4.Postgresql

`Install-Package IdentityServer4.Postgresql`

Register `IDocumentSession` with your prefered DI container.

e.g AspNet Core
```
public void ConfigureServices(IServiceCollection services)
{
  services.AddTransient( _ => DocumentStore.For(connectionString).LightweightSession());
}
 ```
 