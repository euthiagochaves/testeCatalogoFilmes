using CatalogoDeMidia.Dominio.Enums;

namespace CatalogoDeMidia.Aplicacao.Dtos.Requisicoes;

/// <summary>
/// DTO de requisição para adicionar uma nova mídia ao catálogo.
/// </summary>
public class AdicionarMidiaRequisicaoDto
{
    /// <summary>
    /// Título da mídia (obrigatório).
    /// </summary>
    public string Titulo { get; set; } = string.Empty;

    /// <summary>
    /// Ano de lançamento da mídia (obrigatório).
    /// </summary>
    public int AnoLancamento { get; set; }

    /// <summary>
    /// Tipo da mídia: Filme ou Série (obrigatório).
    /// </summary>
    public TipoMidia Tipo { get; set; }

    /// <summary>
    /// Gênero da mídia (opcional).
    /// </summary>
    public string? Genero { get; set; }

    /// <summary>
    /// Nota inicial da mídia, de 0 a 10 (opcional).
    /// </summary>
    public decimal? Nota { get; set; }

    /// <summary>
    /// Indica se a mídia já foi assistida (opcional, padrão: false).
    /// </summary>
    public bool? Assistido { get; set; }
}
