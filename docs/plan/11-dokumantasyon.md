# 11 - Dokümantasyon

## Amaç
Projeyi, kitaptaki kavramlarla ilişkilendiren bir öğrenme kaynağına dönüştürmek.

## Yapılacaklar
- [ ] `README.md`
  - Mimari diyagram (modüller, şemalar, event akışı)
  - Modül kuralları (ne referans verilebilir, ne verilemez)
  - Çalıştırma adımları (`docker compose up`, `dotnet test`)
  - Uçtan uca senaryo
- [ ] `docs/adr/` - Architecture Decision Records:
  - ADR-001: Modular monolith tercih edildi (mikroservis yerine neden)
  - ADR-002: Modül başına şema, tek veritabanı
  - ADR-003: Outbox/Inbox ile asenkron iletişim
  - ADR-004: Kendi CQRS soyutlamamız (MediatR yerine)
  - ADR-005: Architecture testleriyle sınır koruması
- [ ] `docs/modulu-mikroservise-ayirma.md`: Payment modülünü ayırmak için gereken adımlar (event bus -> RabbitMQ, PublicApi -> HTTP client)

## Bitti Tanımı
- Projeyi ilk kez açan biri README ile 10 dakikada çalıştırıp akışı anlayabiliyor
