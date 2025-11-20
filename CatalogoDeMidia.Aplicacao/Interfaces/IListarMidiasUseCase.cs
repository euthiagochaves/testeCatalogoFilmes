using CatalogoDeMidia.Aplicacao.Dtos.Requisicoes;
using CatalogoDeMidia.Aplicacao.Dtos.Respostas;

namespace CatalogoDeMidia.Aplicacao.Interfaces;

/// <summary>
/// Interface para o caso de uso de listagem e filtragem de mídias do catálogo.
/// </summary>
public interface IListarMidiasUseCase
{
    /// <summary>
    /// Executa a listagem de mídias aplicando filtros, paginação e ordenação conforme especificado.
    /// </summary>
    /// <param name="filtros">DTO contendo os critérios de filtro, paginação e ordenação.</param>
    /// <param name="cancellationToken">Token de cancelamento para operações assíncronas.</param>
    /// <returns>Lista somente leitura de mídias que atendem aos critérios especificados.</returns>
    Task<IReadOnlyList<MidiaRespostaDto>> ExecutarAsync(ListarMidiasRequisicaoDto filtros, CancellationToken cancellationToken = default);
}
