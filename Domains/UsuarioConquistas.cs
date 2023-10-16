namespace ms_usuario.Domains
{
    public class UsuarioConquistas : Entity
    {
        public long UsuarioId { get; set; }
        private Usuario _Usuario;
        public virtual Usuario Usuario { get { return _Usuario; } set { _Usuario = value; SetUsuario(value); } }

        private void SetUsuario(Usuario value)
        {
            UsuarioId = value is null ? 0 : value.Id;
        }

        public long ConquistaId { get; set; }
        private Conquistas _Conquista;
        public virtual Conquistas Conquista { get { return _Conquista; } set { _Conquista = value; SetConquista(value); } }

        private void SetConquista(Conquistas value)
        {
            ConquistaId = value is null ? 0 : value.Id;
        }
    }
}
