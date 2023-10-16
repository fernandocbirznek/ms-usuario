using MediatR;
using ms_usuario.Domains;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.AreaInteresseFeature.Queries
{
    public class SelecionarAreaInteresseByIdQuery : IRequest<SelecionarAreaInteresseByIdQueryResponse>
    {
        public long Id { get; set; }
    }

    public class SelecionarAreaInteresseByIdQueryResponse : Entity
    {
        public string Nome { get; set; }
    }

    public class SelecionarAreaInteresseByIdQueryHandler : IRequestHandler<SelecionarAreaInteresseByIdQuery, SelecionarAreaInteresseByIdQueryResponse>
    {
        private readonly IRepository<AreaInteresse> _repository;

        public SelecionarAreaInteresseByIdQueryHandler
        (
            IRepository<AreaInteresse> repository
        )
        {
            _repository = repository;
        }

        public async Task<SelecionarAreaInteresseByIdQueryResponse> Handle
        (
            SelecionarAreaInteresseByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<SelecionarAreaInteresseByIdQuery>());

            AreaInteresse areaInteresse = await GetFirstAsync(request, cancellationToken);

            Validator(areaInteresse, cancellationToken);

            SelecionarAreaInteresseByIdQueryResponse response = new SelecionarAreaInteresseByIdQueryResponse();

            response.Nome = areaInteresse.Nome;
            response.DataCadastro = areaInteresse.DataCadastro;
            response.DataAtualizacao = areaInteresse.DataAtualizacao;
            response.Id = areaInteresse.Id;

            return response;
        }

        private async void Validator
        (
            AreaInteresse areaInteresse,
            CancellationToken cancellationToken
        )
        {
            if (areaInteresse is null) throw new ArgumentNullException("Área de interesse não encontrado");
        }

        private async Task<AreaInteresse> GetFirstAsync
        (
            SelecionarAreaInteresseByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            return await _repository.GetFirstAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken
                );
        }
    }
}