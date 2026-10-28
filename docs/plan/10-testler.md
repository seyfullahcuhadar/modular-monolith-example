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
- [ ] `Product.Reserve` kullanılabilir stoktan fazlası istenirse hata; `CommitReservation` / `ReleaseReservation` sayaçları doğru güncelliyor
- [ ] `Order` geçersiz durum geçişleri
- [ ] `Basket` toplam hesaplama ve aynı ürünün miktar birleştirmesi

## IntegrationTests (Testcontainers + WebApplicationFactory)
Senaryolar: [../gereksinimler.md](../gereksinimler.md#senaryolar)
- [ ] PostgreSQL container ile host'u ayağa kaldır
- [ ] S4 mutlu yol: sipariş `Confirmed`, stok düştü, rezerve 0, bildirim var (polling ile bekleme)
- [ ] S5 ödeme reddi: sipariş `Cancelled`, rezervasyon serbest, stok değişmedi
- [ ] S10 stok yarışı: ikinci sipariş `Cancelled`, ödeme kaydı yok
- [ ] S7 idempotency: aynı event iki kez -> tek sipariş, tek ödeme
- [ ] S8 tekrar deneme: ilk denemede hata fırlatan handler sonunda başarılı

## Bitti Tanımı
- `dotnet test` tüm testler yeşil
- Kasıtlı olarak bir sınır ihlali eklendiğinde architecture testi kırmızı
