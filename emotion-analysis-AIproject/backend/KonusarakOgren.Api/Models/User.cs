namespace KonusarakOgren.Api.Models
{
    // Bu bizim "Kullanıcı" tablomuzu temsil edecek.
    public class User
    {
        public int Id { get; set; } // Veritabanındaki benzersiz anahtar (Primary Key)

        // Proje bizden sadece rumuz istedi 
        public string Nickname { get; set; } 
    }
}