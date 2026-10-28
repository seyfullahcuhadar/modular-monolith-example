# 08 - Notification Modülü ve Stok Düşümü

## Amaç
Yalnızca event **tüketen** bir modül göstermek ve akışı Catalog'daki stok düşümüyle kapatmak.

## Yapılacaklar

### Notification modülü
- [ ] `Notification` entity (`CustomerId`, `Channel`, `Subject`, `Body`, `SentOnUtc`)
- [ ] `IEmailSender` portu + `LoggingEmailSender` (Seq'e log yazar)
- [ ] Handler'lar: `OrderConfirmed`, `OrderCancelled`, `PaymentFailed`
- [ ] `NotificationDbContext` (şema: `notification`), migration
- [ ] `GET /notifications?customerId=`

### Catalog'a ek
- [ ] `OrderConfirmedIntegrationEventHandler` -> her satır için `DecreaseStock`
- [ ] Catalog projesi `Ordering.IntegrationEvents`'e referans verir (izin verilen tek bağımlılık türü)

## Bitti Tanımı
- Başarılı sipariş sonrası stok düşüyor ve bildirim kaydı oluşuyor
- Tam akış Seq'te correlation ile izlenebiliyor
