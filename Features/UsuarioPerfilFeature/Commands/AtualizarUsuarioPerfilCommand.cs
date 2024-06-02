using MediatR;
using ms_usuario.Domains;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.UsuarioPerfilFeature.Commands
{
    public class AtualizarUsuarioPerfilCommand : IRequest<AtualizarUsuarioPerfilCommandResponse>
    {
        public long Id { get; set; }
        public DateTime? DataNascimento { get; set; }
        public byte[]? Foto { get; set; }
        public string? Hobbie { get; set; }
        public long UsuarioId { get; set; }
    }

    public class AtualizarUsuarioPerfilCommandResponse
    {
        public DateTime DataAtualizacao { get; set; }
    }

    public class AtualizarUsuarioPerfilHandler : IRequestHandler<AtualizarUsuarioPerfilCommand, AtualizarUsuarioPerfilCommandResponse>
    {
        private readonly IRepository<UsuarioAreaInteresse> _repositoryUsuarioAreaInteresse;
        private readonly IRepository<UsuarioPerfil> _repository;
        private readonly IRepository<Usuario> _repositoryUsuario;

        public AtualizarUsuarioPerfilHandler
        (
            IRepository<UsuarioAreaInteresse> repositoryUsuarioAreaInteresse,
            IRepository<UsuarioPerfil> repository,
            IRepository<Usuario> repositoryUsuario
        )
        {
            _repositoryUsuarioAreaInteresse = repositoryUsuarioAreaInteresse;
            _repository = repository;
            _repositoryUsuario = repositoryUsuario;
        }

        public async Task<AtualizarUsuarioPerfilCommandResponse> Handle
        (
            AtualizarUsuarioPerfilCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<AtualizarUsuarioPerfilCommand>());

            await Validator(request, cancellationToken);

            UsuarioPerfil usuarioPerfil = await GetFirstAsync(request, cancellationToken);
            usuarioPerfil.Hobbie = request.Hobbie;
            usuarioPerfil.DataNascimento = request.DataNascimento;
            usuarioPerfil.Foto = request.Foto;

            await _repository.UpdateAsync(usuarioPerfil);
            await _repository.SaveChangesAsync(cancellationToken);

            AtualizarUsuarioPerfilCommandResponse response = new AtualizarUsuarioPerfilCommandResponse();
            response.DataAtualizacao = usuarioPerfil.DataAtualizacao;

            return response;
        }

        private async Task Validator
        (
            AtualizarUsuarioPerfilCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request.Id <= 0) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarUsuarioPerfilCommand>(item => item.Id));
            if (!await ExistsAsync(request, cancellationToken)) throw new ArgumentNullException("Usuário perfil não existe");
            if (!await ExistsUsuarioAsync(request, cancellationToken)) throw new ArgumentNullException("Usuário não existe");
        }

        private async Task<UsuarioPerfil> GetFirstAsync
        (
            AtualizarUsuarioPerfilCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repository.GetFirstAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken,
                    item => item.Usuario
                );
        }

        private async Task<bool> ExistsAsync
        (
            AtualizarUsuarioPerfilCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repository.ExistsAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken
                );
        }

        private async Task<bool> ExistsUsuarioAsync
        (
            AtualizarUsuarioPerfilCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repositoryUsuario.ExistsAsync
                (
                    item => item.Id.Equals(request.UsuarioId),
                    cancellationToken
                );
        }
    }
}
