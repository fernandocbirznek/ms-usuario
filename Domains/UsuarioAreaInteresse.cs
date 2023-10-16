namespace ms_usuario.Domains
{
    public class UsuarioAreaInteresse : Entity
    {
        public long UsuarioId { get; set; }
        private Usuario _Usuario;
        public virtual Usuario Usuario { get { return _Usuario; } set { _Usuario = value; SetUsuario(value); } }

        private void SetUsuario(Usuario value)
        {
            UsuarioId = value is null ? 0 : value.Id;
        }

        public long AreaInteresseId { get; set; }
        private AreaInteresse _AreaInteresse;
        public virtual AreaInteresse AreaInteresse { get { return _AreaInteresse; } set { _AreaInteresse = value; SetAreaInteresse(value); } }

        private void SetAreaInteresse(AreaInteresse value)
        {
            AreaInteresseId = value is null ? 0 : value.Id;
        }
    }
}
