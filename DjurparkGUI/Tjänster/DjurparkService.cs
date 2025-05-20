using DjurparkGUI.Data;
using DjurparkGUI.Entities;
using Microsoft.EntityFrameworkCore;

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

        // Hämtar alla djur från databasen och inkluderar deras habitat
        public async Task<List<Djur>> HämtaAllaDjurAsync()
        {
            return await _context.Djur.Include(d => d.Habitat).ToListAsync();
        }

        // Lägger till ett nytt djur i databasen
        public async Task LäggTillDjurAsync(Djur djur)
        {
            _context.Djur.Add(djur);
            await _context.SaveChangesAsync();
        }

        // Uppdaterar statusen på ett djur, t.ex. "Frisk" eller "Under observation"
        public async Task UppdateraDjurStatusAsync(int id, string nyStatus)
        {
            var djur = await _context.Djur.FindAsync(id);
            if (djur != null)
            {
                djur.Status = nyStatus;
                await _context.SaveChangesAsync();
            }
        }

        // Lägger till ett favoritdjur kopplat till en specifik besökare
        public async Task LäggTillFavoritDjurAsync(int besökareId, int djurId)
        {
            var fav = new FavoritDjur { BesökareId = besökareId, DjurId = djurId };
            _context.Set<FavoritDjur>().Add(fav);
            await _context.SaveChangesAsync();
        }
    }
}
