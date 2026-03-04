# AgendaPro

## Visão Geral
O **AgendaPro** é um sistema de agendamento profissional, projetado para que profissionais de todas as áreas possam gerenciar seus compromissos de forma simples e organizada.

O projeto segue **arquitetura em camadas**, utilizando padrões como **Repository**, **Unit of Work** e **CQRS**, garantindo um código limpo, testável e escalável.

---

## Estrutura do Projeto

AgendaPro/
├─ src/
│ ├─ AgendaPro.Api # Camada de API
│ ├─ AgendaPro.Application # Casos de uso e serviços
│ ├─ AgendaPro.Domain # Entidades, interfaces e regras de negócio
│ └─ AgendaPro.Infrastucture # Persistência, repositórios, DbContext
├─ tests/
│ └─ AgendaPro.Tests # Testes unitários e de integração
├─ AgendaPro.sln
└─ README.md


---

## Unit of Work (UoW)

### O que é
O **Unit of Work** coordena operações de persistência entre múltiplos repositórios, garantindo que todas as alterações dentro de um fluxo sejam **atômicas**. Ou seja, **ou tudo é salvo ou nada é aplicado**.

### Por que usamos
- Evita inconsistências no banco de dados.
- Coordena múltiplos repositórios de forma integrada.
- Facilita manutenção, leitura e testes do código.

### Implementação

#### Contrato no Domain
```csharp
public interface IUnitOfWork
{
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}
```
### Implementação na Infraestrutura

```csharp
public sealed class UnitOfWork : IUnitOfWork
{
    private readonly AgendaProDbContext _context;

    public UnitOfWork(AgendaProDbContext context)
    {
        _context = context;
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}
```
### Exemplo de uso em um Command Handler

```csharp
public class CreateAppointmentHandler
{
    private readonly IRepository<Appointment> _repo;
    private readonly IUnitOfWork _uow;

    public CreateAppointmentHandler(IRepository<Appointment> repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Guid> Handle(CreateAppointmentCommand request, CancellationToken ct)
    {
        var appointment = Appointment.Create(request.ProfessionalId, request.ClientId, request.Date);
        await _repo.AddAsync(appointment, ct);

        await _uow.CommitAsync(ct); // garante atomicidade da operação
        return appointment.Id;
    }
}
```
## Repositórios
- Todos os repositórios trabalham com o DbContext e são compatíveis com o Unit of Work.

- Registro no DI:

```csharp
builder.Services.AddScoped<ITagRepository, TagRepository>();
``` 

## Injeção de Dependência (DI)
- Unit of Work registrado como Scoped:

```csharp
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
```
- Cada request HTTP terá sua própria instância, garantindo consistência das transações.

## Testes

- Unitários e de integração garantem:

- CommitAsync persiste alterações no banco.

- Repositórios funcionam corretamente com UoW.

- Rollback ocorre em caso de falha (quando usado pipeline transacional).

```csharp
[Fact]
public async Task CommitAsync_ShouldPersistChanges()
{
    var options = new DbContextOptionsBuilder<AgendaProDbContext>()
        .UseInMemoryDatabase(databaseName: "TestDb")
        .Options;

    await using var context = new AgendaProDbContext(options);
    var uow = new UnitOfWork(context);
    var repo = new TagRepository(context);

    await repo.AddAsync(new TagModel { Name = "Teste" });
    await uow.CommitAsync();

    var savedTag = await context.Tags.FirstOrDefaultAsync();
    Assert.NotNull(savedTag);
    Assert.Equal("Teste", savedTag.Name);
}
```