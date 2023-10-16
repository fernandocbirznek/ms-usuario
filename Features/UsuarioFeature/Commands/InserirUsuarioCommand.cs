using MediatR;
using ms_usuario.Domains;
using ms_usuario.Extensions;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.UsuarioFeature.Commands
{
    public class InserirUsuarioCommand : IRequest<InserirUsuarioCommandResponse>
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
    }

    public class InserirUsuarioCommandResponse
    {
        public long Id { get; set; }
        public DateTime DataCadastro { get; set; }
    }

    public class InserirUsuarioHandler : IRequestHandler<InserirUsuarioCommand, InserirUsuarioCommandResponse>
    {
        private readonly IRepository<Usuario> _repository;

        public InserirUsuarioHandler
        (
            IRepository<Usuario> repository
        )
        {
            _repository = repository;
        }

        public async Task<InserirUsuarioCommandResponse> Handle
        (
            InserirUsuarioCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<InserirUsuarioCommand>());

            await Validator(request, cancellationToken);

            Usuario usuario = request.ToDomain();

            await _repository.AddAsync(usuario, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            InserirUsuarioCommandResponse response = new InserirUsuarioCommandResponse();
            response.DataCadastro = usuario.DataCadastro;
            response.Id = usuario.Id;

            return response;
        }

        private async Task Validator
        (
            InserirUsuarioCommand request,
            CancellationToken cancellationToken
        )
        {
            if (String.IsNullOrEmpty(request.Nome)) throw new ArgumentNullException(MessageHelper.NullFor<InserirUsuarioCommand>(item => item.Nome));
            if (String.IsNullOrEmpty(request.Email)) throw new ArgumentNullException(MessageHelper.NullFor<InserirUsuarioCommand>(item => item.Email));
            if (String.IsNullOrEmpty(request.Senha)) throw new ArgumentNullException(MessageHelper.NullFor<InserirUsuarioCommand>(item => item.Senha));
            if (await ExistsEmailAsync(request, cancellationToken)) throw new ArgumentNullException("Email já cadastrado");
        }

        private async Task<bool> ExistsEmailAsync
        (
            InserirUsuarioCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repository.ExistsAsync
                (
                    item => item.Email.Equals(request.Email),
                    cancellationToken
                );
        }
    }
}
