using MediatR;
using ms_usuario.Domains;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.UsuarioConquistasFeature.Queries
{
    public class SelecionarUsuarioConquistasByUsuarioIdQuery : IRequest<IEnumerable<SelecionarUsuarioConquistasByUsuarioIdQueryResponse>>
    {
        public long Id { get; set; }
    }

    public class SelecionarUsuarioConquistasByUsuarioIdQueryResponse : Entity
    {
        public Conquistas Conquista { get; set; }
    }

    public class SelecionarUsuarioConquistasByUsuarioIdQueryResponseHandler : IRequestHandler<SelecionarUsuarioConquistasByUsuarioIdQuery, IEnumerable<SelecionarUsuarioConquistasByUsuarioIdQueryResponse>>
    {
        private readonly IRepository<UsuarioConquistas> _repository;

        public SelecionarUsuarioConquistasByUsuarioIdQueryResponseHandler
        (
            IRepository<UsuarioConquistas> repository
        )
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SelecionarUsuarioConquistasByUsuarioIdQueryResponse>> Handle
        (
            SelecionarUsuarioConquistasByUsuarioIdQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<SelecionarUsuarioConquistasByUsuarioIdQuery>());

            IEnumerable<UsuarioConquistas> usuarioConquistaMany = await _repository.GetAsync
                (
                    item => item.UsuarioId.Equals(request.Id),
                    cancellationToken,
                    item => item.Conquista
                );

            List<SelecionarUsuarioConquistasByUsuarioIdQueryResponse> responseMany = new List<SelecionarUsuarioConquistasByUsuarioIdQueryResponse>();

            foreach (UsuarioConquistas usuarioConquista in usuarioConquistaMany)
            {
                SelecionarUsuarioConquistasByUsuarioIdQueryResponse response = new SelecionarUsuarioConquistasByUsuarioIdQueryResponse();
                response.Conquista = usuarioConquista.Conquista;
                responseMany.Add(response);
            }

            return responseMany;
        }
    }
}
