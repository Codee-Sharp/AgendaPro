# 🏗️ **AgendaPro – Backend (Clean Architecture + .NET)**

O **AgendaPro** é um projeto backend em **ASP.NET Core** estruturado sob os princípios de **Clean Architecture**, aplicando **CQRS** e **DDD Light** para alcançar uma arquitetura **modular, testável e escalável**.  
A proposta é servir de base para aplicações que exigem separação clara entre camadas, regras de domínio puras e independência de infraestrutura.

---

## 📁 **Estrutura de Pastas**

```plaintext
AgendaPro/
│
├── src/
│   ├── AgendaPro.Api/           → Interface de entrada (REST API)
│   ├── AgendaPro.Application/   → Casos de uso, DTOs e serviços de aplicação
│   ├── AgendaPro.Domain/        → Entidades, regras de negócio e contratos
│   ├── AgendaPro.Infra/         → Implementações concretas (DB, repositórios, serviços externos)
│
└── tests/
    └── AgendaPro.Tests/         → Testes unitários e de integração
```

Cada camada tem uma **responsabilidade clara** e **depende apenas das camadas internas**, seguindo o fluxo da **Clean Architecture**.

---

## 🧩 **Camadas e Responsabilidades**

### **1️⃣ Domain (Núcleo de Negócio)**

> Contém a **lógica de negócio pura**, totalmente **independente de frameworks**, bancos de dados ou bibliotecas externas.

#### **Elementos Principais**
- **Entidades (Entities):**
  - `Professional`, `Client`, `ServiceItem`, `Appointment`
- **Objetos de Valor (Value Objects):**
  - Ex: `Email`, `PhoneNumber`
- **Interfaces de Repositório (Contratos):**
  - `IRepository<T>`
  - `IUnitOfWork`
- **Serviços de Domínio:**
  - `AppointmentRulesService` (para encapsular regras complexas)

#### **Objetivo**
Manter **regras e invariantes de negócio** isoladas e reutilizáveis, sem dependência de infraestrutura.

---

### **2️⃣ Application (Casos de Uso / Orquestração)**

> Camada responsável por **coordenar o fluxo** entre o domínio e as demais camadas (API e Infra).  
> Implementa os **Casos de Uso (Use Cases)** da aplicação e contém as regras de **aplicação**, não de **negócio**.

#### **Elementos Principais**
- **DTOs / Models (Entrada e Saída):**
  - `CreateAppointmentRequest`, `AppointmentResponse`
- **Interfaces de Serviços:**
  - `IAppointmentService`
- **Serviços de Aplicação (Use Cases):**
  - `AppointmentService` (coordena o uso de repositórios e `UnitOfWork`)
- **Mapeamentos:**
  - Conversão entre entidades de domínio e DTOs (via `AutoMapper` ou `Mapster`)

#### **Responsabilidade**
Aplicar **regras de orquestração**, **validações** e **coordenação de transações**, sem implementar lógica de negócio direta — que pertence ao **Domínio**.

#### **Diferença entre Services e Use Cases Services**
- Os **services genéricos** (ex: `NotificationService`, `EmailService`, `StorageService`) são **utilitários** reutilizáveis em diversos contextos da aplicação.  
  Eles representam **operações transversais**, sem dependência de um caso de uso específico.

- Já os **Use Cases Services** são **específicos de um fluxo de negócio**, como `CreateAppointmentHandler` ou `UpdateClientProfileHandler`.  
  Neles, ficam concentradas as **regras de aplicação** (ordem das operações, interações entre entidades, uso de repositórios, validações, e commit de transações).

> Em resumo:  
> - **Services genéricos** = componentes reutilizáveis e independentes.  
> - **Use Cases Services** = implementações que expressam *o que o sistema deve fazer* em cada cenário.


---

### **3️⃣ Infra (Infraestrutura e Persistência)**

> Contém tudo que depende de tecnologia externa (bancos de dados, logs, APIs, etc).  
> É a camada **mais volátil** — deve ser isolada do núcleo.

#### **Elementos Principais**
- **DbContext (EF Core ou ORM):**
  - `AppDbContext`
- **Repositórios:**
  - `EfRepository<T>` (implementação de `IRepository<T>`)
- **Unit of Work:**
  - `UnitOfWork`
- **Migrations / Configuração de Banco**
- **Serviços Externos:**
  - Envio de e-mails, notificações, integrações HTTP, etc.

#### **Objetivo**
Garantir persistência e comunicação externa, mantendo o domínio protegido de detalhes técnicos.

---

### **4️⃣ Api (Interface / Apresentação)**

> É a **porta de entrada** do sistema. Expõe os endpoints e controla a comunicação com o cliente (ex: Front-End, Mobile).

#### **Elementos Principais**
- **Controllers:**
  - `AppointmentsController`, `ClientsController`, etc.
