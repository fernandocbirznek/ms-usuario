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
    public class LoginCommand : IRequest<LoginCommandResponse>
    {
        public string Email { get; set; }
        public string Senha { get; set; }
    }

    public class LoginCommandResponse : Entity
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
    }

    public class LoginHandler : IRequestHandler<LoginCommand, LoginCommandResponse>
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<Usuario> _repository;

        public LoginHandler
        (
            IConfiguration configuration,
            IRepository<Usuario> repository
        )
        {
            _configuration = configuration;
            _repository = repository;
        }

        public async Task<LoginCommandResponse> Handle
        (
            LoginCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<LoginCommand>());

            await Validator(request, cancellationToken);

            Usuario usuario = await _repository.GetSingleAsync
                (
                    item => item.Email.Equals(request.Email),
                    cancellationToken,
                    item => item.Perfil
                );
            string hash = request.ObterHash(usuario.Salt);

            if (hash != usuario.Senha)
                throw new InvalidOperationException("Senha incorreta.");

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

            LoginCommandResponse response = request.ToLoginResponse(usuario);
            response.Token = new JwtSecurityTokenHandler().WriteToken(token);

            return response;
        }

        private async Task Validator
        (
            LoginCommand request,
            CancellationToken cancellationToken
        )
        {
            if (String.IsNullOrEmpty(request.Email)) throw new ArgumentNullException(MessageHelper.NullFor<LoginCommand>(item => item.Email));
            if (String.IsNullOrEmpty(request.Senha)) throw new ArgumentNullException(MessageHelper.NullFor<LoginCommand>(item => item.Senha));
            if (!await ExistsUsuarioAsync(request, cancellationToken)) throw new ArgumentNullException("Usuário não encontrado");
        }

        private async Task<bool> ExistsUsuarioAsync
        (
            LoginCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repository.ExistsAsync
                (
                    item => item.Email.Equals(request.Email),
                    cancellationToken
                );
        }
    }
}
