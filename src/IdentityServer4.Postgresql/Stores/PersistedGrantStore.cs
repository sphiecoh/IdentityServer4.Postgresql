using Marten;
namespace IdentityServer4.Postgresql.Stores
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using IdentityServer4.Models;
    using IdentityServer4.Stores;
    using IdentityServer4.Postgresql.Mappers;

    public class PersistedGrantStore : IPersistedGrantStore
    {
        private readonly IDocumentSession _documentSession;
        public PersistedGrantStore(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
           var grants = await _documentSession.Query<Entities.PersistedGrant>().Where(x => x.SubjectId == subjectId).ToListAsync().ConfigureAwait(false);
            return grants.Select(y => y.ToModel());
        }

        public async Task<PersistedGrant> GetAsync(string key)
        {
            var grant =  await _documentSession.Query<Entities.PersistedGrant>().FirstOrDefaultAsync(x => x.Key == key).ConfigureAwait(false);
            return grant?.ToModel();
        }

        public Task RemoveAllAsync(string subjectId, string clientId)
        {
            _documentSession.DeleteWhere<Entities.PersistedGrant>(grant => grant.SubjectId == subjectId && grant.ClientId == clientId);
            return _documentSession.SaveChangesAsync();
        }

        public Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            _documentSession.DeleteWhere<Entities.PersistedGrant>(grant => grant.Type == type && grant.SubjectId == subjectId && grant.ClientId == clientId);
            return _documentSession.SaveChangesAsync();
        }

        public Task RemoveAsync(string key)
        {
            _documentSession.DeleteWhere<Entities.PersistedGrant>(grant => grant.Key == key);
            return _documentSession.SaveChangesAsync();
        }

        public Task StoreAsync(PersistedGrant grant)
        {
            _documentSession.Store(grant.ToEntity());
            return _documentSession.SaveChangesAsync();
        }
    }
}