# Guia de Qualidade Local – AgendaPro

Este documento descreve como executar localmente todos os mesmos checks que a esteira de CI/CD executa em um Pull Request.

---

## Pré-requisitos

| Ferramenta        | Versão mínima | Instalação                                              |
|-------------------|---------------|---------------------------------------------------------|
| .NET SDK          | 10.0.100      | `global.json` garante a versão correta                  |
| ReportGenerator   | última        | `dotnet tool install -g dotnet-reportgenerator-globaltool` |

---

## 1. Restore e Build

```bash
# Restaurar pacotes da solução raiz
dotnet restore AgendaPro.sln

# Build com warnings como erros (mesmo que a CI)
dotnet build AgendaPro.sln --no-restore --configuration Release -warnaserror
```

---

## 2. Verificação de Formatação

```bash
# Verificar sem alterar (igual ao check da CI)
dotnet format AgendaPro.sln --verify-no-changes --no-restore

# Aplicar correções automaticamente (execute antes de abrir PR)
dotnet format AgendaPro.sln --no-restore
```

> **Atenção:** na primeira execução em uma branch nova, execute `dotnet format` com correção aplicada e inclua as mudanças de formatação no mesmo PR (ou em um PR de formatação dedicado).

---

## 3. Testes com Cobertura

```bash
# Testes unitários
dotnet test tests/AgendaPro.UnitTests/AgendaPro.UnitTests.csproj \
  --configuration Debug \
  --collect:"XPlat Code Coverage" \
  --results-directory TestResults/UnitTests \
  --settings coverlet.runsettings \
  --logger "console;verbosity=normal"

# Testes de integração
dotnet test tests/AgendaPro.Tests/AgendaPro.Tests.csproj \
  --configuration Debug \
  --collect:"XPlat Code Coverage" \
  --results-directory TestResults/IntegrationTests \
  --settings coverlet.runsettings \
  --logger "console;verbosity=normal"
```

### Executar todos os projetos de uma vez

```bash
dotnet test AgendaPro.sln \
  --configuration Debug \
  --collect:"XPlat Code Coverage" \
  --results-directory TestResults \
  --settings coverlet.runsettings
```

---

## 4. Relatório de Cobertura

```bash
# Gerar relatório HTML + Cobertura XML combinado
reportgenerator \
  -reports:"TestResults/**/coverage.cobertura.xml" \
  -targetdir:CoverageReport \
  -reporttypes:"HtmlInline;Cobertura;MarkdownSummaryGithub" \
  -assemblyfilters:"+AgendaPro.*;-AgendaPro.Tests*;-AgendaPro.UnitTests*" \
  -title:"AgendaPro Coverage"

# Abrir relatório no navegador (Windows)
start CoverageReport/index.html

# Abrir relatório no navegador (macOS/Linux)
open CoverageReport/index.html
```

---

## 5. Validar Thresholds Manualmente

```bash
python3 - <<'EOF'
import xml.etree.ElementTree as ET, sys

tree = ET.parse('CoverageReport/Cobertura.xml')
root = tree.getroot()

rate = float(root.attrib.get('line-rate', 0))
pct  = round(rate * 100, 2)
print(f'Global coverage: {pct}%')
if pct < 80:
    print(f'FAIL: Global {pct}% < 80%'); sys.exit(1)
else:
    print('OK: Global threshold passed')
EOF
```

---

## 6. Análise Estática (Roslyn + SonarAnalyzer)

Os analyzers já estão habilitados via `Directory.Build.props`:

```xml
<EnableNETAnalyzers>true</EnableNETAnalyzers>
<AnalysisLevel>latest-recommended</AnalysisLevel>
<!-- SonarAnalyzer.CSharp está referenciado em Directory.Build.props -->
```

O build com `-warnaserror` já valida os analyzers. Para uma visão detalhada:

```bash
dotnet build AgendaPro.sln --no-restore 2>&1 | grep -E "warning|error"
```

---

## 7. Script All-in-One

Salve como `scripts/check-quality.sh` e execute antes de abrir o PR:

```bash
#!/usr/bin/env bash
set -e

echo "=== Restore ==="
dotnet restore AgendaPro.sln

echo "=== Build (Release, -warnaserror) ==="
dotnet build AgendaPro.sln --no-restore --configuration Release -warnaserror

echo "=== Format check ==="
dotnet format AgendaPro.sln --verify-no-changes --no-restore

echo "=== Tests with coverage ==="
dotnet test AgendaPro.sln \
  --no-build \
  --configuration Debug \
  --collect:"XPlat Code Coverage" \
  --results-directory TestResults \
  --settings coverlet.runsettings

echo "=== Coverage report ==="
reportgenerator \
  -reports:"TestResults/**/coverage.cobertura.xml" \
  -targetdir:CoverageReport \
  -reporttypes:"Cobertura;HtmlInline;MarkdownSummaryGithub" \
  -assemblyfilters:"+AgendaPro.*;-AgendaPro.Tests*;-AgendaPro.UnitTests*"

echo "=== Threshold validation ==="
python3 - <<'PYEOF'
import xml.etree.ElementTree as ET, sys
tree = ET.parse('CoverageReport/Cobertura.xml')
root = tree.getroot()
rate = float(root.attrib.get('line-rate', 0))
pct  = round(rate * 100, 2)
print(f'Coverage: {pct}%')
if pct < 80:
    print(f'FAIL: {pct}% < 80%'); sys.exit(1)
print('All checks passed!')
PYEOF
```