- **Middleware:**
  - Tratamento de exceções, autenticação, logs.
- **Injeção de Dependência (DI):**
  - Registro dos serviços, repositórios e configurações.
- **Documentação:**
  - Swagger / OpenAPI.

#### **Responsabilidade**
Traduzir requisições HTTP em comandos/queries da camada de aplicação e retornar respostas formatadas.

---

### **5️⃣ Tests (Testes Automatizados)**

> Assegura a **qualidade e estabilidade** do sistema através de testes automatizados.

#### **Tipos de Testes**
- **Unitários:**
  - Validam entidades e regras de domínio.
- **Integração:**
  - Testam repositórios e casos de uso com banco em memória ou real.
- **End-to-end (opcional):**
  - Testes de API com `WebApplicationFactory`.

#### **Frameworks sugeridos**
- `xUnit` ou `NUnit`
- `FluentAssertions`
- `Moq` ou `NSubstitute` (mocks/stubs)

---

## 🔗 **Fluxo de Dependência (Clean Architecture)**

```plaintext
   +--------------------+
   |    Presentation    |  (API)
   +--------------------+
             ↓
   +--------------------+
   |    Application     |  (Use Cases)
   +--------------------+
             ↓
   +--------------------+
   |      Domain        |  (Entidades, Regras)
   +--------------------+
             ↓
   +--------------------+
   |      Infra         |  (DB, Serviços)
   +--------------------+
```

> Cada camada **só depende da camada imediatamente inferior**, nunca o inverso.  
> Isso garante **independência e testabilidade**.

---

## 📦 **Principais Interfaces (Skeleton)**

### **Domain**

```csharp
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task AddAsync(T entity);
    Task<IEnumerable<T>> GetAllAsync();
    void Update(T entity);
    void Remove(T entity);
}

public interface IUnitOfWork
{
    Task<int> CommitAsync();
}
```

---

### **Application**

```csharp
public interface IAppointmentService
{
    Task<AppointmentResponse> CreateAsync(CreateAppointmentRequest request);
    Task<IEnumerable<AppointmentResponse>> GetAllAsync();
}

public record CreateAppointmentRequest(Guid ProfessionalId, Guid ClientId, DateTime Date, string ServiceName);
public record AppointmentResponse(Guid Id, string Professional, string Client, DateTime Date, string Status);
```

---

### **Infra**

```csharp
public class EfRepository<T> : IRepository<T> where T : class
{
    private readonly AppDbContext _context;
    public EfRepository(AppDbContext context) => _context = context;

    public async Task<T?> GetByIdAsync(Guid id) => await _context.Set<T>().FindAsync(id);
    public async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);
    public async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();
    public void Update(T entity) => _context.Set<T>().Update(entity);
    public void Remove(T entity) => _context.Set<T>().Remove(entity);
}
```

---

## 🧪 **Padrões e Boas Práticas**

- ✅ **Separação de responsabilidades clara**  
- ✅ **Interfaces e Injeção de Dependência (DI)**  
- ✅ **Validação de entrada com FluentValidation**  
- ✅ **Logs estruturados (Serilog)**  
- ✅ **Testes unitários e integração**  

---

## 🚀 **Fluxo de Desenvolvimento (Git Flow)**

```plaintext
main    → código estável (produção)
develop → ambiente de integração
│
├── feature/* → novas funcionalidades
└── hotfix/*  → correções urgentes
```

**Commits Semânticos**

```plaintext
feat:     nova funcionalidade
fix:      correção de bug
docs:     atualização de documentação
refactor: refatoração sem mudar comportamento
test:     criação ou atualização de testes
chore:    tarefas de manutenção
```

**Exemplos:**
```bash
git commit -m "feat(appointment): implement scheduling validation"
git commit -m "fix(client): correct phone number format validation"
git commit -m "refactor(repository): simplify async pattern"
```

---

## 🧱 **Tecnologias Recomendadas**

| Camada | Tecnologias / Libs |
|--------|---------------------|
| API | ASP.NET Core, Swagger, Serilog |
| Application | MediatR, FluentValidation |
| Domain | DDD patterns, Value Objects |
| Infra | EF Core, PostgreSQL, MongoDB, Dapper |
| Tests | xUnit, Moq, FluentAssertions |

---

## 📈 **Roadmap**

- [ ] Adicionar autenticação JWT  
- [ ] Implementar CQRS completo (Commands/Queries)  
- [ ] Cache e Projeções de Leitura  
- [ ] Observabilidade (Health Checks, Metrics, OpenTelemetry)  
- [ ] CI/CD com GitHub Actions  
- [ ] Deploy automatizado (Docker + Cloud Run)

---

## 📜 **Licença**

Distribuído sob a licença **MIT**. Consulte o arquivo `LICENSE` para mais detalhes.
