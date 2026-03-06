using Application.Common.Helpers;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.DTOs
{
    public class RegistroAuditoriaDTO : IMapFrom<AuditoriaRegistro>
    {
        public Guid Id { get; set; } 
        public string Entidade { get; set; } 
        public EnumDTO Acao { get; set; } 
        public string ChavePrimaria { get; set; }
        public DateTime DataHora { get; set; } 
        public Guid? UsuarioId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AuditoriaRegistro, RegistroAuditoriaDTO>()
                .ForMember(ra => ra.Acao, opt => opt.MapFrom(src => src.Acao.ToEnumDto()))
                ;
        }

    }
}
