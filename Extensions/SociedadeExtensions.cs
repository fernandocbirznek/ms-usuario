using ms_usuario.Domains;
using ms_usuario.Features.SociedadeFeature.Commands;

namespace ms_usuario.Extensions
{
    public static class SociedadeExtensions
    {
        public static Sociedade ToDomain(this InserirSociedadeCommand request)
        {
            return new()
            {
                Nome = request.Nome,
                Descricao = request.Descricao,
                DataCadastro = DateTime.Now
            };
        }
    }
}
