using MediatR;
using ms_usuario.Domains;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.UsuarioPerfilFeature.Commands
{
    public class RemoverUsuarioPerfilCommand : IRequest<RemoverUsuarioPerfilCommandResponse>
    {
        public long Id { get; set; }
    }

    public class RemoverUsuarioPerfilCommandResponse
    {
        public long Id { get; set; }
    }

    public class RemoverUsuarioPerfilCommandHandler : IRequestHandler<RemoverUsuarioPerfilCommand, RemoverUsuarioPerfilCommandResponse>
    {
        private readonly IRepository<UsuarioPerfil> _repository;

        public RemoverUsuarioPerfilCommandHandler
        (
            IRepository<UsuarioPerfil> repository
        )
        {
            _repository = repository;
        }

        public async Task<RemoverUsuarioPerfilCommandResponse> Handle
        (
            RemoverUsuarioPerfilCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<RemoverUsuarioPerfilCommand>());

            await Validator(request, cancellationToken);

            UsuarioPerfil usuarioPerfil = await _repository.GetFirstAsync(item => item.Id.Equals(request.Id), cancellationToken);

            await _repository.RemoveAsync(usuarioPerfil);
            await _repository.SaveChangesAsync(cancellationToken);

            RemoverUsuarioPerfilCommandResponse response = new RemoverUsuarioPerfilCommandResponse();
            response.Id = usuarioPerfil.Id;

            return response;
        }

        private async Task Validator
        (
            RemoverUsuarioPerfilCommand request,
            CancellationToken cancellationToken
        )
        {
            if (!(await ExistsAsync(request, cancellationToken))) throw new ArgumentNullException("Perfil usuário não encontrado");
        }

        private async Task<bool> ExistsAsync
        (
            RemoverUsuarioPerfilCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repository.ExistsAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken
                );
        }
    }
}
