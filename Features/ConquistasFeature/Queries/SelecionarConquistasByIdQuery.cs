using MediatR;
using ms_usuario.Domains;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.ConquistasFeature.Queries
{
    public class SelecionarConquistasByIdQuery : IRequest<SelecionarConquistasByIdQueryResponse>
    {
        public long Id { get; set; }
    }

    public class SelecionarConquistasByIdQueryResponse : Entity
    {
        public string Nome { get; set; }
    }

    public class SelecionarConquistasByIdQueryHandler : IRequestHandler<SelecionarConquistasByIdQuery, SelecionarConquistasByIdQueryResponse>
    {
        private readonly IRepository<Conquistas> _repository;

        public SelecionarConquistasByIdQueryHandler
        (
            IRepository<Conquistas> repository
        )
        {
            _repository = repository;
        }

        public async Task<SelecionarConquistasByIdQueryResponse> Handle
        (
            SelecionarConquistasByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<SelecionarConquistasByIdQuery>());

            Conquistas conquistas = await GetFirstAsync(request, cancellationToken);

            Validator(conquistas);

            SelecionarConquistasByIdQueryResponse response = new SelecionarConquistasByIdQueryResponse();

            response.Nome = conquistas.Nome;
            response.DataCadastro = conquistas.DataCadastro;
            response.DataAtualizacao = conquistas.DataAtualizacao;
            response.Id = conquistas.Id;

            return response;
        }

        private async void Validator
        (
            Conquistas conquistas
        )
        {
            if (conquistas is null) throw new ArgumentNullException("Conquista não encontrado");
        }

        private async Task<Conquistas> GetFirstAsync
        (
            SelecionarConquistasByIdQuery request,
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
