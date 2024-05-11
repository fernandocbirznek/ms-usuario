using MediatR;
using Microsoft.AspNetCore.Mvc;
using ms_usuario.Extensions;
using ms_usuario.Features.UsuarioNoticiaFavoritadoFeature.Commands;
using ms_usuario.Features.UsuarioNoticiaFavoritadoFeature.Queries;
using ms_usuario.Features.UsuarioNoticiaFeature.Commands;

namespace ms_usuario.Features.UsuarioNoticiaFavoritadoFeature
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioNoticiaFavoritadoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsuarioNoticiaFavoritadoController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("inserir")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(InserirUsuarioNoticiaFavoritadoCommand request)
        {
            return await this.SendAsync(_mediator, request);
        }

        [HttpDelete("excluir/{usuarioNoticiaFavoritadoId}/noticia/{noticiaId}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Delete(long usuarioNoticiaFavoritadoId, long noticiaId)
        {
            return await this.SendAsync(_mediator, new RemoverUsuarioNoticiaFavoritadoCommand() { Id = usuarioNoticiaFavoritadoId, NoticiaId = noticiaId });
        }

        [HttpGet("selecionar-usuario-noticia-favoritado/{usuarioId}")]
        public async Task<ActionResult> Get(long usuarioId)
        {
            return await this.SendAsync(_mediator, new SelecionarUsuarioNoticiaFavoritadaByUsuarioIdQuery() { Id = usuarioId });
        }
    }
}
