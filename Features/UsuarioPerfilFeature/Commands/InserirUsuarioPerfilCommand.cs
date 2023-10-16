using MediatR;
using ms_usuario.Domains;
using ms_usuario.Extensions;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.UsuarioPerfilFeature.Commands
{
    public class InserirUsuarioPerfilCommand : IRequest<InserirUsuarioPerfilCommandResponse>
    {
        public DateTime? DataNascimento { get; set; }
        public Byte[]? Foto { get; set; }
        public string? Hobbie { get; set; }
        public long UsuarioId { get; set; }
    }

    public class InserirUsuarioPerfilCommandResponse
    {
        public long Id { get; set; }
        public DateTime DataCadastro { get; set; }
    }

    public class InserirUsuarioPerfilHandler : IRequestHandler<InserirUsuarioPerfilCommand, InserirUsuarioPerfilCommandResponse>
    {
        private readonly IRepository<UsuarioPerfil> _repository;
        private readonly IRepository<Usuario> _repositoryUsuario;

        public InserirUsuarioPerfilHandler
        (
            IRepository<UsuarioPerfil> repository,
            IRepository<Usuario> repositoryUsuario
        )
        {
            _repository = repository;
            _repositoryUsuario = repositoryUsuario;
        }

        public async Task<InserirUsuarioPerfilCommandResponse> Handle
        (
            InserirUsuarioPerfilCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<InserirUsuarioPerfilCommand>());

            await Validator(request, cancellationToken);

            UsuarioPerfil usuarioPerfil = request.ToDomain();

            await _repository.AddAsync(usuarioPerfil, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            InserirUsuarioPerfilCommandResponse response = new InserirUsuarioPerfilCommandResponse();
            response.DataCadastro = usuarioPerfil.DataCadastro;
            response.Id = usuarioPerfil.Id;

            return response;
        }

        private async Task Validator
        (
            InserirUsuarioPerfilCommand request,
            CancellationToken cancellationToken
        )
        {
            if (!await ExistsUsuarioAsync(request, cancellationToken)) throw new ArgumentNullException("Usuário não existe");
        }

        private async Task<bool> ExistsUsuarioAsync
        (
            InserirUsuarioPerfilCommand request,
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
