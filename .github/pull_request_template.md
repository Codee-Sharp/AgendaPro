<!--
  Template obrigatório para Pull Requests no AgendaPro.
  Preencha todas as seções antes de solicitar revisão.
  Seções marcadas com (*) são obrigatórias.
-->

## Descrição da mudança (*)

> Descreva claramente **o que** foi feito e **por quê** essa mudança é necessária.
> Evite descrever apenas o arquivo alterado; descreva o comportamento.

---

## Tipo de mudança (*)

<!-- Marque com [x] todos que se aplicam -->

- [ ] Bugfix (correção de comportamento incorreto)
- [ ] Nova funcionalidade (feature)
- [ ] Refatoração (sem mudança de comportamento externo)
- [ ] Melhorias de performance
- [ ] Atualização de dependências
- [ ] Mudança de infraestrutura / DevOps
- [ ] Documentação
- [ ] Testes

---

## Motivação e contexto (*)

> Por que essa mudança é necessária? Há alguma issue, ticket ou decisão técnica associada?
> Referência: `Closes #<número>` ou `Ref: <link>`

---

## Impacto arquitetural

<!-- Responda: a mudança altera a fronteira entre camadas? Adiciona nova dependência entre projetos? -->

- [ ] Impacta apenas uma camada (sem vazamento entre camadas)
- [ ] Impacta múltiplas camadas (justificativa abaixo)
- [ ] Adiciona nova dependência de pacote NuGet (listada abaixo e aprovada)
- [ ] Altera ou cria nova abstração / interface
- [ ] Altera o modelo de dados (migration incluída)

**Detalhes de impacto arquitetural:**
> _(deixe vazio se não aplicável)_

---

## Evidências de teste (*)

> Descreva como a mudança foi testada. Inclua classes de teste criadas/modificadas.

- [ ] Testes unitários criados ou atualizados
- [ ] Testes de integração criados ou atualizados
- [ ] Testado manualmente (descreva o procedimento abaixo)
- [ ] Não há testes (justifique obrigatoriamente)

**Testes adicionados:**
```
tests/AgendaPro.UnitTests/...
tests/AgendaPro.Tests/...
```

**Procedimento de teste manual (se aplicável):**
> _(ex: curl, Swagger, http file)_

---

## Cobertura de código

| Métrica                       | Antes  | Depois |
|-------------------------------|--------|--------|
| Cobertura global              |   ?%   |   ?%   |
| AgendaPro.Domain              |   ?%   |   ?%   |
| AgendaPro.Application         |   ?%   |   ?%   |

> Execute localmente:
> ```bash
> dotnet test AgendaPro.sln --collect:"XPlat Code Coverage" --settings coverlet.runsettings
> reportgenerator -reports:"TestResults/**/coverage.cobertura.xml" -targetdir:CoverageReport -reporttypes:HtmlInline
> ```

---

## Request / Response de exemplo (para endpoints novos ou modificados)

> Inclua exemplos de payload ou adicione ao arquivo `.http` do projeto.

<details>
<summary>Request</summary>

```http
POST /api/exemplo
Content-Type: application/json

{
}
```
</details>

<details>
<summary>Response</summary>

```json
{
  "data": {},
  "success": true,
  "errors": []
}
```
</details>

---

## Riscos e pontos de atenção

<!-- Ex: mudança de contrato de API, alteração de comportamento de cache, remoção de campo -->

- [ ] Breaking change (requer versionamento / comunicação)
- [ ] Risco de performance (justifique)
- [ ] Risco de segurança (ex: input não validado, exposição de dados)
- [ ] Nenhum risco identificado

**Descrição dos riscos:**
> _(deixe vazio se não aplicável)_

---

## Uso de IA (*)

- [ ] Parte deste código foi gerado ou sugerido por IA (GitHub Copilot, ChatGPT, etc.)
- [ ] **Confirmação:** todo o código gerado por IA foi revisado manualmente linha a linha
- [ ] **Confirmação:** código gerado por IA possui cobertura de testes adequada
- [ ] Não houve uso de IA neste PR

**Ferramentas de IA utilizadas (se aplicável):**
> _(ex: GitHub Copilot inline, Copilot Chat, ChatGPT)_

---

## Checklist final do autor (*)

- [ ] O código foi testado localmente e os testes passam
- [ ] O código segue a separação de responsabilidades (Domain / Application / Infrastructure / Api)
- [ ] Regras de domínio não foram colocadas em Controllers
- [ ] Acesso ao banco de dados está restrito à camada Infrastructure
- [ ] Repositories e UnitOfWork estão sendo usados corretamente
- [ ] UseCases não estão com responsabilidade excessiva
- [ ] DTOs não estão sendo usados como entidades de domínio
- [ ] Exceptions não substituem indevidamente o Result Pattern
- [ ] Há validação adequada de entrada (nulos, limites, formatos)
- [ ] Nomes de classes, métodos e variáveis são claros e em português (termos de domínio) ou inglês (termos técnicos)
- [ ] Não há duplicação de lógica evidente
- [ ] `dotnet format` foi executado (`dotnet format AgendaPro.sln`)
- [ ] `dotnet build -warnaserror` passa sem erros
