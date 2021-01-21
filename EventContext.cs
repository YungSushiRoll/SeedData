using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace SeedData
{
    public class EventContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Location> Locations { get; set; }

        public void AddEvent(Event evt)
        {
            this.Events.Add(evt);
        }
        public void SaveEvents()
        {
            this.SaveChanges();
        }
        public void AddLocation(Location loc)
        {
            this.Locations.Add(loc);
            this.SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            optionsBuilder.UseSqlServer(@config["EventContext:ConnectionString"]);
        }
    }
}