# CatalogoDeMidia.McpServer

Servidor MCP (Model Context Protocol) para o Catálogo de Mídias, que expõe ferramentas (tools) para interação com o catálogo via GitHub Copilot e outros clientes MCP.

## Descrição

Este projeto implementa um servidor MCP em .NET 10 que permite a interação com o catálogo de mídias (filmes e séries) através de três ferramentas principais:

1. **AdicionarMidiaAsync** - Adiciona uma nova mídia ao catálogo
2. **ListarMidiasAsync** - Lista mídias com filtros opcionais
3. **AvaliarMidiaAsync** - Define ou atualiza a nota de uma mídia

## Arquitetura

O servidor utiliza:
- **Host genérico** do .NET 10 (`Host.CreateApplicationBuilder`)
- **Injeção de dependência** para casos de uso e repositórios
- **Transporte STDIO** para comunicação via stdin/stdout
- **ModelContextProtocol** (versão 0.4.0-preview.3) para implementação do protocolo MCP

## Configuração

### Connection String

A connection string do PostgreSQL (Supabase) deve ser configurada em:

- **Desenvolvimento**: `appsettings.Development.json`
- **Produção/CI**: Variáveis de ambiente ou GitHub Secrets

Exemplo de configuração em `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "CatalogoDeMidia": "Host=seu-host.supabase.co;Database=postgres;Username=postgres;Password=sua-senha"
  }
}
```

### Ferramentas MCP

As ferramentas são expostas automaticamente através da classe `FerramentasCatalogoDeMidia`, marcada com `[McpServerToolType]`. Cada método público marcado com `[McpServerTool]` se torna uma ferramenta acessível via MCP.

## Uso com GitHub Copilot

Para usar o servidor MCP com o GitHub Copilot:

1. Configure o servidor no arquivo de configuração do Copilot
2. Inicie o servidor
3. Utilize comandos em linguagem natural no chat do Copilot:
   - "adicione o filme Matrix de 1999 como ação com nota 10"
   - "liste todos os filmes de terror"
   - "atualize a nota do filme Alien para 9.5"

## Estrutura do Projeto

```
CatalogoDeMidia.McpServer/
├── Ferramentas/
│   └── FerramentasCatalogoDeMidia.cs    # Implementação das tools MCP
├── Program.cs                            # Configuração e inicialização do host
├── appsettings.json                      # Configurações gerais
├── appsettings.Development.json          # Configurações de desenvolvimento
└── CatalogoDeMidia.McpServer.csproj     # Arquivo do projeto
```

## Dependências

- CatalogoDeMidia.Dominio - Entidades e interfaces de repositório
- CatalogoDeMidia.Aplicacao - Casos de uso e DTOs
- CatalogoDeMidia.Infraestrutura - DbContext e repositórios
- ModelContextProtocol (0.4.0-preview.3) - SDK MCP para .NET
- Microsoft.Extensions.Hosting - Host genérico do .NET

## Desenvolvimento

### Build

```bash
dotnet build
```

### Executar

```bash
dotnet run
```

### Notas Importantes

- O servidor utiliza STDIO para comunicação, não HTTP
- As ferramentas criam scopes de DI automaticamente para resolver serviços `Scoped`
- A conversão de enums (TipoMidia, GeneroMidia) é case-insensitive
- Validações são feitas tanto na camada de ferramentas quanto nos casos de uso
