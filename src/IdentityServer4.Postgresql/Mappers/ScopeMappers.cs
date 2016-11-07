using AutoMapper;
using IdentityServer4.Models;

namespace IdentityServer4.Postgresql.Mappers
{
    public static class ScopeMappers
    {
        static ScopeMappers()
        {
            Mapper = new MapperConfiguration(c => c.CreateMap<Entities.Scope, Scope>().ReverseMap().ForMember( y => y.Id ,x => x.Ignore())).CreateMapper();
        }
        public static IMapper Mapper { get; }

        public static Scope ToModel(this Entities.Scope scope)
        {
            if (scope == null) return null;

            return Mapper.Map<Entities.Scope, Scope>(scope);
        }

        public static Entities.Scope ToEntity(this Scope scope)
        {
            if (scope == null) return null;

            return Mapper.Map<Scope, Entities.Scope>(scope);
        }
    }
}
