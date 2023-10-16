using MediatR;
using ms_usuario.Domains;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.UsuarioAreaInteresseFeature.Commands
{
    public class RemoverUsuarioAreaInteresseCommand : IRequest<RemoverUsuarioAreaInteresseCommandResponse>
    {
        public long Id { get; set; }
    }

    public class RemoverUsuarioAreaInteresseCommandResponse
    {
        public long Id { get; set; }
    }

    public class RemoverUsuarioAreaInteresseCommandHandler : IRequestHandler<RemoverUsuarioAreaInteresseCommand, RemoverUsuarioAreaInteresseCommandResponse>
    {
        private readonly IRepository<UsuarioAreaInteresse> _repository;

        public RemoverUsuarioAreaInteresseCommandHandler
        (
            IRepository<UsuarioAreaInteresse> repository
        )
        {
            _repository = repository;
        }

        public async Task<RemoverUsuarioAreaInteresseCommandResponse> Handle
        (
            RemoverUsuarioAreaInteresseCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<RemoverUsuarioAreaInteresseCommand>());

            UsuarioAreaInteresse usuarioAreaInteresse = await _repository.GetFirstAsync(item => item.Id.Equals(request.Id), cancellationToken);

            Validator(usuarioAreaInteresse);

            await _repository.RemoveAsync(usuarioAreaInteresse);
            await _repository.SaveChangesAsync(cancellationToken);

            RemoverUsuarioAreaInteresseCommandResponse response = new RemoverUsuarioAreaInteresseCommandResponse();
            response.Id = usuarioAreaInteresse.Id;

            return response;
        }

        private void Validator
        (
            UsuarioAreaInteresse usuarioAreaInteresse
        )
        {
            if (usuarioAreaInteresse is null) throw new ArgumentNullException("Usuário área interesse não encontrado");
        }
    }
}
