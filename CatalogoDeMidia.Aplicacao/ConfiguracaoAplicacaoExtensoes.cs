using CatalogoDeMidia.Aplicacao.CasosDeUso.AdicionarMidia;
using CatalogoDeMidia.Aplicacao.CasosDeUso.AvaliarMidia;
using CatalogoDeMidia.Aplicacao.CasosDeUso.ListarMidias;
using CatalogoDeMidia.Aplicacao.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogoDeMidia.Aplicacao;

/// <summary>
/// Classe de extensão para configuração da injeção de dependência da camada de Aplicação.
/// </summary>
public static class ConfiguracaoAplicacaoExtensoes
{
    /// <summary>
    /// Adiciona os serviços da camada de Aplicação ao container de injeção de dependência.
    /// Registra todos os casos de uso disponíveis no catálogo de mídias.
    /// </summary>
    /// <param name="services">Coleção de serviços do container de DI.</param>
    /// <returns>A mesma coleção de serviços para permitir chamadas encadeadas.</returns>
    /// <remarks>
    /// Este método deve ser chamado na inicialização da aplicação (Program.cs) para registrar
    /// todos os casos de uso. Novos casos de uso devem ser adicionados neste método para manter
    /// a configuração centralizada.
    /// </remarks>
    public static IServiceCollection AdicionarAplicacao(this IServiceCollection services)
    {
        // Registrar caso de uso: Adicionar Mídia
        services.AddScoped<IAdicionarMidiaUseCase, AdicionarMidiaUseCase>();

        // Registrar caso de uso: Listar Mídias
        services.AddScoped<IListarMidiasUseCase, ListarMidiasUseCase>();

        // Registrar caso de uso: Avaliar Mídia
        services.AddScoped<IAvaliarMidiaUseCase, AvaliarMidiaUseCase>();

        return services;
    }
}
