using AutoMapper;
using IdentityServer4.Models;

namespace IdentityServer4.Postgresql.Mappers
{
    public static class IdentityResourceMappers
    {
        static IdentityResourceMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<IdentityResourceMapperProfile>())
                .CreateMapper();
        }
        public static IMapper Mapper { get; }

        public static IdentityResource ToModel(this Entities.IdentityResource identityResource)
        {
            if (identityResource == null) return null;

            return Mapper.Map<Entities.IdentityResource, IdentityResource>(identityResource);
        }

        public static Entities.IdentityResource ToEntity(this IdentityResource identityResource)
        {
            if (identityResource == null) return null;

            return Mapper.Map<IdentityResource, Entities.IdentityResource>(identityResource);
        }
    }
}
