using DjurparkGUI.Data;                        // Databas-kontexten
using DjurparkGUI.Entities;                    // Entiteterna som Djur, Besökare etc.
using Microsoft.EntityFrameworkCore;           // För async-metoder och Include

namespace DjurparkGUI.Tjänster
{
    // DjurparkService innehåller all affärslogik för att kommunicera med databasen.
    public class DjurparkService
    {
        private readonly ZooContext _context; // databasinstans

        public DjurparkService(ZooContext context)
        {
            _context = context;
        }

        
        /// Visar alla djur från databasen och inkluderar deras habitat.
        
        public async Task VisaAllaDjurAsync()
        {
            var djurLista = await _context.Djur.Include(d => d.Habitat).ToListAsync();
            foreach (var djur in djurLista)
            {
                Console.WriteLine($"ID: {djur.DjurId} | {djur.Namn} ({djur.Art}) - {djur.Habitat?.Namn}");
            }
        }

       
        /// Lägger till ett nytt djur i databasen (används när djuret redan är skapat som objekt).
       
        public async Task LäggTillDjurAsync(Djur djur)
        {
            _context.Djur.Add(djur);
            await _context.SaveChangesAsync();
        }

        
        /// UI-metod: Låter användaren uppdatera status för ett specifikt djur via konsolen.
       
        public async Task UppdateraDjurStatusAsync()
        {
            Console.Write("Ange ID på djuret du vill uppdatera: ");
            if (!int.TryParse(Console.ReadLine(), out int djurId))
            {
                Console.WriteLine("Felaktigt ID.");
                return;
            }

            Console.Write("Ny status (Frisk/Under observation): ");
            string nyStatus = Console.ReadLine();

            await UppdateraDjurStatusAsync(djurId, nyStatus); // Återanvänd befintlig logik
            Console.WriteLine("Status uppdaterad!");
        }

        
        /// Logikmetod som uppdaterar ett djurs status via ID.
        
        public async Task UppdateraDjurStatusAsync(int id, string nyStatus)
        {
            var djur = await _context.Djur.FindAsync(id);
            if (djur != null)
            {
                djur.Status = nyStatus;
                await _context.SaveChangesAsync();
            }
        }

       
        /// Lägger till ett favoritdjur kopplat till en specifik besökare.
        
        public async Task LäggTillFavoritDjurAsync(int besökareId, int djurId)
        {
            var fav = new FavoritDjur { BesökareId = besökareId, DjurId = djurId };
            _context.Set<FavoritDjur>().Add(fav);
            await _context.SaveChangesAsync();
        }

        
        /// Tar bort ett djur från databasen baserat på dess ID, om det finns.
        
        public async Task TaBortDjurAsync()
        {
            Console.WriteLine("Ta bort djur\n");

            var djurLista = await _context.Djur.ToListAsync();
            foreach (var djur in djurLista)
            {
                Console.WriteLine($"ID: {djur.DjurId} | {djur.Namn} ({djur.Art})");
            }

            Console.Write("\nAnge ID på djuret som ska tas bort: ");
            if (!int.TryParse(Console.ReadLine(), out int djurId))
            {
                Console.WriteLine("❌ Felaktigt ID.");
                return;
            }

            var djurAttTaBort = await _context.Djur.FindAsync(djurId);
            if (djurAttTaBort == null)
            {
                Console.WriteLine("Djuret hittades inte.");
                return;
            }

            _context.Djur.Remove(djurAttTaBort);
            await _context.SaveChangesAsync();

            Console.WriteLine("Djuret borttaget!");
        }

        
        /// Låter användaren lägga till ett nytt habitat via konsolen.
       
