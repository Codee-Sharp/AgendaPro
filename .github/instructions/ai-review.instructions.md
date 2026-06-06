---
applyTo: "**/*.cs"
---

# Regras de revisão de código – AgendaPro

Use estas regras ao revisar **qualquer** código C# no repositório, seja ele escrito por humanos ou gerado por IA.

---

## 1. Separação de camadas (arquitetura)

### 1.1 Domain
- Deve conter **somente**: entidades, value objects, interfaces de repositório/serviço, enums de domínio, Result/Error e regras de negócio puras.
- **Nunca** referencie `Microsoft.EntityFrameworkCore`, `System.Net.Http`, nenhum pacote de infraestrutura ou de apresentação.
- Se uma classe Domain depende de algo externo ao `AgendaPro.Domain`, **bloqueie e explique**.

### 1.2 Application
- Deve conter **somente**: DTOs, Use Cases (orquestração de regras) e interfaces de serviços de aplicação.
- **Nunca** deve instanciar DbContext, Entity Framework, conexões ou qualquer provider de dados diretamente.
- Use Cases devem receber interfaces (ex: `ITagRepository`) por injeção — nunca implementações concretas.
- Se o Use Case tem mais de uma responsabilidade clara, sugira divisão ou extraia método privado.

### 1.3 Infrastructure
- Única camada que pode referenciar EF Core, migrations, providers de banco, e-mail, storage, etc.
- Repositories devem implementar as interfaces definidas em Domain.
- Migrations devem estar em `AgendaPro.Infrastucture/Data/Migrations/`.

### 1.4 Api
- Controllers devem ser **finos**: recebem request, chamam Use Case, retornam `result.ToActionResult()`.
- **Nunca** coloque regras de negócio em Controllers (validações de estado de domínio, cálculos, etc.).
- **Nunca** acesse repositórios diretamente de um Controller.
- Respostas HTTP devem usar `ApiResponse<T>` via `ResultExtensions.ToActionResult()`.

---

## 2. Result Pattern

- Erros de domínio e de validação de entrada **devem** retornar `Result` ou `Result<T>`, **nunca** lançar exceções para fluxos esperados.
- `throw` é permitido apenas para condições verdadeiramente excepcionais (bugs, violações de invariante, falhas de infra não recuperáveis).
- Se encontrar `throw new ArgumentException`, `throw new InvalidOperationException` etc. em fluxos normais de negócio, sinalize.
- Use Cases que retornam `void` em vez de `Result` em casos onde pode haver erro são suspeitos — questione.

---

## 3. Repositories e UnitOfWork

- Métodos de persistência devem ser chamados dentro de unidade de trabalho (`IUnitOfWork.CommitAsync()`).
- Repositórios não devem ter lógica de negócio — apenas operações CRUD e queries.
- Queries complexas podem ser extraídas em specification objects, mas devem ficar em Infrastructure ou Domain (sem EF em Domain).

---

## 4. DTOs

- DTOs não devem ter comportamento (métodos de negócio, validação complexa).
- DTOs de request podem ter anotações de validação de dados (`[Required]`, `[MaxLength]`), mas isso é opcional para DTOs simples.
- Entidades de domínio **nunca** devem ser retornadas diretamente de controllers ou use cases como DTO.
- Mapeamentos entre entidade e DTO devem ser explícitos (sem `AutoMapper` não aprovado).

---

## 5. Validação de entrada

- Toda entrada de API deve ter validação mínima: nulos, strings vazias, valores fora de range.
- Use o `ApiResponseValidationFilter` existente (model state) para validação automática de DTOs.
- Regras de negócio mais complexas pertencem ao Domain ou ao Use Case via Result.

---

## 6. Nomenclatura

- Nomes de classes, métodos e variáveis devem ser **descritivos e sem abreviações obscuras**.
- Interfaces começam com `I` (ex: `ITagRepository`).
- Use Cases: sufixo `UseCase` (ex: `TagUseCase`).
- Repositórios: sufixo `Repository` (ex: `TagRepository`).
- DTOs de request: sufixo `Request` (ex: `CreateTagRequest`).
- DTOs de response: sufixo `Response` ou `Dto` (ex: `TagResponse`).
- Evite nomes genéricos como `Manager`, `Helper`, `Util` sem contexto.

---

## 7. Duplicação de código

- Se a mesma lógica aparece em dois ou mais Use Cases, questione a extração para um serviço de domínio ou método auxiliar.
- Não extraia abstrações para uso único — YAGNI.

---

## 8. Testes

- Todo novo endpoint **deve** ter pelo menos um teste cobrindo:
  - Cenário de sucesso (happy path)
  - Cenário de erro de validação de entrada
  - Cenário de não encontrado / regra de negócio negada
- Toda alteração em regra de negócio existente **deve** ter o teste correspondente atualizado.
- Testes devem ser independentes (sem dependência de ordem de execução, sem estado compartilhado entre testes).
- Use `Moq` para mocks de interfaces — nunca moque entidades ou DTOs.
- Asserções devem ser explícitas: prefira `Assert.Equal(expected, actual)` a `Assert.True(result != null)`.

---

## 9. Política de código gerado por IA

- IA pode sugerir código, **não é fonte de verdade**.
- Código gerado por IA deve ser revisado linha a linha por um humano antes de ir para produção.
- IA **não deve**:
  - Alterar arquitetura sem justificativa documentada no PR
  - Introduzir pacotes NuGet novos sem aprovação explícita no PR
  - Criar abstrações para uso único (ex: interface genérica desnecessária)
  - Remover ou comentar testes para fazer a pipeline passar
  - Reduzir cobertura de testes sem justificativa no PR
  - Substituir o Result Pattern por exceções em fluxos normais
- Se o PR indica uso de IA e o código não tem testes, **bloqueie a aprovação**.

---

## 10. Perguntas para verificação rápida

Ao revisar um PR, responda mentalmente:

1. Existe lógica de negócio fora de Domain ou Application?
2. Existe acesso a banco de dados fora de Infrastructure?
3. Existe algum `throw` onde deveria haver `Result.Failure(...)`?
4. Controllers fazem mais do que chamar um Use Case?
5. DTOs são retornados como entidades ou vice-versa?
6. Existe código novo sem cobertura de teste?
7. Existe duplicação óbvia de lógica entre Use Cases?
8. Código gerado por IA tem evidência de revisão humana?

Se qualquer resposta for "sim" nos itens 1–7 ou "não" no item 8, **solicite mudança antes de aprovar**.
