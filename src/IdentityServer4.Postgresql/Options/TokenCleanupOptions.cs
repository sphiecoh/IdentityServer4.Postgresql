namespace IdentityServer4.Postgresql.Options
{
    public class TokenCleanupOptions
    {
        public int Interval { get; set; } = 60;
    }
}
