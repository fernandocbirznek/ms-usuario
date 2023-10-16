using MediatR;
using ms_usuario.Domains;
using ms_usuario.Extensions;
using ms_usuario.Features.AreaInteresseFeature.Commands;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.ConquistasFeature.Commands
{
    public class InserirConquistasCommand : IRequest<InserirConquistasCommandResponse>
    {
        public string Nome { get; set; }
    }

    public class InserirConquistasCommandResponse
    {
        public long Id { get; set; }
        public DateTime DataCadastro { get; set; }
    }

    public class InserirConquistasHandler : IRequestHandler<InserirConquistasCommand, InserirConquistasCommandResponse>
    {
        private readonly IRepository<Conquistas> _repository;

        public InserirConquistasHandler
        (
            IRepository<Conquistas> repository
        )
        {
            _repository = repository;
        }

        public async Task<InserirConquistasCommandResponse> Handle
        (
            InserirConquistasCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<InserirConquistasCommand>());

            await Validator(request, cancellationToken);

            Conquistas conquistas = request.ToDomain();

            await _repository.AddAsync(conquistas, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            InserirConquistasCommandResponse response = new InserirConquistasCommandResponse();
            response.DataCadastro = conquistas.DataCadastro;
            response.Id = conquistas.Id;

            return response;
        }

        private async Task Validator
        (
            InserirConquistasCommand request,
            CancellationToken cancellationToken
        )
        {
            if (String.IsNullOrEmpty(request.Nome)) throw new ArgumentNullException(MessageHelper.NullFor<InserirConquistasCommand>(item => item.Nome));
            if (await ExistsNomeAsync(request, cancellationToken)) throw new ArgumentNullException("Conquistas já cadastrado");
        }

        private async Task<bool> ExistsNomeAsync
        (
            InserirConquistasCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repository.ExistsAsync
                (
                    item => item.Nome.ToLower().Trim().Equals(request.Nome.ToLower().Trim()),
                    cancellationToken
                );
        }
    }
}
