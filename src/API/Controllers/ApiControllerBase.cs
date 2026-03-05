using Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiControllerBase : ControllerBase
    {
        private ISender _mediator;
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();

        protected ActionResult HandleResult(Resposta result)
        {
            if (result.Sucesso)
            {
                return Ok(result);
            }

            var statusCode = result.Erro?.Codigo ?? 500;
            return StatusCode(statusCode, result);
        }

        protected ActionResult HandleResult<T>(Resposta<T> result)
        {
            if (result.Sucesso)
            {
                return Ok(result);
            }

            var statusCode = result.Erro?.Codigo ?? 500;
            return StatusCode(statusCode, result);
        }
    }
}
