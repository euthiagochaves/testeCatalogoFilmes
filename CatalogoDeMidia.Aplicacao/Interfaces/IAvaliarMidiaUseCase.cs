using CatalogoDeMidia.Aplicacao.Dtos.Requisicoes;
using CatalogoDeMidia.Aplicacao.Dtos.Respostas;

namespace CatalogoDeMidia.Aplicacao.Interfaces;

/// <summary>
/// Interface para o caso de uso de avaliação de uma mídia do catálogo.
/// </summary>
public interface IAvaliarMidiaUseCase
{
    /// <summary>
    /// Executa o fluxo de avaliação de uma mídia, aplicando uma nova nota.
    /// </summary>
    /// <param name="requisicao">DTO contendo os dados necessários para avaliação da mídia (identificação e nota).</param>
    /// <param name="cancellationToken">Token de cancelamento para operações assíncronas.</param>
    /// <returns>DTO representando a mídia com a avaliação atualizada.</returns>
    Task<MidiaRespostaDto> ExecutarAsync(
        AvaliarMidiaRequisicaoDto requisicao,
        CancellationToken cancellationToken = default);
}
