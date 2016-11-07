using Marten.Schema;

namespace IdentityServer4.Postgresql.Entities
{
    public class Scope : Models.Scope
    {
        [Identity]
        public int Id { get; set; }
       
    }
}
