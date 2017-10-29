using System.Collections.Generic;
using IdentityServer4.Postgresql.Entities;
using Xunit;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using IdentityServer4.Postgresql.Services;
using System.Threading.Tasks;

namespace IdentityServer4.Postgresql.IntegrationTests
{
	public class CorsPolicyServiceTests : IClassFixture<DatabaseFixture>
	{
		private readonly DatabaseFixture martenFixture;
		public CorsPolicyServiceTests(DatabaseFixture fixture)
		{
			martenFixture = fixture;
		}
		[Fact]
		public async Task CorsPolicyTest()
		{
			using (var session = martenFixture.Store.LightweightSession())
			{
				var client = new IdentityServer4.Postgresql.Entities.Client
				{
					ClientId = "test",
					ClientName = "testclient",
					AllowedCorsOrigins = new List<ClientCorsOrigin>{
						new ClientCorsOrigin{ Origin = "http://localhost:123/callback"},
						new ClientCorsOrigin{ Origin = "http://localhost:1234/callback"},
						new ClientCorsOrigin{ Origin = "http://localhost:12345/callback"},
					}
				};
				session.Store(client);
				session.SaveChanges();
				var logger = A.Fake<ILogger<CorsPolicyService>>();
				var sut = new IdentityServer4.Postgresql.Services.CorsPolicyService(logger, session);
				var result = await sut.IsOriginAllowedAsync("http://localhost:123/callback");
				Assert.True(result);

			}
		}
	}
}