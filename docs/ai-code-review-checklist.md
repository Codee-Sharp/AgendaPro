# Checklist de Revisão de Código por IA – AgendaPro

Este checklist deve ser usado durante revisões de Pull Requests que contenham código gerado ou assistido por Inteligência Artificial (GitHub Copilot, ChatGPT, etc.).

---

## Como usar

Antes de aprovar um PR com código gerado por IA:

1. Execute o checklist abaixo mentalmente ou anote nos comentários do PR.
2. Itens marcados com **[BLOQUEANTE]** impedem aprovação se não cumpridos.
3. Itens marcados com **[RECOMENDADO]** são boas práticas fortemente incentivadas.

---

## 1. Separação de Camadas

| # | Verificação | Prioridade |
|---|-------------|------------|
| 1.1 | Domain contém apenas entidades, interfaces, enums e regras puras? | **BLOQUEANTE** |
| 1.2 | Domain NÃO referencia Application, Infrastructure ou Api? | **BLOQUEANTE** |
| 1.3 | Application contém apenas DTOs, Use Cases e contratos? | **BLOQUEANTE** |
| 1.4 | Application NÃO referencia diretamente Infrastructure? | **BLOQUEANTE** |
| 1.5 | Application NÃO referencia Api? | **BLOQUEANTE** |
| 1.6 | Infrastructure é a única camada que usa EF Core / acesso a banco? | **BLOQUEANTE** |
| 1.7 | Controllers são finos: recebem request → chamam Use Case → retornam resposta? | **BLOQUEANTE** |
| 1.8 | Nenhuma regra de negócio está implementada diretamente em um Controller? | **BLOQUEANTE** |

---

## 2. Result Pattern

| # | Verificação | Prioridade |
|---|-------------|------------|
| 2.1 | Erros de validação e de negócio retornam `Result` / `Result<T>`? | **BLOQUEANTE** |
| 2.2 | Nenhum `throw new ArgumentException` ou similar foi introduzido para fluxos esperados? | **BLOQUEANTE** |
| 2.3 | Use Cases que podem falhar retornam `Result`, não `void`? | **BLOQUEANTE** |
| 2.4 | Controllers usam `result.ToActionResult()` via `ResultExtensions`? | **BLOQUEANTE** |
| 2.5 | Respostas de API usam `ApiResponse<T>`? | **BLOQUEANTE** |

---

## 3. Repositories e UnitOfWork

| # | Verificação | Prioridade |
|---|-------------|------------|
| 3.1 | Operações de escrita chamam `IUnitOfWork.CommitAsync()`? | **BLOQUEANTE** |
| 3.2 | Repositórios contêm apenas CRUD e queries (sem lógica de negócio)? | RECOMENDADO |
| 3.3 | Use Cases recebem interfaces de repositório por injeção, não implementações concretas? | **BLOQUEANTE** |

---

## 4. DTOs e Mapeamento

| # | Verificação | Prioridade |
|---|-------------|------------|
| 4.1 | Entidades de domínio NÃO são retornadas diretamente de Controllers ou Use Cases? | **BLOQUEANTE** |
| 4.2 | DTOs NÃO possuem métodos de negócio ou lógica de domínio? | **BLOQUEANTE** |
| 4.3 | Mapeamentos entre entidade e DTO são explícitos e rastreáveis? | RECOMENDADO |
| 4.4 | DTOs simples sem lógica estão excluídos de métricas de cobertura? | RECOMENDADO |

---

## 5. Validação de Entrada

| # | Verificação | Prioridade |
|---|-------------|------------|
| 5.1 | Todos os campos obrigatórios de DTOs de request têm validação mínima? | **BLOQUEANTE** |
| 5.2 | Strings vazias e nulos são tratados antes de chegar ao Use Case? | **BLOQUEANTE** |
| 5.3 | Limites de tamanho e formato são validados nos DTOs ou no Use Case? | RECOMENDADO |

---

## 6. Testes

| # | Verificação | Prioridade |
|---|-------------|------------|
| 6.1 | Novos endpoints têm teste cobrindo o caminho feliz? | **BLOQUEANTE** |
| 6.2 | Novos endpoints têm teste cobrindo erro de validação? | **BLOQUEANTE** |
| 6.3 | Novos endpoints têm teste cobrindo recurso não encontrado? | **BLOQUEANTE** |
| 6.4 | Alterações em regras de negócio têm testes unitários atualizados? | **BLOQUEANTE** |
| 6.5 | Os testes são independentes entre si (sem estado compartilhado)? | **BLOQUEANTE** |
| 6.6 | Mocks usam `Moq` para interfaces — entidades e DTOs não são mockados? | RECOMENDADO |
| 6.7 | Asserções são explícitas (`Assert.Equal(expected, actual)`)? | RECOMENDADO |
| 6.8 | Há testes para cenários de borda (valores null, vazio, máximo, etc.)? | RECOMENDADO |

---

## 7. Nomenclatura e Legibilidade

| # | Verificação | Prioridade |
|---|-------------|------------|
| 7.1 | Nomes de classes, métodos e variáveis são descritivos e sem abreviações obscuras? | RECOMENDADO |
| 7.2 | Interfaces começam com `I` (ex: `ITagRepository`)? | RECOMENDADO |
| 7.3 | Use Cases têm sufixo `UseCase` (ex: `TagUseCase`)? | RECOMENDADO |
| 7.4 | DTOs de request têm sufixo `Request`, de response têm sufixo `Response` ou `Dto`? | RECOMENDADO |
| 7.5 | Não há nomes genéricos como `Manager`, `Helper`, `Util` sem contexto claro? | RECOMENDADO |

---

## 8. Qualidade Geral

| # | Verificação | Prioridade |
|---|-------------|------------|
| 8.1 | Não há duplicação óbvia de lógica entre Use Cases? | RECOMENDADO |
| 8.2 | Nenhuma abstração foi criada apenas para uso único? | RECOMENDADO |
| 8.3 | Nenhum pacote NuGet novo foi introduzido sem justificativa no PR? | **BLOQUEANTE** |
| 8.4 | A arquitetura não foi alterada sem justificativa documentada no PR? | **BLOQUEANTE** |

---

## 9. Política de IA

| # | Verificação | Prioridade |
|---|-------------|------------|
| 9.1 | O PR indica se houve uso de IA? | **BLOQUEANTE** |
| 9.2 | Código gerado por IA foi revisado manualmente linha a linha? | **BLOQUEANTE** |
| 9.3 | Nenhum teste foi removido ou comentado para fazer a pipeline passar? | **BLOQUEANTE** |
| 9.4 | A cobertura não foi reduzida sem justificativa explícita? | **BLOQUEANTE** |
| 9.5 | A IA não introduziu abstração desnecessária (interface para um único uso)? | RECOMENDADO |
| 9.6 | A IA não substituiu o Result Pattern por exceções? | **BLOQUEANTE** |

---

## Resultado da revisão

- **Todos os [BLOQUEANTE] OK** → PR pode ser aprovado com comentários menores
- **Algum [BLOQUEANTE] reprovado** → solicite mudança antes de aprovar
- **Itens [RECOMENDADO] pendentes** → deixe comentário para acompanhamento futuro, não bloqueie se o PR for pequeno
