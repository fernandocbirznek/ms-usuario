using MediatR;
using Microsoft.AspNetCore.Mvc;
using ms_usuario.Extensions;
using ms_usuario.Features.SociedadeFeature.Commands;
using ms_usuario.Features.SociedadeFeature.Queries;

namespace ms_usuario.Features.SociedadeFeature
{
    [ApiController]
    [Route("api/[controller]")]
    public class SociedadeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SociedadeController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("inserir")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(InserirSociedadeCommand request)
        {
            return await this.SendAsync(_mediator, request);
        }

        [HttpPut("atualizar")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Put(AtualizarSociedadeCommand request)
        {
            return await this.SendAsync(_mediator, request);
        }

        [HttpDelete("excluir/{sociedadeId}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Delete(long sociedadeId)
        {
            return await this.SendAsync(_mediator, new RemoverSociedadeCommand() { Id = sociedadeId });
        }

        [HttpGet("selecionar-sociedade/{sociedadeId}")]
        public async Task<ActionResult> GetForum(long sociedadeId)
        {
            return await this.SendAsync(_mediator, new SelecionarSociedadeByIdQuery() { Id = sociedadeId });
        }

        [HttpGet("selecionar-sociedades")]
        public async Task<ActionResult> Get()
        {
            return await this.SendAsync(_mediator, new SelecionarSociedadeFiltersQuery());
        }
    }
}
