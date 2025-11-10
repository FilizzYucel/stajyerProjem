using KonusarakOgren.Api.Models; // User ve Message sınıflarımızı kullanabilmek için
using Microsoft.EntityFrameworkCore; // DbContext için bu gerekli

namespace KonusarakOgren.Api.Data
{
    public class AppDbContext : DbContext
    {
        // Bu constructor, veritabanı ayarlarını almamızı sağlar
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Veritabanında "Users" adında bir tablo oluştur, 
        // bu tablo User modelini kullansın.
        public DbSet<User> Users { get; set; }

        // Veritabanında "Messages" adında bir tablo oluştur,
        // bu tablo Message modelini kullansın.
        public DbSet<Message> Messages { get; set; }
    }
}