namespace ms_usuario.Domains
{
    public class Sociedade : Entity
    {
        public string Nome { get; set; }
        public string Descricao{ get; set; }
        public virtual ICollection<Usuario>? Usuario { get; set; }

        public long UsuarioLiderId { get; set; }
        public virtual Usuario UsuarioLider { get; set; }

        public virtual ICollection<UsuarioSociedade> UsuarioSociedades { get; set; }

        public virtual ICollection<Noticia> Noticias { get; set; }
    }
}
