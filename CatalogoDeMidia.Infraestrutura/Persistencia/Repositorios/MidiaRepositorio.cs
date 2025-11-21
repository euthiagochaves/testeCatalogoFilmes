using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CatalogoDeMidia.Dominio.Entidades;
using CatalogoDeMidia.Dominio.Enums;
using CatalogoDeMidia.Dominio.Repositorios;
using CatalogoDeMidia.Infraestrutura.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;

namespace CatalogoDeMidia.Infraestrutura.Persistencia.Repositorios;

/// <summary>
/// Implementação do repositório para operações de persistência da entidade Midia.
/// </summary>
public class MidiaRepositorio : IMidiaRepositorio
{
    private readonly CatalogoDeMidiaDbContext _dbContext;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="MidiaRepositorio"/>.
    /// </summary>
    /// <param name="dbContext">Contexto do banco de dados.</param>
    public MidiaRepositorio(CatalogoDeMidiaDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    /// <summary>
    /// Adiciona uma nova mídia ao repositório.
    /// </summary>
    /// <param name="midia">A mídia a ser adicionada.</param>
    /// <param name="cancellationToken">Token de cancelamento da operação.</param>
    /// <returns>Uma tarefa representando a operação assíncrona.</returns>
    public async Task AdicionarAsync(Midia midia, CancellationToken cancellationToken = default)
    {
        if (midia == null)
        {
            throw new ArgumentNullException(nameof(midia));
        }

        await _dbContext.Midias.AddAsync(midia, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Atualiza uma mídia existente no repositório.
    /// </summary>
    /// <param name="midia">A mídia a ser atualizada.</param>
    /// <param name="cancellationToken">Token de cancelamento da operação.</param>
    /// <returns>Uma tarefa representando a operação assíncrona.</returns>
    public async Task AtualizarAsync(Midia midia, CancellationToken cancellationToken = default)
    {
        if (midia == null)
        {
            throw new ArgumentNullException(nameof(midia));
        }

        _dbContext.Midias.Update(midia);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Remove uma mídia do repositório.
    /// </summary>
    /// <param name="midia">A mídia a ser removida.</param>
    /// <param name="cancellationToken">Token de cancelamento da operação.</param>
    /// <returns>Uma tarefa representando a operação assíncrona.</returns>
    public async Task RemoverAsync(Midia midia, CancellationToken cancellationToken = default)
    {
        if (midia == null)
        {
            throw new ArgumentNullException(nameof(midia));
        }

        _dbContext.Midias.Remove(midia);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Obtém uma mídia pelo seu identificador único.
    /// </summary>
    /// <param name="id">O identificador único da mídia.</param>
    /// <param name="cancellationToken">Token de cancelamento da operação.</param>
    /// <returns>Uma tarefa contendo a mídia encontrada ou null se não existir.</returns>
    public async Task<Midia?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Midias
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    /// <summary>
    /// Obtém mídias pelo título.
    /// </summary>
    /// <param name="titulo">O título da mídia a ser buscado.</param>
    /// <param name="cancellationToken">Token de cancelamento da operação.</param>
    /// <returns>Uma tarefa contendo uma lista de mídias com o título especificado.</returns>
    public async Task<IReadOnlyList<Midia>> ObterPorTituloAsync(string titulo, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(titulo))
        {
            throw new ArgumentException("O título não pode ser nulo ou vazio.", nameof(titulo));
        }

        var midias = await _dbContext.Midias
            .AsNoTracking()
            .Where(m => m.Titulo == titulo)
            .ToListAsync(cancellationToken);

        return midias.AsReadOnly();
    }

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
    public async Task<IReadOnlyList<Midia>> ListarAsync(
        TipoMidia? tipo = null,
        bool? assistido = null,
        string? genero = null,
        decimal? notaMinima = null,
        decimal? notaMaxima = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Midias.AsNoTracking();

        if (tipo.HasValue)
        {
            query = query.Where(m => m.Tipo == tipo.Value);
        }

        if (assistido.HasValue)
        {
            query = query.Where(m => m.Assistido == assistido.Value);
        }

        if (!string.IsNullOrWhiteSpace(genero))
        {
            query = query.Where(m => m.Genero == genero);
        }

        if (notaMinima.HasValue)
        {
            query = query.Where(m => m.Nota.HasValue && m.Nota.Value >= notaMinima.Value);
        }

        if (notaMaxima.HasValue)
        {
            query = query.Where(m => m.Nota.HasValue && m.Nota.Value <= notaMaxima.Value);
        }

        var midias = await query.ToListAsync(cancellationToken);

        return midias.AsReadOnly();
    }
}
