using System;
using System.Linq;
using System.Threading.Tasks;
using DjurparkGUI.Data;
using DjurparkGUI.Entities;
using Microsoft.EntityFrameworkCore;

namespace DjurparkGUI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await using var db = new ZooContext();
            await SeedData(db); // Laddar mockdata vid första körning

            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine("🐾 Välkommen till Djurparken 🐾\n");
                Console.WriteLine("1. Lägg till djur");
                Console.WriteLine("2. Visa alla djur");
                Console.WriteLine("3. Uppdatera djurstatus");
                Console.WriteLine("4. Ta bort djur");
                Console.WriteLine("5. Lägg till habitat");
                Console.WriteLine("6. Visa alla habitat");
                Console.WriteLine("7. Ta bort habitat");
                Console.WriteLine("8. Lägg till besökare");
                Console.WriteLine("9. Visa alla besökare");
                Console.WriteLine("10. Uppdatera besökares kontaktinfo");
                Console.WriteLine("11. Ta bort besökare");
                Console.WriteLine("12. Visa alla besök");
                Console.WriteLine("13. Visa besök för specifik besökare");
                Console.WriteLine("14. Sök efter djur");
                Console.WriteLine("15. Simulera en dag");
                Console.WriteLine("16. Visa statistik");
                Console.WriteLine("17. Avsluta\n");
                Console.Write("Val: ");

                var input = Console.ReadLine();
                Console.Clear();

                switch (input)
                {
                    case "1": await AddAnimal(db); break;
                    case "2": await ViewAnimals(db); break;
                    case "3": await UpdateAnimalStatus(db); break;
                    case "4": await DeleteAnimal(db); break;
                    case "5": await AddHabitat(db); break;
                    case "6": await ViewHabitats(db); break;
                    case "7": await DeleteHabitat(db); break;
                    case "8": await AddVisitor(db); break;
                    case "9": await ViewVisitors(db); break;
                    case "10": await UpdateVisitorContact(db); break;
                    case "11": await DeleteVisitor(db); break;
                    case "12": await ViewVisits(db); break;
                    case "13": await ViewVisitsByVisitor(db); break;
                    case "14": await SearchAnimals(db); break;
                    case "15": await SimulateDay(db); break;
                    case "16": await ShowStatistics(db); break;
                    case "17": isRunning = false; break;
                    default: Console.WriteLine("Felaktigt val!"); break;
                }

                if (isRunning)
                {
                    Console.WriteLine("\nTryck på valfri tangent för att fortsätta...");
                    Console.ReadKey();
                }
            }
        }

        // ------------------------------------------
        // MOCKDATA - Seed för att få initial data
        private static async Task SeedData(ZooContext db)
        {
            if (await db.Habitats.AnyAsync() || await db.Djur.AnyAsync() || await db.Besökare.AnyAsync())
                return; // Om vi redan har data, hoppa över

            // Lägg till habitat
            var savann = new Habitat { Namn = "Savann", Växtlighet = "Gräs", Klimat = "Torrt" };
            var regnskog = new Habitat { Namn = "Regnskog", Växtlighet = "Tät skog", Klimat = "Fuktigt" };
            var arktis = new Habitat { Namn = "Arktis", Växtlighet = "Snö och is", Klimat = "Kallt" };
            await db.Habitats.AddRangeAsync(savann, regnskog, arktis);
            await db.SaveChangesAsync();

            // Lägg till djur
            await db.Djur.AddRangeAsync(
                new Djur { Namn = "Leo", Art = "Lejon", Födelsedatum = DateTime.Now.AddYears(-5), Kön = "Hane", Status = "Frisk", Matkostnad = 2000, Omvårdnadskostnad = 1000, Popularitet = 90, HabitatId = savann.HabitatId },
                new Djur { Namn = "Koko", Art = "Gorilla", Födelsedatum = DateTime.Now.AddYears(-7), Kön = "Hona", Status = "Under observation", Matkostnad = 2500, Omvårdnadskostnad = 1200, Popularitet = 80, HabitatId = regnskog.HabitatId }
            );
            await db.SaveChangesAsync();

            // Lägg till besökare
            await db.Besökare.AddRangeAsync(
                new Besökare { Namn = "Anna Andersson", Ålder = 34, Epost = "anna@example.com", Telefonnummer = "0701234567" },
                new Besökare { Namn = "Erik Ek", Ålder = 42, Epost = "erik@example.com", Telefonnummer = "0707654321" }
            );
            await db.SaveChangesAsync();

            // Lägg till settings
            if (!await db.Settings.AnyAsync())
            {
                var settings = new Settings { EntréPris = 100, MaxAntalBesökare = 300 };
                await db.Settings.AddAsync(settings);
                await db.SaveChangesAsync();
            }

            // Skapa slumpmässiga besök
            var besökareLista = await db.Besökare.ToListAsync();
            Random random = new Random();
            for (int i = 0; i < 30; i++)
            {
                var randomBesökare = besökareLista[random.Next(besökareLista.Count)];
                var randomDatum = DateTime.Today.AddDays(-random.Next(0, 30));
                var nyttBesök = new Besök
                {
                    BesökareId = randomBesökare.BesökareId,
                    Datum = randomDatum,
                    Betald = random.Next(0, 2) == 1
                };
                await db.Besök.AddAsync(nyttBesök);
            }
            await db.SaveChangesAsync();
        }
        // ------------------------------------------
        // CRUD FÖR DJUR

        private static async Task AddAnimal(ZooContext db)
        {
            Console.WriteLine("🐾 Lägg till nytt djur\n");

            Console.Write("Namn: ");
            string namn = Console.ReadLine();

            Console.Write("Art: ");
            string art = Console.ReadLine();

            Console.Write("Födelsedatum (yyyy-mm-dd): ");
            DateTime födelsedatum;
            if (!DateTime.TryParse(Console.ReadLine(), out födelsedatum))
            {
                Console.WriteLine("❌ Felaktigt datumformat.");
                return;
            }

            Console.Write("Kön (Hane/Hona): ");
            string kön = Console.ReadLine();

            Console.Write("Status (Frisk/Under observation): ");
            string status = Console.ReadLine();

            Console.Write("Matkostnad (kr/mån): ");
            decimal matkostnad;
            if (!decimal.TryParse(Console.ReadLine(), out matkostnad))
            {
                Console.WriteLine("❌ Felaktig matkostnad.");
                return;
            }

            Console.Write("Omvårdnadskostnad (kr/mån): ");
            decimal omvårdnadskostnad;
            if (!decimal.TryParse(Console.ReadLine(), out omvårdnadskostnad))
            {
                Console.WriteLine("❌ Felaktig omvårdnadskostnad.");
                return;
            }

            Console.Write("Popularitet (0-100): ");
            int popularitet;
            if (!int.TryParse(Console.ReadLine(), out popularitet))
            {
                Console.WriteLine("❌ Felaktig popularitetspoäng.");
                return;
            }

            var habitats = await db.Habitats.ToListAsync();
            Console.WriteLine("\nTillgängliga habitat:");
            foreach (var h in habitats)
            {
                Console.WriteLine($"{h.HabitatId}. {h.Namn}");
            }

            Console.Write("Ange Habitat-ID: ");
            if (!int.TryParse(Console.ReadLine(), out int habitatId))
            {
                Console.WriteLine("❌ Felaktigt habitat-ID.");
                return;
            }

            var djur = new Djur
            {
                Namn = namn,
                Art = art,
                Födelsedatum = födelsedatum,
                Kön = kön,
                Status = status,
                Matkostnad = matkostnad,
                Omvårdnadskostnad = omvårdnadskostnad,
                Popularitet = popularitet,
                HabitatId = habitatId
            };

            await db.Djur.AddAsync(djur);
            await db.SaveChangesAsync();

            Console.WriteLine("✅ Djur tillagt!");
        }

        private static async Task ViewAnimals(ZooContext db)
        {
            Console.WriteLine("🐾 Alla djur i parken\n");
            var djurLista = await db.Djur.Include(d => d.Habitat).ToListAsync();
            foreach (var djur in djurLista)
            {
                Console.WriteLine($"ID: {djur.DjurId} | {djur.Namn} ({djur.Art}) - {djur.Habitat?.Namn}");
            }
        }

        private static async Task UpdateAnimalStatus(ZooContext db)
        {
            Console.WriteLine("🐾 Uppdatera djurstatus\n");
            await ViewAnimals(db);

            Console.Write("Ange ID på djuret du vill uppdatera: ");
            if (!int.TryParse(Console.ReadLine(), out int djurId))
            {
                Console.WriteLine("❌ Felaktigt ID.");
                return;
            }

            var djur = await db.Djur.FindAsync(djurId);
            if (djur == null)
            {
                Console.WriteLine("❌ Djuret hittades inte.");
                return;
            }

            Console.Write("Ny status (Frisk/Under observation): ");
            string status = Console.ReadLine();

            djur.Status = status;
            await db.SaveChangesAsync();

            Console.WriteLine("✅ Status uppdaterad!");
        }

        private static async Task DeleteAnimal(ZooContext db)
        {
            Console.WriteLine("🐾 Ta bort djur\n");
            await ViewAnimals(db);

            Console.Write("Ange ID på djuret som ska tas bort: ");
            if (!int.TryParse(Console.ReadLine(), out int djurId))
            {
                Console.WriteLine("❌ Felaktigt ID.");
                return;
            }

            var djur = await db.Djur.FindAsync(djurId);
            if (djur == null)
            {
                Console.WriteLine("❌ Djuret hittades inte.");
                return;
            }

            db.Djur.Remove(djur);
            await db.SaveChangesAsync();

            Console.WriteLine("✅ Djuret borttaget!");
        }

        // ------------------------------------------
        // CRUD FÖR HABITAT

        private static async Task AddHabitat(ZooContext db)
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

            await db.Habitats.AddAsync(habitat);
            await db.SaveChangesAsync();

            Console.WriteLine("✅ Habitat tillagt!");
        }

        private static async Task ViewHabitats(ZooContext db)
        {
            Console.WriteLine("🌿 Alla habitat\n");
            var habitats = await db.Habitats.ToListAsync();
            foreach (var h in habitats)
            {
                Console.WriteLine($"ID: {h.HabitatId} | {h.Namn} ({h.Klimat})");
            }
        }

        private static async Task DeleteHabitat(ZooContext db)
        {
            Console.WriteLine("🌿 Ta bort habitat\n");
            await ViewHabitats(db);

            Console.Write("Ange ID på habitatet som ska tas bort: ");
            if (!int.TryParse(Console.ReadLine(), out int habitatId))
            {
                Console.WriteLine("❌ Felaktigt ID.");
                return;
            }

            var habitat = await db.Habitats.FindAsync(habitatId);
            if (habitat == null)
            {
                Console.WriteLine("❌ Habitatet hittades inte.");
                return;
            }

            db.Habitats.Remove(habitat);
            await db.SaveChangesAsync();

            Console.WriteLine("✅ Habitat borttaget!");
        }

        // ------------------------------------------
        // CRUD FÖR BESÖKARE

        private static async Task AddVisitor(ZooContext db)
        {
            Console.WriteLine("👨‍👩‍👧‍👦 Lägg till besökare\n");

            Console.Write("Namn: ");
            string namn = Console.ReadLine();

            Console.Write("Ålder: ");
            if (!int.TryParse(Console.ReadLine(), out int ålder))
            {
                Console.WriteLine("❌ Felaktig ålder.");
                return;
            }

            Console.Write("Epost: ");
            string epost = Console.ReadLine();

            Console.Write("Telefonnummer: ");
            string telefon = Console.ReadLine();

            var besökare = new Besökare
            {
                Namn = namn,
                Ålder = ålder,
                Epost = epost,
                Telefonnummer = telefon
            };

            await db.Besökare.AddAsync(besökare);
            await db.SaveChangesAsync();

            Console.WriteLine("✅ Besökare tillagd!");
        }

        private static async Task ViewVisitors(ZooContext db)
        {
            Console.WriteLine("👨‍👩‍👧‍👦 Alla besökare\n");
            var visitors = await db.Besökare.ToListAsync();
            foreach (var v in visitors)
            {
                Console.WriteLine($"ID: {v.BesökareId} | {v.Namn} ({v.Ålder} år)");
            }
        }

        private static async Task UpdateVisitorContact(ZooContext db)
        {
            Console.WriteLine("👨‍👩‍👧‍👦 Uppdatera besökares kontaktinfo\n");
            await ViewVisitors(db);

            Console.Write("Ange ID på besökare att uppdatera: ");
            if (!int.TryParse(Console.ReadLine(), out int besökareId))
            {
                Console.WriteLine("❌ Felaktigt ID.");
                return;
            }

            var besökare = await db.Besökare.FindAsync(besökareId);
            if (besökare == null)
            {
                Console.WriteLine("❌ Besökaren hittades inte.");
                return;
            }

            Console.Write("Ny e-post: ");
            besökare.Epost = Console.ReadLine();

            Console.Write("Nytt telefonnummer: ");
            besökare.Telefonnummer = Console.ReadLine();

            await db.SaveChangesAsync();
            Console.WriteLine("✅ Kontaktinformation uppdaterad!");
        }

        private static async Task DeleteVisitor(ZooContext db)
        {
            Console.WriteLine("👨‍👩‍👧‍👦 Ta bort besökare\n");
            await ViewVisitors(db);

            Console.Write("Ange ID på besökare att ta bort: ");
            if (!int.TryParse(Console.ReadLine(), out int besökareId))
            {
                Console.WriteLine("❌ Felaktigt ID.");
                return;
            }

            var besökare = await db.Besökare.FindAsync(besökareId);
            if (besökare == null)
            {
                Console.WriteLine("❌ Besökaren hittades inte.");
                return;
            }

            db.Besökare.Remove(besökare);
            await db.SaveChangesAsync();

            Console.WriteLine("✅ Besökare borttagen!");
        }

        // ------------------------------------------
        // VISA ALLA BESÖK

        private static async Task ViewVisits(ZooContext db)
        {
            Console.WriteLine("📅 Alla besök\n");

            var visits = await db.Besök.Include(b => b.Besökare).ToListAsync();
            foreach (var visit in visits)
            {
                string status = visit.Betald ? "Betald" : "Ej betald";
                Console.WriteLine($"Datum: {visit.Datum.ToShortDateString()} | Besökare: {visit.Besökare.Namn} | {status}");
            }
        }

        // ------------------------------------------
        // VISA BESÖK FÖR SPECIFIK BESÖKARE

        private static async Task ViewVisitsByVisitor(ZooContext db)
        {
            Console.WriteLine("👨‍👩‍👧‍👦 Visa besök för en specifik besökare\n");

            await ViewVisitors(db);

            Console.Write("Ange besökare ID: ");
            if (!int.TryParse(Console.ReadLine(), out int visitorId))
            {
                Console.WriteLine("❌ Felaktigt ID.");
                return;
            }

            var visits = await db.Besök
                .Where(b => b.BesökareId == visitorId)
                .ToListAsync();

            if (!visits.Any())
            {
                Console.WriteLine("❌ Ingen besökshistorik för denna besökare.");
                return;
            }

            foreach (var visit in visits)
            {
                Console.WriteLine($"Datum: {visit.Datum.ToShortDateString()} | Betald: {(visit.Betald ? "Ja" : "Nej")}");
            }
        }

        // ------------------------------------------
        // SÖK DJUR PÅ ART ELLER HABITAT

        private static async Task SearchAnimals(ZooContext db)
        {
            Console.WriteLine("🔍 Sök efter djur\n");

            Console.Write("Sök efter (art eller habitatnamn): ");
            string search = Console.ReadLine().ToLower();

            var results = await db.Djur
                .Include(d => d.Habitat)
                .Where(d => d.Art.ToLower().Contains(search) || d.Habitat.Namn.ToLower().Contains(search))
                .ToListAsync();

            if (!results.Any())
            {
                Console.WriteLine("❌ Inga träffar.");
                return;
            }

            foreach (var djur in results)
            {
                Console.WriteLine($"ID: {djur.DjurId} | {djur.Namn} ({djur.Art}) - {djur.Habitat.Namn}");
            }
        }

        // ------------------------------------------
        // SIMULERA EN DAG I PARKEN

        private static async Task SimulateDay(ZooContext db)
        {
            Console.WriteLine("📈 Simulera en dag i parken\n");

            var settings = await db.Settings.FirstOrDefaultAsync();
            var djurLista = await db.Djur.ToListAsync();
            Random rnd = new Random();

            if (settings == null || !djurLista.Any())
            {
                Console.WriteLine("❌ Saknas inställningar eller djurdata.");
                return;
            }

            int basbesökare = rnd.Next(50, 100);
            int extra = djurLista.Sum(d => d.Popularitet) / 2;
            int totalbesökare = Math.Min(basbesökare + extra, settings.MaxAntalBesökare);

            decimal intäkter = totalbesökare * settings.EntréPris;
            decimal kostnader = djurLista.Sum(d => (d.Matkostnad + d.Omvårdnadskostnad) / 30);
            decimal resultat = intäkter - kostnader;

            Console.WriteLine($"Besökare idag: {totalbesökare}");
            Console.WriteLine($"Intäkter: {intäkter:C}");
            Console.WriteLine($"Kostnader: {kostnader:C}");
            Console.WriteLine($"Resultat: {resultat:C}");
        }

        // ------------------------------------------
        // VISAR STATISTIK FÖR DJURPARKEN

        private static async Task ShowStatistics(ZooContext db)
        {
            Console.WriteLine("📊 Statistik för Djurparken\n");

            var djurLista = await db.Djur.Include(d => d.Habitat).ToListAsync();
            var besökLista = await db.Besök.ToListAsync();
            var settings = await db.Settings.FirstOrDefaultAsync();

            if (djurLista.Count == 0 || settings == null)
            {
                Console.WriteLine("❌ Ingen data att visa.");
                return;
            }

            var populärasteDjur = djurLista.OrderByDescending(d => d.Popularitet).FirstOrDefault();
            var mestPopuläraHabitat = djurLista.GroupBy(d => d.Habitat.Namn)
                                                .OrderByDescending(g => g.Count())
                                                .FirstOrDefault();

            Console.WriteLine($"Mest populära djuret: {populärasteDjur?.Namn} ({populärasteDjur?.Art})");
            Console.WriteLine($"Populäraste habitatet: {mestPopuläraHabitat?.Key}");
            Console.WriteLine($"Entrépris: {settings.EntréPris:C}");
            Console.WriteLine($"Max antal besökare: {settings.MaxAntalBesökare}");

            // Visa antal besök senaste 30 dagarna
            var grupperadeBesök = besökLista
                .GroupBy(b => b.Datum.Month)
                .Select(g => new { Månad = g.Key, Antal = g.Count() })
                .OrderBy(g => g.Månad);

            Console.WriteLine("\nAntal besök per månad:");
            foreach (var group in grupperadeBesök)
            {
                Console.WriteLine($"Månad {group.Månad}: {group.Antal} besök");
            }
        }


