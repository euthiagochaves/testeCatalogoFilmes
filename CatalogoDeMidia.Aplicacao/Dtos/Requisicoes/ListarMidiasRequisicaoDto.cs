using CatalogoDeMidia.Dominio.Enums;

namespace CatalogoDeMidia.Aplicacao.Dtos.Requisicoes;

/// <summary>
/// DTO de requisição para listar e filtrar mídias do catálogo.
/// </summary>
public class ListarMidiasRequisicaoDto
{
    /// <summary>
    /// Termo de busca para filtrar por título (busca parcial, opcional).
    /// </summary>
    public string? TermoBusca { get; set; }

    /// <summary>
    /// Filtra por tipo de mídia: Filme ou Série (opcional).
    /// </summary>
    public TipoMidia? Tipo { get; set; }

    /// <summary>
    /// Filtra por gênero da mídia (opcional).
    /// </summary>
    public GeneroMidia? Genero { get; set; }

    /// <summary>
    /// Filtra por ano de lançamento inicial (inclusivo, opcional).
    /// </summary>
    public int? AnoLancamentoInicial { get; set; }

    /// <summary>
    /// Filtra por ano de lançamento final (inclusivo, opcional).
    /// </summary>
    public int? AnoLancamentoFinal { get; set; }

    /// <summary>
    /// Filtra por nota mínima (inclusivo, opcional).
    /// </summary>
    public decimal? NotaMinima { get; set; }

    /// <summary>
    /// Filtra por nota máxima (inclusivo, opcional).
    /// </summary>
    public decimal? NotaMaxima { get; set; }

    /// <summary>
    /// Filtra por mídias assistidas ou não assistidas (opcional).
    /// </summary>
    public bool? Assistido { get; set; }

    /// <summary>
    /// Número da página para paginação (padrão: 1).
    /// </summary>
    public int Pagina { get; set; } = 1;

    /// <summary>
    /// Tamanho da página para paginação (padrão: 10).
    /// </summary>
    public int TamanhoPagina { get; set; } = 10;

    /// <summary>
    /// Campo pelo qual os resultados devem ser ordenados (opcional).
    /// Valores possíveis: "Titulo", "AnoLancamento", "Nota", "DataCriacao", "DataAtualizacao".
    /// </summary>
    public string? OrdenarPor { get; set; }

    /// <summary>
    /// Define se a ordenação deve ser ascendente (true) ou descendente (false).
    /// Padrão: true (ascendente).
    /// </summary>
    public bool OrdenacaoAscendente { get; set; } = true;
}
