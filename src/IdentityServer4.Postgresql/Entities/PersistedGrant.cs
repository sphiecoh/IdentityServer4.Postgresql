using Marten.Schema;

namespace IdentityServer4.Postgresql.Entities
{
    public class PersistedGrant : Models.PersistedGrant
    {
        [Identity]
        public int Id { get; set; }
    }
}
