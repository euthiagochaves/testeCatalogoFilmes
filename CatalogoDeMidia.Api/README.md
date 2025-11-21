# CatalogoDeMidia.Api

## Descrição

API REST para gerenciamento de catálogo de mídias (filmes e séries) construída com ASP.NET Core (.NET 10).

## Configuração

### Connection String

A aplicação utiliza PostgreSQL como banco de dados, hospedado remotamente no **Supabase**.

#### Desenvolvimento Local

Para desenvolvimento local, configure a connection string no arquivo `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "CatalogoDeMidia": "Host=localhost;Port=5432;Database=catalogo_midia;Username=postgres;Password=postgres"
  }
}
```

#### Conexão com Supabase

Para conectar ao PostgreSQL remoto no Supabase, use o seguinte formato:

```json
{
  "ConnectionStrings": {
    "CatalogoDeMidia": "Host=db.<PROJECT_REF>.supabase.co;Port=5432;Database=postgres;Username=postgres;Password=<SUPABASE_PASSWORD>;SslMode=Require"
  }
}
```

Substitua:
- `<PROJECT_REF>` pelo identificador do seu projeto no Supabase
- `<SUPABASE_PASSWORD>` pela senha do banco de dados do seu projeto

#### Produção / CI/CD

Em ambientes de produção ou CI/CD, **não armazene a connection string no código**. Use:
- **Variáveis de ambiente**: Configure a variável `ConnectionStrings__CatalogoDeMidia`
- **GitHub Secrets**: Para pipelines de CI/CD no GitHub Actions
- **Azure Key Vault** ou outro serviço de gerenciamento de secrets

Exemplo de variável de ambiente:
```bash
export ConnectionStrings__CatalogoDeMidia="Host=db.xxxxx.supabase.co;Port=5432;Database=postgres;Username=postgres;Password=xxxxx;SslMode=Require"
```

## Executar a Aplicação

### Pré-requisitos

- .NET 10 SDK instalado
- PostgreSQL rodando localmente ou acesso ao Supabase

### Comandos

```bash
# Restaurar dependências
dotnet restore

# Compilar o projeto
dotnet build

# Executar a aplicação
dotnet run

# Executar com configuração específica de ambiente
ASPNETCORE_ENVIRONMENT=Development dotnet run
```

### Acessar o Swagger

Após iniciar a aplicação, acesse a documentação interativa do Swagger:

```
http://localhost:5000/swagger
```

Ou use a URL HTTPS (se configurada):

```
https://localhost:5001/swagger
```

## Endpoints Disponíveis

### Mídias

- `POST /api/Midias` - Adicionar nova mídia ao catálogo
- `GET /api/Midias` - Listar mídias com filtros opcionais
- `POST /api/Midias/{id}/avaliacoes` - Avaliar uma mídia existente

Consulte a interface do Swagger para detalhes completos sobre os parâmetros e respostas de cada endpoint.

## Estrutura do Projeto

```
CatalogoDeMidia.Api/
├── Controllers/          # Controllers da API
│   └── MidiasController.cs
├── Properties/          # Configurações de inicialização
├── appsettings.json     # Configurações gerais
├── appsettings.Development.json  # Configurações de desenvolvimento
└── Program.cs           # Ponto de entrada e configuração da aplicação
```

## Arquitetura

A API segue os princípios de **Domain-Driven Design (DDD)** com separação em camadas:

- **CatalogoDeMidia.Dominio**: Entidades, enums e interfaces de repositório
- **CatalogoDeMidia.Aplicacao**: Casos de uso e lógica de negócio
- **CatalogoDeMidia.Infraestrutura**: Implementação de repositórios e acesso a dados
- **CatalogoDeMidia.Api**: Camada de apresentação (esta API)

### Injeção de Dependência

O `Program.cs` configura a injeção de dependência através de métodos de extensão:

- `AdicionarInfraestrutura(configuration)`: Registra o DbContext e repositórios
- `AdicionarAplicacao()`: Registra os casos de uso

## Tecnologias

- **.NET 10**
- **ASP.NET Core** (Web API)
- **Entity Framework Core** com provider **Npgsql** (PostgreSQL)
- **Swagger/OpenAPI** para documentação
- **PostgreSQL** remoto no **Supabase**

## Próximas Configurações

Quando necessário, adicionar:
- Políticas de CORS
- Autenticação e autorização (JWT, OAuth)
- Rate limiting
- Tratamento global de erros
- Health checks para monitoramento
