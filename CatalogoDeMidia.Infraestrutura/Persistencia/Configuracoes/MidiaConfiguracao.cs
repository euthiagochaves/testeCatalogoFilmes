using CatalogoDeMidia.Dominio.Entidades;
using CatalogoDeMidia.Dominio.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogoDeMidia.Infraestrutura.Persistencia.Configuracoes;

/// <summary>
/// Configuração de mapeamento para a entidade Midia usando Fluent API do Entity Framework Core.
/// </summary>
public class MidiaConfiguracao : IEntityTypeConfiguration<Midia>
{
    /// <summary>
    /// Configura o mapeamento da entidade Midia para o banco de dados PostgreSQL.
    /// </summary>
    /// <param name="builder">Construtor de configuração da entidade.</param>
    public void Configure(EntityTypeBuilder<Midia> builder)
    {
        // Nome da tabela
        builder.ToTable("midias");

        // Chave primária
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id)
            .HasColumnName("id")
            .HasColumnType("uuid")
            .IsRequired();

        // Título
        builder.Property(m => m.Titulo)
            .HasColumnName("titulo")
            .HasColumnType("varchar(500)")
            .IsRequired();

        // Ano de lançamento
        builder.Property(m => m.AnoLancamento)
            .HasColumnName("ano_lancamento")
            .HasColumnType("integer")
            .IsRequired();

        // Tipo da mídia (enum armazenado como inteiro)
        builder.Property(m => m.Tipo)
            .HasColumnName("tipo")
            .HasColumnType("integer")
            .HasConversion<int>()
            .IsRequired();

        // Gênero (string opcional)
        builder.Property(m => m.Genero)
            .HasColumnName("genero")
            .HasColumnType("varchar(100)")
            .IsRequired(false);

        // Nota (decimal com precisão)
        builder.Property(m => m.Nota)
            .HasColumnName("nota")
            .HasColumnType("numeric(3,1)")
            .IsRequired(false);

        // Assistido (booleano)
        builder.Property(m => m.Assistido)
            .HasColumnName("assistido")
            .HasColumnType("boolean")
            .IsRequired()
            .HasDefaultValue(false);

        // Data de criação (auditoria)
        builder.Property(m => m.DataCriacao)
            .HasColumnName("data_criacao")
            .HasColumnType("timestamptz")
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        // Data de atualização (auditoria)
        builder.Property(m => m.DataAtualizacao)
            .HasColumnName("data_atualizacao")
            .HasColumnType("timestamptz")
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        // Índices para otimização de consultas
        builder.HasIndex(m => m.Titulo)
            .HasDatabaseName("ix_midias_titulo");

        builder.HasIndex(m => m.Tipo)
            .HasDatabaseName("ix_midias_tipo");

        builder.HasIndex(m => m.Assistido)
            .HasDatabaseName("ix_midias_assistido");
    }
}
