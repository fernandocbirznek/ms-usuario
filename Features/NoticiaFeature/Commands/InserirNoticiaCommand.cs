using MediatR;
using ms_usuario.Domains;
using ms_usuario.Extensions;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.NoticiaFeature.Commands
{
    public class InserirNoticiaCommand : IRequest<InserirNoticiaCommandResponse>
    {
        public string Titulo { get; set; }
        public string Resumo { get; set; }
        public string Conteudo { get; set; }
        public long UsuarioCadastroId { get; set; }
        public IEnumerable<long>? AreaInteresseMany { get; set; }
    }

    public class InserirNoticiaCommandResponse
    {
        public long Id { get; set; }
        public DateTime DataCadastro { get; set; }
    }

    public class InserirNoticiaHandler : IRequestHandler<InserirNoticiaCommand, InserirNoticiaCommandResponse>
    {
        private readonly IRepository<Noticia> _repository;
        private readonly IRepository<NoticiaAreaInteresse> _repositoryNoticiaAreaInteresse;

        public InserirNoticiaHandler
        (
            IRepository<Noticia> repository,
            IRepository<NoticiaAreaInteresse> repositoryNoticiaAreaInteresse
        )
        {
            _repository = repository;
            _repositoryNoticiaAreaInteresse = repositoryNoticiaAreaInteresse;
        }

        public async Task<InserirNoticiaCommandResponse> Handle
        (
            InserirNoticiaCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<InserirNoticiaCommand>());

            await Validator(request, cancellationToken);

            Noticia noticia = request.ToDomain();

            await _repository.AddAsync(noticia, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            if (request.AreaInteresseMany is not null)
                foreach (long noticiaAreaInteresse in request.AreaInteresseMany)
                {
                    NoticiaAreaInteresse inserirNoticiaAreaInteresse = new()
                    {
                        NoticiaId = noticia.Id,
                        AreaInteresseId = noticiaAreaInteresse,
                        DataCadastro = DateTime.Now
                    }; ;

                    await _repositoryNoticiaAreaInteresse.AddAsync(inserirNoticiaAreaInteresse, cancellationToken);
                    await _repositoryNoticiaAreaInteresse.SaveChangesAsync(cancellationToken);
                }

            InserirNoticiaCommandResponse response = new InserirNoticiaCommandResponse();
            response.DataCadastro = noticia.DataCadastro;
            response.Id = noticia.Id;

            return response;
        }

        private async Task Validator
        (
            InserirNoticiaCommand request,
            CancellationToken cancellationToken
        )
        {
            if (String.IsNullOrEmpty(request.Titulo)) throw new ArgumentNullException(MessageHelper.NullFor<InserirNoticiaCommand>(item => item.Titulo));
            if (String.IsNullOrEmpty(request.Resumo)) throw new ArgumentNullException(MessageHelper.NullFor<InserirNoticiaCommand>(item => item.Resumo));
            if (String.IsNullOrEmpty(request.Conteudo)) throw new ArgumentNullException(MessageHelper.NullFor<InserirNoticiaCommand>(item => item.Conteudo));
        }
    }
}
