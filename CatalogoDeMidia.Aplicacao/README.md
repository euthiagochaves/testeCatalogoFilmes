# CatalogoDeMidia.Aplicacao

## Propósito

Este projeto contém os casos de uso (application services) que coordenam as operações entre o domínio e a infraestrutura, além de definir DTOs para entrada e saída de dados.

## Estrutura

### CasosDeUso
Implementações dos casos de uso do sistema:
- `AdicionarMidia` - Adicionar nova mídia ao catálogo
- `ListarMidias` - Listar mídias com filtros
- `AvaliarMidia` - Atualizar nota de uma mídia

### Dtos
Data Transfer Objects para comunicação com camadas superiores:
- `Requisicoes` - DTOs de entrada para os casos de uso
- `Respostas` - DTOs de saída dos casos de uso

### Interfaces
Interfaces dos casos de uso para inversão de dependência:
- `IAdicionarMidiaUseCase`
- `IListarMidiasUseCase`
- `IAvaliarMidiaUseCase`

## Dependências

Este projeto depende apenas de:
- `CatalogoDeMidia.Dominio`

## TODO

- [ ] Implementar interfaces dos casos de uso
- [ ] Implementar DTOs de requisição e resposta
- [ ] Implementar casos de uso
