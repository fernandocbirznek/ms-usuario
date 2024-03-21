using MediatR;
using ms_usuario.Domains;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.NoticiaAreaInteresseFeature.Commands
{
    public class RemoverNoticiaAreaInteresseCommand : IRequest<RemoverNoticiaAreaInteresseCommandResponse>
    {
        public long Id { get; set; }
    }

    public class RemoverNoticiaAreaInteresseCommandResponse
    {
        public long Id { get; set; }
    }

    public class RemoverNoticiaAreaInteresseCommandHandler :
        IRequestHandler<RemoverNoticiaAreaInteresseCommand, RemoverNoticiaAreaInteresseCommandResponse>
    {
        private readonly IRepository<NoticiaAreaInteresse> _repository;

        public RemoverNoticiaAreaInteresseCommandHandler
        (
            IRepository<NoticiaAreaInteresse> repository
        )
        {
            _repository = repository;
        }

        public async Task<RemoverNoticiaAreaInteresseCommandResponse> Handle
        (
            RemoverNoticiaAreaInteresseCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<RemoverNoticiaAreaInteresseCommand>());

            await Validator(request, cancellationToken);

            NoticiaAreaInteresse noticiaAreaInteresse = await _repository.GetFirstAsync(item => item.Id.Equals(request.Id), cancellationToken);

            await _repository.RemoveAsync(noticiaAreaInteresse);
            await _repository.SaveChangesAsync(cancellationToken);

            RemoverNoticiaAreaInteresseCommandResponse response = new RemoverNoticiaAreaInteresseCommandResponse();
            response.Id = noticiaAreaInteresse.Id;

            return response;
        }

        private async Task Validator
        (
            RemoverNoticiaAreaInteresseCommand request,
            CancellationToken cancellationToken
        )
        {
            if (!(await ExistsAsync(request, cancellationToken))) throw new ArgumentNullException("NoticiaAreaInteresse não encontrada");
        }

        private async Task<bool> ExistsAsync
        (
            RemoverNoticiaAreaInteresseCommand request,
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
