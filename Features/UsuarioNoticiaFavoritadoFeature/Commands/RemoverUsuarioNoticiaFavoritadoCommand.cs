using MediatR;
using ms_usuario.Domains;
using ms_usuario.Features.NoticiaFeature.Commands;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.UsuarioNoticiaFavoritadoFeature.Commands
{
    public class RemoverUsuarioNoticiaFavoritadoCommand : IRequest<long>
    {
        public long Id { get; set; }
        public long NoticiaId { get; set; }
    }

    public class RemoverUsuarioNoticiaFavoritadoCommandHandler
        : IRequestHandler<RemoverUsuarioNoticiaFavoritadoCommand, long>
    {
        private IMediator _mediator;
        private readonly IRepository<UsuarioNoticiaFavoritado> _repository;

        public RemoverUsuarioNoticiaFavoritadoCommandHandler
        (
            IMediator mediator,
            IRepository<UsuarioNoticiaFavoritado> repository
        )
        {
            _mediator = mediator;
            _repository = repository;
        }

        public async Task<long> Handle
        (
            RemoverUsuarioNoticiaFavoritadoCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<RemoverUsuarioNoticiaFavoritadoCommand>());

            await Validator(request, cancellationToken);

            UsuarioNoticiaFavoritado usuarioNoticiaFavoritado = await _repository.GetFirstAsync(item => item.Id.Equals(request.Id), cancellationToken);

            await _mediator.Send(new AtualizarNoticiaFavoritadoCommand { Id = request.NoticiaId, Adicionar = false });

            await _repository.RemoveAsync(usuarioNoticiaFavoritado);
            await _repository.SaveChangesAsync(cancellationToken);

            return usuarioNoticiaFavoritado.Id;
        }

        private async Task Validator
        (
            RemoverUsuarioNoticiaFavoritadoCommand request,
            CancellationToken cancellationToken
        )
        {
            if (!await ExistsAsync(request, cancellationToken)) throw new ArgumentNullException("Noticia favoritada não encontrada");
        }

        private async Task<bool> ExistsAsync
        (
            RemoverUsuarioNoticiaFavoritadoCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repository.ExistsAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken
                );
        }
    }
}
