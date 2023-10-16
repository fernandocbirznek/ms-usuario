using ms_usuario.Domains;
using ms_usuario.Features.AreaInteresseFeature.Commands;

namespace ms_usuario.Extensions
{
    public static class AreaInteresseExtensions
    {
        public static AreaInteresse ToDomain(this InserirAreaInteresseCommand request)
        {
            return new()
            {
                Nome = request.Nome,
                DataCadastro = DateTime.Now
            };
        }
    }
}
