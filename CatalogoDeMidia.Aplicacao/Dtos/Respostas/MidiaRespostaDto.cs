using CatalogoDeMidia.Dominio.Enums;

namespace CatalogoDeMidia.Aplicacao.Dtos.Respostas;

/// <summary>
/// DTO de resposta representando uma mídia do catálogo.
/// Utilizado para retornar dados de mídias em listagens, consultas detalhadas,
/// e respostas de operações de criação/atualização.
/// </summary>
public class MidiaRespostaDto
{
    /// <summary>
    /// Identificador único da mídia.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Título da mídia.
    /// </summary>
    public string Titulo { get; set; } = string.Empty;

    /// <summary>
    /// Ano de lançamento da mídia.
    /// </summary>
    public int AnoLancamento { get; set; }

    /// <summary>
    /// Tipo da mídia (Filme ou Série).
    /// </summary>
    public TipoMidia Tipo { get; set; }

    /// <summary>
    /// Gênero da mídia.
    /// </summary>
    public string? Genero { get; set; }

    /// <summary>
    /// Nota atribuída à mídia (de 0 a 10).
    /// </summary>
    public decimal? Nota { get; set; }

    /// <summary>
    /// Indica se a mídia já foi assistida.
    /// </summary>
    public bool Assistido { get; set; }

    /// <summary>
    /// Data e hora de criação do registro.
    /// </summary>
    public DateTimeOffset DataCriacao { get; set; }

    /// <summary>
    /// Data e hora da última atualização do registro.
    /// </summary>
    public DateTimeOffset DataAtualizacao { get; set; }
}
