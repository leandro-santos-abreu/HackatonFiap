using AutoMapper;
using HealthMed.Data.DTO;
using HealthMed.Domain.Entity;

namespace HealthMed.Application.Profiles;
public class AgendaProfile : Profile
{
    public AgendaProfile()
    {
        CreateMap<CreateAgendaDTO, AgendaEntity>().ReverseMap();
        CreateMap<UpdateAgendaDTO, AgendaEntity>();
        CreateMap<AgendaEntity, UpdateAgendaDTO>();
        CreateMap<AgendaEntity, ReadAgendaDTO>();
    }
}
