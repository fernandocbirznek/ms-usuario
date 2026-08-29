namespace ms_usuario.Domains
{
    public class UsuarioSociedade : Entity
    {
        public long UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }

        public long SociedadeId { get; set; }
        public virtual Sociedade Sociedade { get; set; }

        public DateTime DataEntrada { get; set; }
    }
}
