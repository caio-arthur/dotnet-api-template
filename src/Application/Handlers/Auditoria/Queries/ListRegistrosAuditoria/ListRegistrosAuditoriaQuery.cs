using Application.Common.DTOs;
using Application.Common.Interfaces;
using Application.Common.Mapping;
using Application.Common.Models;
using Application.Common.Wrappers;
using AutoMapper;
using Domain.Entities;
using Gridify;

namespace Application.Handlers.Auditoria.Queries.ListRegistrosAuditoria
{
    public class ListRegistrosAuditoriaQuery : GridifyQuery, IRequestWrapper<PaginatedList<RegistroAuditoriaDTO>>
    {        
        
    }

    public class ListRegistrosAuditoriaQueryHandler : IHandlerWrapper<ListRegistrosAuditoriaQuery, PaginatedList<RegistroAuditoriaDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public ListRegistrosAuditoriaQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Response<PaginatedList<RegistroAuditoriaDTO>>> Handle(ListRegistrosAuditoriaQuery request, CancellationToken cancellationToken)
        {
            var mapper = new GridifyMapper<RegistroAuditoria>()
                .GenerateMappings();

            var gridifyQueryable = _context.RegistrosAuditoria
               .GridifyQueryable(request, mapper);

            var paginatedList = await gridifyQueryable.ProjectToPaginatedListAsync<RegistroAuditoria, RegistroAuditoriaDTO>(
                _mapper.ConfigurationProvider,
                request.Page,
                request.PageSize,
                cancellationToken);
            return Response.Success(paginatedList);
        }
    }
}
