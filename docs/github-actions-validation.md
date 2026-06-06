# Como Validar a Esteira no GitHub Actions – AgendaPro

Este guia explica como confirmar que o workflow `pr-quality.yml` está rodando corretamente após o setup.

---

## 1. Commitar os arquivos de configuração

Certifique-se de que todos os arquivos abaixo estão no repositório antes de abrir o PR:

```bash
git add .github/workflows/pr-quality.yml
git add .github/pull_request_template.md
git add .editorconfig
git add Directory.Build.props
git add coverlet.runsettings
git add scripts/validate-architecture.sh
git add docs/
git commit -m "ci: add PR quality gate workflow and configuration files"
git push origin <sua-branch>
```

---

## 2. Abrir um Pull Request

1. Acesse o repositório no GitHub
2. Clique em **"Compare & pull request"** após o push (ou vá em **Pull requests → New pull request**)
3. Selecione a branch de destino: `main`, `master` ou `develop`
4. Preencha o template de PR que aparecerá automaticamente
5. Clique em **"Create pull request"**

> O workflow é disparado automaticamente ao abrir ou atualizar um PR com destino a `main`, `master` ou `develop`.

---

## 3. Onde ver a execução no GitHub

### 3.1 Aba "Checks" do PR

1. No PR aberto, clique na aba **"Checks"** (ao lado de "Conversation" e "Files changed")
2. Você verá os jobs:
   - `Architecture Check`
   - `Build & Format Check`
   - `Tests & Coverage Gate`
3. Cada job mostra status: ⏳ Em execução | ✅ Passou | ❌ Falhou

### 3.2 Aba "Actions" do repositório

1. No menu superior do repositório, clique em **"Actions"**
2. No painel esquerdo, selecione **"PR · Quality Gate"**
3. Você verá o histórico de todas as execuções da pipeline

---

## 4. Como identificar se a esteira rodou

Na aba "Checks" do PR, você deve ver o ícone ao lado do último commit:

- ⏳ Amarelo pulsante → rodando
- ✅ Verde → todos os checks passaram
- ❌ Vermelho → ao menos um check falhou
- ⚪ Cinza → ainda não disparou (aguarde alguns segundos após o push)

Se o ícone não aparecer, verifique:
- Se o arquivo `pr-quality.yml` está em `.github/workflows/`
- Se o PR tem a branch de destino correta (`main`, `master` ou `develop`)
- Se o arquivo YAML não tem erros de sintaxe

---

## 5. Como ver os logs de erro

1. Clique em **"Details"** ao lado do check com falha
2. Na página de execução do workflow, clique no job com ❌
3. Expanda o passo que falhou (clique no nome do step)
4. Leia a saída detalhada — erros do .NET são exibidos com `::error::` e ficam em vermelho

### Dica: filtrar erros rapidamente

Na tela de logs do job, clique no ícone de **"!"** (Filter by warning/error) para ver apenas erros.

---

## 6. Como baixar o artifact de cobertura

1. No PR ou na execução do workflow, role até o final da página
2. Procure a seção **"Artifacts"**
3. Clique em **"coverage-report-pr-{número}"** para baixar o relatório HTML
4. Extraia o arquivo `.zip` e abra `index.html` no navegador

O relatório mostra:
- Cobertura por assembly
- Cobertura por classe (vermelho = abaixo do threshold)
- Linhas cobertas e não cobertas por arquivo

---

## 7. Como saber se a cobertura falhou

Quando a cobertura está abaixo do threshold, o step **"Validate coverage thresholds"** falha com mensagem:

```
::error::Global coverage XX.X% < 80% (obrigatório)
::error::AgendaPro.Domain coverage XX.X% < 85% (obrigatório)
```

Além disso:
- Se `marocchino/sticky-pull-request-comment` estiver configurado, um comentário automático aparecerá no PR com o resumo de cobertura
- O check "Tests & Coverage Gate" ficará com ❌

---

## 8. Como corrigir erros comuns

### ❌ "Architecture violation"

```
FAIL  src/AgendaPro.Application/AgendaPro.Application.csproj → referencia 'AgendaPro.Infrastructure'
```

**Solução:** Remova a `<ProjectReference>` que viola a arquitetura do `.csproj` indicado.

---

### ❌ "Formatting is not correct"

```
error: Formatting is not correct in file 'src/AgendaPro.Api/Controllers/TagController.cs'.
```

**Solução:**
```bash
dotnet format AgendaPro.sln --no-restore
git add -u
git commit -m "style: apply dotnet format corrections"
git push
```

---

### ❌ "Build failed" com warnings-as-errors

```
error CA1062: In externally visible method '...', validate parameter '...' is non-null before using it.
```

**Solução:** Corrija o warning indicado ou ajuste a severidade no `.editorconfig` se o warning for intencional:

```ini
dotnet_diagnostic.CA1062.severity = suggestion
```

---

