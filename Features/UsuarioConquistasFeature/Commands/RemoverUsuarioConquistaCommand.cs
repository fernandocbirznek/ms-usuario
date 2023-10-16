using MediatR;
using ms_usuario.Domains;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.UsuarioConquistasFeature.Commands
{
    public class RemoverUsuarioConquistaCommand : IRequest<RemoverUsuarioConquistaCommandResponse>
    {
        public long Id { get; set; }
    }

    public class RemoverUsuarioConquistaCommandResponse
    {
        public long Id { get; set; }
    }

    public class RemoverUsuarioConquistaCommandHandler : IRequestHandler<RemoverUsuarioConquistaCommand, RemoverUsuarioConquistaCommandResponse>
    {
        private readonly IRepository<UsuarioConquistas> _repository;

        public RemoverUsuarioConquistaCommandHandler
        (
            IRepository<UsuarioConquistas> repository
        )
        {
            _repository = repository;
        }

        public async Task<RemoverUsuarioConquistaCommandResponse> Handle
        (
            RemoverUsuarioConquistaCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<RemoverUsuarioConquistaCommand>());

            UsuarioConquistas usuarioConquista = await _repository.GetFirstAsync(item => item.Id.Equals(request.Id), cancellationToken);

            Validator(usuarioConquista);

            await _repository.RemoveAsync(usuarioConquista);
            await _repository.SaveChangesAsync(cancellationToken);

            RemoverUsuarioConquistaCommandResponse response = new RemoverUsuarioConquistaCommandResponse();
            response.Id = usuarioConquista.Id;

            return response;
        }

        private void Validator
        (
            UsuarioConquistas usuarioConquista
        )
        {
            if (usuarioConquista is null) throw new ArgumentNullException("Usuário área conquista não encontrado");
        }
    }
}
