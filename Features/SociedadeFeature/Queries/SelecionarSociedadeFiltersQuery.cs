using MediatR;
using ms_usuario.Domains;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.SociedadeFeature.Queries
{
    public class SelecionarSociedadeFiltersQuery : IRequest<IEnumerable<SelecionarSociedadeFiltersQueryResponse>>
    {
    }

    public class SelecionarSociedadeFiltersQueryResponse : Entity
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
    }

    public class SelecionarSociedadeFiltersQueryResponseHandler : IRequestHandler<SelecionarSociedadeFiltersQuery, IEnumerable<SelecionarSociedadeFiltersQueryResponse>>
    {
        private readonly IRepository<Sociedade> _repository;

        public SelecionarSociedadeFiltersQueryResponseHandler
        (
            IRepository<Sociedade> repository
        )
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SelecionarSociedadeFiltersQueryResponse>> Handle
        (
            SelecionarSociedadeFiltersQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<SelecionarSociedadeFiltersQuery>());

            IEnumerable<Sociedade> sociedadeMany = await _repository.GetAsync(cancellationToken);

            List<SelecionarSociedadeFiltersQueryResponse> responseMany = new List<SelecionarSociedadeFiltersQueryResponse>();

            foreach (Sociedade sociedade in sociedadeMany)
            {
                SelecionarSociedadeFiltersQueryResponse response = new SelecionarSociedadeFiltersQueryResponse();
                response.Nome = sociedade.Nome;
                response.Descricao = sociedade.Descricao;
                response.DataCadastro = sociedade.DataCadastro;
                response.DataAtualizacao = sociedade.DataAtualizacao;
                response.Id = sociedade.Id;
                responseMany.Add(response);
            }

            return responseMany;
        }
    }
}
