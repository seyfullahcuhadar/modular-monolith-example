# 01 - Solution İskeleti

## Amaç
Tüm projelerin oturacağı klasör yapısını, merkezi build/paket ayarlarını ve altyapı servislerini hazırlamak.

## Kavram
Modular monolith'te modüller fiziksel olarak ayrı projelerdir; bu, derleyici seviyesinde sınır sağlar (bir modül diğerine referans vermeden onun tiplerini kullanamaz).

## Yapılacaklar
- [x] Mevcut `ModularMonolithExample/` konsol projesini sil, `.sln` dosyasından çıkar
- [x] Klasörleri oluştur: `src/Bootstrapper`, `src/Common`, `src/Modules`, `tests`
- [x] `Directory.Build.props`: `net10.0`, `Nullable`, `ImplicitUsings`, `TreatWarningsAsErrors`
- [x] `Directory.Packages.props`: Central Package Management (tüm paket versiyonları tek yerde)
- [x] `global.json`: SDK 10.0.403'e sabitlendi
- [x] `.editorconfig` ve `.gitignore`
- [x] `docker-compose.yml`:
  - `postgres` (port 5432, volume)
  - `seq` (port 5341 / UI 8081)
  - `api` (adım 09'da eklenecek)
- [x] Solution klasörleri (Rider'da düzenli görünüm için) - `dotnet sln add` klasör yolundan otomatik oluşturuyor

## Hedef Klasör Yapısı
```
src/
  Bootstrapper/Api
  Common/Common.Domain | Common.Application | Common.Infrastructure | Common.Presentation
  Modules/<Modül>/<Modül>.Domain | .Application | .Infrastructure | .Presentation | .IntegrationEvents | .PublicApi
tests/
  ArchitectureTests
  IntegrationTests
```

## Bitti Tanımı
- [x] `dotnet build` boş solution ile başarılı
- [x] `docker compose up -d` ile postgres ve seq ayakta
