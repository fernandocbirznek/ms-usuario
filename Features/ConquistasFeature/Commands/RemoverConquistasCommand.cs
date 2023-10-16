using MediatR;
using ms_usuario.Domains;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.ConquistasFeature.Commands
{
    public class RemoverConquistasCommand : IRequest<RemoverConquistasCommandResponse>
    {
        public long Id { get; set; }
    }

    public class RemoverConquistasCommandResponse
    {
        public long Id { get; set; }
    }

    public class RemoverConquistasCommandHandler : IRequestHandler<RemoverConquistasCommand, RemoverConquistasCommandResponse>
    {
        private readonly IRepository<Conquistas> _repository;

        public RemoverConquistasCommandHandler
        (
            IRepository<Conquistas> repository
        )
        {
            _repository = repository;
        }

        public async Task<RemoverConquistasCommandResponse> Handle
        (
            RemoverConquistasCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<RemoverConquistasCommand>());

            await Validator(request, cancellationToken);

            Conquistas conquistas = await _repository.GetFirstAsync(item => item.Id.Equals(request.Id), cancellationToken);

            await _repository.RemoveAsync(conquistas);
            await _repository.SaveChangesAsync(cancellationToken);

            RemoverConquistasCommandResponse response = new RemoverConquistasCommandResponse();
            response.Id = conquistas.Id;

            return response;
        }

        private async Task Validator
        (
            RemoverConquistasCommand request,
            CancellationToken cancellationToken
        )
        {
            if (!(await ExistsAsync(request, cancellationToken))) throw new ArgumentNullException("Conquista não encontrada");
        }

        private async Task<bool> ExistsAsync
        (
            RemoverConquistasCommand request,
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
