using KonusarakOgren.Api.Data; // Veritabanı context'i için
using KonusarakOgren.Api.Dtos; // MessageRequest DTO'su için
using KonusarakOgren.Api.Models; // User ve Message modelleri için
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json; // JSON işlemleri için
using System.Text; // UTF8 Encoding için

namespace KonusarakOgren.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        // ADRESİ DEĞİŞTİRMEDİK! (Bu, 200 OK cevabını alan adres)
        // private const string HF_API_URL = "https://filizyucel-emotion-analysis-api.hf.space/gradio_api/call/predict";
        //private const string HF_API_URL = "https://filizyucel-emotion-analysis-api.hf.space/api/predict/";
        private const string HF_API_URL = "https://filizyucel-emotion-analysis-api.hf.space/gradio_api/call/predict";



        public ChatController(AppDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        // GET: /api/chat/messages
        [HttpGet("messages")]
        public async Task<IActionResult> GetMessages()
        {
            var messages = await _context.Messages
                                .Include(m => m.User)
                                .OrderBy(m => m.Timestamp)
                                .ToListAsync();

            var response = messages.Select(m => new 
            {
                m.Id,
                m.Text,
                Nickname = m.User.Nickname,
                m.Timestamp,
                m.Sentiment
            });

            return Ok(response);
        }

        // POST: /api/chat/send
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] MessageRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Nickname) || string.IsNullOrWhiteSpace(request.Text))
            {
                return BadRequest("Rumuz ve mesaj metni boş olamaz.");
            }

            // 1. Kullanıcıyı bul veya oluştur 
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Nickname == request.Nickname);
            if (user == null)
            {
                user = new User { Nickname = request.Nickname };
                _context.Users.Add(user);
                await _context.SaveChangesAsync(); 
            }

            // 2. AI Servisini Çağır
            string sentiment = "Hata - Analiz Edilemedi"; // Varsayılan
            try
            {
                sentiment = await AnalyzeSentimentWithAI(request.Text);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AI Analiz Hatası: {ex.Message}");
            }

            // 3. Mesajı oluştur ve kaydet 
            var message = new Message
            {
                Text = request.Text,
                UserId = user.Id,
                Timestamp = DateTime.UtcNow,
                Sentiment = sentiment
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return Ok(new 
            { 
                message.Id, 
                message.Text, 
                Nickname = user.Nickname, 
                message.Timestamp, 
                message.Sentiment 
            });
        }

        // ---- DÜZELTİLMİŞ ve BASİTLEŞTİRİLMİŞ JSON OKUYUCU ----
 private async Task<string> AnalyzeSentimentWithAI(string text)
{
    var client = _httpClientFactory.CreateClient();

    var requestData = new { data = new object[] { text } };
    var content = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

    // POST -> event_id al
    var postResponse = await client.PostAsync(HF_API_URL, content);
    var postString = await postResponse.Content.ReadAsStringAsync();

    if (!postResponse.IsSuccessStatusCode)
    {
        Console.WriteLine($"POST başarısız: {postResponse.StatusCode} - {postString}");
        return "Hata - API Başarısız";
    }

    string eventId;
    try
    {
        using var doc = JsonDocument.Parse(postString);
        eventId = doc.RootElement.GetProperty("event_id").GetString();
    }
    catch
    {
        Console.WriteLine($"EventId çözümlenemedi. Gelen: {postString}");
        return "Hata - JSON Çözümlenemedi (POST)";
    }

    if (string.IsNullOrEmpty(eventId))
        return "Hata - EventId Yok";

    // GET -> SSE metni oku
    var getUrl = $"{HF_API_URL}/{eventId}";
    var getResp = await client.GetAsync(getUrl);

    var raw = await getResp.Content.ReadAsStringAsync();
    if (!getResp.IsSuccessStatusCode)
    {
        Console.WriteLine($"GET başarısız: {getResp.StatusCode} - {raw}");
        return "Hata - API GET Başarısız";
    }

    // Burada gelen veri genelde şöyle oluyor:
    // event: complete
    // data: ["N\u00f6tr"]
    var dataLine = raw.Split('\n')
                      .FirstOrDefault(l => l.Trim().StartsWith("data:"));
    if (string.IsNullOrEmpty(dataLine))
    {
        Console.WriteLine($"Veri satırı bulunamadı. Gelen: {raw}");
        return "Hata - Veri Yok";
    }

    var jsonPart = dataLine.Trim().Substring("data:".Length).Trim();
    try
    {
        using var doc2 = JsonDocument.Parse(jsonPart);
        var val = doc2.RootElement[0].GetString();
        return val ?? "Hata - Analiz Edilemedi";
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Data JSON hatası: {ex.Message}. Gelen: {jsonPart}");
        return "Hata - JSON Çözümlenemedi (Data)";
    }
}



    }
}