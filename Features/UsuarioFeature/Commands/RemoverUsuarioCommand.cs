﻿using MediatR;
using ms_usuario.Domains;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.UsuarioFeature.Commands
{
    public class RemoverUsuarioCommand : IRequest<long>
    {
        public long Id { get; set; }
    }

    public class RemoverUsuarioCommandHandler : IRequestHandler<RemoverUsuarioCommand, long>
    {
        private readonly IRepository<Usuario> _repository;

        public RemoverUsuarioCommandHandler
        (
            IRepository<Usuario> repository
        )
        {
            _repository = repository;
        }

        public async Task<long> Handle
        (
            RemoverUsuarioCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<RemoverUsuarioCommand>());

            await Validator(request, cancellationToken);

            Usuario usuario = await _repository.GetFirstAsync(item => item.Id.Equals(request.Id), cancellationToken);

            await _repository.RemoveAsync(usuario);
            await _repository.SaveChangesAsync(cancellationToken);

            return usuario.Id;
        }

        private async Task Validator
        (
            RemoverUsuarioCommand request,
            CancellationToken cancellationToken
        )
        {
            if (!(await ExistsAsync(request, cancellationToken))) throw new ArgumentNullException("Usuário não encontrado");
        }

        private async Task<bool> ExistsAsync
        (
            RemoverUsuarioCommand request,
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
