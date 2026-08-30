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
        public long? SociedadeId { get; set; }
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
        public long? SociedadeId { get; set; }
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
            AtualizarDados(noticia, request);

            await _repository.UpdateAsync(noticia);
            await _repository.SaveChangesAsync(cancellationToken);

            List<long> areaInteresseIds = (request.AreaInteresseMany ?? Enumerable.Empty<long>()).ToList();
            await SincronizarAreasInteresseAsync(request, areaInteresseIds, cancellationToken);

            IEnumerable<AreaInteresse> areaInteresseMany = await GetAreaInteresseAsync(cancellationToken);
            List<AreaInteresse> areaInteresseResponse = SelecionarAreasInteresse(areaInteresseIds, areaInteresseMany);

            return ToResponse(noticia, request, areaInteresseResponse);
        }

        private static void AtualizarDados(Noticia noticia, AtualizarNoticiaCommand request)
        {
            noticia.Titulo = request.Titulo;
            noticia.Resumo = request.Resumo;
            noticia.Conteudo = request.Conteudo;
            noticia.SociedadeId = request.SociedadeId;
            noticia.DataAtualizacao = DateTime.Now;
        }

        private async Task SincronizarAreasInteresseAsync
        (
            AtualizarNoticiaCommand request,
            IReadOnlyCollection<long> areaInteresseIds,
            CancellationToken cancellationToken
        )
        {
            List<NoticiaAreaInteresse> noticiaAreaInteresseMany =
                (await GetNoticiaAreaInteresseAsync(request, cancellationToken)).ToList();
            HashSet<long> areaInteresseIdSet = areaInteresseIds.ToHashSet();
            HashSet<long> areaInteresseExistenteIds = noticiaAreaInteresseMany
                .Select(item => item.AreaInteresseId)
                .ToHashSet();

            List<NoticiaAreaInteresse> noticiaAreaInteresseInserir = areaInteresseIds
                .Where(areaInteresseId => !areaInteresseExistenteIds.Contains(areaInteresseId))
                .Select(areaInteresseId => new NoticiaAreaInteresse
                {
                    AreaInteresseId = areaInteresseId,
                    NoticiaId = request.Id
                })
                .ToList();

            foreach (NoticiaAreaInteresse noticiaAreaInteresse in noticiaAreaInteresseMany)
            {
                if (!areaInteresseIdSet.Contains(noticiaAreaInteresse.AreaInteresseId))
                    await _repositoryNoticiaAreaInteresse.RemoveAsync(noticiaAreaInteresse);
            }

            if (noticiaAreaInteresseInserir.Count > 0)
                await _repositoryNoticiaAreaInteresse.AddCollectionAsync(noticiaAreaInteresseInserir, cancellationToken);

            await _repositoryNoticiaAreaInteresse.SaveChangesAsync(cancellationToken);
        }

        private static List<AreaInteresse> SelecionarAreasInteresse
        (
            IEnumerable<long> areaInteresseIds,
            IEnumerable<AreaInteresse> areaInteresseMany
        )
        {
            return areaInteresseIds
                .Select(areaInteresseId => areaInteresseMany.First(area => area.Id.Equals(areaInteresseId)))
                .ToList();
        }

        private static AtualizarNoticiaCommandResponse ToResponse
        (
            Noticia noticia,
            AtualizarNoticiaCommand request,
            IEnumerable<AreaInteresse> areaInteresseMany
        )
        {
            return new AtualizarNoticiaCommandResponse
            {
                Id = request.Id,
                DataCadastro = noticia.DataCadastro,
                DataAtualizacao = noticia.DataAtualizacao.GetValueOrDefault(),
                AreaInteresseMany = areaInteresseMany,
                Titulo = request.Titulo,
                Resumo = request.Resumo,
                Conteudo = request.Conteudo,
                UsuarioCadastroId = noticia.UsuarioCadastroId,
                SociedadeId = noticia.SociedadeId
            };
        }

        private async Task Validator
        (
            AtualizarNoticiaCommand request,
            CancellationToken cancellationToken
        )
        {
            if (String.IsNullOrEmpty(request.Titulo)) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarNoticiaCommand>(item => item.Titulo));
            if (String.IsNullOrEmpty(request.Resumo)) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarNoticiaCommand>(item => item.Resumo));
            if (String.IsNullOrEmpty(request.Conteudo)) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarNoticiaCommand>(item => item.Conteudo));
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
                ) ?? throw new ArgumentNullException("Noticia não existe");
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
