using CatalogoDeMidia.Aplicacao;
using CatalogoDeMidia.Infraestrutura.Persistencia.Configuracoes;
using CatalogoDeMidia.McpServer.Ferramentas;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;

// Criar o host genérico para o servidor MCP usando .NET 10
var builder = Host.CreateApplicationBuilder(args);

// Configurar logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

// Registrar serviços da camada de Infraestrutura (DbContext, repositórios)
builder.Services.AdicionarInfraestrutura(builder.Configuration);

// Registrar serviços da camada de Aplicação (casos de uso)
builder.Services.AdicionarAplicacao();

// Configurar o servidor MCP
builder.Services.AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly(typeof(FerramentasCatalogoDeMidia).Assembly);

var host = builder.Build();

// Executar o host - o servidor MCP escutará requisições via stdin/stdout
await host.RunAsync();
