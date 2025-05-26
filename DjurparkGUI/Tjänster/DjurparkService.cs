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


    }
}
