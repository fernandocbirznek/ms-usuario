using MediatR;
using ms_usuario.Domains;
using ms_usuario.Extensions;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.AreaInteresseFeature.Commands
{
    public class InserirAreaInteresseCommand : IRequest<InserirAreaInteresseCommandResponse>
    {
        public string Nome { get; set; }
    }

    public class InserirAreaInteresseCommandResponse
    {
        public long Id { get; set; }
        public DateTime DataCadastro { get; set; }

        public string Nome { get; set; }
    }

    public class InserirAreaInteresseHandler : IRequestHandler<InserirAreaInteresseCommand, InserirAreaInteresseCommandResponse>
    {
        private readonly IRepository<AreaInteresse> _repository;

        public InserirAreaInteresseHandler
        (
            IRepository<AreaInteresse> repository
        )
        {
            _repository = repository;
        }

        public async Task<InserirAreaInteresseCommandResponse> Handle
        (
            InserirAreaInteresseCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<InserirAreaInteresseCommand>());

            await Validator(request, cancellationToken);

            AreaInteresse areaInteresse = request.ToDomain();

            await _repository.AddAsync(areaInteresse, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            InserirAreaInteresseCommandResponse response = new InserirAreaInteresseCommandResponse();
            response.DataCadastro = areaInteresse.DataCadastro;
            response.Id = areaInteresse.Id;

            response.Nome = areaInteresse.Nome;

            return response;
        }

        private async Task Validator
        (
            InserirAreaInteresseCommand request,
            CancellationToken cancellationToken
        )
        {
            if (String.IsNullOrEmpty(request.Nome)) throw new ArgumentNullException(MessageHelper.NullFor<InserirAreaInteresseCommand>(item => item.Nome));
            if (await ExistsNomeAsync(request, cancellationToken)) throw new ArgumentNullException("Area interesse já cadastrado");
        }

        private async Task<bool> ExistsNomeAsync
        (
            InserirAreaInteresseCommand request,
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
