using MediatR;
using Microsoft.AspNetCore.Mvc;
using ms_usuario.Extensions;
using ms_usuario.Features.UsuarioConquistasFeature.Commands;
using ms_usuario.Features.UsuarioConquistasFeature.Queries;

namespace ms_usuario.Features.UsuarioConquistasFeature
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioConquistasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsuarioConquistasController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("inserir")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(InserirUsuarioConquistasCommand request)
        {
            return await this.SendAsync(_mediator, request);
        }

        [HttpDelete("excluir/{usuarioConquistaId}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Delete(long usuarioConquistaId)
        {
            return await this.SendAsync(_mediator, new RemoverUsuarioConquistaCommand() { Id = usuarioConquistaId });
        }

        [HttpGet("selecionar-conquistas-usuario/{usuarioId}")]
        public async Task<ActionResult> GetForum(long usuarioId)
        {
            return await this.SendAsync(_mediator, new SelecionarUsuarioConquistasByUsuarioIdQuery() { Id = usuarioId });
        }

    }
}
