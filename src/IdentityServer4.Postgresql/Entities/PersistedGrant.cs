using Marten.Schema;
using System;

namespace IdentityServer4.Postgresql.Entities
{
    public class PersistedGrant : EntityKey
    {
        public PersistedGrant()
        {
            Id = Guid.NewGuid().ToString();
        }
       
        public string Key { get; set; }
        public string Type { get; set; }
        public string SubjectId { get; set; }
        public string ClientId { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime Expiration { get; set; }
        public string Data { get; set; }
    }
}