        public async Task LäggTillHabitatAsync()
        {
            Console.WriteLine("🌿 Lägg till nytt habitat\n");

            Console.Write("Namn: ");
            string namn = Console.ReadLine();

            Console.Write("Växtlighet: ");
            string växtlighet = Console.ReadLine();

            Console.Write("Klimat: ");
            string klimat = Console.ReadLine();

            var habitat = new Habitat
            {
                Namn = namn,
                Växtlighet = växtlighet,
                Klimat = klimat
            };

            _context.Habitats.Add(habitat);
            await _context.SaveChangesAsync();

            Console.WriteLine("✅ Habitat tillagt!");
        }

        
        /// Skriver ut alla habitat i databasen till konsolen.
       
        public async Task VisaAllaHabitatAsync()
        {
            Console.WriteLine(" Alla habitat\n");

            var habitats = await _context.Habitats.ToListAsync();
            foreach (var h in habitats)
            {
                Console.WriteLine($"ID: {h.HabitatId} | {h.Namn} ({h.Klimat}) – Växtlighet: {h.Växtlighet}");
            }
        }

       
        /// Låter användaren ta bort ett habitat baserat på ID.
        /// Kontroll görs så att ID är giltigt.
        
        public async Task TaBortHabitatAsync()
        {
            Console.WriteLine("Ta bort habitat\n");

            // Lista alla habitat för att välja
            var habitats = await _context.Habitats.ToListAsync();
            foreach (var h in habitats)
            {
                Console.WriteLine($"ID: {h.HabitatId} | {h.Namn}");
            }

            Console.Write("\nAnge ID på habitatet som ska tas bort: ");
            if (!int.TryParse(Console.ReadLine(), out int habitatId))
            {
                Console.WriteLine("Felaktigt ID.");
                return;
            }

            var habitat = await _context.Habitats.FindAsync(habitatId);
            if (habitat == null)
            {
                Console.WriteLine("Habitatet hittades inte.");
                return;
            }

            _context.Habitats.Remove(habitat);
            await _context.SaveChangesAsync();

            Console.WriteLine("Habitat borttaget!");
        }

        
        /// Låter användaren lägga till en ny besökare via konsolen.
        
        public async Task LäggTillBesökareAsync()
        {
            Console.WriteLine("Lägg till ny besökare\n");

            Console.Write("Namn: ");
            string namn = Console.ReadLine();

            Console.Write("Ålder: ");
            if (!int.TryParse(Console.ReadLine(), out int ålder))
            {
                Console.WriteLine("Felaktig ålder.");
                return;
            }

            Console.Write("E-post: ");
            string epost = Console.ReadLine();

            Console.Write("Telefonnummer: ");
            string telefonnummer = Console.ReadLine();

            var besökare = new Besökare
            {
                Namn = namn,
                Ålder = ålder,
                Epost = epost,
                Telefonnummer = telefonnummer
            };

            _context.Besökare.Add(besökare);
            await _context.SaveChangesAsync();

            Console.WriteLine("Besökare tillagd!");
        }

        
        /// Visar alla besökare med namn och ålder.
        
        public async Task VisaAllaBesökareAsync()
        {
            Console.WriteLine("Alla besökare\n");

            var besökareLista = await _context.Besökare.ToListAsync();
            foreach (var b in besökareLista)
            {
                Console.WriteLine($"ID: {b.BesökareId} | {b.Namn} ({b.Ålder} år)");
            }
        }

        
        /// Låter användaren uppdatera kontaktinfo (e-post, telefon) för en besökare.
        
        public async Task UppdateraBesökareKontaktAsync()
        {
            Console.WriteLine("Uppdatera kontaktinfo för besökare\n");
            await VisaAllaBesökareAsync(); // Visa först

            Console.Write("\nAnge ID på besökaren du vill uppdatera: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Felaktigt ID.");
                return;
            }

            var besökare = await _context.Besökare.FindAsync(id);
            if (besökare == null)
            {
                Console.WriteLine("Besökaren hittades inte.");
                return;
            }

            Console.Write("Ny e-post: ");
            besökare.Epost = Console.ReadLine();

            Console.Write("Nytt telefonnummer: ");
            besökare.Telefonnummer = Console.ReadLine();

