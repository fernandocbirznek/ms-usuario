namespace ms_usuario.Domains
{
    public class NoticiaAreaInteresse : Entity
    {
        public long NoticiaId { get; set; }
        private Noticia _Noticia;
        public virtual Noticia Noticia { get { return _Noticia; } set { _Noticia = value; SetNoticia(value); } }

        private void SetNoticia(Noticia value)
        {
            NoticiaId = value is null ? 0 : value.Id;
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
