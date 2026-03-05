using Application.Common.Helpers;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.DTOs
{
    public class RegistroAuditoriaDTO : IMapFrom<RegistroAuditoria>
    {
        public Guid Id { get; set; } 
        public string Entidade { get; set; } 
        public EnumDTO Acao { get; set; } 
        public string ChavePrimaria { get; set; }
        public DateTime DataHora { get; set; } 
        public Guid? UsuarioId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RegistroAuditoria, RegistroAuditoriaDTO>()
                .ForMember(ra => ra.Acao, opt => opt.MapFrom(src => src.Acao.ToEnumDto()))
                ;
        }

    }
}
