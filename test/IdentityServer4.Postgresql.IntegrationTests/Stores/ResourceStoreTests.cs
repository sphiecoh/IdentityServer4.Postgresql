using GenFu;
using IdentityServer4.Postgresql.Entities;
using IdentityServer4.Postgresql.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace IdentityServer4.Postgresql.IntegrationTests.Stores
{
    public class ResourceStoreTests : IClassFixture<DatabaseFixture>
    {
        private DatabaseFixture db;
        public ResourceStoreTests(DatabaseFixture fixture)
        {
            db = fixture;
        }

        [Fact]
        public void FindApiResourceAsync_ShoudReturnSavedResource()
        {
            var apiResource = A.New<ApiResource>();
            using (var session = db.Store.LightweightSession())
            {
                session.Store(apiResource);
                session.SaveChanges();
            }
            using (var session = db.Store.LightweightSession())
            {
                var store = new ResourceStore(session);
               var foundresource = store.FindApiResourceAsync(apiResource.Name).Result;
                Assert.NotNull(foundresource);
                Assert.Equal(apiResource.Name, foundresource.Name);
            }
        }
        [Fact]
        public void FindApiResourcesByScopeAsync_ShouldFindByScopes()
        {
            A.Configure<ApiResource>().Fill(x => x.UserClaims, A.ListOf<ApiResourceClaim>(2));
            A.Configure<ApiResource>().Fill(x => x.Scopes, A.ListOf<ApiScope>());
           
           
            var apiResources = A.ListOf<ApiResource>(5);

            using (var session = db.Store.LightweightSession())
            {
                session.StoreObjects(apiResources);
                session.SaveChanges();
            }
            using (var session = db.Store.LightweightSession())
            {
                var store = new ResourceStore(session);
                var _scopes = apiResources.Select(y => y.Name);
                var result = store.FindApiResourcesByScopeAsync(_scopes).Result;
                Assert.True(apiResources.Count == result.Count());
            }
        }

        [Fact]
       
        public void FindIdentityResourcesByScopeAsync_ShouldFindByScopes()
        {
            A.Configure<IdentityResource>().Fill(x => x.UserClaims, A.ListOf<IdentityClaim>(3));
            var resources = A.ListOf<IdentityResource>();
            using (var session = db.Store.LightweightSession())
            {
                session.StoreObjects(resources);
                session.SaveChanges();
            }
            using (var session = db.Store.LightweightSession())
            {
                var store = new ResourceStore(session);
                var scopes = resources.Select(x => x.Name);
                var result = store.FindIdentityResourcesByScopeAsync(scopes).Result;
                Assert.True(resources.Count == result.Count());
            }
        }

        [Fact]
        public void GetAllResources_ShouldReturnAllResources()
        {

            var claims = A.ListOf<ApiResourceClaim>(2);
            var scopes = A.ListOf<ApiScope>();
            A.Configure<ApiResource>().Fill(x => x.UserClaims, claims);
            A.Configure<ApiResource>().Fill(x => x.Scopes, scopes);
            A.Configure<IdentityResource>().Fill(x => x.UserClaims, A.ListOf<IdentityClaim>(3));
            var resources = A.ListOf<IdentityResource>(5);
            var apiResources = A.ListOf<ApiResource>(5);


            using (var session = db.Store.LightweightSession())
            {
                session.StoreObjects(resources);
                session.StoreObjects(apiResources);
                session.SaveChanges();
            }
            using (var session = db.Store.LightweightSession())
            {
                var store = new ResourceStore(session);
                var result = store.GetAllResources().Result;

                Assert.True(apiResources.Count <= result.ApiResources.Count);
                Assert.True(resources.Count <= result.IdentityResources.Count);


            }
        }
    }
}
