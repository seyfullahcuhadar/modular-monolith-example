# 10 - Testler

## Amaç
Modül sınırlarını **otomatik olarak** korumak ve uçtan uca akışı doğrulamak.

## Kavram
Modular monolith'in en büyük riski zamanla "big ball of mud"a dönüşmesidir. Architecture testleri bu erozyonu CI'da yakalar.

## ArchitectureTests (NetArchTest.Rules)
- [ ] Modül X'in Domain/Application/Infrastructure/Presentation katmanları, modül Y'nin aynı katmanlarına bağımlı olamaz
- [ ] İzin verilen tek modüller arası bağımlılık: `*.PublicApi` ve `*.IntegrationEvents`
- [ ] Domain, Application ve Infrastructure'a bağımlı olamaz
- [ ] Application, Infrastructure ve Presentation'a bağımlı olamaz
- [ ] Handler'lar `sealed` ve public değil
- [ ] Domain event'ler `sealed` ve `DomainEvent`'ten türemeli
- [ ] Entity'lerin public setter'ı olmamalı

## Unit Testler (Domain)
- [ ] `Product.DecreaseStock` yetersiz stok
- [ ] `Order` geçersiz durum geçişleri
- [ ] `Basket` toplam hesaplama

## IntegrationTests (Testcontainers + WebApplicationFactory)
- [ ] PostgreSQL container ile host'u ayağa kaldır
- [ ] Mutlu yol: checkout -> sipariş `Confirmed`, stok düştü, bildirim var (polling ile bekleme)
- [ ] Başarısız ödeme: sipariş `Cancelled`, stok değişmedi
- [ ] Idempotency: aynı event iki kez -> tek sipariş

## Bitti Tanımı
- `dotnet test` tüm testler yeşil
- Kasıtlı olarak bir sınır ihlali eklendiğinde architecture testi kırmızı
