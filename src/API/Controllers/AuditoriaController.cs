using Application.Common.DTOs;
using Application.Common.Models;
using Application.Handlers.Auditoria.Commands.RecuperarRegistroExcluido;
using Application.Handlers.Auditoria.Queries.ListRegistrosAuditoria;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/auditoria")]
    [ApiController]
    public class AuditoriaController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<Resposta<ListaPaginada<RegistroAuditoriaDTO>>>> ListRegistrosAuditoria([FromQuery] ListRegistrosAuditoriaQuery query)
        {
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }

        [HttpPost("{id}/recuperar-registro")]
        public async Task<ActionResult<Resposta<RecuperarRegistroExcluidoResponse>>> RecuperarRegistro(Guid id)
        {
            var command = new RecuperarRegistroExcluidoCommand { Id = id };
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

    }
}
