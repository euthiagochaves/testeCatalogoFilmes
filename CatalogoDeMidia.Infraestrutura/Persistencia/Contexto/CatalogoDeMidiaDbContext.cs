using CatalogoDeMidia.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace CatalogoDeMidia.Infraestrutura.Persistencia.Contexto;

/// <summary>
/// Contexto do Entity Framework Core para o catálogo de mídias.
/// Responsável por mapear as entidades de domínio para o banco de dados PostgreSQL.
/// </summary>
public class CatalogoDeMidiaDbContext : DbContext
{
    /// <summary>
    /// Construtor que recebe as opções de configuração do contexto.
    /// </summary>
    /// <param name="options">Opções de configuração do DbContext.</param>
    public CatalogoDeMidiaDbContext(DbContextOptions<CatalogoDeMidiaDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// DbSet para a entidade Midia.
    /// </summary>
    public DbSet<Midia> Midias { get; set; } = null!;

    /// <summary>
    /// Configura o modelo de dados aplicando as configurações de mapeamento.
    /// </summary>
    /// <param name="modelBuilder">Construtor do modelo de dados.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplica todas as configurações de entidades do assembly atual
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogoDeMidiaDbContext).Assembly);
    }
}
