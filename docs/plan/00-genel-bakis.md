# 00 - Genel Bakış

## Amaç
"Building Software Architecture" kitabındaki **Modular Monolith** mimarisini, tatmin edici derinlikte bir e-ticaret uygulamasıyla somutlaştırmak.

## Modular Monolith'in Gösterilecek Özellikleri
| Özellik | Projede nasıl gösterilecek |
|---|---|
| Tek deploy birimi | Tek `Api` host tüm modülleri yükler |
| Güçlü modül sınırları | Her modül kendi projelerinde, `internal` tipler, sadece `PublicApi` ve `IntegrationEvents` dışa açık |
| Veri sahipliği | Tek PostgreSQL, modül başına ayrı şema + ayrı `DbContext` + ayrı migration |
| Senkron iletişim | `ICatalogApi` gibi public kontratlar |
| Asenkron iletişim | Integration event + Outbox/Inbox + in-memory event bus |
| Sınırların korunması | NetArchTest ile architecture testleri |
| Evrimleşebilirlik | Event bus soyut; ileride RabbitMQ ile bir modül mikroservise ayrılabilir |

## Modüller
- **Catalog**: Ürün, fiyat, stok
- **Basket**: Sepet, checkout
- **Ordering**: Sipariş ve durum makinesi
- **Payment**: Sahte ödeme sağlayıcı
- **Notification**: Bildirim kayıtları

## Teknoloji Yığını
- .NET 10, ASP.NET Core Minimal API
- EF Core + Npgsql (PostgreSQL)
- Kendi CQRS soyutlamalarımız + Scrutor (decorator)
- FluentValidation, Serilog + Seq
- xUnit, NetArchTest.Rules, Testcontainers
- Docker Compose

## Adımlar
| # | Dosya | Konu |
|---|---|---|
| 01 | [01-iskelet.md](01-iskelet.md) | Solution iskeleti, build ayarları, docker-compose |
| 02 | [02-common.md](02-common.md) | Ortak building block'lar |
| 03 | [03-catalog.md](03-catalog.md) | Catalog modülü + public API |
| 04 | [04-basket.md](04-basket.md) | Basket modülü + senkron iletişim |
| 05 | [05-outbox-inbox.md](05-outbox-inbox.md) | Event bus, Outbox, Inbox |
| 06 | [06-ordering.md](06-ordering.md) | Ordering modülü + durum makinesi |
| 07 | [07-payment.md](07-payment.md) | Payment modülü |
| 08 | [08-notification.md](08-notification.md) | Notification modülü |
| 09 | [09-bootstrapper.md](09-bootstrapper.md) | Api host, cross-cutting concerns |
| 10 | [10-testler.md](10-testler.md) | Architecture ve integration testleri |
| 11 | [11-dokumantasyon.md](11-dokumantasyon.md) | README ve mimari kararlar (ADR) |

> Not: Adım 09 (host) erken bir sürümüyle adım 03'te ayağa kaldırılır, her modül eklendikçe genişletilir. 09 dosyası host'un son halini tanımlar.

## Çalışma Kuralı
Her adım sonunda: `dotnet build` hatasız, ilgili testler yeşil, adımın "Bitti Tanımı" maddeleri işaretli.
