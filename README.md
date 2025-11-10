# Konuşarak Öğren — Full‑Stack + AI Stajyer Projesi

Bu proje, `Konuşarak Öğren` için geliştirilmiş 3 günlük bir Full‑Stack + AI duygu analizi staj projesinin kaynak kodlarını içerir. Proje; react ile web ve mobil arayüzleri, .NET tabanlı bir backend API'yi ve bir Gradio tabanlı AI servisini kapsar. Canlı olarak Vercel (frontend), Render (backend) ve Hugging Face Spaces (AI) üzerinde yayınlanmıştır.

## Demo (Canlı Linkler)

- Web (Vercel): https://stajyer-projem.vercel.app
- Backend API (Render): https://stajyerprojem-api.onrender.com  (örnek endpoint: `/api/chat/messages`)
- AI Servis (Hugging Face Space): https://filizyucel-emotion-analysis-api.hf.space

---

## Teknolojiler

- AI: Python + Gradio (Hugging Face Spaces)
- Backend: .NET Web API (Entity Framework Core, SQLite for development)
- Frontend (Web): React (Create React App)
- Frontend (Mobile): React Native (React Native CLI)
- Deployment: Vercel (frontend), Render (backend), Hugging Face Spaces (AI)

> Not: Render üzerinde deploy sırasında EF Core migrasyonları ile veritabanı tabloları oluşturulmaktadır. Ücretsiz Render dosya sisteminin kalıcılığı sınırlı olabileceğinden production için kalıcı bir veritabanı yoktur.

---

## Proje Klasör Yapısı (özet)

- `ai-service/` — Gradio tabanlı AI "app.py" pozitif/nötr/negatif duygu durumları için basit ai algoritması yazdım bazı belirli kelimelere göre hangi duygu durumu olduğunu belirliyor klasör detayı websitede (Hugging Face Space - api )
- `backend/KonusarakOgren.Api/` — .NET API (tek tek eklendiler > Controllers, Models, Data, Migrations, Dockerfile, appsettings.json) ilk program.cs ile ilerleyip renger 'da ücretsiz servis açarken .net bulunmadığı için docker üzerinden açtım ve dockerfile ise bunun için eklendi.
- `frontend/chat-web/` — React web uygulaması burası için login ve chat olarak 2 sayfa components oluşturup websiteden giren birinin adını loginde alıp kaydedip sohbet sayfasına geçince orada gözükmesini ve veritabanına kaydedilmesini sağladım.
- `EmotionApp/` — React Native mobil uygulaması asıl kodlamamız burada App.tsx içerisindedir.

## API (kısa)

- `GET /api/chat/messages` — Kaydedilmiş mesajları listeler
- `POST /api/chat/send` — Yeni mesaj gönderir

POST body (JSON):

```json
{
	"Nickname": "kullaniciRumuzu",
	"Text": "Göndermek istediğiniz metin"
}
```

Response: Gönderilen mesajın kayıt bilgilerini (id, text, nickname, timestamp, sentiment) döner.

---

## Yerel Çalıştırma (özet)

Gereksinimler:
- .NET 8 SDK
- Node.js + npm
- Python 3 (AI servisi için)

Çalıştırma adımları (örnek):

```powershell
git clone https://github.com/FilizzYucel/stajyerProjem.git
cd stajyerProjem

# Backend
cd emotion-analysis-AIproject/backend/KonusarakOgren.Api
dotnet restore
dotnet run

# Frontend
cd ../../frontend/chat-web
npm install
npm start
# Tarayıcı: http://localhost:3000
```

---

## Bilinen Notlar / Troubleshooting

- Eğer tarayıcıda CORS hatası alırsanız, `backend/KonusarakOgren.Api/Program.cs` içindeki CORS ayarlarını ve `appsettings.json` içindeki `CorsOrigins` değerlerini kontrol edin.
- İlk deploy sırasında `no such table` hatası alırsanız EF Core migrasyonlarının çalıştırıldığından emin olun. Bu repo startup sırasında `db.Database.Migrate()` çağrısı ile migrasyonları uygular.
- Kullandığım AI araçları: Gemini PRO, chatGPT, VScode Chat.


---

Geliştirici: Filiz Yücel — GitHub: https://github.com/FilizzYucel