### ❌ Testes falharam

```
Failed   AgendaPro.UnitTests.Tags.TagUseCaseTests.CreateTag_WithEmptyName_ShouldReturnFailure
```

**Solução:** Execute localmente:
```bash
dotnet test tests/AgendaPro.UnitTests/AgendaPro.UnitTests.csproj --verbosity detailed
```

Leia a stack trace e corrija o teste ou o código.

---

### ❌ Cobertura abaixo do mínimo

**Solução:**
1. Baixe o artifact de cobertura
2. Abra `index.html` e identifique classes com cobertura baixa
3. Escreva testes para os caminhos não cobertos
4. Execute localmente para confirmar (veja [run-tests-and-coverage.md](run-tests-and-coverage.md))

---

## 9. Como testar se a esteira está bloqueando PRs corretamente

### 9.1 Testar bloqueio por violação arquitetural

1. Abra `src/AgendaPro.Application/AgendaPro.Application.csproj`
2. Adicione temporariamente: `<ProjectReference Include="..\AgendaPro.Api\AgendaPro.Api.csproj" />`
3. Faça commit e push
4. **Resultado esperado:** job `Architecture Check` deve falhar com erro visível

### 9.2 Testar bloqueio por formatação

1. Adicione espaços extras ou quebras de linha desnecessárias em qualquer `.cs`
2. Faça commit e push sem rodar `dotnet format`
3. **Resultado esperado:** step `Verify formatting` deve falhar

### 9.3 Testar bloqueio por cobertura insuficiente

1. Adicione um método público em um Use Case sem escrever testes para ele
2. Faça commit e push
3. **Resultado esperado:** step `Validate coverage thresholds` deve falhar se a cobertura cair abaixo do threshold

### 9.4 Testar bloqueio por teste falhando

1. Quebre um teste intencionalmente: mude um `Assert.Equal(expected, actual)` para um valor errado
2. Faça commit e push
3. **Resultado esperado:** step `Run AgendaPro.UnitTests` deve falhar com o nome do teste

---

## 10. Como validar localmente antes de abrir o PR

Execute a sequência completa de qualidade localmente:

```bash
# 1. Restaurar dependências
dotnet restore AgendaPro.sln

# 2. Verificar formatação
dotnet format AgendaPro.sln --verify-no-changes --no-restore

# 3. Build com warnings como erros
dotnet build AgendaPro.sln --configuration Release --no-restore -warnaserror

# 4. Validar arquitetura
bash scripts/validate-architecture.sh

# 5. Testes com cobertura
dotnet test AgendaPro.sln \
  --configuration Release \
  --no-build \
  --collect:"XPlat Code Coverage" \
  --results-directory ./TestResults \
  --settings coverlet.runsettings

# 6. Gerar relatório
reportgenerator \
  -reports:"./TestResults/**/coverage.cobertura.xml" \
  -targetdir:"./CoverageReport" \
  -reporttypes:"Html;Cobertura;TextSummary" \
  -assemblyfilters:"+AgendaPro.*;-AgendaPro.Tests*;-AgendaPro.UnitTests*"

# 7. Ver resumo
cat ./CoverageReport/Summary.txt
```

Se todos os passos acima passarem localmente, a pipeline no GitHub também passará (exceto por diferenças de ambiente de SO).

---

## 11. Configurar Branch Protection Rules (obrigatório para bloquear merge)

A pipeline por si só não bloqueia o merge — você precisa configurar Branch Protection:

1. Acesse: **Settings → Branches → Add rule** (ou edite a regra da `main`)
2. Branch name pattern: `main`
3. Marque: ✅ **Require status checks to pass before merging**
4. Marque: ✅ **Require branches to be up to date before merging**
5. Em "Status checks", adicione:
   - `Architecture Check`
   - `Build & Format Check`
   - `Tests & Coverage Gate`
6. Marque: ✅ **Require at least 1 approving review**
7. Marque: ✅ **Dismiss stale pull request approvals when new commits are pushed**
8. Marque: ✅ **Require conversation resolution before merging**
9. Clique em **Save changes**

> **Atenção:** Os nomes dos status checks devem corresponder exatamente ao campo `name:` dos jobs no YAML.

---

## Referência rápida

| Quero saber... | Onde olhar |
|----------------|------------|
| Se a pipeline rodou | Aba "Checks" do PR |
| Detalhes de um erro | "Details" → job falhado → step falhado |
| Cobertura por classe | Artifact `coverage-report-pr-N` → `index.html` |
| Resumo de cobertura | Comentário automático no PR (se configurado) |
| Logs completos | GitHub Actions → PR · Quality Gate → execução |
| Validar localmente | [run-tests-and-coverage.md](run-tests-and-coverage.md) |
| Regras de qualidade | [quality-policy.md](quality-policy.md) |
| Checklist de revisão IA | [ai-code-review-checklist.md](ai-code-review-checklist.md) |
