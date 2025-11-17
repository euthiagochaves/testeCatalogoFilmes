using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CatalogoDeMidia.Dominio.Entidades;
using CatalogoDeMidia.Dominio.Enums;

namespace CatalogoDeMidia.Dominio.Repositorios;

/// <summary>
/// Interface de repositório para operações de persistência da entidade Midia.
/// </summary>
public interface IMidiaRepositorio
{
    /// <summary>
    /// Adiciona uma nova mídia ao repositório.
    /// </summary>
    /// <param name="midia">A mídia a ser adicionada.</param>
    /// <param name="cancellationToken">Token de cancelamento da operação.</param>
    /// <returns>Uma tarefa representando a operação assíncrona.</returns>
    Task AdicionarAsync(Midia midia, CancellationToken cancellationToken = default);

    /// <summary>
    /// Atualiza uma mídia existente no repositório.
    /// </summary>
    /// <param name="midia">A mídia a ser atualizada.</param>
    /// <param name="cancellationToken">Token de cancelamento da operação.</param>
    /// <returns>Uma tarefa representando a operação assíncrona.</returns>
    Task AtualizarAsync(Midia midia, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove uma mídia do repositório.
    /// </summary>
    /// <param name="midia">A mídia a ser removida.</param>
    /// <param name="cancellationToken">Token de cancelamento da operação.</param>
    /// <returns>Uma tarefa representando a operação assíncrona.</returns>
    Task RemoverAsync(Midia midia, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém uma mídia pelo seu identificador único.
    /// </summary>
    /// <param name="id">O identificador único da mídia.</param>
    /// <param name="cancellationToken">Token de cancelamento da operação.</param>
    /// <returns>Uma tarefa contendo a mídia encontrada ou null se não existir.</returns>
    Task<Midia?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém mídias pelo título.
    /// </summary>
    /// <param name="titulo">O título da mídia a ser buscado.</param>
    /// <param name="cancellationToken">Token de cancelamento da operação.</param>
    /// <returns>Uma tarefa contendo uma lista de mídias com o título especificado.</returns>
    Task<IReadOnlyList<Midia>> ObterPorTituloAsync(string titulo, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lista mídias aplicando filtros opcionais.
    /// </summary>
    /// <param name="tipo">Filtra por tipo de mídia (opcional).</param>
    /// <param name="assistido">Filtra por mídias assistidas ou não (opcional).</param>
    /// <param name="genero">Filtra por gênero (opcional).</param>
    /// <param name="notaMinima">Filtra por nota mínima (opcional).</param>
    /// <param name="notaMaxima">Filtra por nota máxima (opcional).</param>
    /// <param name="cancellationToken">Token de cancelamento da operação.</param>
    /// <returns>Uma tarefa contendo uma lista de mídias que atendem aos filtros especificados.</returns>
    Task<IReadOnlyList<Midia>> ListarAsync(
        TipoMidia? tipo = null,
        bool? assistido = null,
        string? genero = null,
        decimal? notaMinima = null,
        decimal? notaMaxima = null,
        CancellationToken cancellationToken = default);
}
