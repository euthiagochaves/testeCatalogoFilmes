# CatalogoDeMidia

Projeto de catálogo de mídias (filmes e séries) pensado para servir como **laboratório de arquitetura moderna com .NET** e, ao mesmo tempo, **backend inteligente** para o GitHub Copilot via **MCP (Model Context Protocol)**.

O objetivo é ter uma solução completa, organizada em camadas (DDD), com API HTTP, banco local em SQLite e um MCP Server que expõe tools para que o Copilot consiga **adicionar, listar e avaliar mídias** conversando em linguagem natural.

---

## Visão geral

O sistema permite:

- **Cadastrar mídias** (filmes e séries) com título, tipo, gênero, ano de lançamento, etc.
- **Listar mídias** com filtros e/ou paginação (por título, tipo, gênero, ano, etc.).
- **Avaliar mídias** (nota/comentário), mantendo o estado atualizado no catálogo.
- **Expor essas operações como API HTTP** (para qualquer cliente).
- **Expor essas mesmas operações como tools MCP** (para o GitHub Copilot):
  - `adicionar_midia`
  - `listar_midias`
  - `avaliar_midia`

Tudo isso utilizando uma arquitetura em camadas, separando bem domínio, aplicação, infraestrutura, API e MCP Server.

---

## Tecnologias e padrões

- **Linguagem**
  - C# 14 (target para o futuro .NET 10)

- **Runtime / Framework**
  - .NET 10 (projetado para rodar em versão futura do .NET)
  - ASP.NET Core (para a API)
  - Host genérico do .NET (para o MCP Server)

- **Arquitetura**
  - DDD (Domain-Driven Design) em camadas:
    - Domínio
    - Aplicação
    - Infraestrutura
    - API
    - MCP Server
  - Separação clara de responsabilidades:
    - Domínio não conhece infraestrutura.
    - Aplicação orquestra casos de uso.
    - Infraestrutura implementa acesso a dados.
    - API/MCP Server atuam apenas como camadas de transporte.

- **Banco de dados**
  - SQLite local
  - Entity Framework Core (DbContext + Fluent API para mapeamentos)

- **Integração com IA**
  - MCP (Model Context Protocol) para GitHub Copilot
  - MCP Server em .NET com transporte via STDIO
  - Tools definidas em C# e descobertas por assembly via MCP SDK

- **Documentação**
  - Swagger / OpenAPI para a API HTTP
  - Pasta `instructions/` com arquivos de arquitetura e guias para IA (GitHub Copilot / MCP)

---

## Estrutura da solution

A solution principal é `CatalogoDeMidia.sln`, com os seguintes projetos:

- **`CatalogoDeMidia.Dominio`**
  - Entidade de domínio `Midia`
  - Enums:
    - `TipoMidia` (filme, série, etc.)
    - `GeneroMidia` (ação, drama, comédia, etc.)
  - Interfaces de repositório:
    - `IMidiaRepositorio`
  - Regras de negócio e modelos puros do domínio

- **`CatalogoDeMidia.Aplicacao`**
  - DTOs:
    - `AdicionarMidiaRequisicaoDto`
    - `ListarMidiasRequisicaoDto`
    - `AvaliarMidiaRequisicaoDto`
    - `MidiaRespostaDto`
  - Interfaces de casos de uso:
    - `IAdicionarMidiaUseCase`
    - `IListarMidiasUseCase`
    - `IAvaliarMidiaUseCase`
  - Implementações de casos de uso:
    - `AdicionarMidiaUseCase`
    - `ListarMidiasUseCase`
    - `AvaliarMidiaUseCase`
  - Classe de configuração de DI:
    - `ConfiguracaoAplicacaoExtensoes`

- **`CatalogoDeMidia.Infraestrutura`**
  - `CatalogoDeMidiaDbContext` (EF Core / SQLite)
  - Mapeamento via Fluent API:
    - `MidiaConfiguracao`
  - Repositórios concretos:
    - `MidiaRepositorio` (implementa `IMidiaRepositorio`)
  - Classe de configuração de DI:
    - `ConfiguracaoInfraestruturaExtensoes`
  - Responsável por conexão com DB, migrations, etc.

- **`CatalogoDeMidia.Api`**
  - API HTTP para consumo externo
  - `Program.cs` configurando:
    - DI (Aplicação + Infraestrutura)
    - SQLite + EF Core
    - Swagger / OpenAPI
  - Controllers:
    - `MidiasController`
      - `POST /api/midias` → adicionar mídia
      - `GET /api/midias` → listar mídias
      - `POST /api/midias/{id}/avaliacoes` (ou rota equivalente) → avaliar mídia

- **`CatalogoDeMidia.McpServer`**
  - Host genérico (`Host.CreateApplicationBuilder`)
  - Registro de:
    - Infraestrutura
    - Aplicação
    - Tools MCP
  - MCP Server configurado com:
    - Transporte STDIO
    - Descoberta de tools por assembly
  - Classe de tools:
    - `FerramentasCatalogoDeMidia`
      - Tool MCP `adicionar_midia`
      - Tool MCP `listar_midias`
      - Tool MCP `avaliar_midia`

- **Pasta `instructions/`**
  - Arquivos de instrução usados pela IA (GitHub Copilot / MCP), por exemplo:
    - `00-visao-geral-e-objetivos.md`
    - `10-tecnologias-e-padroes.md`
    - `20-arquitetura-ddd-e-dependencias.md`
    - `30-mcp-server-e-tools.md`
    - `40-estrutura-da-solution-e-detalhamento.md`
  - Servem como “fonte da verdade” para:
    - Arquitetura
    - Nomes de projetos
    - Padrões de código
    - Contratos entre camadas
    - Como a IA deve gerar código dentro do projeto

---

## Fluxo de alto nível

1. **Camada de transporte (API ou MCP)** recebe a requisição:
   - HTTP (REST) via `CatalogoDeMidia.Api`, ou
   - Tool MCP via `CatalogoDeMidia.McpServer` / `FerramentasCatalogoDeMidia`.

2. **Camada de Aplicação** é acionada:
   - Controller ou tool chama o caso de uso:
     - `IAdicionarMidiaUseCase`
     - `IListarMidiasUseCase`
     - `IAvaliarMidiaUseCase`
   - Entradas são passadas via DTOs.

3. **Camada de Domínio + Infraestrutura**:
   - Casos de uso usam `IMidiaRepositorio` para:
     - Persistir nova mídia
     - Buscar mídias filtradas
     - Atualizar avaliações
   - `IMidiaRepositorio` é implementado pela Infraestrutura, que usa EF Core/SQLite.

4. **Resposta**:
   - Casos de uso retornam `MidiaRespostaDto` (ou lista) para a Aplicação.
   - API/MCP convertem a resposta em JSON no formato esperado pelo cliente ou pelo GitHub Copilot.

---

## Como rodar (visão geral)

> OBS: comandos e detalhes exatos podem variar conforme a versão do .NET 10 e os arquivos `instructions`. Ajuste conforme o seu ambiente.

1. **Restaurar pacotes e compilar**

```bash
dotnet restore
dotnet build
