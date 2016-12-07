using AutoMapper;
using IdentityServer4.Models;

namespace IdentityServer4.Postgresql.Mappers
{
    public static class PersistedGrantMappers
    {
        static PersistedGrantMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<PersistedGrantMapperProfile>())
                 .CreateMapper();
        }
        public static IMapper Mapper { get; }

        public static PersistedGrant ToModel(this Entities.PersistedGrant token)
        {
            if (token == null) return null;

            return Mapper.Map<Entities.PersistedGrant, PersistedGrant>(token);
        }

        public static Entities.PersistedGrant ToEntity(this PersistedGrant token)
        {
            if (token == null) return null;

            return Mapper.Map<PersistedGrant, Entities.PersistedGrant>(token);
        }

        public static void UpdateEntity(this PersistedGrant token, Entities.PersistedGrant target)
        {
            Mapper.Map(token, target);
        }
    }
}
