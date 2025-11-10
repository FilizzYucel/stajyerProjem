# 1. Adım: .NET 8 SDK (Projemizi derlemek için)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 2. Adım: Proje dosyalarını kopyala
COPY emotion-analysis-AIproject/backend/KonusarakOgren.Api/*.csproj ./
RUN dotnet restore

# 3. Adım: Tüm kaynak kodunu kopyala ve projeyi derle (Build)
COPY emotion-analysis-AIproject/backend/KonusarakOgren.Api/ ./
RUN dotnet build "KonusarakOgren.Api.csproj" -c Release -o /app/build

# 4. Adım: Derlenen projeyi yayınla (Publish)
FROM build AS publish
RUN dotnet publish "KonusarakOgren.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 5. Adım: Sonuç (Çalıştırma Adımı)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Render'ın hangi portu dinleyeceğini bilmesi için
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "KonusarakOgren.Api.dll"]