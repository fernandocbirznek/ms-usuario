using MediatR;
using ms_usuario.Domains;
using ms_usuario.Extensions;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.UsuarioFeature.Commands
{
    public class AtualizarUsuarioCommand : IRequest<AtualizarUsuarioCommandResponse>
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public int TipoUsuario { get; set; }
        public long? SociedadeId { get; set; }
    }

    public class AtualizarUsuarioCommandResponse
    {
        public DateTime DataAtualizacao { get; set; }
    }

    public class AtualizarUsuarioHandler : IRequestHandler<AtualizarUsuarioCommand, AtualizarUsuarioCommandResponse>
    {
        private readonly IRepository<Usuario> _repository;
        private readonly IRepository<Sociedade> _repositorySociedade;

        public AtualizarUsuarioHandler
        (
            IRepository<Usuario> repository,
            IRepository<Sociedade> repositorySociedade
        )
        {
            _repository = repository;
            _repositorySociedade = repositorySociedade;
        }

        public async Task<AtualizarUsuarioCommandResponse> Handle
        (
            AtualizarUsuarioCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<AtualizarUsuarioCommand>());

            await Validator(request, cancellationToken);

            Usuario usuario = await GetFirstAsync(request, cancellationToken);
            request.ToDomain(usuario);

            await _repository.UpdateAsync(usuario);
            await _repository.SaveChangesAsync(cancellationToken);

            AtualizarUsuarioCommandResponse response = new AtualizarUsuarioCommandResponse();
            response.DataAtualizacao = usuario.DataAtualizacao;

            return response;
        }

        private async Task Validator
        (
            AtualizarUsuarioCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request.Id <= 0) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarUsuarioCommand>(item => item.Id));
            if (String.IsNullOrEmpty(request.Nome)) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarUsuarioCommand>(item => item.Nome));
            if (String.IsNullOrEmpty(request.Senha)) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarUsuarioCommand>(item => item.Senha));
            if (String.IsNullOrEmpty(request.Email)) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarUsuarioCommand>(item => item.Email));
            if (request.TipoUsuario <= 0) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarUsuarioCommand>(item => item.TipoUsuario));
            if (!(await ExistsAsync(request, cancellationToken))) throw new ArgumentNullException("Usuario não encontrado");
            if (await ExistsEmailAsync(request, cancellationToken)) throw new ArgumentNullException("Email já cadastrado");
            if (!await ExistsSociedadeAsync(request, cancellationToken)) throw new ArgumentNullException("Sociedade não existe");
        }

        private async Task<Usuario> GetFirstAsync
        (
            AtualizarUsuarioCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repository.GetFirstAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken
                );
        }

        private async Task<bool> ExistsAsync
        (
            AtualizarUsuarioCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repository.ExistsAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken
                );
        }

        private async Task<bool> ExistsEmailAsync
        (
            AtualizarUsuarioCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repository.ExistsAsync
                (
                    item => item.Email.Equals(request.Email) &&
                    !item.Id.Equals(request.Id),
                    cancellationToken
                );
        }

        private async Task<bool> ExistsSociedadeAsync
        (
            AtualizarUsuarioCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repositorySociedade.ExistsAsync
                (
                    item => item.Id.Equals(request.SociedadeId),
                    cancellationToken
                );
        }
    }
}
