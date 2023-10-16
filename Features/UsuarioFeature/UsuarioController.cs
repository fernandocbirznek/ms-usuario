using MediatR;
using Microsoft.AspNetCore.Mvc;
using ms_usuario.Extensions;
using ms_usuario.Features.UsuarioFeature.Commands;
using ms_usuario.Features.UsuarioFeature.Queries;

namespace ms_usuario.Features.UsuarioFeature
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsuarioController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("inserir")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(InserirUsuarioCommand request)
        {
            return await this.SendAsync(_mediator, request);
        }

        [HttpPost("login")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(LoginCommand request)
        {
            return await this.SendAsync(_mediator, request);
        }

        [HttpPut("atualizar")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Put(AtualizarUsuarioCommand request)
        {
            return await this.SendAsync(_mediator, request);
        }

        [HttpDelete("excluir/{usuarioId}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Delete(long usuarioId)
        {
            return await this.SendAsync(_mediator, new RemoverUsuarioCommand() { Id = usuarioId });
        }

        [HttpGet("selecionar-usuario/{usuarioId}")]
        public async Task<ActionResult> GetUsuario(long usuarioId)
        {
            return await this.SendAsync(_mediator, new SelecionarUsuarioByIdQuery() { Id = usuarioId });
        }

        [HttpGet("selecionar-usuarios-sociedade/{sociedadeId}")]
        public async Task<ActionResult> GetUsuarioManySociedade(long sociedadeId)
        {
            return await this.SendAsync(_mediator, new SelecionarUsuarioBySociedadeIdQuery() { Id = sociedadeId });
        }

        [HttpGet("selecionar-usuarios")]
        public async Task<ActionResult> Get()
        {
            return await this.SendAsync(_mediator, new SelecionarUsuarioFiltersQuery());
        }
    }
}
