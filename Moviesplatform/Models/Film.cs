using Moviesplatform.Models;

namespace Plateforme_Filmes.Models
{
    public class Film
    {
        public int Id { get; set; }
        public string Titre { get; set; }
        public string Genre { get; set; }
        public int Annee { get; set; }

        public int CategoryId { get; set; } 
        public Category Category { get; set; }

    }
}
