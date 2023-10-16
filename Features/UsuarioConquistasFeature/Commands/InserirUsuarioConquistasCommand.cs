using MediatR;
using ms_usuario.Domains;
using ms_usuario.Extensions;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.UsuarioConquistasFeature.Commands
{
    public class InserirUsuarioConquistasCommand : IRequest<InserirUsuarioConquistasCommandResponse>
    {
        public long UsuarioId { get; set; }
        public long ConquistaId { get; set; }
    }


    public class InserirUsuarioConquistasCommandResponse
    {
        public long Id { get; set; }
        public DateTime DataCadastro { get; set; }
    }

    public class InserirUsuarioConquistasHandler : IRequestHandler<InserirUsuarioConquistasCommand, InserirUsuarioConquistasCommandResponse>
    {
        private readonly IRepository<UsuarioConquistas> _repository;
        private readonly IRepository<Conquistas> _repositoryConquistas;
        private readonly IRepository<Usuario> _repositoryUsuario;

        public InserirUsuarioConquistasHandler
        (
            IRepository<UsuarioConquistas> repository,
            IRepository<Conquistas> repositoryConquistas,
            IRepository<Usuario> repositoryUsuario
        )
        {
            _repository = repository;
            _repositoryConquistas = repositoryConquistas;
            _repositoryUsuario = repositoryUsuario;
        }

        public async Task<InserirUsuarioConquistasCommandResponse> Handle
        (
            InserirUsuarioConquistasCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<InserirUsuarioConquistasCommand>());

            await Validator(request, cancellationToken);

            UsuarioConquistas usuarioConquistas = request.ToDomain();

            await _repository.AddAsync(usuarioConquistas, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            InserirUsuarioConquistasCommandResponse response = new InserirUsuarioConquistasCommandResponse();
            response.DataCadastro = usuarioConquistas.DataCadastro;
            response.Id = usuarioConquistas.Id;

            return response;
        }

        private async Task Validator
        (
            InserirUsuarioConquistasCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request.UsuarioId <= 0) throw new ArgumentNullException(MessageHelper.NullFor<InserirUsuarioConquistasCommand>(item => item.UsuarioId));
            if (request.ConquistaId <= 0) throw new ArgumentNullException(MessageHelper.NullFor<InserirUsuarioConquistasCommand>(item => item.ConquistaId));
            if (!(await ExistsUsuarioAsync(request, cancellationToken))) throw new ArgumentNullException("Usuário não encontrado");
            if (!(await ExistsConquistasAsync(request, cancellationToken))) throw new ArgumentNullException("Conquista não encontrada");
        }

        private async Task<bool> ExistsUsuarioAsync
        (
            InserirUsuarioConquistasCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repositoryUsuario.ExistsAsync
                (
                    item => item.Id.Equals(request.UsuarioId),
                    cancellationToken
                );
        }

        private async Task<bool> ExistsConquistasAsync
        (
            InserirUsuarioConquistasCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repositoryConquistas.ExistsAsync
                (
                    item => item.Id.Equals(request.ConquistaId),
                    cancellationToken
                );
        }
    }
}
