using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CatalogoDeMidia.Aplicacao.Dtos.Requisicoes;
using CatalogoDeMidia.Aplicacao.Dtos.Respostas;
using CatalogoDeMidia.Aplicacao.Interfaces;
using CatalogoDeMidia.Dominio.Entidades;
using CatalogoDeMidia.Dominio.Repositorios;

namespace CatalogoDeMidia.Aplicacao.CasosDeUso.AvaliarMidia;

/// <summary>
/// Implementação do caso de uso para avaliar uma mídia do catálogo.
/// </summary>
public class AvaliarMidiaUseCase : IAvaliarMidiaUseCase
{
    private readonly IMidiaRepositorio _midiaRepositorio;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="AvaliarMidiaUseCase"/>.
    /// </summary>
    /// <param name="midiaRepositorio">Repositório de mídias.</param>
    /// <exception cref="ArgumentNullException">Lançada quando o repositório é nulo.</exception>
    public AvaliarMidiaUseCase(IMidiaRepositorio midiaRepositorio)
    {
        _midiaRepositorio = midiaRepositorio ?? throw new ArgumentNullException(nameof(midiaRepositorio));
    }

    /// <summary>
    /// Executa o fluxo de avaliação de uma mídia, aplicando uma nova nota.
    /// </summary>
    /// <param name="requisicao">DTO contendo os dados necessários para avaliação da mídia (identificação e nota).</param>
    /// <param name="cancellationToken">Token de cancelamento para operações assíncronas.</param>
    /// <returns>DTO representando a mídia com a avaliação atualizada.</returns>
    /// <exception cref="ArgumentNullException">Lançada quando a requisição é nula.</exception>
    /// <exception cref="ArgumentException">Lançada quando os dados da requisição são inválidos ou a mídia não é encontrada.</exception>
    public async Task<MidiaRespostaDto> ExecutarAsync(
        AvaliarMidiaRequisicaoDto requisicao,
        CancellationToken cancellationToken = default)
    {
        ValidarRequisicao(requisicao);

        var midia = await ObterMidiaAsync(requisicao, cancellationToken);

        midia.DefinirNota(requisicao.NovaNota);

        await _midiaRepositorio.AtualizarAsync(midia, cancellationToken);

        return MapearParaResposta(midia);
    }

    /// <summary>
    /// Valida a requisição de avaliação de mídia.
    /// </summary>
    /// <param name="requisicao">Requisição a ser validada.</param>
    /// <exception cref="ArgumentNullException">Lançada quando a requisição é nula.</exception>
    /// <exception cref="ArgumentException">Lançada quando nem IdMidia nem Titulo são fornecidos.</exception>
    private static void ValidarRequisicao(AvaliarMidiaRequisicaoDto requisicao)
    {
        if (requisicao is null)
        {
            throw new ArgumentNullException(nameof(requisicao), "A requisição não pode ser nula.");
        }

        if (!requisicao.IdMidia.HasValue && string.IsNullOrWhiteSpace(requisicao.Titulo))
        {
            throw new ArgumentException(
                "Pelo menos um dos campos IdMidia ou Titulo deve ser fornecido para identificar a mídia.",
                nameof(requisicao));
        }
    }

    /// <summary>
    /// Obtém a mídia a ser avaliada com base nos critérios fornecidos na requisição.
    /// </summary>
    /// <param name="requisicao">Requisição contendo os critérios de busca.</param>
    /// <param name="cancellationToken">Token de cancelamento da operação.</param>
    /// <returns>A mídia encontrada.</returns>
    /// <exception cref="ArgumentException">Lançada quando a mídia não é encontrada ou múltiplas mídias são encontradas.</exception>
    private async Task<Midia> ObterMidiaAsync(
        AvaliarMidiaRequisicaoDto requisicao,
        CancellationToken cancellationToken)
    {
        Midia? midia = null;

        // Prioriza busca por IdMidia se fornecido
        if (requisicao.IdMidia.HasValue)
        {
            midia = await _midiaRepositorio.ObterPorIdAsync(requisicao.IdMidia.Value, cancellationToken);
        }
        // Caso contrário, busca por Titulo
        else if (!string.IsNullOrWhiteSpace(requisicao.Titulo))
        {
            var midias = await _midiaRepositorio.ObterPorTituloAsync(requisicao.Titulo, cancellationToken);

            if (midias.Count > 1)
            {
                throw new ArgumentException(
                    "Múltiplas mídias encontradas com o título especificado. Use o IdMidia para identificar a mídia específica.",
                    nameof(requisicao));
            }

            midia = midias.FirstOrDefault();
        }

        if (midia is null)
        {
            throw new ArgumentException("Mídia não encontrada.", nameof(requisicao));
        }

        return midia;
    }

    /// <summary>
    /// Mapeia a entidade de domínio Midia para o DTO de resposta.
    /// </summary>
    /// <param name="midia">Entidade de mídia a ser mapeada.</param>
    /// <returns>DTO de resposta com os dados da mídia.</returns>
    private static MidiaRespostaDto MapearParaResposta(Midia midia)
    {
        return new MidiaRespostaDto
        {
            Id = midia.Id,
            Titulo = midia.Titulo,
            AnoLancamento = midia.AnoLancamento,
            Tipo = midia.Tipo,
            Genero = midia.Genero,
            Nota = midia.Nota,
            Assistido = midia.Assistido,
            DataCriacao = midia.DataCriacao,
            DataAtualizacao = midia.DataAtualizacao
        };
    }
}
