# CatalogoDeMidia.Infraestrutura

## Propósito

Este projeto contém a implementação da camada de infraestrutura, incluindo acesso a dados com Entity Framework Core e SQLite.

## Estrutura

### Persistencia/Contexto
- `CatalogoDeMidiaDbContext` - Contexto do Entity Framework Core

### Persistencia/Repositorios
Implementações concretas das interfaces de repositório:
- `MidiaRepositorio` - Implementação de `IMidiaRepositorio`

### Persistencia/Configuracoes
Configurações de entidades do EF Core (Fluent API)

## Dependências

Este projeto depende de:
- `CatalogoDeMidia.Dominio`
- Entity Framework Core
- SQLite

## TODO

- [ ] Implementar `CatalogoDeMidiaDbContext`
- [ ] Implementar `MidiaRepositorio`
- [ ] Criar configurações de entidades
- [ ] Criar método de extensão para registro de DI
