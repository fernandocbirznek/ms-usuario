namespace ms_usuario.Domains
{
    public class Conquistas : Entity
    {
        public string Nome { get; set; }
        public virtual ICollection<UsuarioConquistas>? UsuarioConquistas { get; set; }
    }
}
