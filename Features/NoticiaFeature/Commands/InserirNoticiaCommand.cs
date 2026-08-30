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
        public long? SociedadeId { get; set; }
        public IEnumerable<long>? AreaInteresseMany { get; set; }
    }

    public class InserirNoticiaCommandResponse
    {
        public long Id { get; set; }
        public DateTime DataCadastro { get; set; }

        public string Titulo { get; set; }
        public string Resumo { get; set; }
        public string Conteudo { get; set; }
        public long UsuarioCadastroId { get; set; }
        public long? SociedadeId { get; set; }
        public IEnumerable<long>? AreaInteresseMany { get; set; }
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

            Validator(request);

            Noticia noticia = request.ToDomain();

            await _repository.AddAsync(noticia, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            if (request.AreaInteresseMany is not null)
            {
                IEnumerable<NoticiaAreaInteresse> noticiaAreaInteresseMany = request.AreaInteresseMany
                    .Select(areaInteresseId => new NoticiaAreaInteresse
                    {
                        NoticiaId = noticia.Id,
                        AreaInteresseId = areaInteresseId,
                        DataCadastro = DateTime.Now
                    });

                await _repositoryNoticiaAreaInteresse.AddCollectionAsync(noticiaAreaInteresseMany, cancellationToken);
                await _repositoryNoticiaAreaInteresse.SaveChangesAsync(cancellationToken);
            }

            InserirNoticiaCommandResponse response = new InserirNoticiaCommandResponse();
            response.DataCadastro = noticia.DataCadastro;
            response.Id = noticia.Id;

            response.AreaInteresseMany = request.AreaInteresseMany;
            response.Titulo = request.Titulo;
            response.Resumo = request.Resumo;
            response.Conteudo = request.Conteudo;
            response.UsuarioCadastroId = noticia.UsuarioCadastroId;
            response.SociedadeId = noticia.SociedadeId;

            return response;
        }

        private void Validator
        (
            InserirNoticiaCommand request
        )
        {
            if (String.IsNullOrEmpty(request.Titulo)) throw new ArgumentNullException(MessageHelper.NullFor<InserirNoticiaCommand>(item => item.Titulo));
            if (String.IsNullOrEmpty(request.Resumo)) throw new ArgumentNullException(MessageHelper.NullFor<InserirNoticiaCommand>(item => item.Resumo));
            if (String.IsNullOrEmpty(request.Conteudo)) throw new ArgumentNullException(MessageHelper.NullFor<InserirNoticiaCommand>(item => item.Conteudo));
        }
    }
}
