using Application.Common.Models;
using MediatR;

namespace Application.Common.Wrappers
{
    public interface IHandlerWrapper<in TRequest, TResponse> : IRequestHandler<TRequest, Resposta<TResponse>>
        where TRequest : IRequestWrapper<TResponse>
    {
    }

    public interface IHandlerWrapper<in TRequest> : IRequestHandler<TRequest, Resposta>
        where TRequest : IRequestWrapper
    {
    }
}
