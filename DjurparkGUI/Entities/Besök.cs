namespace DjurparkGUI.Entities
{
    public class Besök
    {
        public int BesökId { get; set; }
        public int BesökareId { get; set; }
        public DateTime Datum { get; set; }
        public bool Betald { get; set; }

        public Besökare Besökare { get; set; } // Navigation
    }
}
