using CatalogoDeMidia.Aplicacao.Dtos.Requisicoes;
using CatalogoDeMidia.Aplicacao.Dtos.Respostas;
using CatalogoDeMidia.Aplicacao.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CatalogoDeMidia.Api.Controllers;

/// <summary>
/// Controller responsável por gerenciar as operações relacionadas a mídias do catálogo.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MidiasController : ControllerBase
{
    private readonly IAdicionarMidiaUseCase _adicionarMidiaUseCase;
    private readonly IListarMidiasUseCase _listarMidiasUseCase;
    private readonly IAvaliarMidiaUseCase _avaliarMidiaUseCase;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="MidiasController"/>.
    /// </summary>
    /// <param name="adicionarMidiaUseCase">Caso de uso para adicionar mídia.</param>
    /// <param name="listarMidiasUseCase">Caso de uso para listar mídias.</param>
    /// <param name="avaliarMidiaUseCase">Caso de uso para avaliar mídia.</param>
    public MidiasController(
        IAdicionarMidiaUseCase adicionarMidiaUseCase,
        IListarMidiasUseCase listarMidiasUseCase,
        IAvaliarMidiaUseCase avaliarMidiaUseCase)
    {
        _adicionarMidiaUseCase = adicionarMidiaUseCase;
        _listarMidiasUseCase = listarMidiasUseCase;
        _avaliarMidiaUseCase = avaliarMidiaUseCase;
    }

    /// <summary>
    /// Adiciona uma nova mídia ao catálogo.
    /// </summary>
    /// <param name="requisicao">Dados da mídia a ser adicionada.</param>
    /// <param name="cancellationToken">Token de cancelamento para operações assíncronas.</param>
    /// <returns>Mídia recém-criada.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(MidiaRespostaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MidiaRespostaDto>> Adicionar(
        [FromBody] AdicionarMidiaRequisicaoDto requisicao,
        CancellationToken cancellationToken)
    {
        var resultado = await _adicionarMidiaUseCase.ExecutarAsync(requisicao, cancellationToken);
        return CreatedAtAction(nameof(Adicionar), new { id = resultado.Id }, resultado);
    }

    /// <summary>
    /// Lista as mídias do catálogo aplicando filtros opcionais.
    /// </summary>
    /// <param name="requisicao">Filtros e parâmetros de paginação.</param>
    /// <param name="cancellationToken">Token de cancelamento para operações assíncronas.</param>
    /// <returns>Lista de mídias que atendem aos critérios especificados.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<MidiaRespostaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<MidiaRespostaDto>>> Listar(
        [FromQuery] ListarMidiasRequisicaoDto requisicao,
        CancellationToken cancellationToken)
    {
        var resultado = await _listarMidiasUseCase.ExecutarAsync(requisicao, cancellationToken);
        return Ok(resultado);
    }

    /// <summary>
    /// Avalia uma mídia atribuindo uma nova nota.
    /// </summary>
    /// <param name="id">Identificador único da mídia.</param>
    /// <param name="requisicao">Dados da avaliação.</param>
    /// <param name="cancellationToken">Token de cancelamento para operações assíncronas.</param>
    /// <returns>Mídia com a avaliação atualizada.</returns>
    [HttpPost("{id:guid}/avaliacoes")]
    [ProducesResponseType(typeof(MidiaRespostaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MidiaRespostaDto>> Avaliar(
        Guid id,
        [FromBody] AvaliarMidiaRequisicaoDto requisicao,
        CancellationToken cancellationToken)
    {
        requisicao.IdMidia = id;
        var resultado = await _avaliarMidiaUseCase.ExecutarAsync(requisicao, cancellationToken);
        return Ok(resultado);
    }
}
