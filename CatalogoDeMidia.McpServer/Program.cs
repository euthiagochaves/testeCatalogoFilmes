using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// Criar o host genérico para o servidor MCP
var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((context, services) =>
{
    // Configurar logging
    services.AddLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
        logging.SetMinimumLevel(LogLevel.Information);
    });

    // TODO: Registrar serviços da camada de Aplicação (casos de uso)
    // Exemplo: services.AddScoped<IAdicionarMidiaUseCase, AdicionarMidiaUseCase>();
    // Exemplo: services.AddScoped<IListarMidiasUseCase, ListarMidiasUseCase>();
    // Exemplo: services.AddScoped<IAvaliarMidiaUseCase, AvaliarMidiaUseCase>();

    // TODO: Registrar serviços da camada de Infraestrutura (DbContext, repositórios)
    // Utilizar a mesma configuração da API para reutilizar código
    // Exemplo: services.AdicionarInfraestrutura(connectionString);
    // Connection string para SQLite: "Data Source=catalogodemidia.db"

    // TODO: Registrar o servidor MCP e as ferramentas (tools)
    // Será implementado quando integrar com a biblioteca MCP
    // Exemplo: services.AddMcpServer();
    // Exemplo: services.AddSingleton<FerramentasCatalogoDeMidia>();
});

var host = builder.Build();

// TODO: Inicializar o servidor MCP
// O servidor MCP escutará requisições via stdin/stdout (protocolo JSON-RPC)
// e despachará para as ferramentas (tools) registradas:
// - adicionar_midia
// - listar_midias
// - avaliar_midia

Console.WriteLine("CatalogoDeMidia.McpServer iniciando...");
Console.WriteLine("TODO: Implementar protocolo MCP e registro de ferramentas");

// await host.RunAsync();
Console.WriteLine("Servidor MCP placeholder - implementação futura");
