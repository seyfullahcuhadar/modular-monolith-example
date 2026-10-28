# 02 - Common (Building Blocks)

## Amaç
Tüm modüllerin kullanacağı, **iş mantığı içermeyen** teknik yapı taşlarını oluşturmak.

## Kavram
Common katmanı "shared kernel" değil, teknik altyapıdır. İçine domain kavramı (ör. `Product`) koymak modülleri birbirine bağlar; bu yüzden yalnızca jenerik tipler bulunur.

## Yapılacaklar

### Common.Domain
- [ ] `Entity`, `AggregateRoot` (domain event listesi ile)
- [ ] `IDomainEvent`, `DomainEvent` (Id, OccurredOnUtc)
- [ ] `Result`, `Result<T>`, `Error` (ErrorType: Validation, NotFound, Conflict, Failure)

### Common.Application
- [ ] `ICommand`, `ICommand<TResponse>`, `IQuery<TResponse>`
- [ ] `ICommandHandler<...>`, `IQueryHandler<...>`
- [ ] Decorator'lar: `ValidationDecorator` (FluentValidation), `LoggingDecorator`
- [ ] `IDomainEventHandler<T>`
- [ ] `IIntegrationEvent`, `IIntegrationEventHandler<T>`, `IEventBus`
- [ ] `IDateTimeProvider`

### Common.Infrastructure
- [ ] `DateTimeProvider`
- [ ] Handler + decorator kaydı için Scrutor tabanlı extension
- [ ] (Outbox/Inbox ve EventBus adım 05'te eklenecek)

### Common.Presentation
- [ ] `IEndpoint` arayüzü (`MapEndpoint(IEndpointRouteBuilder)`)
- [ ] Assembly taramasıyla endpoint kaydı
- [ ] `Result` -> `IResult` / ProblemDetails dönüştürücü

## Bitti Tanımı
- Common projeleri derleniyor
- Common.Domain hiçbir projeye bağımlı değil
