namespace ms_usuario.Domains
{
    public class UsuarioNoticiaFavoritado : Entity
    {
        public long UsuarioId { get; set; }
        public long NoticiaId { get; set; }

        private Noticia _Noticia;
        public virtual Noticia Noticia { get { return _Noticia; } set { Noticia = value; SetNoticia(value); } }

        private void SetNoticia(Noticia value)
        {
            NoticiaId = value is null ? 0 : value.Id;
        }
    }
}
