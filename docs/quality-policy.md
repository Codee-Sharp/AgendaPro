# Política de Qualidade de Código – AgendaPro

Este documento define as regras de qualidade obrigatórias para o projeto AgendaPro.
Aplica-se a todo código que entra na base via Pull Request, independente de ter sido escrito por humano ou gerado por IA.

---

## 1. Princípios Gerais

- **Qualidade não é opcional.** A pipeline bloqueia PRs que não atendam os critérios mínimos.
- **IA é ferramenta, não autor.** Todo código gerado por IA deve passar por revisão humana e ter cobertura de testes.
- **Arquitetura é um contrato.** Violações de separação de camadas são bugs, não opções de design.
- **Testes não são negociáveis.** Remover ou ignorar testes para fazer a pipeline passar é causa de bloqueio imediato.

---

## 2. Thresholds de Cobertura de Código

| Escopo | Threshold Mínimo | Tipo |
|--------|------------------|------|
| Cobertura global | **80%** | Bloqueante (CI falha) |
| `AgendaPro.Domain` | **85%** | Bloqueante (CI falha) |
| `AgendaPro.Application` | **85%** | Bloqueante (CI falha) |
| Código novo no PR | **90%** | Bloqueante (via SonarCloud*) |

> \* O threshold de código novo requer SonarCloud ativo. Sem ele, o revisor deve verificar manualmente no relatório HTML.

### 2.1 Exclusões de Cobertura Permitidas

As exclusões são justificáveis apenas para:

| Tipo de arquivo | Exclusão | Justificativa |
|----------------|----------|---------------|
| `Migrations/` | ✅ Excluído | Código gerado pelo EF Core, sem lógica de negócio |
| `Program.cs` | ✅ Excluído | Bootstrap de aplicação |
| `*DbContext*` | ✅ Excluído | Configuração de infraestrutura |
| `InfrastructureExtensions.cs` | ✅ Excluído | Registro de DI sem lógica testável |
| DTOs sem comportamento | ✅ Excluído | Use `[ExcludeFromCodeCoverage]` |
| Arquivos `*.Designer.cs` | ✅ Excluído | Gerados por tooling |

**Nunca excluir:**
- Use Cases
- Entidades de domínio
- Repositórios com lógica de query
- Controllers (o fluxo de resposta deve ser testado)
- Qualquer arquivo que contenha regra de negócio

---

## 3. Regras de Qualidade do Build

| Regra | Configuração |
|-------|-------------|
| Warnings como erros | `-warnaserror` em Release |
| Analyzers Roslyn | `latest-recommended` via `Directory.Build.props` |
| SonarAnalyzer.CSharp | Ativo em todos os projetos via `Directory.Build.props` |
| Nullable reference types | Habilitado em todos os projetos |
| Formatação | `dotnet format --verify-no-changes` obrigatório |

---

## 4. Regras Arquiteturais

### 4.1 Dependências permitidas

```
Api          → Application, Infrastructure
Application  → Domain
Infrastructure → Domain, Application (para interfaces)
Domain       → (nenhuma dependência do projeto)
```

### 4.2 Violações que bloqueiam o PR

- `AgendaPro.Domain.csproj` referenciando `Application`, `Infrastructure` ou `Api`
- `AgendaPro.Application.csproj` referenciando `Api`
- `AgendaPro.Application.csproj` referenciando `Infrastructure` diretamente
- Lógica de banco de dados em `Application` ou `Domain`
- Regras de negócio implementadas em Controllers
- Entidades de domínio retornadas diretamente por Controllers

### 4.3 Validação automatizada

O script `scripts/validate-architecture.sh` é executado como job separado na pipeline de PR.
Se falhar, o PR não pode ser mergeado.

---

## 5. Política de Código Gerado por IA

### 5.1 O que é permitido

- IA pode sugerir código, testes, refatorações e documentação
- IA pode ser usada para boilerplate e scaffolding de Use Cases e Controllers
- IA pode sugerir mensagens de erro e nomes de variáveis

### 5.2 O que é proibido

| Proibição | Razão |
|-----------|-------|
| Aceitar código de IA sem revisão linha a linha | IA erra, especialmente em contexto específico do domínio |
| IA alterar arquitetura sem justificativa documentada | Mudanças de arquitetura requerem decisão consciente |
| IA introduzir pacotes NuGet novos sem aprovação | Aumenta superfície de ataque e débito técnico |
| IA criar abstrações desnecessárias para uso único | Viola YAGNI e dificulta manutenção |
| IA remover testes para fazer a pipeline passar | Jamais. Causa bloqueio imediato do PR |
| IA reduzir cobertura sem justificativa explícita no PR | Cobertura baixa = risco oculto |
| IA substituir Result Pattern por exceções | Viola contrato arquitetural do projeto |
| IA usar `throw` para fluxos de negócio esperados | Erros esperados devem usar `Result.Failure(...)` |

### 5.3 Rastreabilidade

Todo PR que use IA deve declarar isso no template de PR:
- Indicar quais ferramentas foram usadas
- Confirmar que o código foi revisado manualmente
- Confirmar que há testes cobrindo o código gerado

---

## 6. Critérios de Aprovação de PR

Um PR só pode ser aprovado quando:

- [ ] Todos os checks da pipeline estão verdes
- [ ] Validação arquitetural passou
- [ ] Cobertura global ≥ 80%
- [ ] Cobertura de Domain e Application ≥ 85%
- [ ] Pelo menos **1 revisor humano** aprovou
- [ ] Template de PR preenchido completamente
- [ ] Código gerado por IA declarado e revisado (quando aplicável)
- [ ] Nenhum comentário de bloqueio pendente

---

## 7. Critérios de Bloqueio Automático (pipeline)

| Critério | Job responsável |
|----------|----------------|
| Build com erro | `build-and-format` |
| Código fora de formatação | `build-and-format` |
| Violação de dependência entre camadas | `architecture-check` |
| Algum teste falhou | `test-and-coverage` |
| Cobertura global < 80% | `test-and-coverage` |
| Cobertura Domain/Application < 85% | `test-and-coverage` |

---

## 8. Exceções e Justificativas

Exceções às regras de cobertura devem ser documentadas no PR com:

1. Qual threshold não foi atingido
2. Por quê o código não pode ser testado (ex: integração real com serviço externo)
3. Plano para cobertura futura
4. Aprovação explícita de pelo menos 2 revisores

**Atenção:** Exceções são temporárias. Se a exceção existir por mais de 2 sprints, ela se torna um bug de qualidade.

---

## 9. Evolução desta Política

Esta política é revisada a cada 3 meses ou quando:

- Os thresholds se tornarem insustentáveis ou triviais demais
- Uma nova camada for adicionada à arquitetura
- Uma nova ferramenta de IA for adotada pela equipe
- Um incidente de produção for rastreado até uma falha de qualidade de código

Qualquer alteração nesta política deve ser feita via PR com revisão de toda a equipe.
