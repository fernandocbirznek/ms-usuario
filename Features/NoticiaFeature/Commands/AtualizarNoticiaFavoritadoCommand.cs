using MediatR;
using ms_usuario.Domains;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.NoticiaFeature.Commands
{
    public class AtualizarNoticiaFavoritadoCommand : IRequest<AtualizarNoticiaFavoritadoCommandResponse>
    {
        public long Id { get; set; }
        public bool Adicionar {  get; set; }
    }

    public class AtualizarNoticiaFavoritadoCommandResponse
    {
        public DateTime DataAtualizacao { get; set; }
    }

    public class AtualizarNoticiaFavoritadoHandler :
        IRequestHandler<AtualizarNoticiaFavoritadoCommand, AtualizarNoticiaFavoritadoCommandResponse>
    {
        private readonly IRepository<Noticia> _repository;

        public AtualizarNoticiaFavoritadoHandler
        (
            IRepository<Noticia> repository
        )
        {
            _repository = repository;
        }

        public async Task<AtualizarNoticiaFavoritadoCommandResponse> Handle
        (
            AtualizarNoticiaFavoritadoCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<AtualizarNoticiaFavoritadoCommand>());

            await Validator(request, cancellationToken);

            Noticia noticia = await GetFirstAsync(request, cancellationToken);
            noticia.Favoritado = request.Adicionar ? noticia.Favoritado + 1 : noticia.Favoritado - 1;
            noticia.DataAtualizacao = DateTime.Now;

            await _repository.UpdateAsync(noticia);
            await _repository.SaveChangesAsync(cancellationToken);

            AtualizarNoticiaFavoritadoCommandResponse response = new AtualizarNoticiaFavoritadoCommandResponse();
            response.DataAtualizacao = noticia.DataAtualizacao;

            return response;
        }

        private async Task Validator
        (
            AtualizarNoticiaFavoritadoCommand request,
            CancellationToken cancellationToken
        )
        {

            if (!await ExistsNoticiaAsync(request, cancellationToken)) throw new ArgumentNullException("Noticia não existe");
        }

        private async Task<Noticia> GetFirstAsync
        (
            AtualizarNoticiaFavoritadoCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repository.GetFirstAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken
                );
        }

        private async Task<bool> ExistsNoticiaAsync
        (
            AtualizarNoticiaFavoritadoCommand request,
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
