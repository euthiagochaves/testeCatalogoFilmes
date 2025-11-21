using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CatalogoDeMidia.Aplicacao.Dtos.Requisicoes;
using CatalogoDeMidia.Aplicacao.Dtos.Respostas;
using CatalogoDeMidia.Aplicacao.Interfaces;
using CatalogoDeMidia.Dominio.Entidades;
using CatalogoDeMidia.Dominio.Repositorios;

namespace CatalogoDeMidia.Aplicacao.CasosDeUso.ListarMidias;

/// <summary>
/// Implementação do caso de uso para listar e filtrar mídias do catálogo.
/// </summary>
public class ListarMidiasUseCase : IListarMidiasUseCase
{
    private readonly IMidiaRepositorio _midiaRepositorio;
    private const int TamanhoPaginaMinimo = 1;
    private const int TamanhoPaginaMaximo = 100;
    private const int TamanhoPaginaPadrao = 10;
    private const int PaginaPadrao = 1;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="ListarMidiasUseCase"/>.
    /// </summary>
    /// <param name="midiaRepositorio">Repositório de mídias.</param>
    /// <exception cref="ArgumentNullException">Lançada quando o repositório é nulo.</exception>
    public ListarMidiasUseCase(IMidiaRepositorio midiaRepositorio)
    {
        _midiaRepositorio = midiaRepositorio ?? throw new ArgumentNullException(nameof(midiaRepositorio));
    }

    /// <summary>
    /// Executa a listagem de mídias aplicando filtros, paginação e ordenação conforme especificado.
    /// </summary>
    /// <param name="filtros">DTO contendo os critérios de filtro, paginação e ordenação.</param>
    /// <param name="cancellationToken">Token de cancelamento para operações assíncronas.</param>
    /// <returns>Lista somente leitura de mídias que atendem aos critérios especificados.</returns>
    /// <exception cref="ArgumentNullException">Lançada quando os filtros são nulos.</exception>
    public async Task<IReadOnlyList<MidiaRespostaDto>> ExecutarAsync(
        ListarMidiasRequisicaoDto filtros,
        CancellationToken cancellationToken = default)
    {
        ValidarENormalizarFiltros(filtros);

        // Converter GeneroMidia enum para string se necessário
        string? generoString = filtros.Genero?.ToString();

        // Buscar mídias com filtros suportados pelo repositório
        var midias = await _midiaRepositorio.ListarAsync(
            tipo: filtros.Tipo,
            assistido: filtros.Assistido,
            genero: generoString,
            notaMinima: filtros.NotaMinima,
            notaMaxima: filtros.NotaMaxima,
            cancellationToken: cancellationToken);

        // Aplicar filtros adicionais em memória
        var midiasFiltradas = AplicarFiltrosAdicionais(midias, filtros);

        // Aplicar ordenação
        var midiasOrdenadas = AplicarOrdenacao(midiasFiltradas, filtros);

        // Aplicar paginação
        var midiasPaginadas = AplicarPaginacao(midiasOrdenadas, filtros);

        // Mapear para DTOs de resposta
        return midiasPaginadas.Select(MapearParaResposta).ToList();
    }

    /// <summary>
    /// Valida e normaliza os parâmetros de filtro e paginação.
    /// </summary>
    /// <param name="filtros">Filtros a serem validados e normalizados.</param>
    /// <exception cref="ArgumentNullException">Lançada quando os filtros são nulos.</exception>
    private static void ValidarENormalizarFiltros(ListarMidiasRequisicaoDto filtros)
    {
        if (filtros is null)
        {
            throw new ArgumentNullException(nameof(filtros), "Os filtros não podem ser nulos.");
        }

        // Normalizar página
        if (filtros.Pagina < PaginaPadrao)
        {
            filtros.Pagina = PaginaPadrao;
        }

        // Normalizar tamanho da página
        if (filtros.TamanhoPagina < TamanhoPaginaMinimo)
        {
            filtros.TamanhoPagina = TamanhoPaginaPadrao;
        }
        else if (filtros.TamanhoPagina > TamanhoPaginaMaximo)
        {
            filtros.TamanhoPagina = TamanhoPaginaMaximo;
        }
    }

