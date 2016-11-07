using IdentityServer4.Models;
using IdentityServer4.Postgresql.Mappers;
using Xunit;

namespace IdentityServer4.Postgresql.UnitTests
{
    public class ScopesMapperTests
    {
        [Fact]
        public void ScopeAutomapperConfigurationIsValid()
        {
            var model = new Scope();
            var mappedEntity = model.ToEntity();
            var mappedModel = mappedEntity.ToModel();

            Assert.NotNull(mappedModel);
            Assert.NotNull(mappedEntity);
            ScopeMappers.Mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
