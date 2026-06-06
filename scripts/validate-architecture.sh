#!/usr/bin/env bash
# =============================================================================
# validate-architecture.sh
# Valida que as referências entre projetos respeitam a arquitetura em camadas.
#
# Regras verificadas:
#   Domain      → NÃO pode referenciar Application, Infrastructure ou Api
#   Application → NÃO pode referenciar Api
#   Application → NÃO pode referenciar Infrastructure diretamente
# =============================================================================
set -euo pipefail

ERRORS=0

# ── Cores para output legível ─────────────────────────────────────────────────
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# ── Helper: verifica se um .csproj contém referência proibida ─────────────────
check_no_reference() {
    local project_file="$1"
    local forbidden="$2"
    local reason="$3"

    if [ ! -f "$project_file" ]; then
        echo -e "${YELLOW}SKIP${NC}  $project_file não encontrado — pulando verificação."
        return 0
    fi

    # grep -q retorna 0 se encontrar, 1 se não encontrar
    if grep -qi "$forbidden" "$project_file"; then
        echo -e "${RED}FAIL${NC}  $project_file → referencia '$forbidden'"
        echo -e "      Motivo: $reason"
        # Emite anotação de erro no GitHub Actions
        echo "::error file=$project_file,title=Architecture Violation::$project_file referencia '$forbidden'. $reason"
        ERRORS=$((ERRORS + 1))
    else
        echo -e "${GREEN}OK${NC}    $project_file → não referencia '$forbidden'"
    fi
}

echo ""
echo "════════════════════════════════════════════════════════════"
echo "  Validação Arquitetural – AgendaPro"
echo "════════════════════════════════════════════════════════════"
echo ""

# ── 1. AgendaPro.Domain ───────────────────────────────────────────────────────
# Domain é a camada mais interna; não pode depender de nenhuma outra camada do projeto.
DOMAIN="src/AgendaPro.Domain/AgendaPro.Domain.csproj"

echo "[ Domain ]"
check_no_reference "$DOMAIN" "AgendaPro.Application" \
    "Domain não pode depender de Application."
check_no_reference "$DOMAIN" "AgendaPro.Infrastucture" \
    "Domain não pode depender de Infrastructure."
check_no_reference "$DOMAIN" "AgendaPro.Infrastructure" \
    "Domain não pode depender de Infrastructure."
check_no_reference "$DOMAIN" "AgendaPro.Api" \
    "Domain não pode depender de Api."
echo ""

# ── 2. AgendaPro.Application ──────────────────────────────────────────────────
# Application orquestra regras; não pode depender de Api nem de Infrastructure diretamente.
APP="src/AgendaPro.Application/AgendaPro.Application.csproj"

echo "[ Application ]"
check_no_reference "$APP" "AgendaPro.Api" \
    "Application não pode depender de Api."
check_no_reference "$APP" "AgendaPro.Infrastucture" \
    "Application não deve depender de Infrastructure diretamente — use interfaces de Domain."
check_no_reference "$APP" "AgendaPro.Infrastructure" \
    "Application não deve depender de Infrastructure diretamente — use interfaces de Domain."
echo ""

# ── Resultado ─────────────────────────────────────────────────────────────────
echo "════════════════════════════════════════════════════════════"
if [ "$ERRORS" -gt 0 ]; then
    echo -e "  ${RED}FALHOU${NC}: $ERRORS violação(ões) arquitetural(is) encontrada(s)."
    echo "  Corrija as referências nos .csproj antes de fazer merge."
    echo "════════════════════════════════════════════════════════════"
    exit 1
else
    echo -e "  ${GREEN}PASSOU${NC}: Todas as regras arquiteturais foram respeitadas."
    echo "════════════════════════════════════════════════════════════"
    exit 0
fi
