# Mimari ve İlerleme

Bu doküman projenin şu anki durumunu ve hedeflenen mimariyi diyagramlarla özetler. Her adım tamamlandıkça güncellenir.

**Durum:** 11 adımın 4'ü tamamlandı. Catalog ve Basket ayakta; senkron iletişim `ICatalogApi` üzerinden çalışıyor.

## 1. Yol Haritası

```mermaid
flowchart LR
    s01["01 İskelet - TAMAM"] --> s02["02 Common - TAMAM"]
    s02 --> s03["03 Catalog + ilk host - TAMAM"]
    s03 --> s04["04 Basket - TAMAM"]
    s04 --> s05["05 Outbox / Inbox - SIRADAKİ"]
    s05 --> s06["06 Ordering"]
    s06 --> s07["07 Payment"]
    s07 --> s08["08 Notification"]
    s08 --> s09["09 Host son hali"]
    s09 --> s10["10 Testler"]
    s10 --> s11["11 Dokümantasyon"]
```

Adımların detayları: [plan/00-genel-bakis.md](plan/00-genel-bakis.md)

## 2. Şu An Elimizde Olanlar

1. adımda dosya ve altyapı tarafı kuruldu, Postgres ve Seq container'ları çalışıyor. 2. adımda dört ortak proje yazıldı:

```mermaid
flowchart TB
    subgraph infra [Altyapı - Adım 01]
        sln["ModularMonolithExample.sln"]
        props["Directory.Build.props ve Directory.Packages.props"]
        compose["docker-compose: Postgres 17 ve Seq"]
    end
    subgraph common [Common Projeleri - Adım 02]
        domain["Common.Domain"]
        app["Common.Application"]
        cinfra["Common.Infrastructure"]
        pres["Common.Presentation"]
    end
    app --> domain
    cinfra --> app
    pres --> domain
```

Projelerin içindeki temel tipler:

```mermaid
classDiagram
    class Entity~TId~ {
        +TId Id
        +Equals()
    }
    class AggregateRoot~TId~ {
        +DomainEvents
        +ClearDomainEvents()
        #Raise(IDomainEvent)
    }
    class Result {
        +IsSuccess
        +Error
        +Success()
        +Failure(Error)
    }
    class Error {
        +Code
        +Description
        +ErrorType Type
    }
    class ValidationError {
        +Errors
    }
    class ICommandHandler~TCommand,TResponse~ {
        +Handle(command, ct) Result
    }
    class IQueryHandler~TQuery,TResponse~ {
        +Handle(query, ct) Result
    }
    class IEventBus {
        +PublishAsync(IIntegrationEvent)
    }
    class IEndpoint {
        +MapEndpoint(app)
    }
    class ApiResults {
        +Problem(Result) IResult
    }
    Entity <|-- AggregateRoot
    Error <|-- ValidationError
    Result --> Error
    ApiResults ..> Result
```

| Proje | İçerik |
|---|---|
| `Common.Domain` | `Entity`, `AggregateRoot`, `DomainEvent`, `Result`, `Error` |
| `Common.Application` | Command/query/handler arayüzleri, `IEventBus`, decorator'lar, `AddApplication()` |
| `Common.Infrastructure` | `AddInfrastructure()` (`TimeProvider`), `AddModuleDbContext()`, `ApplyModuleMigrationsAsync()`; Outbox/Inbox adım 05'te |
| `Common.Presentation` | `IEndpoint`, `ApiResults`, `Match` extension'ları |

## 3. Bir İstek Handler'a Nasıl Ulaşıyor

Adım 02 sonunda smoke test ile doğrulanan zincir:

```mermaid
sequenceDiagram
    participant Endpoint
    participant Logging as LoggingDecorator
    participant Validation as ValidationDecorator
    participant Handler
    Endpoint->>Logging: Handle(command)
    Logging->>Validation: Handle(command)
    alt Geçersiz komut
        Validation-->>Logging: ValidationError
    else Geçerli komut
        Validation->>Handler: Handle(command)
        Handler-->>Validation: Result
        Validation-->>Logging: Result
    end
    Logging-->>Endpoint: Result ve süre logu
    Endpoint->>Endpoint: "Match: 200 veya ProblemDetails (400/404/409/500)"
```

## 4. Hedef Mimari

Tek süreç, tek deploy birimi; ama her modülün kendi şeması var.

```mermaid
flowchart TB
    client["İstemci / Api.http"] --> api["Api Host - tek deploy birimi"]
    subgraph modules [Modüller]
        catalog["Catalog"]
        basket["Basket"]
        ordering["Ordering"]
        payment["Payment"]
        notification["Notification"]
    end
    api --> catalog
    api --> basket
    api --> ordering
    api --> payment
    api --> notification
    basket -->|"senkron: ICatalogApi"| catalog
    bus["In-memory Event Bus + Outbox/Inbox"]
    basket -.-> bus
    ordering -.-> bus
    payment -.-> bus
    catalog -.-> bus
    bus -.-> ordering
    bus -.-> payment
    bus -.-> catalog
    bus -.-> notification
    subgraph db [PostgreSQL - tek instance]
        sc["catalog şeması"]
        sb["basket şeması"]
        so["ordering şeması"]
        sp["payment şeması"]
        sn["notification şeması"]
    end
    catalog --> sc
    basket --> sb
    ordering --> so
    payment --> sp
    notification --> sn
```

