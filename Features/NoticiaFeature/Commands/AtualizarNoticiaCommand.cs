using MediatR;
using ms_usuario.Domains;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.NoticiaFeature.Commands
{
    public class AtualizarNoticiaCommand : IRequest<AtualizarNoticiaCommandResponse>
    {
        public long Id { get; set; }
        public string Titulo { get; set; }
        public string Resumo { get; set; }
        public string Conteudo { get; set; }
        public long UsuarioCadastroId { get; set; }
        public IEnumerable<long>? AreaInteresseMany { get; set; }
    }

    public class AtualizarNoticiaCommandResponse
    {
        public long Id { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
       
        public IEnumerable<AreaInteresse> AreaInteresseMany { get; set; }
        public string Titulo { get; set; }
        public string Resumo { get; set; }
        public string Conteudo { get; set; }
        public long UsuarioCadastroId { get; set; }
    }

    public class AtualizarNoticiaHandler : IRequestHandler<AtualizarNoticiaCommand, AtualizarNoticiaCommandResponse>
    {
        private readonly IRepository<Noticia> _repository;
        private readonly IRepository<AreaInteresse> _repositoryAreaInteresse;
        private readonly IRepository<NoticiaAreaInteresse> _repositoryNoticiaAreaInteresse;

        public AtualizarNoticiaHandler
        (
            IRepository<Noticia> repository,
            IRepository<AreaInteresse> repositoryAreaInteresse,
            IRepository<NoticiaAreaInteresse> repositoryNoticiaAreaInteresse
        )
        {
            _repository = repository;
            _repositoryAreaInteresse = repositoryAreaInteresse;
            _repositoryNoticiaAreaInteresse = repositoryNoticiaAreaInteresse;
        }

        public async Task<AtualizarNoticiaCommandResponse> Handle
        (
            AtualizarNoticiaCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<AtualizarNoticiaCommand>());

            await Validator(request, cancellationToken);

            Noticia noticia = await GetFirstAsync(request, cancellationToken);
            noticia.Titulo = request.Titulo;
            noticia.Resumo = request.Resumo;
            noticia.Conteudo = request.Conteudo;
            noticia.DataAtualizacao = DateTime.Now;

            await _repository.UpdateAsync(noticia);
            await _repository.SaveChangesAsync(cancellationToken);

            IEnumerable<NoticiaAreaInteresse> noticiaAreaInteresseMany = await GetNoticiaAreaInteresseAsync(request, cancellationToken);

            foreach (long areaInteresseId in request.AreaInteresseMany)
            {
                NoticiaAreaInteresse inserirNoticiaAreaInteresse = new NoticiaAreaInteresse();
                inserirNoticiaAreaInteresse.AreaInteresseId = areaInteresseId;
                inserirNoticiaAreaInteresse.NoticiaId = request.Id;

                if (!noticiaAreaInteresseMany.Any(item => item.AreaInteresseId.Equals(inserirNoticiaAreaInteresse.AreaInteresseId)))
                {
                    await _repositoryNoticiaAreaInteresse.UpdateAsync(inserirNoticiaAreaInteresse);
                    await _repositoryNoticiaAreaInteresse.SaveChangesAsync(cancellationToken);
                }
            }

            foreach (NoticiaAreaInteresse noticiaAreaInteresse in noticiaAreaInteresseMany)
            {
                if (!request.AreaInteresseMany.Any(item => item.Equals(noticiaAreaInteresse.AreaInteresseId)))
                {
                    await _repositoryNoticiaAreaInteresse.RemoveAsync(noticiaAreaInteresse);
                    await _repositoryNoticiaAreaInteresse.SaveChangesAsync(cancellationToken);
                }
            }

            IEnumerable<AreaInteresse> areaInteresseMany = await GetAreaInteresseAsync(cancellationToken);

            List<AreaInteresse> areaInteresseResponse = new List<AreaInteresse>();
            foreach (long item in request.AreaInteresseMany)
            {
                AreaInteresse areaInteresse = areaInteresseMany.First(area => area.Id.Equals(item));
                if (areaInteresse is not null)
                    areaInteresseResponse.Add(areaInteresse);
            }

            AtualizarNoticiaCommandResponse response = new AtualizarNoticiaCommandResponse();
            response.Id = request.Id;
            response.DataCadastro = noticia.DataCadastro;
            response.DataAtualizacao = noticia.DataAtualizacao;

            response.AreaInteresseMany = areaInteresseResponse;
            response.Titulo = request.Titulo;
            response.Resumo = request.Resumo;
            response.Conteudo = request.Conteudo;
            response.UsuarioCadastroId = response.UsuarioCadastroId;

            return response;
        }

        private async Task Validator
        (
            AtualizarNoticiaCommand request,
            CancellationToken cancellationToken
        )
        {
            if (String.IsNullOrEmpty(request.Titulo)) throw new ArgumentNullException(MessageHelper.NullFor<InserirNoticiaCommand>(item => item.Titulo));
            if (String.IsNullOrEmpty(request.Resumo)) throw new ArgumentNullException(MessageHelper.NullFor<InserirNoticiaCommand>(item => item.Resumo));
            if (String.IsNullOrEmpty(request.Conteudo)) throw new ArgumentNullException(MessageHelper.NullFor<InserirNoticiaCommand>(item => item.Conteudo));
            if (!await ExistsNoticiaAsync(request, cancellationToken)) throw new ArgumentNullException("Noticia não existe");
        }

        private async Task<Noticia> GetFirstAsync
        (
            AtualizarNoticiaCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repository.GetFirstAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken
                );
        }

        private async Task<IEnumerable<NoticiaAreaInteresse>> GetNoticiaAreaInteresseAsync
        (
            AtualizarNoticiaCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repositoryNoticiaAreaInteresse.GetAsync
                (
                    item => item.NoticiaId.Equals(request.Id),
                    cancellationToken
                );
        }

        private async Task<bool> ExistsNoticiaAsync
        (
            AtualizarNoticiaCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repository.ExistsAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken
                );
        }

        private async Task<IEnumerable<AreaInteresse>> GetAreaInteresseAsync
        (
            CancellationToken cancellationToken
        )
        {
            return await _repositoryAreaInteresse.GetAsync
                (
                    cancellationToken
                );
        }
    }
}
