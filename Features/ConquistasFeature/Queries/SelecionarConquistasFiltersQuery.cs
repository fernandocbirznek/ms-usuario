using MediatR;
using ms_usuario.Domains;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.ConquistasFeature.Queries
{
    public class SelecionarConquistasFiltersQuery : IRequest<IEnumerable<SelecionarConquistasFiltersQueryResponse>>
    {
    }

    public class SelecionarConquistasFiltersQueryResponse : Entity
    {
        public string Nome { get; set; }
    }

    public class SelecionarConquistasFiltersQueryResponseHandler : IRequestHandler<SelecionarConquistasFiltersQuery, IEnumerable<SelecionarConquistasFiltersQueryResponse>>
    {
        private readonly IRepository<Conquistas> _repository;

        public SelecionarConquistasFiltersQueryResponseHandler
        (
            IRepository<Conquistas> repository
        )
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SelecionarConquistasFiltersQueryResponse>> Handle
        (
            SelecionarConquistasFiltersQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<SelecionarConquistasFiltersQuery>());

            IEnumerable<Conquistas> conquistasMany = await _repository.GetAsync(cancellationToken);

            List<SelecionarConquistasFiltersQueryResponse> responseMany = new List<SelecionarConquistasFiltersQueryResponse>();

            foreach (Conquistas conquistas in conquistasMany)
            {
                SelecionarConquistasFiltersQueryResponse response = new SelecionarConquistasFiltersQueryResponse();
                response.Nome = conquistas.Nome;
                response.DataCadastro = conquistas.DataCadastro;
                response.DataAtualizacao = conquistas.DataAtualizacao;
                response.Id = conquistas.Id;
                responseMany.Add(response);
            }

            return responseMany;
        }
    }
}