## 5. Modülün İçi ve Sınır Kuralı

Örnek: Basket modülü Catalog'a nasıl bağlanır. Adım 10'daki architecture testleri bu kuralı otomatik denetler.

```mermaid
flowchart LR
    subgraph basketMod [Basket Modülü]
        bPres["Basket.Presentation"] --> bApp["Basket.Application"]
        bApp --> bDom["Basket.Domain"]
        bInf["Basket.Infrastructure"] --> bApp
    end
    subgraph catalogMod [Catalog Modülü]
        cPub["Catalog.PublicApi"]
        cEvt["Catalog.IntegrationEvents"]
        cApp["Catalog.Application"]
        cDom["Catalog.Domain"]
        cInf["Catalog.Infrastructure"]
        cInf -.->|implements| cPub
    end
    bApp -->|"İZİNLİ"| cPub
    bApp -->|"İZİNLİ"| cEvt
    bApp -.-x|"YASAK"| cDom
    bInf -.-x|"YASAK"| cInf
```

Kural: Bir modül başka bir modülün yalnızca `*.PublicApi` ve `*.IntegrationEvents` projelerine referans verebilir.

## 6. Sipariş Akışı - Stok Rezervasyonlu Saga (Adım 04 - 08)

Modüller birbirini doğrudan çağırmaz, event'lerle haberleşir. Stok ödemeden önce rezerve edilir, ödeme başarısız olursa serbest bırakılır (telafi). Karar gerekçesi ve event kataloğu: [gereksinimler.md](gereksinimler.md#karar-stok-rezervasyonu-saga--telafi)

```mermaid
sequenceDiagram
    participant Basket
    participant Ordering
    participant Catalog
    participant Payment
    participant Notification
    Basket->>Ordering: BasketCheckedOut
    Note over Ordering: Pending
    Ordering->>Catalog: OrderPlaced
    alt Stok yeterli
        Note over Catalog: reserved += miktar
        Catalog->>Ordering: StockReserved
        Note over Ordering: StockReserved
        Ordering->>Payment: OrderPaymentRequested
        alt Ödeme başarılı
            Payment->>Ordering: PaymentSucceeded
            Note over Ordering: Confirmed
            Ordering->>Catalog: OrderConfirmed
            Note over Catalog: stock -= miktar, reserved -= miktar
            Ordering->>Notification: OrderConfirmed
        else Ödeme reddedildi
            Payment->>Ordering: PaymentFailed
            Note over Ordering: Cancelled
            Ordering->>Catalog: OrderCancelled
            Note over Catalog: "reserved -= miktar (telafi)"
            Ordering->>Notification: OrderCancelled
        end
    else Stok yetersiz
        Catalog->>Ordering: StockReservationFailed
        Note over Ordering: Cancelled
        Ordering->>Notification: OrderCancelled
    end
```

Sipariş durum makinesi:

```mermaid
stateDiagram-v2
    [*] --> Pending: BasketCheckedOut
    Pending --> StockReserved: StockReserved
    Pending --> Cancelled: StockReservationFailed
    StockReserved --> Confirmed: PaymentSucceeded
    StockReserved --> Cancelled: PaymentFailed
    Confirmed --> [*]
    Cancelled --> [*]
```

Gereksinimler, endpoint'ler ve senaryolar: [gereksinimler.md](gereksinimler.md)

## 7. Catalog Modülü (Adım 03 - Tamamlandı)

Uygulama ilk kez `dotnet run` ile çalışıyor; S1 senaryosu `Api.http` ile doğrulandı.

```mermaid
flowchart LR
    http["POST /catalog/products"] --> ep["CreateProductEndpoint"]
    ep --> h["CreateProductHandler - Logging ve Validation ile"]
    h --> agg["Product aggregate"]
    h --> repo["ProductRepository"]
    get["GetProductByIdHandler"] --> repo
    repo --> ctx["CatalogDbContext"]
    ctx --> tbl["catalog.products tablosu"]
    pub["ICatalogApi"] --> get
```

## 8. Basket ve Senkron Sınır (Adım 04 - Tamamlandı)

Basket, Catalog'un tablosuna dokunmaz. Ürün bilgisi yalnızca `ICatalogApi` ile istenir; fiyat sepete **snapshot** olarak yazılır.

```mermaid
sequenceDiagram
    participant Client
    participant Basket
    participant CatalogApi as ICatalogApi
    participant Catalog
    Client->>Basket: POST /basket/{id}/items
    Basket->>CatalogApi: GetProductAsync(productId)
    CatalogApi->>Catalog: GetProductById query
    Catalog-->>Basket: ProductDto (name, price, available)
    alt Ürün yok
        Basket-->>Client: 404
    else Stok yetersiz
        Basket-->>Client: 409
    else Uygun
        Note over Basket: Satır yoksa ekle, varsa miktarı artır. Fiyatı değiştirme.
        Basket-->>Client: 204
    end
```

S2, S3 ve S6 `Api.http` ile doğrulandı. Checkout `BasketCheckedOut` domain event'ini üretir ama henüz yayınlamaz; bu adım 05'te Outbox'a bağlanacak.
