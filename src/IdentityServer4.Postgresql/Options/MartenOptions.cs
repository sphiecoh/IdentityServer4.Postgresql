namespace IdentityServer4.Postgresql.Options
{
    public class MartenOptions
    {
        public string ConnectionString { get; set; }
        public string SchemaName { get; set; } = "public";
    }
}