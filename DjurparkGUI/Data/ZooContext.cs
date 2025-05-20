using DjurparkGUI.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DjurparkGUI.Data
{
    public class ZooContext : DbContext
    {
        // DB-tabeller
        public DbSet<Djur> Djur { get; set; }
        public DbSet<Habitat> Habitats { get; set; }
        public DbSet<Besökare> Besökare { get; set; }
        public DbSet<Besök> Besök { get; set; }
        public DbSet<Settings> Settings { get; set; }

        // Här ansluter vi till en lokal databas
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Ursprunglig lokal connection string
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=DjurparkDB;Trusted_Connection=True;");

            // Du kan ersätta med Azure-connection string om man vill, byt ut adminuser, password och servernamn till ens Azure info:

            //optionsBuilder.UseSqlServer("Server=tcp:djurpark-sqlserver.database.windows.net,1433;" +
            //"Initial Catalog=DjurparkDB;" +
            //"Persist Security Info=False;" +
            //"User ID=adminuser;" +
            //"Password=SuperSäkertLösen123;" +
            //"MultipleActiveResultSets=False;" +
            //"Encrypt=True;" +
            //"TrustServerCertificate=False;" +
            //"Connection Timeout=30;");
        }

        // Här lägger vi till modellen för vår många-till-många-relation
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Definierar composite key (sammansatt nyckel) för FavoritDjur
            modelBuilder.Entity<FavoritDjur>()
                .HasKey(fd => new { fd.BesökareId, fd.DjurId });

            // Besökare har många favoritdjur
            modelBuilder.Entity<FavoritDjur>()
                .HasOne(fd => fd.Besökare)
                .WithMany(b => b.FavoritDjur)
                .HasForeignKey(fd => fd.BesökareId);

            // Djur gillas av många besökare
            modelBuilder.Entity<FavoritDjur>()
                .HasOne(fd => fd.Djur)
                .WithMany(d => d.GillasAvBesökare)
                .HasForeignKey(fd => fd.DjurId);
        }
    }
}
