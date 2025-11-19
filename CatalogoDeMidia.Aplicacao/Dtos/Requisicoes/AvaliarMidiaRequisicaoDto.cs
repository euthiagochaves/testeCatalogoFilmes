namespace CatalogoDeMidia.Aplicacao.Dtos.Requisicoes;

/// <summary>
/// DTO de requisição para avaliar uma mídia do catálogo.
/// </summary>
public class AvaliarMidiaRequisicaoDto
{
    /// <summary>
    /// Identificador único da mídia a ser avaliada (opcional).
    /// Deve ser fornecido IdMidia ou Titulo para identificar a mídia.
    /// </summary>
    public Guid? IdMidia { get; set; }

    /// <summary>
    /// Título da mídia a ser avaliada (opcional).
    /// Usado quando IdMidia não é fornecido. Pode retornar múltiplas mídias se houver títulos duplicados.
    /// </summary>
    public string? Titulo { get; set; }

    /// <summary>
    /// Nova nota a ser atribuída à mídia.
    /// Deve estar entre 0 e 10.
    /// </summary>
    public decimal NovaNota { get; set; }
}
