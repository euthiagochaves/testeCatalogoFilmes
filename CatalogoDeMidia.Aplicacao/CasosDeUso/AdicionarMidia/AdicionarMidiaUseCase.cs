using System;
using System.Threading;
using System.Threading.Tasks;
using CatalogoDeMidia.Aplicacao.Dtos.Requisicoes;
using CatalogoDeMidia.Aplicacao.Dtos.Respostas;
using CatalogoDeMidia.Aplicacao.Interfaces;
using CatalogoDeMidia.Dominio.Entidades;
using CatalogoDeMidia.Dominio.Repositorios;

namespace CatalogoDeMidia.Aplicacao.CasosDeUso.AdicionarMidia;

/// <summary>
/// Implementação do caso de uso para adicionar uma nova mídia ao catálogo.
/// </summary>
public class AdicionarMidiaUseCase : IAdicionarMidiaUseCase
{
    private readonly IMidiaRepositorio _midiaRepositorio;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="AdicionarMidiaUseCase"/>.
    /// </summary>
    /// <param name="midiaRepositorio">Repositório de mídias.</param>
    /// <exception cref="ArgumentNullException">Lançada quando o repositório é nulo.</exception>
    public AdicionarMidiaUseCase(IMidiaRepositorio midiaRepositorio)
    {
        _midiaRepositorio = midiaRepositorio ?? throw new ArgumentNullException(nameof(midiaRepositorio));
    }

    /// <summary>
    /// Executa o fluxo de criação de uma nova mídia no catálogo.
    /// </summary>
    /// <param name="requisicao">DTO contendo os dados necessários para criação da mídia.</param>
    /// <param name="cancellationToken">Token de cancelamento para operações assíncronas.</param>
    /// <returns>DTO representando a mídia recém-criada.</returns>
    /// <exception cref="ArgumentNullException">Lançada quando a requisição é nula.</exception>
    /// <exception cref="ArgumentException">Lançada quando os dados da requisição são inválidos.</exception>
    public async Task<MidiaRespostaDto> ExecutarAsync(
        AdicionarMidiaRequisicaoDto requisicao,
        CancellationToken cancellationToken = default)
    {
        ValidarRequisicao(requisicao);

        var midia = new Midia(
            titulo: requisicao.Titulo,
            anoLancamento: requisicao.AnoLancamento,
            tipo: requisicao.Tipo,
            genero: requisicao.Genero,
            nota: requisicao.Nota,
            assistido: requisicao.Assistido ?? false
        );

        await _midiaRepositorio.AdicionarAsync(midia, cancellationToken);

        return MapearParaResposta(midia);
    }

    /// <summary>
    /// Valida a requisição de adição de mídia.
    /// </summary>
    /// <param name="requisicao">Requisição a ser validada.</param>
    /// <exception cref="ArgumentNullException">Lançada quando a requisição é nula.</exception>
    /// <exception cref="ArgumentException">Lançada quando os campos obrigatórios não estão preenchidos.</exception>
    private static void ValidarRequisicao(AdicionarMidiaRequisicaoDto requisicao)
    {
        if (requisicao is null)
        {
            throw new ArgumentNullException(nameof(requisicao), "A requisição não pode ser nula.");
        }

        if (string.IsNullOrWhiteSpace(requisicao.Titulo))
        {
            throw new ArgumentException("O título da mídia é obrigatório.", nameof(requisicao));
        }
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
