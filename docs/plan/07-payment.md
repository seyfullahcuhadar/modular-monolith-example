# 07 - Payment Modülü

## Amaç
Dış sistem entegrasyonunu (ödeme sağlayıcı) bir modül arkasında izole etmek.

## Kavram
Dış sağlayıcı bir port (`IPaymentProvider`) arkasındadır; gerçek sağlayıcıya geçiş yalnızca bu modülü etkiler.

## Yapılacaklar

### Payment.Domain
- [ ] `Payment` aggregate (`OrderId`, `Amount`, `Status`: Pending/Succeeded/Failed, `FailureReason`)

### Payment.Application
- [ ] `OrderPlacedIntegrationEventHandler` -> `ProcessPayment`
- [ ] `IPaymentProvider` portu
- [ ] Query: `GetPaymentByOrderId`

### Payment.Infrastructure
- [ ] `FakePaymentProvider`: yapılandırılabilir kural (ör. tutar > 10.000 ise reddet) - başarısız senaryoyu test etmek için
- [ ] `PaymentDbContext` (şema: `payment`), migration

### Payment.IntegrationEvents
- [ ] `PaymentSucceededIntegrationEvent`, `PaymentFailedIntegrationEvent`

### Payment.Presentation
- [ ] `GET /payments/{orderId}`

## Bitti Tanımı
- Normal tutarda sipariş `Confirmed`, yüksek tutarda `Cancelled` oluyor
