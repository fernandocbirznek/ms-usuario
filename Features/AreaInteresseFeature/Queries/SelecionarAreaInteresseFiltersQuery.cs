using MediatR;
using ms_usuario.Domains;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.AreaInteresseFeature.Queries
{
    public class SelecionarAreaInteresseFiltersQuery : IRequest<IEnumerable<SelecionarAreaInteresseFiltersQueryResponse>>
    {
    }

    public class SelecionarAreaInteresseFiltersQueryResponse : Entity
    {
        public string Nome { get; set; }
    }

    public class SelecionarAreaInteresseFiltersQueryResponseHandler : IRequestHandler<SelecionarAreaInteresseFiltersQuery, IEnumerable<SelecionarAreaInteresseFiltersQueryResponse>>
    {
        private readonly IRepository<AreaInteresse> _repository;

        public SelecionarAreaInteresseFiltersQueryResponseHandler
        (
            IRepository<AreaInteresse> repository
        )
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SelecionarAreaInteresseFiltersQueryResponse>> Handle
        (
            SelecionarAreaInteresseFiltersQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<SelecionarAreaInteresseFiltersQuery>());

            IEnumerable<AreaInteresse> areaInteresseMany = await _repository.GetAsync(cancellationToken);

            List<SelecionarAreaInteresseFiltersQueryResponse> responseMany = new List<SelecionarAreaInteresseFiltersQueryResponse>();

            foreach (AreaInteresse areaInteresse in areaInteresseMany)
            {
                SelecionarAreaInteresseFiltersQueryResponse response = new SelecionarAreaInteresseFiltersQueryResponse();
                response.Nome = areaInteresse.Nome;
                response.DataCadastro = areaInteresse.DataCadastro;
                response.DataAtualizacao = areaInteresse.DataAtualizacao;
                response.Id = areaInteresse.Id;
                responseMany.Add(response);
            }

            return responseMany;
        }
    }
}
