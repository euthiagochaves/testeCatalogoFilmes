# CatalogoDeMidia.Dominio

## Propósito

Este projeto contém as entidades de domínio, regras de negócio essenciais e definições de interfaces de repositório para o Catálogo de Mídias.

## Estrutura

### Entidades
Contém as entidades de domínio do sistema:
- `Midia` - Entidade principal representando filmes e séries

### Enums
Define os tipos enumerados do domínio:
- `TipoMidia` - Filme ou Série
- `GeneroMidia` (opcional) - Gêneros como Terror, Comédia, Drama, Ação, etc.

### Repositorios
Interfaces de repositório que definem contratos de acesso a dados:
- `IMidiaRepositorio` - Interface para operações de persistência de mídias

## Dependências

Este projeto **não deve** depender de nenhum outro projeto da solução, apenas de bibliotecas básicas do .NET.

## TODO

- [ ] Implementar entidade `Midia` com propriedades e métodos de domínio
- [ ] Implementar enum `TipoMidia`
- [ ] Implementar interface `IMidiaRepositorio`
