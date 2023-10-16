using MediatR;
using Microsoft.AspNetCore.Mvc;
using ms_usuario.Extensions;
using ms_usuario.Features.UsuarioAreaInteresseFeature.Commands;
using ms_usuario.Features.UsuarioAreaInteresseFeature.Queries;

namespace ms_usuario.Features.UsuarioAreaInteresseFeature
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioAreaInteresseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsuarioAreaInteresseController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("inserir")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(InserirUsuarioAreaInteresseCommand request)
        {
            return await this.SendAsync(_mediator, request);
        }

        [HttpDelete("excluir/{usuarioAreaInteresseId}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Delete(long usuarioAreaInteresseId)
        {
            return await this.SendAsync(_mediator, new RemoverUsuarioAreaInteresseCommand() { Id = usuarioAreaInteresseId });
        }

        [HttpGet("selecionar-areas-interesse-usuario/{usuarioId}")]
        public async Task<ActionResult> GetForum(long usuarioId)
        {
            return await this.SendAsync(_mediator, new SelecionarUsuarioAreaInteresseByUsuarioIdQuery() { Id = usuarioId });
        }

    }
}
