using CatalogoDeMidia.Dominio.Enums;

namespace CatalogoDeMidia.Dominio.Entidades;

/// <summary>
/// Entidade de domínio que representa uma mídia (filme ou série) no catálogo.
/// </summary>
public class Midia
{
    /// <summary>
    /// Identificador único da mídia.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Título da mídia.
    /// </summary>
    public string Titulo { get; private set; }

    /// <summary>
    /// Ano de lançamento da mídia.
    /// </summary>
    public int AnoLancamento { get; private set; }

    /// <summary>
    /// Tipo da mídia (Filme ou Série).
    /// </summary>
    public TipoMidia Tipo { get; private set; }

    /// <summary>
    /// Gênero da mídia.
    /// </summary>
    public string? Genero { get; private set; }

    /// <summary>
    /// Nota atribuída à mídia.
    /// </summary>
    public decimal? Nota { get; private set; }

    /// <summary>
    /// Indica se a mídia já foi assistida.
    /// </summary>
    public bool Assistido { get; private set; }

    /// <summary>
    /// Data e hora de criação do registro.
    /// </summary>
    public DateTimeOffset DataCriacao { get; private set; }

    /// <summary>
    /// Data e hora da última atualização do registro.
    /// </summary>
    public DateTimeOffset DataAtualizacao { get; private set; }

    /// <summary>
    /// Construtor protegido para uso de ORMs.
    /// </summary>
    protected Midia()
    {
        Titulo = string.Empty;
    }

    /// <summary>
    /// Cria uma nova instância de Midia com os dados básicos obrigatórios.
    /// </summary>
    /// <param name="titulo">Título da mídia.</param>
    /// <param name="anoLancamento">Ano de lançamento da mídia.</param>
    /// <param name="tipo">Tipo da mídia (Filme ou Série).</param>
    /// <param name="genero">Gênero da mídia (opcional).</param>
    /// <param name="nota">Nota inicial da mídia (opcional).</param>
    /// <param name="assistido">Indica se a mídia já foi assistida (padrão: false).</param>
    /// <exception cref="ArgumentException">Lançada quando os parâmetros obrigatórios são inválidos.</exception>
    public Midia(
        string titulo,
        int anoLancamento,
        TipoMidia tipo,
        string? genero = null,
        decimal? nota = null,
        bool assistido = false)
    {
        ValidarTitulo(titulo);
        ValidarAnoLancamento(anoLancamento);
        ValidarNota(nota);

        Id = Guid.NewGuid();
        Titulo = titulo;
        AnoLancamento = anoLancamento;
        Tipo = tipo;
        Genero = genero;
        Nota = nota;
        Assistido = assistido;
        DataCriacao = DateTimeOffset.UtcNow;
        DataAtualizacao = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Define ou atualiza a nota da mídia.
    /// </summary>
    /// <param name="novaNota">Nova nota a ser atribuída.</param>
    /// <exception cref="ArgumentException">Lançada quando a nota é inválida.</exception>
    public void DefinirNota(decimal novaNota)
    {
        ValidarNota(novaNota);
        Nota = novaNota;
        DataAtualizacao = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Marca a mídia como assistida.
    /// </summary>
    public void MarcarComoAssistido()
    {
        Assistido = true;
        DataAtualizacao = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Atualiza os dados básicos da mídia.
    /// </summary>
    /// <param name="novoTitulo">Novo título da mídia.</param>
    /// <param name="novoAno">Novo ano de lançamento.</param>
    /// <param name="novoTipo">Novo tipo da mídia.</param>
    /// <param name="novoGenero">Novo gênero da mídia (opcional).</param>
    /// <exception cref="ArgumentException">Lançada quando os parâmetros são inválidos.</exception>
    public void AtualizarDadosBasicos(
        string novoTitulo,
        int novoAno,
        TipoMidia novoTipo,
        string? novoGenero = null)
    {
        ValidarTitulo(novoTitulo);
        ValidarAnoLancamento(novoAno);

        Titulo = novoTitulo;
        AnoLancamento = novoAno;
        Tipo = novoTipo;
        Genero = novoGenero;
        DataAtualizacao = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Valida o título da mídia.
    /// </summary>
    /// <param name="titulo">Título a ser validado.</param>
    /// <exception cref="ArgumentException">Lançada quando o título é nulo ou vazio.</exception>
    private static void ValidarTitulo(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
        {
            throw new ArgumentException("O título da mídia não pode ser nulo ou vazio.", nameof(titulo));
        }
    }

    /// <summary>
    /// Valida o ano de lançamento da mídia.
    /// </summary>
    /// <param name="anoLancamento">Ano de lançamento a ser validado.</param>
    /// <exception cref="ArgumentException">Lançada quando o ano de lançamento é inválido.</exception>
    private static void ValidarAnoLancamento(int anoLancamento)
    {
        const int anoMinimo = 1888; // Primeiro filme da história
        var anoAtual = DateTime.UtcNow.Year;

        if (anoLancamento < anoMinimo || anoLancamento > anoAtual + 5)
        {
            throw new ArgumentException(
                $"O ano de lançamento deve estar entre {anoMinimo} e {anoAtual + 5}.",
                nameof(anoLancamento));
        }
    }

    /// <summary>
    /// Valida a nota da mídia.
    /// </summary>
    /// <param name="nota">Nota a ser validada.</param>
    /// <exception cref="ArgumentException">Lançada quando a nota é inválida.</exception>
    private static void ValidarNota(decimal? nota)
    {
        if (nota.HasValue && (nota.Value < 0 || nota.Value > 10))
        {
            throw new ArgumentException("A nota deve estar entre 0 e 10.", nameof(nota));
        }
    }
}
