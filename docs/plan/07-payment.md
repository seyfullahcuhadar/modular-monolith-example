# 07 - Payment Modülü

## Amaç
Dış sistem entegrasyonunu (ödeme sağlayıcı) bir modül arkasında izole etmek.

## Kavram
Dış sağlayıcı bir port (`IPaymentProvider`) arkasındadır; gerçek sağlayıcıya geçiş yalnızca bu modülü etkiler.

## Yapılacaklar

### Payment.Domain
- [ ] `Payment` aggregate (`OrderId`, `Amount`, `Status`: Pending/Succeeded/Failed, `FailureReason`)
- [ ] Sipariş başına tek ödeme (OrderId üzerinde unique index)

### Payment.Application
- [ ] `OrderPaymentRequestedIntegrationEventHandler` -> `ProcessPayment`
- [ ] `IPaymentProvider` portu
- [ ] Query: `GetPaymentByOrderId`

### Payment.Infrastructure
- [ ] `FakePaymentProvider`: 10.000 TRY üzerindeki tutarları reddeder (eşik yapılandırmadan okunur). Başarısız senaryo bu kuralla üretilir
- [ ] `PaymentDbContext` (şema: `payment`), migration

### Payment.IntegrationEvents
- [ ] `PaymentSucceededIntegrationEvent` (OrderId, PaymentId)
- [ ] `PaymentFailedIntegrationEvent` (OrderId, Reason)

### Payment.Presentation
- [ ] `GET /payments/{orderId}`

## Bitti Tanımı
- Normal tutarda sipariş `Confirmed`, 10.000 TRY üzerinde `Cancelled` oluyor
- Stoğu rezerve edilemeyen sipariş için ödeme kaydı oluşmuyor
