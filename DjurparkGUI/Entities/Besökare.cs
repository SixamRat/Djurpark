namespace DjurparkGUI.Entities
{
    public class Besökare
    {
        public int BesökareId { get; set; }
        public string Namn { get; set; }
        public int Ålder { get; set; }
        public string Epost { get; set; }
        public string Telefonnummer { get; set; }

        public List<Besök> Besök { get; set; } // Navigation
        // En besökare kan ha flera besök, så vi använder en lista för att lagra dem.

        // Samlar alla djur som besökaren markerat som favorit.
        public ICollection<FavoritDjur> FavoritDjur { get; set; } = new List<FavoritDjur>();
    }
}
