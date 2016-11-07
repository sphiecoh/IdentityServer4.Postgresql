namespace IdentityServer4.Postgresql.Extensions
{
    public static class ScopeExtensions
    {
        public static Entities.Scope ToScope(this Models.Scope scope)
            => new Entities.Scope { Name = scope.Name,
                DisplayName = scope.DisplayName,
                ShowInDiscoveryDocument = scope.ShowInDiscoveryDocument,
                Required = scope.Required,
                Type = scope.Type,
                Emphasize = scope.Emphasize,
                Claims = scope.Claims,
                IncludeAllClaimsForUser = scope.IncludeAllClaimsForUser,
                Enabled = scope.Enabled,
                AllowUnrestrictedIntrospection = scope.AllowUnrestrictedIntrospection,
                Description = scope.Description,
                ClaimsRule = scope.ClaimsRule,
                ScopeSecrets = scope.ScopeSecrets
            };
        
    }
}