    /// <summary>
    /// Aplica filtros adicionais que não são suportados pelo repositório.
    /// </summary>
    /// <param name="midias">Lista de mídias a serem filtradas.</param>
    /// <param name="filtros">Filtros a serem aplicados.</param>
    /// <returns>Lista de mídias após aplicação dos filtros.</returns>
    private static IEnumerable<Midia> AplicarFiltrosAdicionais(
        IReadOnlyList<Midia> midias,
        ListarMidiasRequisicaoDto filtros)
    {
        var resultado = midias.AsEnumerable();

        // Filtro por termo de busca no título
        if (!string.IsNullOrWhiteSpace(filtros.TermoBusca))
        {
            var termoBuscaNormalizado = filtros.TermoBusca.Trim().ToLowerInvariant();
            resultado = resultado.Where(m => 
                m.Titulo.ToLowerInvariant().Contains(termoBuscaNormalizado));
        }

        // Filtro por intervalo de ano de lançamento
        if (filtros.AnoLancamentoInicial.HasValue)
        {
            resultado = resultado.Where(m => m.AnoLancamento >= filtros.AnoLancamentoInicial.Value);
        }

        if (filtros.AnoLancamentoFinal.HasValue)
        {
            resultado = resultado.Where(m => m.AnoLancamento <= filtros.AnoLancamentoFinal.Value);
        }

        return resultado;
    }

    /// <summary>
    /// Aplica ordenação às mídias conforme especificado nos filtros.
    /// </summary>
    /// <param name="midias">Mídias a serem ordenadas.</param>
    /// <param name="filtros">Filtros contendo critérios de ordenação.</param>
    /// <returns>Lista ordenada de mídias.</returns>
    private static IEnumerable<Midia> AplicarOrdenacao(
        IEnumerable<Midia> midias,
        ListarMidiasRequisicaoDto filtros)
    {
        if (string.IsNullOrWhiteSpace(filtros.OrdenarPor))
        {
            // Ordenação padrão por data de criação descendente
            return filtros.OrdenacaoAscendente
                ? midias.OrderBy(m => m.DataCriacao)
                : midias.OrderByDescending(m => m.DataCriacao);
        }

        var campoOrdenacao = filtros.OrdenarPor.Trim();

        return campoOrdenacao.ToLowerInvariant() switch
        {
            "titulo" => filtros.OrdenacaoAscendente
                ? midias.OrderBy(m => m.Titulo)
                : midias.OrderByDescending(m => m.Titulo),

            "anolancamento" => filtros.OrdenacaoAscendente
                ? midias.OrderBy(m => m.AnoLancamento)
                : midias.OrderByDescending(m => m.AnoLancamento),

            "nota" => filtros.OrdenacaoAscendente
                ? midias.OrderBy(m => m.Nota ?? 0) // Notas nulas são tratadas como 0
                : midias.OrderByDescending(m => m.Nota ?? 0), // Notas nulas são tratadas como 0

            "datacriacao" => filtros.OrdenacaoAscendente
                ? midias.OrderBy(m => m.DataCriacao)
                : midias.OrderByDescending(m => m.DataCriacao),

            "dataatualizacao" => filtros.OrdenacaoAscendente
                ? midias.OrderBy(m => m.DataAtualizacao)
                : midias.OrderByDescending(m => m.DataAtualizacao),

            _ => filtros.OrdenacaoAscendente
                ? midias.OrderBy(m => m.DataCriacao)
                : midias.OrderByDescending(m => m.DataCriacao)
        };
    }

    /// <summary>
    /// Aplica paginação às mídias conforme especificado nos filtros.
    /// </summary>
    /// <param name="midias">Mídias a serem paginadas.</param>
    /// <param name="filtros">Filtros contendo parâmetros de paginação.</param>
    /// <returns>Lista paginada de mídias.</returns>
    private static IEnumerable<Midia> AplicarPaginacao(
        IEnumerable<Midia> midias,
        ListarMidiasRequisicaoDto filtros)
    {
        var skip = (filtros.Pagina - 1) * filtros.TamanhoPagina;
        return midias.Skip(skip).Take(filtros.TamanhoPagina);
    }

    /// <summary>
    /// Mapeia a entidade de domínio Midia para o DTO de resposta.
    /// </summary>
    /// <param name="midia">Entidade de mídia a ser mapeada.</param>
    /// <returns>DTO de resposta com os dados da mídia.</returns>
    private static MidiaRespostaDto MapearParaResposta(Midia midia)
    {
        return new MidiaRespostaDto
        {
            Id = midia.Id,
            Titulo = midia.Titulo,
            AnoLancamento = midia.AnoLancamento,
            Tipo = midia.Tipo,
            Genero = midia.Genero,
            Nota = midia.Nota,
            Assistido = midia.Assistido,
            DataCriacao = midia.DataCriacao,
            DataAtualizacao = midia.DataAtualizacao
        };
    }
}
