# 10-tecnologias-e-padroes

## Tecnologias principais

- **Linguagem**: C# 14
- **Plataforma**: .NET 10
- **IDE prevista**: Visual Studio 2026 Insiders
- **Banco de dados**: SQLite (arquivo local)
- **Acesso a dados**:
  - Utilizar **Entity Framework Core** para facilitar o mapeamento objeto-relacional.
  - Opcionalmente, podem ser usados comandos SQL diretos onde necessário, mas o padrão deve ser EF Core.
- **API HTTP**:
  - Projeto ASP.NET Core (target .NET 10).
  - Pode ser implementada como **Minimal API** ou com **Controllers**; a preferência deve ser definida na arquitetura, mas manter tudo bem organizado.
- **MCP Server**:
  - Projeto de console em .NET 10 que implementa o protocolo MCP para exposição de tools ao GitHub Copilot.
  - O MCP Server será responsável por orquestrar as chamadas aos casos de uso (camada de Aplicação).

## Padrões e princípios

1. **DDD (Domain-Driven Design) simplificado**
   - Separar claramente:
     - **Domínio**: entidades, agregados, enums, interfaces de repositório.
     - **Aplicação**: casos de uso, DTOs de entrada/saída, serviços de aplicação.
     - **Infraestrutura**: implementação dos repositórios, DbContext, migrações.
     - **Api**: endpoints HTTP de entrada, mapeamento entre DTOs de requisição/resposta e casos de uso.
     - **McpServer**: adaptação das tools MCP para casos de uso da Aplicação.
   - O domínio deve ter o mínimo de dependências externas possível.

2. **Separação de responsabilidades**
   - A camada de **Api** não deve conter lógica de negócio; apenas orquestração e mapeamento de entrada/saída.
   - A camada de **McpServer** também não deve conter lógica de negócio; apenas traduzir chamadas MCP em chamadas para a camada de Aplicação.
   - Toda regra de negócio deve estar na camada de **Aplicação** (casos de uso) e, quando fizer sentido, na camada de **Domínio** (métodos de entidades, invariantes).

3. **Injeção de dependência**
   - Usar o container padrão do .NET (DI configurado a partir de `Host.CreateDefaultBuilder` ou equivalente no .NET 10).
   - Registrar serviços de aplicação, repositórios e DbContext na inicialização.
   - Tanto a **Api** quanto o **McpServer** devem reutilizar a mesma configuração de DI, idealmente através de um método de extensão compartilhado (por exemplo, em `CatalogoDeMidia.Infraestrutura` ou `CatalogoDeMidia.Aplicacao`).

4. **Persistência / SQLite**
   - O arquivo do banco SQLite deve ser local, com nome padrão sugerido: `catalogo_de_midia.db`.
   - O caminho pode ser uma pasta `Data` na raiz da aplicação (por exemplo: `Data/catalogo_de_midia.db`).
   - Deve existir uma migração inicial que cria a tabela principal de mídias.

5. **Tratamento de erros e validações**
   - Casos de uso devem realizar validações de regras de negócio (por exemplo: título obrigatório, ano razoável, tipo de mídia válido, nota dentro de um intervalo definido).
   - Erros de validação devem ser retornados de forma clara para:
     - API (HTTP status adequado, como 400 Bad Request);
     - MCP (mensagem de erro amigável para o usuário da IA).
   - Exceções técnicas (falha de conexão, erro no banco) devem ser logadas e propagadas de forma controlada.

6. **Logging**
   - Tanto a Api quanto o McpServer devem utilizar logging via `ILogger<T>`.
   - Eventos importantes: criação de mídia, avaliação atualizada, consultas com filtros, erros ao acessar o banco.

7. **Comentários e documentação**
   - Classes e métodos públicos devem ter comentários XML (`///`) com descrições claras em português do Brasil.
   - Os arquivos `.md` desta pasta `instructions` serão usados pelo GitHub Copilot para entender o contexto e gerar código alinhado com essas regras.

