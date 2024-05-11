using MediatR;
using ms_usuario.Domains;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.NoticiaFeature.Queries
{
    public class SelecionarNoticiaManyHomeQuery : IRequest<IEnumerable<SelecionarNoticiaManyHomeQueryResponse>>
    {
        public long Id { get; set; }
    }

    public class SelecionarNoticiaManyHomeQueryResponse : Entity
    {
        public string Titulo { get; set; }
        public string Resumo { get; set; }
        public string Conteudo { get; set; }
        public long Favoritado { get; set; }
        public long UsuarioCadastroId { get; set; }
        public string UsuarioCadastroNome { get; set; }
        public IEnumerable<AreaInteresse> AreaInteresseMany { get; set; }
    }

    public class SelecionarNoticiaManyHomeQueryResponseHandler :
        IRequestHandler<SelecionarNoticiaManyHomeQuery, IEnumerable<SelecionarNoticiaManyHomeQueryResponse>>
    {
        private readonly IRepository<Noticia> _repository;
        private readonly IRepository<AreaInteresse> _repositoryAreaInteresse;
        private readonly IRepository<Usuario> _repositoryUsuario;

        public SelecionarNoticiaManyHomeQueryResponseHandler
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

        public async Task<IEnumerable<SelecionarNoticiaManyHomeQueryResponse>> Handle
        (
            SelecionarNoticiaManyHomeQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<SelecionarNoticiaManyHomeQuery>());

            IEnumerable<Noticia> noticiaMany = await GetAsync(request, cancellationToken);
            IEnumerable<AreaInteresse> areaInteresseMany = await GetAreaInteresseAsync(cancellationToken);

            List<SelecionarNoticiaManyHomeQueryResponse> responseMany = new List<SelecionarNoticiaManyHomeQueryResponse>();

            foreach (Noticia noticia in noticiaMany)
            {
                List<AreaInteresse> noticiaAreaInteresse = new List<AreaInteresse>();
                foreach (NoticiaAreaInteresse item in noticia.NoticiaAreaInteresseMany)
                {
                    AreaInteresse areaInteresse = areaInteresseMany.First(area => area.Id.Equals(item.AreaInteresseId));
                    if (areaInteresse is not null)
                        noticiaAreaInteresse.Add(areaInteresse);
                }

                SelecionarNoticiaManyHomeQueryResponse response = new SelecionarNoticiaManyHomeQueryResponse();
                response.Titulo = noticia.Titulo;
                response.Resumo = noticia.Resumo;
                response.Conteudo = noticia.Conteudo;
                response.Favoritado = noticia.Favoritado;
                response.AreaInteresseMany = noticiaAreaInteresse;
                response.DataCadastro = noticia.DataCadastro;
                response.DataAtualizacao = noticia.DataAtualizacao;
                response.UsuarioCadastroId = noticia.UsuarioCadastroId;
                response.UsuarioCadastroNome = await GetUsuarioCadastroNomeAsync(noticia.UsuarioCadastroId, cancellationToken);
                response.Id = noticia.Id;
                responseMany.Add(response);
            }

            return responseMany;
        }

        private async Task<IEnumerable<Noticia>> GetAsync
        (
            SelecionarNoticiaManyHomeQuery request,
            CancellationToken cancellationToken
        )
        {
            return await _repository.GetAsync
                (
                    cancellationToken,
                    item => item.NoticiaAreaInteresseMany
                );
        }

        private async Task<string> GetUsuarioCadastroNomeAsync
        (
            long usuarioCadastroId,
            CancellationToken cancellationToken
        )
        {
            var usuario = await _repositoryUsuario.GetFirstAsync
                (
                    item => item.Id.Equals(usuarioCadastroId),
                    cancellationToken
                );

            if (usuario is not null)
                return usuario.Nome;
 
            return string.Empty;
        }

        private async Task<IEnumerable<AreaInteresse>> GetAreaInteresseAsync
        (
            CancellationToken cancellationToken
        )
        {
            return await _repositoryAreaInteresse.GetAsync
                (
                    cancellationToken
                );
        }
    }
}
