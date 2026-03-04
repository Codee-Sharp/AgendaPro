# Result Pattern na AgendaPro
Este documento descreve a implementação do Result Pattern no projeto AgendaPro, mostrando como ele foi usado para padronizar resultados de sucesso e falha em UseCases e como é transformado em resposta de API.
## Objetivo
 - O Result Pattern foi implementado para:

Padronizar o retorno de métodos com sucesso ou falha.

Evitar o uso de exceções para fluxo normal de erro.

Facilitar a transformação do resultado em respostas HTTP consistentes para a API.

## 2 Estrutura do Result Pattern

No projeto, existem duas classes principais:

### 2.1 Result base (sem tipo genérico)
```csharp
using System;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore.Update;

namespace AgendaPro.Domain.Shared
{
    public class Result
    {
        public bool IsSuccess { get; }
        public IReadOnlyList<Error> Errors { get; }

        public bool IsFailure => !IsSuccess;

        protected Result(bool isSuccess, IEnumerable<Error> errors)
        {
            IsSuccess = isSuccess;
            Errors = new ReadOnlyCollection<Error>(errors.ToList());
        }

        public static Result Success() => new(true, Array.Empty<Error>());

        public static Result Failure(params Error[] errors) => new(false, errors);

        public static Result Combine(params Result[] results)
        {
            var errors = results.SelectMany(r => r.Errors).ToArray();
            return errors.Any() ? Failure(errors) : Success();
        }
    }
}

```
### 2.2. Result genérico (Result<T>)

```csharp

using System;

namespace AgendaPro.Domain.Shared;

public class Result<T> : Result
{
  public T? Value { get; }

    private Result(T? value) : base(true, Array.Empty<Error>())
    {
        Value = value;
    }

    public Result(T? value, IEnumerable<Error> errors) : base(false, errors)
    {
        Value = value;
    }

    public static Result<T> Success(T value) => new(value);

    public static new Result<T> Failure(params Error[] errors)
        => new(default, errors);
        
    public T GetValueOrThrow()
    {
        if (IsFailure)
            throw new InvalidOperationException("Cannot access Value on a failed result.");
        return Value!;
    }
}

```

### 2.3 Classe de erro(Error)

```csharp
public sealed record Error(
    string Code,
    string Message,
    string? Field = null,
    IReadOnlyDictionary<string, string>? Metadata = null
);

```

## 3. Uso no UseCase

Exemplo de implementação no TagUseCase:

```csharp

using AgendaPro.Application.Tags.Dtos;
using AgendaPro.Domain.Shared;
using AgendaPro.Domain.Tags.Models;
using AgendaPro.Domain.Tags.Repositories;

namespace AgendaPro.Application.Tags.UseCase
{
    public class TagUseCase(ITagRepository tagRepository)
    {
        public async Task<Result<TagDto>> CreateAsync(TagDto tagDto)
        {
            if (string.IsNullOrWhiteSpace(tagDto.Name))
            {
                return Result<TagDto>.Failure(new Error("TAG001: Nome da tag não foi informado", "O nome da tag é obrigatório"));
            }

            var userId = Guid.Empty;
            var model = new TagModel(tagDto.Name, userId);

            await tagRepository.SaveAsync(model);

            var reponse = new TagDto(model);

            return Result<TagDto>.Success(reponse);
        }
    }
}
```
Result<T>.Success(value) → retorna sucesso com o objeto criado.

Result<T>.Failure(errors) → retorna falha com lista de erros.

## 4 Transformação api Response

```csharp
if (result.IsFailure)
{
    var errorMessages = result.Errors.Select(e => e.Message).ToList();
    return BadRequest(new ApiResponse<TagDto>(errorMessages));
}

return Ok(new ApiResponse<TagDto>(result.Value));
```

ApiResponse<T> adiciona o traceId para rastrear requisições.

O JSON enviado ao cliente fica assim:
### -> Sucesso
 curl -X POST http://localhost:5015/api/tags \
> -H "Content-Type: application/json" \
> -d '{"name":"Minha Tag"}'


```json
{
    "success":true,
    "data":{"id":"019a1cae-b103-759f-aeb9-8b30027462eb",
    "name":"Minha Tag"},
    "errors":null,
    "traceId":"2b70c29a-104c-465d-a4f2-d8fb403cd0cb"}

```
### -> Falha

 curl -X POST http://localhost:5015/api/tags \
> -H "Content-Type: application/json" \
> -d '{"name":""}'
```json
{
    "success":false,
    "data":null,
    "errors":["O nome da tag é obrigatório"],
    "traceId":"2a12960f-160f-43da-bd3b-cf3ac669c908"}
```

## 5 Beneficios

Padronização do fluxo de sucesso/falha.

Facilita logging e rastreamento de erros (traceId).

Reduz uso de exceções para fluxo normal de validação.

Separa claramente lógica de negócio (UseCase) da resposta HTTP (ApiResponse).