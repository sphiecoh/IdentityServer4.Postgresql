using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Postgresql.Entities
{
    public class ClientClaim
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
