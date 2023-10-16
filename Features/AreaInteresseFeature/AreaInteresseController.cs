using MediatR;
using Microsoft.AspNetCore.Mvc;
using ms_usuario.Extensions;
using ms_usuario.Features.AreaInteresseFeature.Commands;
using ms_usuario.Features.AreaInteresseFeature.Queries;

namespace ms_usuario.Features.AreaInteresseFeature
{
    [ApiController]
    [Route("api/[controller]")]
    public class AreaInteresseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AreaInteresseController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("inserir")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(InserirAreaInteresseCommand request)
        {
            return await this.SendAsync(_mediator, request);
        }

        [HttpPut("atualizar")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Put(AtualizarAreaInteresseCommand request)
        {
            return await this.SendAsync(_mediator, request);
        }

        [HttpDelete("excluir/{areaInteresseId}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Delete(long areaInteresseId)
        {
            return await this.SendAsync(_mediator, new RemoverAreaInteresseCommand() { Id = areaInteresseId });
        }

        [HttpGet("selecionar-area-interesse/{areaInteresseId}")]
        public async Task<ActionResult> GetForum(long areaInteresseId)
        {
            return await this.SendAsync(_mediator, new SelecionarAreaInteresseByIdQuery() { Id = areaInteresseId });
        }

        [HttpGet("selecionar-area-interesse")]
        public async Task<ActionResult> Get()
        {
            return await this.SendAsync(_mediator, new SelecionarAreaInteresseFiltersQuery());
        }
    }
}
