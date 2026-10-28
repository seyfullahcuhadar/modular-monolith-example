# 03 - Catalog Modülü

## Amaç
İlk modülü uçtan uca kurarak diğer modüller için **şablon** oluşturmak.

## Kavram
- Her modül kendi şemasına sahiptir: `catalog`
- Modül dışarıya yalnızca `Catalog.PublicApi` (senkron) ve `Catalog.IntegrationEvents` (asenkron) açar
- Diğer tüm tipler `internal` olur

## Yapılacaklar

### Catalog.Domain
- [ ] `Product` aggregate: `Id`, `Name`, `Description`, `Price` (Money VO), `StockQuantity`
- [ ] Davranışlar: `Create`, `ChangePrice`, `DecreaseStock` (yetersiz stokta hata)
- [ ] Domain event'ler: `ProductCreated`, `ProductPriceChanged`
- [ ] `CatalogErrors`

### Catalog.Application
- [ ] Command'lar: `CreateProduct`, `UpdateProductPrice`
- [ ] Query'ler: `GetProductById`, `GetProducts` (sayfalama)
- [ ] Validator'lar
- [ ] `IProductRepository`, `IUnitOfWork`

### Catalog.Infrastructure
- [ ] `CatalogDbContext` (`HasDefaultSchema("catalog")`)
- [ ] Entity configuration'ları, repository
- [ ] İlk migration
- [ ] `CatalogModule.AddCatalogModule(IServiceCollection, IConfiguration)`

### Catalog.PublicApi
- [ ] `ICatalogApi`: `GetProductAsync(Guid id)` -> `ProductDto?`
- [ ] Implementasyon `Catalog.Infrastructure` içinde (`internal`)

### Catalog.Presentation
- [ ] Endpoint'ler: `POST /catalog/products`, `GET /catalog/products/{id}`, `GET /catalog/products`, `PUT /catalog/products/{id}/price`

### Api (erken sürüm)
- [ ] Minimal `Program.cs`: modülü ekle, endpoint'leri map'le, migration uygula, OpenAPI

## Bitti Tanımı
- Swagger/Scalar üzerinden ürün oluşturulup okunabiliyor
- Veritabanında `catalog.products` tablosu var
