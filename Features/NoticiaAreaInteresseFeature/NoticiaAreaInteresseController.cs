using MediatR;
using Microsoft.AspNetCore.Mvc;
using ms_usuario.Domains;
using ms_usuario.Extensions;
using ms_usuario.Features.NoticiaAreaInteresseFeature.Commands;
using ms_usuario.Features.NoticiaAreaInteresseFeature.Queries;

namespace ms_usuario.Features.NoticiaAreaInteresseFeature
{
    [ApiController]
    [Route("api/[controller]")]
    public class NoticiaAreaInteresseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NoticiaAreaInteresseController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("inserir")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(InserirNoticiaAreaInteresseCommand request)
        {
            return await this.SendAsync(_mediator, request);
        }

        [HttpDelete("excluir/{noticiaAreaInteresseId}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Delete(long noticiaAreaInteresseId)
        {
            return await this.SendAsync(_mediator, new RemoverNoticiaAreaInteresseCommand() { Id = noticiaAreaInteresseId });
        }

        [HttpGet("selecionar-noticia-area-interesse")]
        public async Task<ActionResult> Get()
        {
            return await this.SendAsync(_mediator, new SelecionarNoticiaAreaInteresseFiltersQuery());
        }
    }
}
