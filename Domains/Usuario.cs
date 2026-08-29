using ms_usuario.Domains.Enums;

namespace ms_usuario.Domains
{
    public class Usuario : Entity
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Salt { get; set; }
        public TipoUsuarioEnum TipoUsuario { get; set; }
        public virtual ICollection<UsuarioAreaInteresse> UsuarioAreaInteresses { get; set; }
        public virtual ICollection<UsuarioConquistas> UsuarioConquistas { get; set; }
        public long ComentarioForum { get; set; }
        public long TopicoForum { get; set; }
        public long ComentarioAula { get; set; }
        public long CurtirAula { get; set; }
        public long NoticiaVisualizada { get; set; }
        public long PerfilId { get; set; }

        private UsuarioPerfil _Perfil;
        public virtual UsuarioPerfil Perfil
        {
            get { return _Perfil; }
            set { _Perfil = value; SetPerfil(value); }
        }

        private void SetPerfil(UsuarioPerfil value)
        {
            PerfilId = value is null ? 0 : value.Id;
        }

        public virtual ICollection<UsuarioSociedade> UsuarioSociedades { get; set; }
        public virtual ICollection<Sociedade> SociedadesLideradas { get; set; }
    }
}
