namespace DjurparkGUI.Entities
{
    public class Djur
    {
        public int DjurId { get; set; }
        public string Namn { get; set; }
        public string Art { get; set; }
        public DateTime Födelsedatum { get; set; }
        public string Kön { get; set; }
        public string Status { get; set; }
        public decimal Matkostnad { get; set; }
        public decimal Omvårdnadskostnad { get; set; }
        public int Popularitet { get; set; }

        public int HabitatId { get; set; }
        public Habitat Habitat { get; set; } // Navigation
    }
}
