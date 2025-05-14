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
    }
}
