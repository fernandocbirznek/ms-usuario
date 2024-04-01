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
        public virtual IEnumerable<AreaInteresse> UsuarioAreaInteresses { get; set; }
        public virtual IEnumerable<Conquistas> UsuarioConquistas { get; set; }
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
        private readonly IRepository<AreaInteresse> _repositoryAreaInteresse;
        private readonly IRepository<Conquistas> _repositoryConquista;
        public SelecionarUsuarioByIdQueryHandler
        (
            IRepository<Usuario> repository,
            IRepository<AreaInteresse> repositoryAreaInteresse,
            IRepository<Conquistas> repositoryConquista
        )
        {
            _repository = repository;
            _repositoryAreaInteresse = repositoryAreaInteresse;
            _repositoryConquista = repositoryConquista;
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
            IEnumerable<AreaInteresse> areaInteresseMany = await GetAreaInteresseAsync(cancellationToken);
            IEnumerable<Conquistas> conquistaMany = await GetConquistaAsync(cancellationToken);


            Validator(usuario);

            List<AreaInteresse> usuarioAreaInteresse = new List<AreaInteresse>();
            foreach (UsuarioAreaInteresse item in usuario.UsuarioAreaInteresses)
            {
                AreaInteresse areaInteresse = areaInteresseMany.First(area => area.Id.Equals(item.AreaInteresseId));
                if (areaInteresse is not null)
                    usuarioAreaInteresse.Add(areaInteresse);
            }

            List<Conquistas> usuarioConquistaMany = new List<Conquistas>();
            foreach (UsuarioConquistas item in usuario.UsuarioConquistas)
            {
                Conquistas conquista = conquistaMany.First(area => area.Id.Equals(item.ConquistaId));
                if (conquista is not null)
                    usuarioConquistaMany.Add(conquista);
            }

            SelecionarUsuarioByIdQueryResponse response = new SelecionarUsuarioByIdQueryResponse();
            response.Nome = usuario.Nome;
            response.DataCadastro = usuario.DataCadastro;
            response.DataAtualizacao = usuario.DataAtualizacao;
            response.Id = usuario.Id;
            response.Email = usuario.Email;
            response.TipoUsuario = usuario.TipoUsuario;
            response.ComentarioAula = usuario.ComentarioAula;
            response.ComentarioForum = usuario.ComentarioForum;
            response.UsuarioAreaInteresses = usuarioAreaInteresse;
            response.UsuarioConquistas = usuarioConquistaMany;
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

        private async Task<IEnumerable<Conquistas>> GetConquistaAsync
        (
            CancellationToken cancellationToken
        )
        {
            return await _repositoryConquista.GetAsync
                (
                    cancellationToken
                );
        }
    }
}
