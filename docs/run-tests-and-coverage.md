# Como Rodar Testes e Cobertura Localmente – AgendaPro

Execute estes comandos antes de abrir um PR para garantir que a pipeline vai passar.

---

## Pré-requisitos

```bash
# Verificar versão do SDK (.NET 10 exigido pelo global.json)
dotnet --version

# Instalar ReportGenerator globalmente (se ainda não instalado)
dotnet tool install --global dotnet-reportgenerator-globaltool

# Verificar instalação
reportgenerator --version
```

---

## Passo a Passo Completo

### 1. Restore das dependências

```bash
dotnet restore AgendaPro.sln
```

### 2. Verificação de formatação

```bash
# Verificar sem alterar
dotnet format AgendaPro.sln --verify-no-changes --no-restore

# Corrigir automaticamente (execute se o passo acima falhar)
dotnet format AgendaPro.sln --no-restore
```

> Se o formato precisou de correções, inclua as mudanças no commit antes de abrir o PR.

### 3. Build

```bash
dotnet build AgendaPro.sln --configuration Release --no-restore
```

### 4. Testes com coleta de cobertura

```bash
# Todos os projetos de teste de uma vez
dotnet test AgendaPro.sln \
  --configuration Release \
  --no-build \
  --collect:"XPlat Code Coverage" \
  --results-directory ./TestResults \
  --settings coverlet.runsettings \
  --verbosity normal
```

Ou por projeto separadamente:

```bash
# Testes unitários
dotnet test tests/AgendaPro.UnitTests/AgendaPro.UnitTests.csproj \
  --configuration Release \
  --no-build \
  --collect:"XPlat Code Coverage" \
  --results-directory ./TestResults/UnitTests \
  --settings coverlet.runsettings

# Testes de integração
dotnet test tests/AgendaPro.Tests/AgendaPro.Tests.csproj \
  --configuration Release \
  --no-build \
  --collect:"XPlat Code Coverage" \
  --results-directory ./TestResults/IntegrationTests \
  --settings coverlet.runsettings
```

### 5. Gerar relatório de cobertura

```bash
reportgenerator \
  -reports:"./TestResults/**/coverage.cobertura.xml" \
  -targetdir:"./CoverageReport" \
  -reporttypes:"Html;Cobertura;TextSummary" \
  -assemblyfilters:"+AgendaPro.*;-AgendaPro.Tests*;-AgendaPro.UnitTests*" \
  -title:"AgendaPro Coverage Local"
```

### 6. Visualizar resumo no terminal

```bash
cat ./CoverageReport/Summary.txt
```

Saída esperada:

```
Summary
  Generated on: ...
  Parser: CoberturaParser
  Assemblies: 4
  Classes: XX
  Files: XX
  Line coverage: XX.X% (XXX of XXX)
  Branch coverage: XX.X% (XX of XX)
```

### 7. Abrir relatório HTML

```bash
# Windows
start ./CoverageReport/index.html

# macOS
open ./CoverageReport/index.html

# Linux
xdg-open ./CoverageReport/index.html
```

### 8. Validar thresholds

```bash
# Verificar se a cobertura global está acima de 80%
python3 - <<'EOF'
import xml.etree.ElementTree as ET, sys
tree = ET.parse('./CoverageReport/Cobertura.xml')
root = tree.getroot()
rate = float(root.attrib.get('line-rate', 0))
pct  = round(rate * 100, 2)
print(f'Cobertura global: {pct}%')
if pct < 80:
    print(f'FALHOU: {pct}% < 80% obrigatório'); sys.exit(1)
else:
    print('OK: threshold de cobertura atingido')
EOF
```

### 9. Validar arquitetura

```bash
bash scripts/validate-architecture.sh
```

---

## Script all-in-one (PowerShell)

Salve como `scripts/check-quality.ps1` e execute antes de abrir o PR:

