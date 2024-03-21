using MediatR;
using ms_usuario.Domains;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.NoticiaFeature.Commands
{
    public class RemoverNoticiaCommand : IRequest<RemoverNoticiaCommandResponse>
    {
        public long Id { get; set; }
    }

    public class RemoverNoticiaCommandResponse
    {
        public long Id { get; set; }
    }

    public class RemoverNoticiaCommandHandler : IRequestHandler<RemoverNoticiaCommand, RemoverNoticiaCommandResponse>
    {
        private readonly IRepository<Noticia> _repository;

        public RemoverNoticiaCommandHandler
        (
            IRepository<Noticia> repository
        )
        {
            _repository = repository;
        }

        public async Task<RemoverNoticiaCommandResponse> Handle
        (
            RemoverNoticiaCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<RemoverNoticiaCommand>());

            await Validator(request, cancellationToken);

            Noticia noticia = await _repository.GetFirstAsync(item => item.Id.Equals(request.Id), cancellationToken);

            await _repository.RemoveAsync(noticia);
            await _repository.SaveChangesAsync(cancellationToken);

            RemoverNoticiaCommandResponse response = new RemoverNoticiaCommandResponse();
            response.Id = noticia.Id;

            return response;
        }

        private async Task Validator
        (
            RemoverNoticiaCommand request,
            CancellationToken cancellationToken
        )
        {
            if (!(await ExistsAsync(request, cancellationToken))) throw new ArgumentNullException("Noticia não encontrada");
        }

        private async Task<bool> ExistsAsync
        (
            RemoverNoticiaCommand request,
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
