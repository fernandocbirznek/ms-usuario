using MediatR;
using ms_usuario.Domains;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.UsuarioFeature.Queries
{
    public class SelecionarUsuarioByIdQuery : IRequest<SelecionarUsuarioByIdQueryResponse>
    {
        public long Id { get; set; }
    }

    public class SelecionarUsuarioByIdQueryResponse : Entity
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

    public class SelecionarUsuarioByIdQueryHandler : IRequestHandler<SelecionarUsuarioByIdQuery, SelecionarUsuarioByIdQueryResponse>
    {
        private readonly IRepository<Usuario> _repository;

        public SelecionarUsuarioByIdQueryHandler
        (
            IRepository<Usuario> repository
        )
        {
            _repository = repository;
        }

        public async Task<SelecionarUsuarioByIdQueryResponse> Handle
        (
            SelecionarUsuarioByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<SelecionarUsuarioByIdQuery>());

            Usuario usuario = await GetFirstAsync(request, cancellationToken);

            Validator(usuario);

            SelecionarUsuarioByIdQueryResponse response = new SelecionarUsuarioByIdQueryResponse();
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
            response.SociedadeId = usuario.SociedadeId;
            response.Foto = usuario.Perfil.Foto;
            response.PerfilId = usuario.PerfilId;
            response.CurtirAula = usuario.CurtirAula;
            response.NoticiaVisualizada = usuario.NoticiaVisualizada;

            return response;
        }

        private void Validator
        (
            Usuario usuario
        )
        {
            if (usuario is null) throw new ArgumentNullException("Usuário não encontrado");
        }

        private async Task<Usuario> GetFirstAsync
        (
            SelecionarUsuarioByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            return await _repository.GetFirstAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken,
                    item => item.Perfil,
                    item => item.UsuarioConquistas,
                    item => item.UsuarioAreaInteresses
                );
        }
    }
}
