# 03 - Catalog Modülü

## Amaç
İlk modülü uçtan uca kurarak diğer modüller için **şablon** oluşturmak.

## Kavram
- Her modül kendi şemasına sahiptir: `catalog`
- Modül dışarıya yalnızca `Catalog.PublicApi` (senkron) ve `Catalog.IntegrationEvents` (asenkron) açar
- Diğer tüm tipler `internal` olur

## Yapılacaklar

### Catalog.Domain
- [x] `Product` aggregate: `Id`, `Name`, `Description`, `Price` (Money VO, yalnızca TRY), `StockQuantity`, `ReservedQuantity`, `AvailableQuantity`
- [x] Davranışlar: `Create`, `ChangePrice`, `AddStock`
- [ ] Rezervasyon davranışları (`Reserve`, `CommitReservation`, `ReleaseReservation`) adım 06'da eklenecek
- [x] Domain event'ler: `ProductCreatedDomainEvent`, `ProductPriceChangedDomainEvent` (dispatch adım 05'te)
- [x] `CatalogErrors`

### Catalog.Application
- [x] Command'lar: `CreateProduct`, `UpdateProductPrice`, `AddStock`
- [x] Query'ler: `GetProductById`, `GetProducts` (sayfalama + isim araması, `ILIKE`)
- [x] Validator'lar
- [x] `IProductRepository`, `IUnitOfWork` (modüle özel arayüzler; her modülün kendi `IUnitOfWork`'ü olur)

### Catalog.Infrastructure
- [x] `CatalogDbContext` (`HasDefaultSchema("catalog")`, migration geçmişi de `catalog` şemasında)
- [x] Entity configuration'ları, repository (`Money` complex property: `price_amount`, `price_currency`; stok check constraint'leri)
- [x] İlk migration (`InitialCatalog`)
- [x] `CatalogModule.AddCatalogModule(IServiceCollection, IConfiguration)`

### Catalog.PublicApi
- [x] `ICatalogApi`: `GetProductAsync(Guid id)` -> `ProductDto?` (Id, Name, Price, AvailableQuantity)
- [x] Implementasyon `Catalog.Infrastructure` içinde (`internal`); `GetProductById` query handler'ını çağırır, DbContext'i atlamaz

### Catalog.Presentation
- [x] Endpoint'ler: `POST /catalog/products`, `GET /catalog/products/{id}`, `GET /catalog/products`, `PUT /catalog/products/{id}/price`, `POST /catalog/products/{id}/stock`
- Detaylar ve durum kodları: [../gereksinimler.md](../gereksinimler.md#catalog-catalog)

### Api (erken sürüm)
- [x] Minimal `Program.cs`: modülü ekle, endpoint'leri map'le, migration uygula (Development), OpenAPI + Scalar
- [x] `Api.http`: S1 istekleri ve hata durumları

## Common'a Eklenenler
- `ValidationDecorator.QueryHandler`: query'ler de doğrulanır (`GET /catalog/products?pageSize=500` -> 400)
- `PagedResult<T>` (`Common.Application.Paging`)
- `AddModuleDbContext<TDbContext>(connectionString, schema)`: Npgsql + snake_case + şemaya özel migration geçmişi
- `ApplyModuleMigrationsAsync()`: kayıtlı tüm modül DbContext'lerinin migration'larını uygular

## Tasarım Notları
- Modül içi tipler `internal`; katmanlar arası erişim `InternalsVisibleTo` ile. Dışarı açık olanlar: `CatalogModule`, `ICatalogApi`/`ProductDto` ve host'un handler taraması için `Catalog.Application.AssemblyReference`.
- `AddApplication()` host'ta **bir kez** ve tüm modül assembly'leriyle çağrılır; birden fazla çağrı decorator'ları iki kez sarar.
- Migration komutu: `dotnet ef migrations add <Ad> --project src/Modules/Catalog/Catalog.Infrastructure --startup-project src/Bootstrapper/Api --context CatalogDbContext --output-dir Database/Migrations`

## Bitti Tanımı
- [x] Senaryo S1 (ürün yönetimi) Scalar veya `Api.http` üzerinden çalışıyor
- [x] Veritabanında `catalog.products` tablosu var
