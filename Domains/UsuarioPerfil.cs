namespace ms_usuario.Domains
{
    public class UsuarioPerfil : Entity
    {
        public DateTime? DataNascimento { get; set; }
        public byte[]? Foto { get; set; }
        public string? Hobbie { get; set; }
        public Usuario Usuario { get; set; }
    }
}
