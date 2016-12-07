using System;
using Marten;

namespace IdentityServer4.Postgresql.IntegrationTests
{
    public class DatabaseFixture : IDisposable
    {
        public IDocumentStore Store;

        public DatabaseFixture()
        {
            this.Store = DocumentStore.For("User ID=postgres;Password=skhokho;Host=localhost;Port=5432;Database=idsrv4_test;");
        }
        public void Dispose()
        {
            Store.Advanced.Clean.DeleteAllDocuments();
            Store.Dispose();
        }
    }
}
