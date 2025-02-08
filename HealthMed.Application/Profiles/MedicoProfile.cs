using AutoMapper;
using HealthMed.Data.DTO;
using HealthMed.Domain.Dto;
using HealthMed.Domain.Entity;

namespace HealthMed.Application.Profiles;
public class MedicoProfile : Profile
{
    public MedicoProfile()
    {
        CreateMap<CreateMedicoDTO, MedicoEntity>().ReverseMap();
        CreateMap<UpdateMedicoDTO, MedicoEntity>();
        CreateMap<MedicoEntity, UpdateMedicoDTO>();
        CreateMap<MedicoEntity, ReadMedicoResumoDTO>();
    }
}
