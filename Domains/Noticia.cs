namespace ms_usuario.Domains
{
    public class Noticia : Entity
    {
        public string Titulo { get; set; }
        public string Resumo { get; set; }
        public string Conteudo { get; set; }
        public long UsuarioCadastroId { get; set; }
        public virtual ICollection<NoticiaAreaInteresse> NoticiaAreaInteresseMany { get; set; }
    }
}
