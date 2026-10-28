# 03 - Catalog Modülü

## Amaç
İlk modülü uçtan uca kurarak diğer modüller için **şablon** oluşturmak.

## Kavram
- Her modül kendi şemasına sahiptir: `catalog`
- Modül dışarıya yalnızca `Catalog.PublicApi` (senkron) ve `Catalog.IntegrationEvents` (asenkron) açar
- Diğer tüm tipler `internal` olur

## Yapılacaklar

### Catalog.Domain
- [ ] `Product` aggregate: `Id`, `Name`, `Description`, `Price` (Money VO, yalnızca TRY), `StockQuantity`, `ReservedQuantity`, `AvailableQuantity`
- [ ] Davranışlar: `Create`, `ChangePrice`, `AddStock`
- [ ] Rezervasyon davranışları (`Reserve`, `CommitReservation`, `ReleaseReservation`) adım 06'da eklenecek
- [ ] Domain event'ler: `ProductCreated`, `ProductPriceChanged`
- [ ] `CatalogErrors`

### Catalog.Application
- [ ] Command'lar: `CreateProduct`, `UpdateProductPrice`, `AddStock`
- [ ] Query'ler: `GetProductById`, `GetProducts` (sayfalama + isim araması)
- [ ] Validator'lar
- [ ] `IProductRepository`, `IUnitOfWork`

### Catalog.Infrastructure
- [ ] `CatalogDbContext` (`HasDefaultSchema("catalog")`)
- [ ] Entity configuration'ları, repository
- [ ] İlk migration
- [ ] `CatalogModule.AddCatalogModule(IServiceCollection, IConfiguration)`

### Catalog.PublicApi
- [ ] `ICatalogApi`: `GetProductAsync(Guid id)` -> `ProductDto?` (Id, Name, Price, AvailableQuantity)
- [ ] Implementasyon `Catalog.Infrastructure` içinde (`internal`)

### Catalog.Presentation
- [ ] Endpoint'ler: `POST /catalog/products`, `GET /catalog/products/{id}`, `GET /catalog/products`, `PUT /catalog/products/{id}/price`, `POST /catalog/products/{id}/stock`
- Detaylar ve durum kodları: [../gereksinimler.md](../gereksinimler.md#catalog-catalog)

### Api (erken sürüm)
- [ ] Minimal `Program.cs`: modülü ekle, endpoint'leri map'le, migration uygula, OpenAPI

## Bitti Tanımı
- Senaryo S1 (ürün yönetimi) Scalar veya `Api.http` üzerinden çalışıyor
- Veritabanında `catalog.products` tablosu var
