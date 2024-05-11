using MediatR;
using ms_usuario.Domains;
using ms_usuario.Extensions;
using ms_usuario.Features.NoticiaFeature.Commands;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.UsuarioNoticiaFeature.Commands
{
    public class InserirUsuarioNoticiaFavoritadoCommand : IRequest<InserirUsuarioNoticiaFavoritadoCommandResponse>
    {
        public long UsuarioId { get; set; }
        public long NoticiaId { get; set; }
    }

    public class InserirUsuarioNoticiaFavoritadoCommandResponse
    {
        public long Id { get; set; }
        public long UsuarioId { get; set; }
        public long NoticiaId { get; set; }
        public DateTime DataCadastro { get; set; }
    }

    public class InserirUsuarioNoticiaFavoritadoHandler :
        IRequestHandler<InserirUsuarioNoticiaFavoritadoCommand, InserirUsuarioNoticiaFavoritadoCommandResponse>
    {
        private IMediator _mediator;

        private readonly IRepository<UsuarioNoticiaFavoritado> _repository;
        private readonly IRepository<Noticia> _repositoryNoticia;

        public InserirUsuarioNoticiaFavoritadoHandler
        (
            IMediator mediator,

            IRepository<UsuarioNoticiaFavoritado> repository,
            IRepository<Noticia> repositoryNoticia
        )
        {
            _mediator = mediator;

            _repository = repository;
            _repositoryNoticia = repositoryNoticia;
        }

        public async Task<InserirUsuarioNoticiaFavoritadoCommandResponse> Handle
        (
            InserirUsuarioNoticiaFavoritadoCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<InserirUsuarioNoticiaFavoritadoCommand>());

            await Validator(request, cancellationToken);

            UsuarioNoticiaFavoritado usuarioNoticiaFavoritado = request.ToDomain();

            await _mediator.Send(new AtualizarNoticiaFavoritadoCommand { Id = request.NoticiaId, Adicionar = true });

            await _repository.AddAsync(usuarioNoticiaFavoritado, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            InserirUsuarioNoticiaFavoritadoCommandResponse response = new InserirUsuarioNoticiaFavoritadoCommandResponse();
            response.DataCadastro = usuarioNoticiaFavoritado.DataCadastro;
            response.Id = usuarioNoticiaFavoritado.Id;
            response.UsuarioId = request.UsuarioId;
            response.NoticiaId = request.NoticiaId;

            return response;
        }

        private async Task Validator
        (
            InserirUsuarioNoticiaFavoritadoCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request.NoticiaId <= 0) throw new ArgumentNullException(MessageHelper.NullFor<InserirUsuarioNoticiaFavoritadoCommand>(item => item.NoticiaId));
            if (request.UsuarioId <= 0) throw new ArgumentNullException(MessageHelper.NullFor<InserirUsuarioNoticiaFavoritadoCommand>(item => item.UsuarioId));
            if (!await ExistsNoticiaAsync(request, cancellationToken)) throw new ArgumentNullException("Noticia não encontrada");
        }

        private async Task<bool> ExistsNoticiaAsync
        (
            InserirUsuarioNoticiaFavoritadoCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repositoryNoticia.ExistsAsync
                (
                    item => item.Id.Equals(request.NoticiaId),
                    cancellationToken
                );
        }
    }
}
