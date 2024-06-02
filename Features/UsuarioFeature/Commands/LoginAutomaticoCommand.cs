using MediatR;
using Microsoft.IdentityModel.Tokens;
using ms_usuario.Domains;
using ms_usuario.Extensions;
using ms_usuario.Helpers;
using ms_usuario.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ms_usuario.Features.UsuarioFeature.Commands
{
    public class LoginAutomaticoCommand : IRequest<LoginAutomaticoCommandResponse>
    {
        public string Token { get; set; }
    }

    public class LoginAutomaticoCommandResponse : Entity
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public int TipoUsuario { get; set; }
        public long ComentarioForum { get; set; }
        public long TopicoForum { get; set; }
        public long ComentarioAula { get; set; }
        public long CurtirAula { get; set; }
        public long NoticiaVisualizada { get; set; }
        public long? UsuarioPerfilId { get; set; }
        public DateTime? DataNascimento { get; set; }
        public byte[]? Foto { get; set; }
        public string? Hobbie { get; set; }
        public long? SociedadeId { get; set; }
        public string Token { get; set; }
        public virtual IEnumerable<AreaInteresse> UsuarioAreaInteresses { get; set; }
        //public virtual IEnumerable<UsuarioConquistas> UsuarioConquistas { get; set; }
    }

    public class LoginAutomaticoHandler : IRequestHandler<LoginAutomaticoCommand, LoginAutomaticoCommandResponse>
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<AreaInteresse> _repositoryAreaInteresse;
        private readonly IRepository<Usuario> _repository;

        public LoginAutomaticoHandler
        (
            IConfiguration configuration,
            IRepository<AreaInteresse> repositoryAreaInteresse,
            IRepository<Usuario> repository
        )
        {
            _configuration = configuration;
            _repositoryAreaInteresse = repositoryAreaInteresse;
            _repository = repository;
        }

        public async Task<LoginAutomaticoCommandResponse> Handle
        (
            LoginAutomaticoCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<LoginAutomaticoCommand>());

            await Validator(request, cancellationToken);

            IEnumerable<AreaInteresse> areaInteresseMany = await GetAreaInteresseAsync(cancellationToken);

            //DECODIFICAR TOKEN PARA PEGAR UsuarioId
            var handler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["authentication:issuer"],
                ValidAudience = _configuration["authentication:audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["authentication:secret"]!))
            };

            SecurityToken validatedToken;
            ClaimsPrincipal principal = handler.ValidateToken(request.Token, tokenValidationParameters, out validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var usuarioIdClaim = principal.Claims.First(claim => claim.Type == "usuarioId");

            if (usuarioIdClaim == null)
                throw new SecurityTokenException("Invalid token: usuarioId claim not found");

            var usuarioId = long.Parse(usuarioIdClaim.Value);

            //

            Usuario usuario = await _repository.GetSingleAsync
                (
                    item => item.Id.Equals(usuarioId),
                    cancellationToken,
                    item => item.Perfil,
                    item => item.UsuarioAreaInteresses,
                    item => item.UsuarioConquistas
                );

            List<AreaInteresse> usuarioAreaInteresse = new List<AreaInteresse>();
            foreach (UsuarioAreaInteresse item in usuario.UsuarioAreaInteresses)
            {
                AreaInteresse areaInteresse = areaInteresseMany.First(area => area.Id.Equals(item.AreaInteresseId));
                if (areaInteresse is not null)
                    usuarioAreaInteresse.Add(areaInteresse);
            }

            string chaveJwt = _configuration.GetValue<string>("authentication:secret")!;

            IList<Claim> claims = new List<Claim>
            {
                new("usuarioId", usuario.Id.ToString())
            };

            JwtSecurityToken token = new
                (
                    issuer: _configuration.GetValue<string>("authentication:issuer"),
                    audience: _configuration.GetValue<string>("authentication:audience"),
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("authentication:timeout")),
                    signingCredentials: new SigningCredentials
                        (
                            new SymmetricSecurityKey
                            (
                                Encoding.ASCII.GetBytes(chaveJwt)
                            ),
                            SecurityAlgorithms.HmacSha256
                        )
                );

            LoginAutomaticoCommandResponse response = request.ToLoginAutomaticoResponse(usuarioAreaInteresse, usuario);
            response.Token = new JwtSecurityTokenHandler().WriteToken(token);

            return response;
        }

        private async Task Validator
        (
            LoginAutomaticoCommand request,
            CancellationToken cancellationToken
        )
        {
            if (String.IsNullOrEmpty(request.Token)) throw new ArgumentNullException(MessageHelper.NullFor<LoginAutomaticoCommand>(item => item.Token));
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