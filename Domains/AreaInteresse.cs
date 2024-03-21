namespace ms_usuario.Domains
{
    public class AreaInteresse : Entity
    {
        public string Nome { get; set; }
        public virtual ICollection<UsuarioAreaInteresse>? UsuarioAreaInteresses { get; set; }
        public virtual ICollection<NoticiaAreaInteresse>? NoticiaAreaInteresseMany { get; set; }
    }
}
