using MediatR;
using ms_usuario.Domains;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.UsuarioAreaInteresseFeature.Queries
{
    public class SelecionarUsuarioAreaInteresseByUsuarioIdQuery : IRequest<IEnumerable<SelecionarUsuarioAreaInteresseByUsuarioIdQueryResponse>>
    {
        public long Id { get; set; }
    }

    public class SelecionarUsuarioAreaInteresseByUsuarioIdQueryResponse : Entity
    {
        public AreaInteresse AreaInteresse { get; set; }
    }

    public class SelecionarUsuarioAreaInteresseByUsuarioIdQueryResponseHandler : IRequestHandler<SelecionarUsuarioAreaInteresseByUsuarioIdQuery, IEnumerable<SelecionarUsuarioAreaInteresseByUsuarioIdQueryResponse>>
    {
        private readonly IRepository<UsuarioAreaInteresse> _repository;

        public SelecionarUsuarioAreaInteresseByUsuarioIdQueryResponseHandler
        (
            IRepository<UsuarioAreaInteresse> repository
        )
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SelecionarUsuarioAreaInteresseByUsuarioIdQueryResponse>> Handle
        (
            SelecionarUsuarioAreaInteresseByUsuarioIdQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<SelecionarUsuarioAreaInteresseByUsuarioIdQuery>());

            IEnumerable<UsuarioAreaInteresse> usuarioInteresseMany = await _repository.GetAsync
                (
                    item => item.UsuarioId.Equals(request.Id),
                    cancellationToken,
                    item => item.AreaInteresse
                );

            List<SelecionarUsuarioAreaInteresseByUsuarioIdQueryResponse> responseMany = new List<SelecionarUsuarioAreaInteresseByUsuarioIdQueryResponse>();

            foreach (UsuarioAreaInteresse usuarioInteresse in usuarioInteresseMany)
            {
                SelecionarUsuarioAreaInteresseByUsuarioIdQueryResponse response = new SelecionarUsuarioAreaInteresseByUsuarioIdQueryResponse();
                response.AreaInteresse = usuarioInteresse.AreaInteresse;
                responseMany.Add(response);
            }

            return responseMany;
        }
    }
}
