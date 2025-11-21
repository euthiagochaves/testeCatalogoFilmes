using CatalogoDeMidia.Dominio.Repositorios;
using CatalogoDeMidia.Infraestrutura.Persistencia.Contexto;
using CatalogoDeMidia.Infraestrutura.Persistencia.Repositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogoDeMidia.Infraestrutura.Persistencia.Configuracoes;

/// <summary>
/// Classe de extensão para configuração da camada de infraestrutura.
/// Responsável por registrar o DbContext e repositórios na injeção de dependência.
/// </summary>
public static class ConfiguracaoInfraestruturaExtensoes
{
    /// <summary>
    /// Adiciona os serviços de infraestrutura ao container de injeção de dependência.
    /// Registra o DbContext configurado para PostgreSQL (Supabase) e os repositórios.
    /// </summary>
    /// <param name="services">A coleção de serviços do container de DI.</param>
    /// <param name="configuration">A configuração da aplicação contendo a connection string.</param>
    /// <returns>A coleção de serviços para encadeamento de chamadas.</returns>
    /// <exception cref="InvalidOperationException">
    /// Lançada quando a connection string "CatalogoDeMidia" não é encontrada na configuração.
    /// </exception>
    public static IServiceCollection AdicionarInfraestrutura(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Obter a connection string da configuração
        var connectionString = configuration.GetConnectionString("CatalogoDeMidia");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "A connection string 'CatalogoDeMidia' não foi encontrada na configuração. " +
                "Certifique-se de que ela está definida em appsettings.json ou nas variáveis de ambiente.");
        }

        // Registrar o DbContext com PostgreSQL (Supabase)
        services.AddDbContext<CatalogoDeMidiaDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                // Configurações específicas do Npgsql, se necessário
                // Por exemplo: npgsqlOptions.MigrationsAssembly("CatalogoDeMidia.Infraestrutura");
            });
        });

        // Registrar repositórios com lifetime Scoped (apropriado para DbContext)
        services.AddScoped<IMidiaRepositorio, MidiaRepositorio>();

        return services;
    }
}
