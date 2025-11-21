# CatalogoDeMidia.Infraestrutura

## Propósito

Este projeto contém a implementação da camada de infraestrutura, incluindo acesso a dados com Entity Framework Core e PostgreSQL (Supabase).

## Estrutura

### Persistencia/Contexto
- `CatalogoDeMidiaDbContext` - Contexto do Entity Framework Core configurado para PostgreSQL

### Persistencia/Repositorios
Implementações concretas das interfaces de repositório:
- `MidiaRepositorio` - Implementação de `IMidiaRepositorio`

### Persistencia/Configuracoes
- **Configurações de entidades do EF Core (Fluent API)**
  - `MidiaConfiguracao` - Mapeamento da entidade Midia para PostgreSQL com naming convention snake_case
- **Configurações de Injeção de Dependência**
  - `ConfiguracaoInfraestruturaExtensoes` - Método de extensão para registrar DbContext e repositórios

## Dependências

Este projeto depende de:
- `CatalogoDeMidia.Dominio`
- Entity Framework Core 10.0.0
- Npgsql.EntityFrameworkCore.PostgreSQL 10.0.0-rc.2

## Uso

### Registro de Serviços de Infraestrutura

Para registrar o DbContext e repositórios nos projetos consumidores (API, MCP Server), adicione no `Program.cs`:

```csharp
using CatalogoDeMidia.Infraestrutura.Persistencia.Configuracoes;

// Registrar infraestrutura (DbContext + repositórios)
builder.Services.AdicionarInfraestrutura(builder.Configuration);
```

### Configuração da Connection String

A connection string deve ser configurada com o nome `"CatalogoDeMidia"` no `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "CatalogoDeMidia": "Host=seu-host.supabase.co;Port=5432;Database=postgres;Username=postgres;Password=sua-senha;SSL Mode=Require;"
  }
}
```

Para ambientes de produção/CI, utilize variáveis de ambiente seguindo o padrão:
```
ConnectionStrings__CatalogoDeMidia=Host=...
```

## Características

- ✅ Conexão com PostgreSQL remoto no Supabase via Npgsql
- ✅ Configuração de migrations no assembly `CatalogoDeMidia.Infraestrutura`
- ✅ Naming convention snake_case para tabelas e colunas no PostgreSQL
- ✅ Tipos PostgreSQL apropriados: `uuid`, `timestamptz`, `numeric`, etc.
- ✅ Repositórios registrados com lifetime `Scoped`
- ✅ Validação de connection string com mensagens de erro claras
