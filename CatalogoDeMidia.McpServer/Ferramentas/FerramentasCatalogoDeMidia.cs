using CatalogoDeMidia.Aplicacao.Dtos.Requisicoes;
using CatalogoDeMidia.Aplicacao.Dtos.Respostas;
using CatalogoDeMidia.Aplicacao.Interfaces;
using CatalogoDeMidia.Dominio.Enums;
using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Server;

namespace CatalogoDeMidia.McpServer.Ferramentas;

/// <summary>
/// Classe que expõe as ferramentas (tools) MCP para o catálogo de mídias.
/// Cada método público decorado com [McpServerTool] se torna uma tool acessível via MCP.
/// </summary>
[McpServerToolType]
public class FerramentasCatalogoDeMidia
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Construtor que recebe o service provider para criar scopes.
    /// </summary>
    public FerramentasCatalogoDeMidia(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Tool MCP para adicionar uma nova mídia ao catálogo.
    /// </summary>
    /// <param name="titulo">Título da mídia (obrigatório).</param>
    /// <param name="ano_lancamento">Ano de lançamento (obrigatório).</param>
    /// <param name="tipo">Tipo da mídia: "Filme" ou "Serie" (obrigatório).</param>
    /// <param name="genero">Gênero da mídia (opcional).</param>
    /// <param name="nota">Nota inicial de 0 a 10 (opcional).</param>
    /// <param name="assistido">Se já foi assistida (opcional, padrão: false).</param>
    /// <returns>Dados da mídia criada.</returns>
    [McpServerTool]
    public async Task<MidiaRespostaDto> AdicionarMidiaAsync(
        string titulo,
        int ano_lancamento,
        string tipo,
        string? genero = null,
        decimal? nota = null,
        bool assistido = false,
        CancellationToken cancellationToken = default)
    {
        // Converter string do tipo para enum
        if (!Enum.TryParse<TipoMidia>(tipo, ignoreCase: true, out var tipoMidia))
        {
            throw new ArgumentException(
                $"Tipo inválido: '{tipo}'. Use 'Filme' ou 'Serie'.",
                nameof(tipo));
        }

        using var scope = _serviceProvider.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IAdicionarMidiaUseCase>();

        var requisicao = new AdicionarMidiaRequisicaoDto
        {
            Titulo = titulo,
            AnoLancamento = ano_lancamento,
            Tipo = tipoMidia,
            Genero = genero,
            Nota = nota,
            Assistido = assistido
        };

        return await useCase.ExecutarAsync(requisicao, cancellationToken);
    }

    /// <summary>
    /// Tool MCP para listar mídias do catálogo com filtros opcionais.
    /// </summary>
    /// <param name="tipo">Filtrar por tipo: "Filme" ou "Serie" (opcional).</param>
    /// <param name="assistido">Filtrar por status de assistido (opcional).</param>
    /// <param name="genero">Filtrar por gênero (opcional).</param>
    /// <param name="nota_minima">Nota mínima para filtro (opcional).</param>
    /// <param name="nota_maxima">Nota máxima para filtro (opcional).</param>
    /// <returns>Lista de mídias que atendem aos filtros.</returns>
    [McpServerTool]
    public async Task<IReadOnlyList<MidiaRespostaDto>> ListarMidiasAsync(
        string? tipo = null,
        bool? assistido = null,
        string? genero = null,
        decimal? nota_minima = null,
        decimal? nota_maxima = null,
        CancellationToken cancellationToken = default)
    {
        TipoMidia? tipoMidia = null;
        
        if (!string.IsNullOrWhiteSpace(tipo))
        {
            if (!Enum.TryParse<TipoMidia>(tipo, ignoreCase: true, out var tipoEnum))
            {
                throw new ArgumentException(
                    $"Tipo inválido: '{tipo}'. Use 'Filme' ou 'Serie'.",
                    nameof(tipo));
            }
            tipoMidia = tipoEnum;
        }

        GeneroMidia? generoMidia = null;
        
        if (!string.IsNullOrWhiteSpace(genero))
        {
            if (!Enum.TryParse<GeneroMidia>(genero, ignoreCase: true, out var generoEnum))
            {
                throw new ArgumentException(
                    $"Gênero inválido: '{genero}'. Use um dos valores: Acao, Aventura, Comedia, Drama, Suspense, Terror, Romance, FiccaoCientifica, Animacao, Documentario, Outro.",
                    nameof(genero));
            }
            generoMidia = generoEnum;
        }

        using var scope = _serviceProvider.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IListarMidiasUseCase>();

        var filtros = new ListarMidiasRequisicaoDto
        {
            Tipo = tipoMidia,
            Assistido = assistido,
            Genero = generoMidia,
            NotaMinima = nota_minima,
            NotaMaxima = nota_maxima
        };

        return await useCase.ExecutarAsync(filtros, cancellationToken);
    }

    /// <summary>
    /// Tool MCP para avaliar (definir nota de) uma mídia existente.
    /// </summary>
    /// <param name="nova_nota">Nova nota a ser atribuída (obrigatório, 0 a 10).</param>
    /// <param name="id_midia">ID da mídia (opcional, se não informado usa título).</param>
    /// <param name="titulo">Título da mídia (opcional, se não informado usa ID).</param>
    /// <returns>Dados atualizados da mídia avaliada.</returns>
    /// <exception cref="ArgumentException">
    /// Lançada quando nem ID nem título são fornecidos.
    /// </exception>
    [McpServerTool]
    public async Task<MidiaRespostaDto> AvaliarMidiaAsync(
        decimal nova_nota,
        Guid? id_midia = null,
        string? titulo = null,
        CancellationToken cancellationToken = default)
    {
        if (id_midia == null && string.IsNullOrWhiteSpace(titulo))
        {
            throw new ArgumentException(
                "É necessário fornecer 'id_midia' ou 'titulo' para identificar a mídia.");
        }

        using var scope = _serviceProvider.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IAvaliarMidiaUseCase>();

        var requisicao = new AvaliarMidiaRequisicaoDto
        {
            IdMidia = id_midia,
            Titulo = titulo,
            NovaNota = nova_nota
        };

        return await useCase.ExecutarAsync(requisicao, cancellationToken);
    }
}
