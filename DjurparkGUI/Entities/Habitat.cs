namespace DjurparkGUI.Entities
{
    public class Habitat
    {
        public int HabitatId { get; set; }
        public string Namn { get; set; }
        public string Växtlighet { get; set; }
        public string Klimat { get; set; }

        public List<Djur> Djur { get; set; } // Navigation
    }
}
