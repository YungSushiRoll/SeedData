using System;
using NLog.Web;
using System.IO;
using System.Linq;

namespace SeedData
{
    class Program
    {
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
        static void Main(string[] args)
        {
            logger.Info("Program started");

            try
            {
                Location currentLocation = new Location{};

                DateTime now = DateTime.Now;
                DateTime eventDate = now.AddMonths(-6);
                Random rnd = new Random();
                var db = new EventContext();
                
                while (eventDate < now)
                {
                    int evtPerDay = rnd.Next(0,6);
                    
                    for (int i = 0; i < evtPerDay; i++)
                    {
                        int hour = rnd.Next(0,24);
                        int min = rnd.Next(0,60);
                        int sec = rnd.Next(0,60);
                        int location = rnd.Next(1,4);
                        var locationQuery = db.Locations;

                        foreach (var room in locationQuery)
                        {
                            if (location == room.LocationId)
                            {
                                currentLocation = room;
                            }
                        }

                        DateTime currentDate = new DateTime(eventDate.Year, eventDate.Month, eventDate.Day, hour, min, sec);

                        Event currentEvent = new Event {TimeStamp = currentDate, LocationId = location, Location = currentLocation, Flagged = false};


                        db.AddEvent(currentEvent);
                    }
                    eventDate = eventDate.AddDays(1);
                }
                db.SaveEvents();
                logger.Info("Retrieving Event Data");

                // Display all Events with Locations
                var query = db.Events.OrderBy(e => e.TimeStamp);

                Console.WriteLine("Event Activity\n//////////////////////////////////////////");
                
                foreach (var item in query)
                {
                    Console.WriteLine(item.TimeStamp + " - " + item.Location.Name);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + " " + ex.InnerException);
            }

            logger.Info("Program ended");
        }
    }
    public class Location
    {
        public int LocationId { get; set; }
        public string Name { get; set; }
    }
    public class Event
    {
        public int EventId { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool Flagged { get; set; }
        // foreign key for location 
        public int LocationId { get; set; }
        // navigation property
        public Location Location { get; set; }
    }
}
