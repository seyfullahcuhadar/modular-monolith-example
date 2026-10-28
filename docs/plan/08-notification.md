# 08 - Notification Modülü ve Saga'nın Kapanışı

## Amaç
Yalnızca event **tüketen** bir modül göstermek ve saga'yı Catalog'daki rezervasyonu kesinleştirerek veya serbest bırakarak kapatmak.

## Yapılacaklar

### Notification modülü
- [ ] `Notification` entity (`CustomerId`, `Channel`, `Subject`, `Body`, `SentOnUtc`)
- [ ] `IEmailSender` portu + `LoggingEmailSender` (Seq'e log yazar)
- [ ] Handler'lar: `OrderConfirmed`, `OrderCancelled` (sebep bildirime yazılır)
- [ ] `NotificationDbContext` (şema: `notification`), migration
- [ ] `GET /notifications?customerId=`

### Catalog'a ek: Rezervasyonun kapanışı
- [ ] `OrderConfirmed` handler -> `StockReservation` kaydından satırları oku -> `CommitReservation` (stock ve reserved düşer)
- [ ] `OrderCancelled` handler -> rezervasyon varsa `ReleaseReservation` (telafi). Yoksa etkisiz
- [ ] Her iki handler da idempotent: zaten `Committed` veya `Released` olan rezervasyon tekrar işlenmez

## Bitti Tanımı
- Senaryolar S4 (mutlu yol), S5 (ödeme reddi + telafi) ve S10 (stok yarışı) uçtan uca çalışıyor
- Tam akış Seq'te correlation Id ile izlenebiliyor
