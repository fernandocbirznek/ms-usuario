using MediatR;
using ms_usuario.Domains;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.UsuarioNoticiaFavoritadoFeature.Queries
{
    public class SelecionarUsuarioNoticiaFavoritadaByUsuarioIdQuery
        : IRequest<IEnumerable<SelecionarUsuarioNoticiaFavoritadaByUsuarioIdQueryResponse>>
    {
        public long Id { get; set; }
    }

    public class SelecionarUsuarioNoticiaFavoritadaByUsuarioIdQueryResponse : Entity
    {
        public long UsuarioId { get; set; }
        public long NoticiaId { get; set; }
    }

    public class SelecionarUsuarioNoticiaFavoritadaByUsuarioIdQueryHandler
        : IRequestHandler<SelecionarUsuarioNoticiaFavoritadaByUsuarioIdQuery,
            IEnumerable<SelecionarUsuarioNoticiaFavoritadaByUsuarioIdQueryResponse>>
    {
        private readonly IRepository<UsuarioNoticiaFavoritado> _repository;

        public SelecionarUsuarioNoticiaFavoritadaByUsuarioIdQueryHandler
        (
            IRepository<UsuarioNoticiaFavoritado> repository
        )
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SelecionarUsuarioNoticiaFavoritadaByUsuarioIdQueryResponse>> Handle
        (
            SelecionarUsuarioNoticiaFavoritadaByUsuarioIdQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<SelecionarUsuarioNoticiaFavoritadaByUsuarioIdQuery>());

            IEnumerable<UsuarioNoticiaFavoritado> usuarioNoticiaFavoritadoMany = await GetAsync(request, cancellationToken);

            List<SelecionarUsuarioNoticiaFavoritadaByUsuarioIdQueryResponse> responseMany =
                new List<SelecionarUsuarioNoticiaFavoritadaByUsuarioIdQueryResponse>();

            foreach (UsuarioNoticiaFavoritado usuarioNoticiaFavoritado in usuarioNoticiaFavoritadoMany)
            {
                SelecionarUsuarioNoticiaFavoritadaByUsuarioIdQueryResponse response =
                    new SelecionarUsuarioNoticiaFavoritadaByUsuarioIdQueryResponse();
                response.NoticiaId = usuarioNoticiaFavoritado.NoticiaId;
                response.UsuarioId = usuarioNoticiaFavoritado.UsuarioId;

                response.DataCadastro = usuarioNoticiaFavoritado.DataCadastro;
                response.DataAtualizacao = usuarioNoticiaFavoritado.DataAtualizacao;
                response.Id = usuarioNoticiaFavoritado.Id;
                responseMany.Add(response);
            }

            return responseMany;
        }

        private async Task<IEnumerable<UsuarioNoticiaFavoritado>> GetAsync
        (
            SelecionarUsuarioNoticiaFavoritadaByUsuarioIdQuery request,
            CancellationToken cancellationToken
        )
        {
            return await _repository.GetAsync
                (
                    item => item.UsuarioId.Equals(request.Id),
                    cancellationToken
                );
        }
    }
}
