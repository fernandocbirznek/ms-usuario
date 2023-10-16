using MediatR;
using ms_usuario.Domains;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.AreaInteresseFeature.Commands
{
    public class AtualizarAreaInteresseCommand : IRequest<AtualizarAreaInteresseCommandResponse>
    {
        public long Id { get; set; }
        public string Nome { get; set; }
    }

    public class AtualizarAreaInteresseCommandResponse
    {
        public DateTime DataAtualizacao { get; set; }
    }

    public class AtualizarAreaInteresseHandler : IRequestHandler<AtualizarAreaInteresseCommand, AtualizarAreaInteresseCommandResponse>
    {
        private readonly IRepository<AreaInteresse> _repository;

        public AtualizarAreaInteresseHandler
        (
            IRepository<AreaInteresse> repository
        )
        {
            _repository = repository;
        }

        public async Task<AtualizarAreaInteresseCommandResponse> Handle
        (
            AtualizarAreaInteresseCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<AtualizarAreaInteresseCommand>());

            await Validator(request, cancellationToken);

            AreaInteresse areaInteresse = await GetFirstAsync(request, cancellationToken);
            areaInteresse.Nome = request.Nome;

            await _repository.UpdateAsync(areaInteresse);
            await _repository.SaveChangesAsync(cancellationToken);

            AtualizarAreaInteresseCommandResponse response = new AtualizarAreaInteresseCommandResponse();
            response.DataAtualizacao = areaInteresse.DataAtualizacao;

            return response;
        }

        private async Task Validator
        (
            AtualizarAreaInteresseCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request.Id <= 0) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarAreaInteresseCommand>(item => item.Id));
            if (String.IsNullOrEmpty(request.Nome)) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarAreaInteresseCommand>(item => item.Nome));
            if (!(await ExistsAsync(request, cancellationToken))) throw new ArgumentNullException("Área da Física não encontrada");
            if (await ExistsNomeAsync(request, cancellationToken)) throw new ArgumentNullException("Área de interesse já cadastrado");
        }

        private async Task<AreaInteresse> GetFirstAsync
        (
            AtualizarAreaInteresseCommand request,
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
            AtualizarAreaInteresseCommand request,
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
            AtualizarAreaInteresseCommand request,
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
