using MediatR;
using ms_usuario.Domains;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.UsuarioAreaInteresseFeature.Commands
{
    public class RemoverUsuarioAreaInteresseCommand : IRequest<RemoverUsuarioAreaInteresseCommandResponse>
    {
        public long UsuarioId { get; set; }
        public long AreaInteresseId { get; set; }
    }

    public class RemoverUsuarioAreaInteresseCommandResponse
    {
        public long UsuarioId { get; set; }
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

            UsuarioAreaInteresse usuarioAreaInteresse = await 
                _repository
                    .GetFirstAsync
                    (
                        item => item.UsuarioId.Equals(request.UsuarioId) && item.AreaInteresseId.Equals(request.AreaInteresseId), 
                        cancellationToken
                    );

            Validator(usuarioAreaInteresse);

            await _repository.RemoveAsync(usuarioAreaInteresse);
            await _repository.SaveChangesAsync(cancellationToken);

            RemoverUsuarioAreaInteresseCommandResponse response = new RemoverUsuarioAreaInteresseCommandResponse();
            response.UsuarioId = usuarioAreaInteresse.UsuarioId;

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
