# 09 - Bootstrapper (Api Host)

## Amaç
Tüm modülleri tek bir süreçte barındıran host'u son haline getirmek.

## Kavram
Host "ince"dir: iş mantığı içermez, yalnızca modülleri bir araya getirir (composition root).

## Yapılacaklar
- [ ] `Program.cs`: her modül için `AddXModule()` ve `MapXEndpoints()`
- [ ] Modül konfigürasyonları: `modules.catalog.json` vb. ayrı appsettings dosyaları
- [ ] Serilog + Seq, request logging, `CorrelationId` middleware
- [ ] Global exception handler + ProblemDetails
- [ ] Başlangıçta tüm modül migration'larını uygula (Development)
- [ ] OpenAPI + Scalar UI, endpoint'leri modül adıyla gruplama (tag)
- [ ] Health checks: `/health` (postgres)
- [ ] Seed data (birkaç ürün)
- [ ] `Dockerfile` ve docker-compose'a `api` servisi
- [ ] `Api.http` dosyası: uçtan uca senaryo istekleri

## Bitti Tanımı
- `docker compose up` ile tüm sistem ayakta
- `Api.http` ile ürün -> sepet -> checkout -> sipariş -> ödeme -> bildirim akışı çalışıyor
