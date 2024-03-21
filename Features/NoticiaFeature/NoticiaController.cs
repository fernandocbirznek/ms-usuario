using MediatR;
using Microsoft.AspNetCore.Mvc;
using ms_usuario.Extensions;
using ms_usuario.Features.NoticiaFeature.Commands;
using ms_usuario.Features.NoticiaFeature.Queries;

namespace ms_usuario.Features.NoticiaFeature
{
    [ApiController]
    [Route("api/[controller]")]
    public class NoticiaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NoticiaController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("inserir")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(InserirNoticiaCommand request)
        {
            return await this.SendAsync(_mediator, request);
        }

        [HttpPut("atualizar")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Put(AtualizarNoticiaCommand request)
        {
            return await this.SendAsync(_mediator, request);
        }

        [HttpDelete("excluir/{noticiaId}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Delete(long noticiaId)
        {
            return await this.SendAsync(_mediator, new RemoverNoticiaCommand() { Id = noticiaId });
        }

        [HttpGet("selecionar-noticia/{noticiaId}")]
        public async Task<ActionResult> GetNoticia(long noticiaId)
        {
            return await this.SendAsync(_mediator, new SelecionarNoticiaByIdQuery() { Id = noticiaId });
        }

        [HttpGet("selecionar-noticia-many/{professorId}")]
        public async Task<ActionResult> GetNoticiaProfessor(long professorId)
        {
            return await this.SendAsync(_mediator, new SelecionarNoticiaManyByProfessorIdQuery() { Id = professorId });
        }

        [HttpGet("selecionar-noticia-many/home")]
        public async Task<ActionResult> GetNoticiaHome()
        {
            return await this.SendAsync(_mediator, new SelecionarNoticiaManyHomeQuery());
        }
    }
}
