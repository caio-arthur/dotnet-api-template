using Application.Common.Models;
using MediatR;

namespace Application.Common.Wrappers
{
    public interface IRequestWrapper<TResponse> : IRequest<Resposta<TResponse>>
    {
    }

    public interface IRequestWrapper : IRequest<Resposta>
    {
    }

}
