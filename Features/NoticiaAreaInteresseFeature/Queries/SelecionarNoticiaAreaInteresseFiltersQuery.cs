using MediatR;
using ms_usuario.Domains;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.NoticiaAreaInteresseFeature.Queries
{
    public class SelecionarNoticiaAreaInteresseFiltersQuery : IRequest<IEnumerable<SelecionarNoticiaAreaInteresseFiltersQueryResponse>>
    {
    }

    public class SelecionarNoticiaAreaInteresseFiltersQueryResponse : Entity
    {
        public long AreaInteresseId { get; set; }
        public long NoticiaId { get; set; }
    }

    public class SelecionarNoticiaAreaInteresseFiltersQueryResponseHandler :
        IRequestHandler<SelecionarNoticiaAreaInteresseFiltersQuery, IEnumerable<SelecionarNoticiaAreaInteresseFiltersQueryResponse>>
    {
        private readonly IRepository<NoticiaAreaInteresse> _repository;

        public SelecionarNoticiaAreaInteresseFiltersQueryResponseHandler
        (
            IRepository<NoticiaAreaInteresse> repository
        )
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SelecionarNoticiaAreaInteresseFiltersQueryResponse>> Handle
        (
            SelecionarNoticiaAreaInteresseFiltersQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<SelecionarNoticiaAreaInteresseFiltersQuery>());

            IEnumerable<NoticiaAreaInteresse> conquistasMany = await _repository.GetAsync(cancellationToken);

            List<SelecionarNoticiaAreaInteresseFiltersQueryResponse> responseMany = new List<SelecionarNoticiaAreaInteresseFiltersQueryResponse>();

            foreach (NoticiaAreaInteresse conquistas in conquistasMany)
            {
                SelecionarNoticiaAreaInteresseFiltersQueryResponse response = new SelecionarNoticiaAreaInteresseFiltersQueryResponse();
                response.NoticiaId = conquistas.NoticiaId;
                response.AreaInteresseId = conquistas.AreaInteresseId;
                response.DataCadastro = conquistas.DataCadastro;
                response.DataAtualizacao = conquistas.DataAtualizacao;
                response.Id = conquistas.Id;
                responseMany.Add(response);
            }

            return responseMany;
        }
    }
}
