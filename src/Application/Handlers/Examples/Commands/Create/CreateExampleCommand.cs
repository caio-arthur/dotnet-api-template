//using Application.Common.Interfaces;
//using Application.Common.Models;
//using Application.Common.Wrappers;

//namespace Application.Handlers.Examples.Commands.Create
//{
//    public class CreateExampleCommand : IRequestWrapper
//    {

//    }

//    public class CreateExampleCommandHandler : IHandlerWrapper<CreateExampleCommand>
//    {
//        private readonly IApplicationDbContext _context;

//        public CreateExampleCommandHandler(IApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<Response> Handle(CreateExampleCommand request, CancellationToken cancellationToken)
//        {
//            var entity = new Example()
//            {
//                Id = Guid.NewGuid()
//            };

//            await _context.Examples.AddAsync(entity, cancellationToken);
//            await _context.SaveChangesAsync(cancellationToken);

//            return Response.Success();  
//        }
//    }
//}
