using MediatR;
using ms_usuario.Domains;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.SociedadeFeature.Commands
{
    public class AtualizarSociedadeCommand : IRequest<AtualizarSociedadeCommandResponse>
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public long UsuarioLiderId { get; set; }
    }

    public class AtualizarSociedadeCommandResponse
    {
        public long Id { get; set; }
        public DateTime DataAtualizacao { get; set; }

        public string Nome { get; set; }
        public string Descricao { get; set; }
        public long UsuarioLiderId { get; set; }
    }

    public class AtualizarSociedadeHandler : IRequestHandler<AtualizarSociedadeCommand, AtualizarSociedadeCommandResponse>
    {
        private readonly IRepository<Sociedade> _repository;

        public AtualizarSociedadeHandler
        (
            IRepository<Sociedade> repository
        )
        {
            _repository = repository;
        }

        public async Task<AtualizarSociedadeCommandResponse> Handle
        (
            AtualizarSociedadeCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<AtualizarSociedadeCommand>());

            await Validator(request, cancellationToken);

            Sociedade sociedade = await GetFirstAsync(request, cancellationToken);
            sociedade.Nome = request.Nome;
            sociedade.Descricao = request.Descricao;
            sociedade.UsuarioLiderId = request.UsuarioLiderId;

            await _repository.UpdateAsync(sociedade);
            await _repository.SaveChangesAsync(cancellationToken);

            AtualizarSociedadeCommandResponse response = new AtualizarSociedadeCommandResponse();
            response.DataAtualizacao = sociedade.DataAtualizacao.GetValueOrDefault();
            response.Id = request.Id;
            response.Nome = request.Nome;
            response.Descricao = request.Descricao;
            response.UsuarioLiderId = request.UsuarioLiderId;

            return response;
        }

        private async Task Validator
        (
            AtualizarSociedadeCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request.Id <= 0) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarSociedadeCommand>(item => item.Id));
            if (String.IsNullOrEmpty(request.Nome)) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarSociedadeCommand>(item => item.Nome));
            if (String.IsNullOrEmpty(request.Descricao)) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarSociedadeCommand>(item => item.Descricao));
            if (!(await ExistsAsync(request, cancellationToken))) throw new ArgumentNullException("Sociedade não encontrada");
            if (await ExistsNomeAsync(request, cancellationToken)) throw new ArgumentNullException("Sociedade já cadastrada");
            if (request.UsuarioLiderId <= 0) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarSociedadeCommand>(item => item.UsuarioLiderId));
        }

        private async Task<Sociedade> GetFirstAsync
        (
            AtualizarSociedadeCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repository.GetFirstAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken
                ) ?? throw new ArgumentNullException("Sociedade não encontrada");
        }

        private async Task<bool> ExistsAsync
        (
            AtualizarSociedadeCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repository.ExistsAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken
                );
        }

        private async Task<bool> ExistsNomeAsync
        (
            AtualizarSociedadeCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repository.ExistsAsync
                (
                    item => item.Nome.ToLower().Trim().Equals(request.Nome.ToLower().Trim()) &&
                    !item.Id.Equals(request.Id),
                    cancellationToken
                );
        }
    }
}
