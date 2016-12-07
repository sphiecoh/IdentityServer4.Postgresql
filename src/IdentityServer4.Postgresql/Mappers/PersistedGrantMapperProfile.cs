using AutoMapper;

namespace IdentityServer4.Postgresql.Mappers
{
    public class PersistedGrantMapperProfile : Profile
    {
        public PersistedGrantMapperProfile()
        {
            CreateMap<Entities.PersistedGrant, Models.PersistedGrant>(MemberList.Destination);

            // model to entity
            CreateMap<Models.PersistedGrant, Entities.PersistedGrant>(MemberList.Source);
        }
       
       
    }
}
