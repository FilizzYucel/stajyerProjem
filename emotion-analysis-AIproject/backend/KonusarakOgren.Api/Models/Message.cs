using System.ComponentModel.DataAnnotations.Schema;

namespace KonusarakOgren.Api.Models
{
    // Bu bizim "Mesaj" tablomuzu temsil edecek.
    public class Message
    {
        public int Id { get; set; } // Benzersiz mesaj Id'si
        public string Text { get; set; } // Mesajın içeriği
        public DateTime Timestamp { get; set; } // Mesajın gönderildiği zaman

        // Projede istenen duygu skoru (Pozitif, Nötr, Negatif) 
        public string Sentiment { get; set; } 

        // Bu mesajı hangi kullanıcının gönderdiğini bilmemiz gerek (Foreign Key)
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}