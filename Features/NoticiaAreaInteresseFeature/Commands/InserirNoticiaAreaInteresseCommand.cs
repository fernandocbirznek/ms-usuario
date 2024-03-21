using MediatR;
using ms_usuario.Domains;
using ms_usuario.Extensions;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.NoticiaAreaInteresseFeature.Commands
{
    public class InserirNoticiaAreaInteresseCommand : IRequest<InserirNoticiaAreaInteresseCommandResponse>
    {
        public long NoticiaId { get; set; }
        public long AreaInteresseId { get; set; }
    }


    public class InserirNoticiaAreaInteresseCommandResponse
    {
        public long Id { get; set; }
        public DateTime DataCadastro { get; set; }
    }

    public class InserirNoticiaAreaInteresseHandler : IRequestHandler<InserirNoticiaAreaInteresseCommand, InserirNoticiaAreaInteresseCommandResponse>
    {
        private readonly IRepository<NoticiaAreaInteresse> _repository;
        private readonly IRepository<AreaInteresse> _repositoryAreaInteresse;
        private readonly IRepository<Noticia> _repositoryNoticia;

        public InserirNoticiaAreaInteresseHandler
        (
            IRepository<NoticiaAreaInteresse> repository,
            IRepository<AreaInteresse> repositoryAreaInteresse,
            IRepository<Noticia> repositoryNoticia
        )
        {
            _repository = repository;
            _repositoryAreaInteresse = repositoryAreaInteresse;
            _repositoryNoticia = repositoryNoticia;
        }

        public async Task<InserirNoticiaAreaInteresseCommandResponse> Handle
        (
            InserirNoticiaAreaInteresseCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<InserirNoticiaAreaInteresseCommand>());

            await Validator(request, cancellationToken);

            NoticiaAreaInteresse noticiaAreaInteresse = request.ToDomain();

            await _repository.AddAsync(noticiaAreaInteresse, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            InserirNoticiaAreaInteresseCommandResponse response = new InserirNoticiaAreaInteresseCommandResponse();
            response.DataCadastro = noticiaAreaInteresse.DataCadastro;
            response.Id = noticiaAreaInteresse.Id;

            return response;
        }

        private async Task Validator
        (
            InserirNoticiaAreaInteresseCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request.NoticiaId <= 0) throw new ArgumentNullException(MessageHelper.NullFor<InserirNoticiaAreaInteresseCommand>(item => item.NoticiaId));
            if (request.AreaInteresseId <= 0) throw new ArgumentNullException(MessageHelper.NullFor<InserirNoticiaAreaInteresseCommand>(item => item.AreaInteresseId));
            if (!(await ExistsAreaInteresseAsync(request, cancellationToken))) throw new ArgumentNullException("Área interesse não encontrado");
        }

        private async Task<bool> ExistsAreaInteresseAsync
        (
            InserirNoticiaAreaInteresseCommand request,
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
