using MediatR;
using ms_usuario.Domains;
using ms_usuario.Extensions;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.UsuarioAreaInteresseFeature.Commands
{
    public class InserirUsuarioAreaInteresseCommand : IRequest<InserirUsuarioAreaInteresseCommandResponse>
    {
        public long UsuarioId { get; set; }
        public long AreaInteresseId { get; set; }
    }


    public class InserirUsuarioAreaInteresseCommandResponse
    {
        public long Id { get; set; }
        public DateTime DataCadastro { get; set; }
    }

    public class InserirUsuarioAreaInteresseHandler : IRequestHandler<InserirUsuarioAreaInteresseCommand, InserirUsuarioAreaInteresseCommandResponse>
    {
        private readonly IRepository<UsuarioAreaInteresse> _repository;
        private readonly IRepository<AreaInteresse> _repositoryAreaInteresse;
        private readonly IRepository<Usuario> _repositoryUsuario;

        public InserirUsuarioAreaInteresseHandler
        (
            IRepository<UsuarioAreaInteresse> repository,
            IRepository<AreaInteresse> repositoryAreaInteresse,
            IRepository<Usuario> repositoryUsuario
        )
        {
            _repository = repository;
            _repositoryAreaInteresse = repositoryAreaInteresse;
            _repositoryUsuario = repositoryUsuario;
        }

        public async Task<InserirUsuarioAreaInteresseCommandResponse> Handle
        (
            InserirUsuarioAreaInteresseCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<InserirUsuarioAreaInteresseCommand>());

            await Validator(request, cancellationToken);

            UsuarioAreaInteresse usuarioAreaInteresse = request.ToDomain();

            await _repository.AddAsync(usuarioAreaInteresse, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            InserirUsuarioAreaInteresseCommandResponse response = new InserirUsuarioAreaInteresseCommandResponse();
            response.DataCadastro = usuarioAreaInteresse.DataCadastro;
            response.Id = usuarioAreaInteresse.Id;

            return response;
        }

        private async Task Validator
        (
            InserirUsuarioAreaInteresseCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request.UsuarioId <= 0) throw new ArgumentNullException(MessageHelper.NullFor<InserirUsuarioAreaInteresseCommand>(item => item.UsuarioId));
            if (request.AreaInteresseId <= 0) throw new ArgumentNullException(MessageHelper.NullFor<InserirUsuarioAreaInteresseCommand>(item => item.AreaInteresseId));
            if (!(await ExistsUsuarioAsync(request, cancellationToken))) throw new ArgumentNullException("Usuário não encontrado");
            if (!(await ExistsAreaInteresseAsync(request, cancellationToken))) throw new ArgumentNullException("Área interesse não encontrado");
        }

        private async Task<bool> ExistsUsuarioAsync
        (
            InserirUsuarioAreaInteresseCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repositoryUsuario.ExistsAsync
                (
                    item => item.Id.Equals(request.UsuarioId),
                    cancellationToken
                );
        }

        private async Task<bool> ExistsAreaInteresseAsync
        (
            InserirUsuarioAreaInteresseCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repositoryAreaInteresse.ExistsAsync
                (
                    item => item.Id.Equals(request.AreaInteresseId),
                    cancellationToken
                );
        }
    }
}
