using MediatR;
using ms_usuario.Domains;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.ConquistasFeature.Commands
{
    public class AtualizarConquistasCommand : IRequest<AtualizarConquistasCommandResponse>
    {
        public long Id { get; set; }
        public string Nome { get; set; }
    }

    public class AtualizarConquistasCommandResponse
    {
        public DateTime DataAtualizacao { get; set; }
    }

    public class AtualizarConquistasHandler : IRequestHandler<AtualizarConquistasCommand, AtualizarConquistasCommandResponse>
    {
        private readonly IRepository<Conquistas> _repository;

        public AtualizarConquistasHandler
        (
            IRepository<Conquistas> repository
        )
        {
            _repository = repository;
        }

        public async Task<AtualizarConquistasCommandResponse> Handle
        (
            AtualizarConquistasCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<AtualizarConquistasCommand>());

            await Validator(request, cancellationToken);

            Conquistas conquistas = await GetFirstAsync(request, cancellationToken);
            conquistas.Nome = request.Nome;

            await _repository.UpdateAsync(conquistas);
            await _repository.SaveChangesAsync(cancellationToken);

            AtualizarConquistasCommandResponse response = new AtualizarConquistasCommandResponse();
            response.DataAtualizacao = conquistas.DataAtualizacao;

            return response;
        }

        private async Task Validator
        (
            AtualizarConquistasCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request.Id <= 0) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarConquistasCommand>(item => item.Id));
            if (String.IsNullOrEmpty(request.Nome)) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarConquistasCommand>(item => item.Nome));
            if (!(await ExistsAsync(request, cancellationToken))) throw new ArgumentNullException("Conquista não encontrada");
            if (await ExistsNomeAsync(request, cancellationToken)) throw new ArgumentNullException("Conquistae já cadastrada");
        }

        private async Task<Conquistas> GetFirstAsync
        (
            AtualizarConquistasCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repository.GetFirstAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken
                );
        }

        private async Task<bool> ExistsAsync
        (
            AtualizarConquistasCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repository.ExistsAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken
                );
        }

        private async Task<bool> ExistsNomeAsync
        (
            AtualizarConquistasCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repository.ExistsAsync
                (
                    item => item.Nome.ToLower().Trim().Equals(request.Nome.ToLower().Trim()) &&
                    !item.Id.Equals(request.Id),
                    cancellationToken
                );
        }
    }
}
