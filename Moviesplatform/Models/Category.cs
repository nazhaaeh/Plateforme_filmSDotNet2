namespace Plateforme_Filmes.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Nom { get; set; }

        public List<Film> Movies { get; set; } = new List<Film>();
        public List<Series> Series { get; set; } = new List<Series>();
    }
}
