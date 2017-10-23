using System;
using Marten;

namespace IdentityServer4.Postgresql.IntegrationTests
{
	public class DatabaseFixture : IDisposable
	{
		public IDocumentStore Store;

		public DatabaseFixture()
		{
            
			this.Store = DocumentStore.For(c => 
            {
                c.Connection($"User ID={GetEnv("PG_USER","sa")};Password={GetEnv("PG_PASSWORD","skhokho")};Host={GetEnv("PG_HOST","localhost")};Port={GetEnv("PG_PORT","5432")};Database={GetEnv("PG_DATABASE","idsrv4_test")};");
                c.PLV8Enabled = false;
                
            });
            
		}
		public void Dispose()
		{
			Store.Advanced.Clean.DeleteAllDocuments();
			Store.Dispose();
		}
		private string GetEnv(string name, string defaultVal)
		{
			return Environment.GetEnvironmentVariable(name) ?? defaultVal; 
		}
	}
}
