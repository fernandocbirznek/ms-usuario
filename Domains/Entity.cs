namespace ms_usuario.Domains
{
    public class Entity
    {
        public long Id { get; set; }
        public Guid IdExterno { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public DateTime? DataRemocao { get; set; }
    }
}
