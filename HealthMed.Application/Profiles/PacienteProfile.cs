using AutoMapper;
using HealthMed.Data.DTO;
using HealthMed.Domain.Entity;

namespace HealthMed.Application.Profiles;

public class PacienteProfile : Profile
{
    public PacienteProfile()
    {
           CreateMap<CreatePacienteDTO, PacienteEntity>().ReverseMap();
        CreateMap<UpdatePacienteDTO, PacienteEntity>();
        CreateMap<PacienteEntity, UpdatePacienteDTO>();
    }
}
