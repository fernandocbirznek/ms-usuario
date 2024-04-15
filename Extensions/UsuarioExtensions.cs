using ms_usuario.Domains;
using ms_usuario.Features.UsuarioFeature.Commands;
using System.Security.Cryptography;
using System.Text;

namespace ms_usuario.Extensions
{
    public static class UsuarioExtensions
    {
        public static Usuario ToDomain
        (
            this InserirUsuarioCommand request
        )
        {
            string salt = CreateSalt();
            return new()
            {
                Nome = request.Nome,
                Email = request.Email,
                Senha = CriptografarSenha(request.Senha, salt),
                Salt = salt,
                TipoUsuario = 1,
                DataCadastro = DateTime.Now,
                Perfil = new()
            };
        }

        public static void ToDomain
        (
            this AtualizarUsuarioCommand request,
            Usuario usuario
        )
        {
            //string salt = CreateSalt();
            usuario.Nome = request.Nome;
            usuario.Email = request.Email;
            //usuario.Senha = CriptografarSenha(request.Senha, salt);
            //usuario.Salt = salt;
            usuario.TipoUsuario = request.TipoUsuario;
            //usuario.SociedadeId = request.SociedadeId;
            usuario.DataAtualizacao = DateTime.Now;
        }

        public static string ObterHash
        (
            this LoginCommand request,
            string salt
        )
        {
            return Sha256(request.Senha + salt);
        }

        public static LoginCommandResponse ToLoginResponse
        (
            this LoginCommand request,
            List<AreaInteresse> areaInteresseMany,
            Usuario usuario
        )
        {
            return new()
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                TipoUsuario = usuario.TipoUsuario,
                ComentarioForum = usuario.ComentarioForum,
                TopicoForum = usuario.TopicoForum,
                ComentarioAula = usuario.ComentarioAula,
                CurtirAula = usuario.CurtirAula,
                NoticiaVisualizada = usuario.NoticiaVisualizada,
                UsuarioPerfilId = usuario.PerfilId,
                DataNascimento = usuario.Perfil.DataNascimento,
                Foto = usuario.Perfil.Foto,
                Hobbie = usuario.Perfil.Hobbie,
                SociedadeId = usuario.SociedadeId,
                UsuarioAreaInteresses = areaInteresseMany,
               // UsuarioConquistas = usuario.UsuarioConquistas
            };
        }

        private static string CriptografarSenha(string senha, string salt)
        {
            return Sha256(senha + salt);
        }

        private static string Sha256(string randomString)
        {
            var crypt = new SHA256Managed();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash += theByte.ToString("x2");
            }
            return hash;
        }

        private static string CreateSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[32];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }
    }
}
