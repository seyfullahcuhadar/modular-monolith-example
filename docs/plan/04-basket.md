# 04 - Basket Modülü

## Amaç
**Senkron modüller arası iletişimi** doğru şekilde göstermek.

## Kavram
Basket, ürün fiyatını Catalog'un veritabanından **okumaz**; yalnızca `ICatalogApi` üzerinden ister. Basket projeleri sadece `Catalog.PublicApi`'ye referans verir.

```mermaid
flowchart LR
    BasketApp["Basket.Application"] --> CatalogPublicApi["Catalog.PublicApi"]
    CatalogInfra["Catalog.Infrastructure"] -.->|implements| CatalogPublicApi
```

## Yapılacaklar

### Basket.Domain
- [ ] `Basket` aggregate (`CustomerId`, `Items`)
- [ ] `BasketItem` (`ProductId`, `ProductName`, `UnitPrice`, `Quantity`) - fiyat snapshot'ı
- [ ] Davranışlar: `AddItem`, `RemoveItem`, `Clear`, `Checkout`
- [ ] Domain event: `BasketCheckedOutDomainEvent`

### Basket.Application
- [ ] `AddItemToBasket` (ICatalogApi ile ürün + fiyat doğrulama)
- [ ] `RemoveItemFromBasket`, `GetBasket`, `CheckoutBasket`

### Basket.Infrastructure
- [ ] `BasketDbContext` (şema: `basket`), migration

### Basket.IntegrationEvents
- [ ] `BasketCheckedOutIntegrationEvent` (`CustomerId`, `Items`, `TotalAmount`)
  - Yayınlama adım 05'te Outbox ile bağlanacak

### Basket.Presentation
- [ ] `GET /basket/{customerId}`, `POST /basket/{customerId}/items`, `DELETE /basket/{customerId}/items/{productId}`, `POST /basket/{customerId}/checkout`

## Bitti Tanımı
- Var olmayan ürün sepete eklenemiyor (404 ProblemDetails)
- Basket projelerinde Catalog'un yalnızca PublicApi referansı var
