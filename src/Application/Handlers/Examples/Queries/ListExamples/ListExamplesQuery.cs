//using Application.Common.DTOs;
//using Application.Common.Interfaces;
//using Application.Common.Mapping;
//using Application.Common.Models;
//using Application.Common.Wrappers;
//using AutoMapper;
//using Domain.Entities;
//using Gridify;

//namespace Application.Handlers.Examples.Queries
//{
//    public class ListExampleQuery : GridifyQuery, IRequestWrapper<PaginatedList<ExampleDTO>>
//    {

//    }

//    public class ListExampleQueryHandler : IHandlerWrapper<ListExampleQuery, PaginatedList<ExampleDTO>>
//    {
//        private readonly IApplicationDbContext _context;
//        private readonly IMapper _mapper;

//        public ListExampleQueryHandler(IApplicationDbContext context, IMapper mapper)
//        {
//            _context = context;
//            _mapper = mapper;
//        }

//        public async Task<Response<PaginatedList<ExampleDTO>>> Handle(ListExampleQuery request, CancellationToken cancellationToken)
//        {
//            var mapper = new GridifyMapper<Example>()
//                .GenerateMappings();
//                .AddMap("ContatoNome", a => a.Contato.Nome)
//                .AddMap("CorretorNome", a => a.Corretor.Nome)
//                .AddMap("LoteamentoNome", a => a.Loteamento.Nome)
//                .AddMap("LoteadoraId", a => a.Loteamento.Empresas
//                                                .Where(el => el.Principal)
//                                                .Select(el => el.EmpresaId));

//            var gridifyQueryable = _context.Examples
//               .Where(x => x.Status != Status.Indisponivel)
//               .GridifyQueryable(request, mapper);

//            var paginatedList = await gridifyQueryable.ProjectToPaginatedListAsync<Example, ExampleDTO>(
//                _mapper.ConfigurationProvider,
//                request.Page,
//                request.PageSize,
//                cancellationToken);

//            return Response.Success(paginatedList);
//        }
//    }

//}