            await _context.SaveChangesAsync();
            Console.WriteLine("Kontaktinfo uppdaterad!");

        }
        
        /// Tar bort en besökare från databasen via ID.
        
        public async Task TaBortBesökareAsync()
        {
            Console.WriteLine(" Ta bort besökare\n");
            await VisaAllaBesökareAsync();

            Console.Write("\nAnge ID på besökare att ta bort: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine(" Felaktigt ID.");
                return;
            }

            var besökare = await _context.Besökare.FindAsync(id);
            if (besökare == null)
            {
                Console.WriteLine("❌ Besökaren hittades inte.");
                return;
            }

            _context.Besökare.Remove(besökare);
            await _context.SaveChangesAsync();

            Console.WriteLine(" Besökare borttagen!");
        }
        
        /// Visar alla besök med datum, besökare och om biljetten var betald.
        
        public async Task VisaAllaBesökAsync()
        {
            Console.WriteLine(" Alla besök\n");

            var besök = await _context.Besök.Include(b => b.Besökare).ToListAsync();
            foreach (var b in besök)
            {
                string status = b.Betald ? "Betald" : "Ej betald";
                Console.WriteLine($"Datum: {b.Datum.ToShortDateString()} | Besökare: {b.Besökare.Namn} | {status}");
            }
        }
        
        /// Visar alla besök som gjorts av en specifik besökare via ID.
       
        public async Task VisaBesökFörBesökareAsync()
        {
            Console.WriteLine("Visa besök för en specifik besökare\n");

            // Lista alla besökare så användaren kan välja
            var besökareLista = await _context.Besökare.ToListAsync();
            foreach (var b in besökareLista)
            {
                Console.WriteLine($"ID: {b.BesökareId} | {b.Namn}");
            }

            Console.Write("\nAnge ID på besökare: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Felaktigt ID.");
                return;
            }

            var besök = await _context.Besök
                .Where(b => b.BesökareId == id)
                .ToListAsync();

            if (!besök.Any())
            {
                Console.WriteLine("Denna besökare har inga registrerade besök.");
                return;
            }

            foreach (var b in besök)
            {
                Console.WriteLine($"Datum: {b.Datum.ToShortDateString()} | Betald: {(b.Betald ? "Ja" : "Nej")}");
            }
        }
        
        /// Låter användaren registrera ett nytt besök till djurparken via konsolen.
        /// Användaren väljer en befintlig besökare och anger datum samt om biljetten var betald.
        
        public async Task LäggTillBesökAsync()
        {
            Console.WriteLine("Lägg till nytt besök\n");

            // Hämta alla besökare och visa dem
            var besökareLista = await _context.Besökare.ToListAsync();
            if (!besökareLista.Any())
            {
                Console.WriteLine("Det finns inga registrerade besökare.");
                return;
            }

            Console.WriteLine("Tillgängliga besökare:");
            foreach (var b in besökareLista)
            {
                Console.WriteLine($"ID: {b.BesökareId} | {b.Namn}");
            }

            // Mata in besökare-ID
            Console.Write("\nAnge ID på besökaren: ");
            if (!int.TryParse(Console.ReadLine(), out int besökareId) ||
                !besökareLista.Any(b => b.BesökareId == besökareId))
            {
                Console.WriteLine("Ogiltigt ID.");
                return;
            }

            // Datum för besöket
            Console.Write("Ange datum för besöket (yyyy-mm-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime datum))
            {
                Console.WriteLine("Felaktigt datumformat.");
                return;
            }

            // Betald biljett
            Console.Write("Var biljetten betald? (j/n): ");
            string svar = Console.ReadLine().ToLower();
            bool betald = svar == "j" || svar == "ja";

            // Skapa nytt besöksobjekt
            var nyttBesök = new Besök
            {
                BesökareId = besökareId,
                Datum = datum,
                Betald = betald
            };

            _context.Besök.Add(nyttBesök);
            await _context.SaveChangesAsync();

            Console.WriteLine("Besök tillagt!");
        }


    }
}
