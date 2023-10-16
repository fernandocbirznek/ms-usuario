using MediatR;
using Microsoft.AspNetCore.Mvc;
using ms_usuario.Extensions;
using ms_usuario.Features.ConquistasFeature.Commands;
using ms_usuario.Features.ConquistasFeature.Queries;

namespace ms_usuario.Features.ConquistasFeature
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConquistasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ConquistasController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("inserir")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(InserirConquistasCommand request)
        {
            return await this.SendAsync(_mediator, request);
        }

        [HttpPut("atualizar")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Put(AtualizarConquistasCommand request)
        {
            return await this.SendAsync(_mediator, request);
        }

        [HttpDelete("excluir/{conquistaId}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Delete(long conquistaId)
        {
            return await this.SendAsync(_mediator, new RemoverConquistasCommand() { Id = conquistaId });
        }

        [HttpGet("selecionar-conquista/{conquistaId}")]
        public async Task<ActionResult> GetForum(long conquistaId)
        {
            return await this.SendAsync(_mediator, new SelecionarConquistasByIdQuery() { Id = conquistaId });
        }

        [HttpGet("selecionar-conquistas")]
        public async Task<ActionResult> Get()
        {
            return await this.SendAsync(_mediator, new SelecionarConquistasFiltersQuery());
        }
    }
}
