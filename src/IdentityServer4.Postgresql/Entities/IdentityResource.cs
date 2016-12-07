using Marten.Schema;
using System;
using System.Collections.Generic;

namespace IdentityServer4.Postgresql.Entities
{
    public class IdentityResource : EntityKey
    {
        public IdentityResource()
        {
            Id = Guid.NewGuid().ToString();
        }

        public bool Enabled { get; set; } = true;
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Emphasize { get; set; }
        public bool ShowInDiscoveryDocument { get; set; } = true;
        public List<IdentityClaim> UserClaims { get; set; }
    }

}
