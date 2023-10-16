using MediatR;
using ms_usuario.Domains;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.AreaInteresseFeature.Commands
{
    public class RemoverAreaInteresseCommand : IRequest<RemoverAreaInteresseCommandResponse>
    {
        public long Id { get; set; }
    }

    public class RemoverAreaInteresseCommandResponse
    {
        public long Id { get; set; }
    }

    public class RemoverAreaInteresseCommandHandler : IRequestHandler<RemoverAreaInteresseCommand, RemoverAreaInteresseCommandResponse>
    {
        private readonly IRepository<AreaInteresse> _repository;

        public RemoverAreaInteresseCommandHandler
        (
            IRepository<AreaInteresse> repository
        )
        {
            _repository = repository;
        }

        public async Task<RemoverAreaInteresseCommandResponse> Handle
        (
            RemoverAreaInteresseCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<RemoverAreaInteresseCommand>());

            await Validator(request, cancellationToken);

            AreaInteresse areaInteresse = await _repository.GetFirstAsync(item => item.Id.Equals(request.Id), cancellationToken);

            await _repository.RemoveAsync(areaInteresse);
            await _repository.SaveChangesAsync(cancellationToken);

            RemoverAreaInteresseCommandResponse response = new RemoverAreaInteresseCommandResponse();
            response.Id = areaInteresse.Id;

            return response;
        }

        private async Task Validator
        (
            RemoverAreaInteresseCommand request,
            CancellationToken cancellationToken
        )
        {
            if (!(await ExistsAsync(request, cancellationToken))) throw new ArgumentNullException("Área de interesse não encontrada");
        }

        private async Task<bool> ExistsAsync
        (
            RemoverAreaInteresseCommand request,
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
