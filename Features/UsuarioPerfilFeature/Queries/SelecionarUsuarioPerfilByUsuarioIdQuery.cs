using MediatR;
using ms_usuario.Domains;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.UsuarioPerfilFeature.Queries
{
    public class SelecionarUsuarioPerfilByUsuarioIdQuery : IRequest<SelecionarUsuarioPerfilByUsuarioIdQueryResponse>
    {
        public long Id { get; set; }
    }

    public class SelecionarUsuarioPerfilByUsuarioIdQueryResponse : Entity
    {
        public DateTime? DataNascimento { get; set; }
        public Byte[]? Foto { get; set; }
        public string? Hobbie { get; set; }
        public long UsuarioId { get; set; }
    }

    public class SelecionarUsuarioPerfilByUsuarioIdQueryHandler : IRequestHandler<SelecionarUsuarioPerfilByUsuarioIdQuery, SelecionarUsuarioPerfilByUsuarioIdQueryResponse>
    {
        private readonly IRepository<UsuarioPerfil> _repository;

        public SelecionarUsuarioPerfilByUsuarioIdQueryHandler
        (
            IRepository<UsuarioPerfil> repository
        )
        {
            _repository = repository;
        }

        public async Task<SelecionarUsuarioPerfilByUsuarioIdQueryResponse> Handle
        (
            SelecionarUsuarioPerfilByUsuarioIdQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<SelecionarUsuarioPerfilByUsuarioIdQuery>());

            UsuarioPerfil usuarioPerfil = await GetFirstAsync(request, cancellationToken);

            Validator(usuarioPerfil);

            SelecionarUsuarioPerfilByUsuarioIdQueryResponse response = new SelecionarUsuarioPerfilByUsuarioIdQueryResponse();

            response.DataNascimento = usuarioPerfil.DataNascimento;
            response.DataCadastro = usuarioPerfil.DataCadastro;
            response.DataAtualizacao = usuarioPerfil.DataAtualizacao;
            response.Hobbie = usuarioPerfil.Hobbie;
            response.Foto = usuarioPerfil.Foto;
            response.Id = usuarioPerfil.Id;

            return response;
        }

        private async void Validator
        (
            UsuarioPerfil usuarioPerfil
        )
        {
            if (usuarioPerfil is null) throw new ArgumentNullException("Perfil usuário não encontrado");
        }

        private async Task<UsuarioPerfil> GetFirstAsync
        (
            SelecionarUsuarioPerfilByUsuarioIdQuery request,
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
