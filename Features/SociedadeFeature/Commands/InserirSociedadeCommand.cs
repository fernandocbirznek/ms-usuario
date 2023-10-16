using MediatR;
using ms_usuario.Domains;
using ms_usuario.Extensions;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.SociedadeFeature.Commands
{
    public class InserirSociedadeCommand : IRequest<InserirSociedadeCommandResponse>
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
    }

    public class InserirSociedadeCommandResponse
    {
        public long Id { get; set; }
        public DateTime DataCadastro { get; set; }
    }

    public class InserirSociedadeHandler : IRequestHandler<InserirSociedadeCommand, InserirSociedadeCommandResponse>
    {
        private readonly IRepository<Sociedade> _repository;

        public InserirSociedadeHandler
        (
            IRepository<Sociedade> repository
        )
        {
            _repository = repository;
        }

        public async Task<InserirSociedadeCommandResponse> Handle
        (
            InserirSociedadeCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<InserirSociedadeCommand>());

            await Validator(request, cancellationToken);

            Sociedade sociedade = request.ToDomain();

            await _repository.AddAsync(sociedade, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            InserirSociedadeCommandResponse response = new InserirSociedadeCommandResponse();
            response.DataCadastro = sociedade.DataCadastro;
            response.Id = sociedade.Id;

            return response;
        }

        private async Task Validator
        (
            InserirSociedadeCommand request,
            CancellationToken cancellationToken
        )
        {
            if (String.IsNullOrEmpty(request.Nome)) throw new ArgumentNullException(MessageHelper.NullFor<InserirSociedadeCommand>(item => item.Nome));
            if (String.IsNullOrEmpty(request.Descricao)) throw new ArgumentNullException(MessageHelper.NullFor<InserirSociedadeCommand>(item => item.Descricao));
            if (await ExistsNomeAsync(request, cancellationToken)) throw new ArgumentNullException("Conquistas já cadastrado");
        }

        private async Task<bool> ExistsNomeAsync
        (
            InserirSociedadeCommand request,
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
