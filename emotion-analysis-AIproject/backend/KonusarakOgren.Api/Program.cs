using KonusarakOgren.Api.Data; // AppDbContext için bunu ekledik
using Microsoft.EntityFrameworkCore; // UseSqlite için bunu ekledik

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
// api için gerekli servisleri ekliyoruz


// 1. CORS (Cross-Origin Resource Sharing) Ayarlarını Ekleme
// (appsettings.json'daki adresleri okuyup politikayı belirler)
var corsConfig = builder.Configuration.GetSection("CorsOrigins");
var vercelApp = corsConfig["VercelApp"];
var localApp = corsConfig["LocalApp"];

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(vercelApp, localApp) // Sadece bu adreslere izin ver
              .AllowAnyHeader()                 // Tüm Header'lara izin ver
              .AllowAnyMethod();                // Tüm Metotlara (GET, POST, PUT) izin ver
    });
});

// 2. Veritabanı Bağlantısını (AppDbContext) Ekleme
// (appsettings.json'daki "DefaultConnection" yolunu okur)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// ----- EKLEMELER BİTTİ -----

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

//CORS kullanması için
app.UseCors(); // <-- 

app.UseAuthorization();

app.MapControllers();

app.Run();