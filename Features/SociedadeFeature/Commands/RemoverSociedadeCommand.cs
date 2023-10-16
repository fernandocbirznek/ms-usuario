using MediatR;
using ms_usuario.Domains;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.SociedadeFeature.Commands
{
    public class RemoverSociedadeCommand : IRequest<RemoverSociedadeCommandResponse>
    {
        public long Id { get; set; }
    }

    public class RemoverSociedadeCommandResponse
    {
        public long Id { get; set; }
    }

    public class RemoverSociedadeCommandHandler : IRequestHandler<RemoverSociedadeCommand, RemoverSociedadeCommandResponse>
    {
        private readonly IRepository<Sociedade> _repository;

        public RemoverSociedadeCommandHandler
        (
            IRepository<Sociedade> repository
        )
        {
            _repository = repository;
        }

        public async Task<RemoverSociedadeCommandResponse> Handle
        (
            RemoverSociedadeCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<RemoverSociedadeCommand>());

            await Validator(request, cancellationToken);

            Sociedade sociedade = await _repository.GetFirstAsync(item => item.Id.Equals(request.Id), cancellationToken);

            await _repository.RemoveAsync(sociedade);
            await _repository.SaveChangesAsync(cancellationToken);

            RemoverSociedadeCommandResponse response = new RemoverSociedadeCommandResponse();
            response.Id = sociedade.Id;

            return response;
        }

        private async Task Validator
        (
            RemoverSociedadeCommand request,
            CancellationToken cancellationToken
        )
        {
            if (!(await ExistsAsync(request, cancellationToken))) throw new ArgumentNullException("Sociedade não encontrada");
        }

        private async Task<bool> ExistsAsync
        (
            RemoverSociedadeCommand request,
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
