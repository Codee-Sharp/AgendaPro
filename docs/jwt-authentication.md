
# 🔑 Configuração do Ambiente de Desenvolvimento
### Configuração do JWT

O arquivo `appsettings.json` deve conter a configuração abaixo:

```json
{
  "Jwt": {
    "Issuer": "AgendaPro",
    "Audience": "AgendaPro.Api",
    "ExpirationMinutes": 60
  }
}
```

A propriedade `Jwt:Secret` **não deve** ser adicionada ao `appsettings.json`. Ela deve ser configurada exclusivamente por meio dos User Secrets (desenvolvimento) ou por variáveis de ambiente (produção).

---
## User Secrets

O projeto utiliza **User Secrets** para armazenar informações sensíveis durante o desenvolvimento, como a chave utilizada para assinar os tokens JWT.

> **Importante:** a chave JWT **não deve** ser adicionada ao `appsettings.json` nem enviada para o repositório.
---
### Inicializando os User Secrets

Execute o comando abaixo na pasta `src/AgendaPro.Api`:

```bash
dotnet user-secrets init
```

> Esse comando cria um `UserSecretsId` no arquivo `AgendaPro.Api.csproj`. Esse identificador faz parte do projeto e deve ser versionado normalmente.
---
### Configurando a chave JWT

Após inicializar os User Secrets, configure a chave localmente:

```bash
dotnet user-secrets set "Jwt:Secret" "SUA_CHAVE_SECRETA_COM_NO_MINIMO_32_CARACTERES"
```
---
### Verificando a configuração

```bash
dotnet user-secrets list
```
---
Saída esperada:

```text
Jwt:Secret = ********
```

> O valor da Secret é armazenado apenas na máquina do desenvolvedor e **não é enviado ao Git**.
