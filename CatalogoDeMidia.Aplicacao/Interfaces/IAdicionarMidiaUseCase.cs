using System.Threading;
using System.Threading.Tasks;
using CatalogoDeMidia.Aplicacao.Dtos.Requisicoes;
using CatalogoDeMidia.Aplicacao.Dtos.Respostas;

namespace CatalogoDeMidia.Aplicacao.Interfaces
{
    /// <summary>
    /// Interface para o caso de uso de adição de uma nova mídia ao catálogo.
    /// </summary>
    public interface IAdicionarMidiaUseCase
    {
        /// <summary>
        /// Executa o fluxo de criação de uma nova mídia no catálogo.
        /// </summary>
        /// <param name="requisicao">DTO contendo os dados necessários para criação da mídia.</param>
        /// <param name="cancellationToken">Token de cancelamento para operações assíncronas.</param>
        /// <returns>DTO representando a mídia recém-criada.</returns>
        Task<MidiaRespostaDto> ExecutarAsync(
            AdicionarMidiaRequisicaoDto requisicao,
            CancellationToken cancellationToken = default);
    }
}