using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer4.Postgresql.Mappers;
using Xunit;
using GenFu;
using IdentityServer4.Postgresql.Entities;
using IdentityServer4.Postgresql.Stores;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace IdentityServer4.Postgresql.IntegrationTests.Stores
{
   
    public class PersistedGrantStoreTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture martenFixture;
        public PersistedGrantStoreTests(DatabaseFixture databaseFixture)
        {
            martenFixture = databaseFixture;
        }
        [Fact]
        public void StoreAsync_WhenPersistedGrantStored_ExpectSuccess()
        {
            var persistedGrant = GenFu.GenFu.New<PersistedGrant>();

            using (var session = martenFixture.Store.OpenSession())
            {
                var store = new PersistedGrantStore(session);
                store.StoreAsync(persistedGrant.ToModel()).Wait();
            }

            using (var session = martenFixture.Store.OpenSession())
            {
                var foundGrant = session.Query<PersistedGrant>().First();
                Assert.NotNull(foundGrant);
            }
        }
        [Fact]
        public void GetAsync_WithKeyAndPersistedGrantExists_ExpectPersistedGrantReturned()
        {
            var persistedGrant = GenFu.GenFu.New<PersistedGrant>();
            using (var session = martenFixture.Store.OpenSession())
            {
                var store = new PersistedGrantStore(session);
                store.StoreAsync(persistedGrant.ToModel()).Wait();
            }
            using (var session = martenFixture.Store.OpenSession())
            {
                 var store = new PersistedGrantStore(session);
                var foundGrant = store.GetAsync(persistedGrant.Key).Result;
                Assert.NotNull(foundGrant);
            }

        }

        [Fact]
        public void GetAllAsync_ShouldReturnSavedGrants()
        {
            int count = 10;
            string subject = Guid.NewGuid().ToString();
            GenFu.GenFu.Configure<PersistedGrant>().Fill(x => x.SubjectId, subject);
            var persistedGrants = GenFu.GenFu.ListOf<PersistedGrant>(count);
            using (var session = martenFixture.Store.LightweightSession())
            {
                session.StoreObjects(persistedGrants);
                session.SaveChanges();
            }
            using (var session = martenFixture.Store.LightweightSession())
            {
                var _store = new PersistedGrantStore(session);
                var foundGrants =  _store.GetAllAsync(subject).Result;
                Assert.True(foundGrants.Count() == count);
            }
        }

        [Fact]
        public void RemoveAllAsync_ShouldRemoveAllGrantsBySubjectAndClientId()
        {
            string subject = "mysubject";
            string client = "myclient";
            GenFu.GenFu.Configure<PersistedGrant>().Fill(x => x.SubjectId, subject).Fill(x => x.ClientId,client);
            var persistedGrants = GenFu.GenFu.ListOf<PersistedGrant>();
            using (var session = martenFixture.Store.LightweightSession())
            {
                session.StoreObjects(persistedGrants);
                session.SaveChanges();
            }
            using (var session = martenFixture.Store.LightweightSession())
            {
                var _store = new PersistedGrantStore(session);
                 _store.RemoveAllAsync(subject,client).Wait();
                var grants = session.Query<PersistedGrant>().Where(x => x.SubjectId == subject && x.ClientId == client).ToList();
                Assert.True(grants.Count == 0 );
            }
        }

        [Fact]
        public void RemoveAllAsync_ShouldRemoveAllGrantsBySubjectAndClientIdAndType()
        {
            var persistedGrant = GenFu.GenFu.New<PersistedGrant>();
            using (var session = martenFixture.Store.LightweightSession())
            {
                session.Store(persistedGrant);
                session.SaveChanges();
            }
            using (var session = martenFixture.Store.LightweightSession())
            {
                var _store = new PersistedGrantStore(session);
                _store.RemoveAllAsync(persistedGrant.SubjectId, persistedGrant.ClientId,persistedGrant.Type).Wait();
                var grants = session.Query<PersistedGrant>().Where(x => x.SubjectId == persistedGrant.SubjectId && x.ClientId == persistedGrant.ClientId && x.Type == persistedGrant.Type).ToList();
                Assert.True(grants.Count == 0);
            }
        }
        [Fact]
        public void RemoveAsync_ShouldRemoveByKey()
        {
            var persistedGrant = GenFu.GenFu.New<PersistedGrant>();
            using (var session = martenFixture.Store.LightweightSession())
            {
                session.Store(persistedGrant);
                session.SaveChanges();
            }
            using (var session = martenFixture.Store.LightweightSession())
            {
                var _store = new PersistedGrantStore(session);
                _store.RemoveAsync(persistedGrant.Key).Wait();
                var grant = session.Query<PersistedGrant>().FirstOrDefault(x => x.Key == persistedGrant.Key);
                Assert.Null(grant);
            }
        }
    }
}
