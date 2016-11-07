using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Marten;
using IdentityServer4.Postgresql.Mappers;

namespace IdentityServer4.Postgresql.Stores
{

    public class ScopeStore : IScopeStore
    {
        private readonly IDocumentSession _documentSession;
        public ScopeStore(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }
        public async Task<IEnumerable<Scope>> FindScopesAsync(IEnumerable<string> scopeNames)
        {
           var scopes = await _documentSession.Query<Entities.Scope>().ToListAsync().ConfigureAwait(false);
            return scopes.Where(x => scopeNames.Contains(x.Name)).Select(x => x.ToModel());
        }

        public  Task<IEnumerable<Scope>> GetScopesAsync(bool publicOnly = true)
        {
            var scopes = publicOnly ? _documentSession.Query<Entities.Scope>().Where(x => x.ShowInDiscoveryDocument).AsEnumerable() : _documentSession.Query<Entities.Scope>().AsEnumerable();
            return Task.FromResult(scopes.Select(x => x.ToModel()));
        }
    }
}