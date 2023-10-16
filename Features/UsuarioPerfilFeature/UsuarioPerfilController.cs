using MediatR;
using Microsoft.AspNetCore.Mvc;
using ms_usuario.Extensions;
using ms_usuario.Features.UsuarioPerfilFeature.Commands;
using ms_usuario.Features.UsuarioPerfilFeature.Queries;

namespace ms_usuario.Features.UsuarioPerfilFeature
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioPerfilController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsuarioPerfilController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("inserir")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(InserirUsuarioPerfilCommand request)
        {
            return await this.SendAsync(_mediator, request);
        }

        [HttpPut("atualizar")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Put(AtualizarUsuarioPerfilCommand request)
        {
            return await this.SendAsync(_mediator, request);
        }

        [HttpDelete("excluir/{usuarioPerfilId}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Delete(long usuarioPerfilId)
        {
            return await this.SendAsync(_mediator, new RemoverUsuarioPerfilCommand() { Id = usuarioPerfilId });
        }

        [HttpGet("selecionar-perfil-usuario/{usuarioId}")]
        public async Task<ActionResult> GetForum(long usuarioId)
        {
            return await this.SendAsync(_mediator, new SelecionarUsuarioPerfilByUsuarioIdQuery() { Id = usuarioId });
        }
    }
}