No Windows (PowerShell):

```powershell
# Execute a mesma sequência via PowerShell
dotnet restore AgendaPro.sln
dotnet build AgendaPro.sln --no-restore --configuration Release -warnaserror
dotnet format AgendaPro.sln --verify-no-changes --no-restore
dotnet test AgendaPro.sln --no-build --configuration Debug `
  --collect:"XPlat Code Coverage" --results-directory TestResults --settings coverlet.runsettings
reportgenerator -reports:"TestResults/**/coverage.cobertura.xml" `
  -targetdir:CoverageReport -reporttypes:"Cobertura;HtmlInline" `
  -assemblyfilters:"+AgendaPro.*;-AgendaPro.Tests*;-AgendaPro.UnitTests*"
Start-Process CoverageReport/index.html
```

---

## 8. Critérios de Aprovação do PR

| Critério                                          | Threshold | Bloqueante |
|---------------------------------------------------|-----------|------------|
| Build sem erros (`-warnaserror`)                  | —         | ✅ Sim      |
| Formatação (`dotnet format --verify-no-changes`)  | —         | ✅ Sim      |
| Todos os testes passam                            | 100%      | ✅ Sim      |
| Cobertura global de linhas                        | ≥ 80%     | ✅ Sim      |
| Cobertura AgendaPro.Domain                        | ≥ 85%     | ✅ Sim      |
| Cobertura AgendaPro.Application                   | ≥ 85%     | ✅ Sim      |
| Cobertura de código novo (via SonarCloud)         | ≥ 90%     | ✅ Sim*     |
| PR template preenchido                            | —         | ✅ Sim      |
| Revisão humana de código gerado por IA            | —         | ✅ Sim      |
| Aprovação de pelo menos 1 revisor humano          | —         | ✅ Sim      |

> \* O threshold de 90% para código novo requer SonarCloud com `sonar.newCode.referenceBranch` configurado.
> Sem SonarCloud, o revisor deve verificar manualmente no relatório HTML quais linhas novas ficaram sem cobertura.

---

## 9. Recomendações para Evolução Futura

### 9.1 Testes de Arquitetura com NetArchTest

Adicione o pacote `NetArchTest.Rules` ao projeto `AgendaPro.UnitTests` e escreva regras como:

```csharp
// Domain não deve depender de nenhuma outra camada do projeto
Types.InAssembly(typeof(SomeEntity).Assembly)
     .Should().NotHaveDependencyOn("AgendaPro.Application")
     .And().NotHaveDependencyOn("AgendaPro.Infrastucture")
     .And().NotHaveDependencyOn("AgendaPro.Api")
     .GetResult().IsSuccessful.Should().BeTrue();
```

### 9.2 SonarCloud (threshold de código novo)

1. Crie conta em sonarcloud.io e vincule o repositório.
2. Adicione os secrets `SONAR_TOKEN`, `SONAR_PROJECT_KEY` e `SONAR_ORGANIZATION` no GitHub.
3. Descomente o job `sonarcloud` no workflow `.github/workflows/pr-quality.yml`.
4. Configure `sonar.newCode.referenceBranch=main` para exigir 90% de cobertura em código novo.

### 9.3 Branch Protection Rules

Configure no GitHub (`Settings → Branches → main`):

- **Require status checks before merging:**
  - `Build & Format Check`
  - `Tests & Coverage Gate`
- **Require branches to be up to date before merging**
- **Require at least 1 approving review**
- **Dismiss stale reviews when new commits are pushed**
- **Require conversation resolution before merging**

### 9.4 Mutation Testing

Considere adicionar `Stryker.NET` para validar a efetividade dos testes:

```bash
dotnet tool install --global dotnet-stryker
cd tests/AgendaPro.UnitTests
dotnet stryker --project AgendaPro.Domain --threshold-break 70
```

### 9.5 DAST / Security Scanning

Para APIs, considere adicionar OWASP ZAP ou `dotnet-outdated` para verificação de dependências vulneráveis:

```bash
dotnet tool install --global dotnet-outdated-tool
dotnet outdated AgendaPro.sln --fail-on-updates
```
