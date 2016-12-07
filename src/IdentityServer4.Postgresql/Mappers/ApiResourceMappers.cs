using AutoMapper;

namespace IdentityServer4.Postgresql.Mappers
{

    public static class ApiResourceMappers
    {
        static ApiResourceMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ApiResourceMapperProfile>())
                .CreateMapper();
        }
        public static IMapper Mapper { get; }

        public static Models.ApiResource ToModel(this Entities.ApiResource resource)
        {
            return resource == null ? null : Mapper.Map<Models.ApiResource>(resource);
        }

        public static Entities.ApiResource ToEntity(this Models.ApiResource resource)
        {
            return resource == null ? null : Mapper.Map<Entities.ApiResource>(resource);
        }
    }
}
