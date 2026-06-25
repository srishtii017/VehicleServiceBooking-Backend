using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace BookingService.Model
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Booking> Bookings { get; set; }
    }
}
