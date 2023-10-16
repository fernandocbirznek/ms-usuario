namespace ms_usuario.Domains
{
    public class Sociedade : Entity
    {
        public string Nome { get; set; }
        public string Descricao{ get; set; }
        public virtual ICollection<Usuario>? Usuario { get; set; }
    }
}
