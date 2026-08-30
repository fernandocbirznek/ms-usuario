using MediatR;
using ms_usuario.Domains;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.NoticiaFeature.Queries
{
    public class SelecionarManyNoticiaFiltersQuery : IRequest<IEnumerable<SelecionarManyNoticiaFiltersQueryResponse>>
    {
    }

    public class SelecionarManyNoticiaFiltersQueryResponse : Entity
    {
        public string Titulo { get; set; }
        public string Resumo { get; set; }
        public string Conteudo { get; set; }
        public long Favoritado { get; set; }
        public long UsuarioCadastroId { get; set; }
        public string UsuarioCadastroNome { get; set; }
        public long? SociedadeId { get; set; }
        public IEnumerable<AreaInteresse> AreaInteresseMany { get; set; }
    }

    public class SelecionarManyNoticiaFiltersQueryResponseHandler :
        IRequestHandler<SelecionarManyNoticiaFiltersQuery, IEnumerable<SelecionarManyNoticiaFiltersQueryResponse>>
    {
        private readonly IRepository<Noticia> _repository;
        private readonly IRepository<AreaInteresse> _repositoryAreaInteresse;
        private readonly IRepository<Usuario> _repositoryUsuario;

        public SelecionarManyNoticiaFiltersQueryResponseHandler
        (
            IRepository<Noticia> repository,
            IRepository<AreaInteresse> repositoryAreaInteresse,
            IRepository<Usuario> repositoryUsuario
        )
        {
            _repository = repository;
            _repositoryAreaInteresse = repositoryAreaInteresse;
            _repositoryUsuario = repositoryUsuario;
        }

        public async Task<IEnumerable<SelecionarManyNoticiaFiltersQueryResponse>> Handle
        (
            SelecionarManyNoticiaFiltersQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<SelecionarManyNoticiaFiltersQuery>());

            List<Noticia> noticiaMany = (await GetAsync(cancellationToken)).ToList();
            IEnumerable<AreaInteresse> areaInteresseMany = await GetAreaInteresseAsync(cancellationToken);
            IReadOnlyDictionary<long, string> usuarioCadastroNomes = await GetUsuarioCadastroNomesAsync(noticiaMany, cancellationToken);

            List<SelecionarManyNoticiaFiltersQueryResponse> responseMany = new List<SelecionarManyNoticiaFiltersQueryResponse>();

            foreach (Noticia noticia in noticiaMany)
            {
                List<AreaInteresse> noticiaAreaInteresse = new List<AreaInteresse>();
                foreach (NoticiaAreaInteresse item in noticia.NoticiaAreaInteresseMany)
                {
                    AreaInteresse areaInteresse = areaInteresseMany.First(area => area.Id.Equals(item.AreaInteresseId));
                    if (areaInteresse is not null)
                        noticiaAreaInteresse.Add(areaInteresse);
                }

                SelecionarManyNoticiaFiltersQueryResponse response = new SelecionarManyNoticiaFiltersQueryResponse();
                response.Titulo = noticia.Titulo;
                response.Resumo = noticia.Resumo;
                response.Conteudo = noticia.Conteudo;
                response.Favoritado = noticia.Favoritado;
                response.AreaInteresseMany = noticiaAreaInteresse;
                response.DataCadastro = noticia.DataCadastro;
                response.DataAtualizacao = noticia.DataAtualizacao;
                response.UsuarioCadastroId = noticia.UsuarioCadastroId;
                response.SociedadeId = noticia.SociedadeId;
                response.UsuarioCadastroNome = usuarioCadastroNomes.GetValueOrDefault(noticia.UsuarioCadastroId, string.Empty);
                response.Id = noticia.Id;
                responseMany.Add(response);
            }

            return responseMany;
        }

        private async Task<IEnumerable<Noticia>> GetAsync
        (
            CancellationToken cancellationToken
        )
        {
            return await _repository.GetAsNoTrackingAsync
                (
                    cancellationToken,
                    item => item.NoticiaAreaInteresseMany
                );
        }

        private async Task<IReadOnlyDictionary<long, string>> GetUsuarioCadastroNomesAsync
        (
            IEnumerable<Noticia> noticiaMany,
            CancellationToken cancellationToken
        )
        {
            List<long> usuarioCadastroIds = noticiaMany
                .Select(noticia => noticia.UsuarioCadastroId)
                .Distinct()
                .ToList();

            if (usuarioCadastroIds.Count == 0)
                return new Dictionary<long, string>();

            IEnumerable<Usuario> usuarioMany = await _repositoryUsuario.GetAsNoTrackingAsync
                (
                    usuario => usuarioCadastroIds.Contains(usuario.Id),
                    cancellationToken
                );

            return usuarioMany.ToDictionary(usuario => usuario.Id, usuario => usuario.Nome);
        }

        private async Task<IEnumerable<AreaInteresse>> GetAreaInteresseAsync
        (
            CancellationToken cancellationToken
        )
        {
            return await _repositoryAreaInteresse.GetAsNoTrackingAsync
                (
                    cancellationToken
                );
        }
    }
}
