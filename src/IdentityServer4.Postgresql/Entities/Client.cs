using Marten.Schema;

namespace  IdentityServer4.Postgresql.Entities
{
    public class Client : Models.Client
    {
        [Identity]
        public int Id { get; set; }
    }
}
