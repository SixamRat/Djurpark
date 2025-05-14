using DjurparkGUI.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DjurparkGUI.Data
{
    public class ZooContext : DbContext
    {
        public DbSet<Djur> Djur { get; set; }
        public DbSet<Habitat> Habitats { get; set; }
        public DbSet<Besökare> Besökare { get; set; }
        public DbSet<Besök> Besök { get; set; }
        public DbSet<Settings> Settings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Anslutning till SQL Server LocalDB
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=DjurparkDB;Trusted_Connection=True;");
        }
    }
}