```powershell
#!/usr/bin/env pwsh

Write-Host "=== Restore ===" -ForegroundColor Cyan
dotnet restore AgendaPro.sln

Write-Host "`n=== Format check ===" -ForegroundColor Cyan
dotnet format AgendaPro.sln --verify-no-changes --no-restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "Formatação incorreta. Execute: dotnet format AgendaPro.sln" -ForegroundColor Red
    exit 1
}

Write-Host "`n=== Build (Release, -warnaserror) ===" -ForegroundColor Cyan
dotnet build AgendaPro.sln --no-restore --configuration Release -warnaserror

Write-Host "`n=== Architecture check ===" -ForegroundColor Cyan
bash scripts/validate-architecture.sh

Write-Host "`n=== Tests with coverage ===" -ForegroundColor Cyan
dotnet test AgendaPro.sln `
  --no-build `
  --configuration Release `
  --collect:"XPlat Code Coverage" `
  --results-directory TestResults `
  --settings coverlet.runsettings

Write-Host "`n=== Coverage report ===" -ForegroundColor Cyan
reportgenerator `
  -reports:"TestResults/**/coverage.cobertura.xml" `
  -targetdir:CoverageReport `
  "-reporttypes:Html;Cobertura;TextSummary" `
  "-assemblyfilters:+AgendaPro.*;-AgendaPro.Tests*;-AgendaPro.UnitTests*"

Write-Host "`n=== Coverage summary ===" -ForegroundColor Cyan
Get-Content CoverageReport/Summary.txt

Write-Host "`nAbrir relatório HTML..." -ForegroundColor Green
Start-Process CoverageReport/index.html
```

---

## Script all-in-one (Bash / Linux / macOS / WSL)

```bash
#!/usr/bin/env bash
set -e

echo "=== Restore ==="
dotnet restore AgendaPro.sln

echo "=== Format check ==="
dotnet format AgendaPro.sln --verify-no-changes --no-restore

echo "=== Build ==="
dotnet build AgendaPro.sln --no-restore --configuration Release -warnaserror

echo "=== Architecture check ==="
bash scripts/validate-architecture.sh

echo "=== Tests with coverage ==="
dotnet test AgendaPro.sln \
  --no-build \
  --configuration Release \
  --collect:"XPlat Code Coverage" \
  --results-directory TestResults \
  --settings coverlet.runsettings

echo "=== Coverage report ==="
reportgenerator \
  -reports:"TestResults/**/coverage.cobertura.xml" \
  -targetdir:CoverageReport \
  -reporttypes:"Html;Cobertura;TextSummary" \
  -assemblyfilters:"+AgendaPro.*;-AgendaPro.Tests*;-AgendaPro.UnitTests*"

echo "=== Coverage summary ==="
cat CoverageReport/Summary.txt
```

---

## Erros comuns e soluções

### `dotnet format` falhou com "Formatting is not correct"

```bash
# Corrigir automaticamente
dotnet format AgendaPro.sln --no-restore
git add -u
git commit -m "style: apply dotnet format corrections"
```

### Nenhum arquivo de cobertura encontrado pelo ReportGenerator

Verifique se o arquivo `coverlet.runsettings` existe na raiz do repositório e se o path `./TestResults` contém subpastas com `coverage.cobertura.xml`:

```bash
find ./TestResults -name "coverage.cobertura.xml"
```

### Build falhou com CA-XXXX (Roslyn analyzer)

Verifique o `.editorconfig` na raiz. A maioria dos avisos de analyzers pode ser ajustada em `.editorconfig` com:

```ini
dotnet_diagnostic.CA1234.severity = suggestion  # ou none
```

### Cobertura abaixo do threshold

1. Veja o relatório HTML em `CoverageReport/index.html`
2. Identifique as classes em vermelho (cobertura < threshold)
3. Escreva testes para os cenários não cobertos
4. Re-execute os passos 4–6 acima
