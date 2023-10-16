using MediatR;
using ms_usuario.Domains;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.UsuarioFeature.Queries
{
    public class SelecionarUsuarioFiltersQuery : IRequest<IEnumerable<SelecionarUsuarioFiltersQueryResponse>>
    {
    }

    public class SelecionarUsuarioFiltersQueryResponse : Entity
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public int TipoUsuario { get; set; }
        public virtual ICollection<UsuarioAreaInteresse> UsuarioAreaInteresse { get; set; }
        public virtual ICollection<UsuarioConquistas> UsuarioConquistas { get; set; }
        public long ComentarioForum { get; set; }
        public long TopicoForum { get; set; }
        public long ComentarioAula { get; set; }
        public long CurtirAula { get; set; }
        public long NoticiaVisualizada { get; set; }
        public long? PerfilId { get; set; }
        public long? SociedadeId { get; set; }
        public DateTime? DataNascimento { get; set; }
        public byte[]? Foto { get; set; }
        public string? Hobbie { get; set; }
    }

    public class SelecionarUsuarioFiltersQueryResponseHandler : IRequestHandler<SelecionarUsuarioFiltersQuery, IEnumerable<SelecionarUsuarioFiltersQueryResponse>>
    {
        private readonly IRepository<Usuario> _repository;

        public SelecionarUsuarioFiltersQueryResponseHandler
        (
            IRepository<Usuario> repository
        )
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SelecionarUsuarioFiltersQueryResponse>> Handle
        (
            SelecionarUsuarioFiltersQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<SelecionarUsuarioFiltersQuery>());

            IEnumerable<Usuario> usuarioMany = await _repository.GetAsync
                (
                    cancellationToken,
                    item => item.Perfil,
                    item => item.UsuarioConquistas,
                    item => item.UsuarioAreaInteresses
                );

            List<SelecionarUsuarioFiltersQueryResponse> responseMany = new List<SelecionarUsuarioFiltersQueryResponse>();

            foreach (Usuario usuario in usuarioMany)
            {
                SelecionarUsuarioFiltersQueryResponse response = new SelecionarUsuarioFiltersQueryResponse();
                response.Nome = usuario.Nome;
                response.DataCadastro = usuario.DataCadastro;
                response.DataAtualizacao = usuario.DataAtualizacao;
                response.Id = usuario.Id;
                response.Email = usuario.Email;
                response.TipoUsuario = usuario.TipoUsuario;
                response.ComentarioAula = usuario.ComentarioAula;
                response.ComentarioForum = usuario.ComentarioForum;
                response.UsuarioAreaInteresse = usuario.UsuarioAreaInteresses;
                response.UsuarioConquistas = usuario.UsuarioConquistas;
                response.TopicoForum = usuario.TopicoForum;
                response.DataNascimento = usuario.Perfil.DataNascimento;
                response.Hobbie = usuario.Perfil.Hobbie;
                response.Foto = usuario.Perfil.Foto;
                response.SociedadeId = usuario.SociedadeId;
                response.PerfilId = usuario.PerfilId;
                response.CurtirAula = usuario.CurtirAula;
                response.NoticiaVisualizada = usuario.NoticiaVisualizada;
                responseMany.Add(response);
            }

            return responseMany;
        }
    }
}
