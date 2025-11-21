using CatalogoDeMidia.Aplicacao;
using CatalogoDeMidia.Infraestrutura.Persistencia.Configuracoes;

// ============================================================================
// Criação do builder da aplicação Web API (.NET 10)
// ============================================================================
// O builder é responsável por configurar todos os serviços e dependências
// da aplicação antes de construir o pipeline HTTP.
var builder = WebApplication.CreateBuilder(args);

// ============================================================================
// Registro de serviços da camada de Apresentação (API)
// ============================================================================

// Adicionar suporte a Controllers para os endpoints da API
builder.Services.AddControllers();

// Adicionar suporte a exploração de endpoints para documentação OpenAPI
builder.Services.AddEndpointsApiExplorer();

// Configurar o Swagger para documentação interativa da API
// O Swagger permite testar os endpoints diretamente no navegador
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Catálogo de Mídia API",
        Version = "v1",
        Description = "API para gerenciamento de catálogo de mídias (filmes e séries)."
    });
});

// ============================================================================
// Registro de serviços da camada de Infraestrutura
// ============================================================================
// A camada de Infraestrutura é responsável por:
// - Configurar o DbContext do Entity Framework Core para PostgreSQL (Supabase)
// - Registrar as implementações dos repositórios
// - A connection string é obtida da configuração (appsettings.json ou variáveis de ambiente)
//
// IMPORTANTE: A connection string "CatalogoDeMidia" deve apontar para o PostgreSQL
// remoto no Supabase. Para desenvolvimento local, configure em appsettings.Development.json.
// Para ambientes de produção/CI, use variáveis de ambiente ou GitHub Secrets.
builder.Services.AdicionarInfraestrutura(builder.Configuration);

// ============================================================================
// Registro de serviços da camada de Aplicação
// ============================================================================
// A camada de Aplicação contém os casos de uso (use cases) que implementam
// a lógica de negócio da aplicação. Este método registra todos os casos de uso:
// - IAdicionarMidiaUseCase
// - IListarMidiasUseCase
// - IAvaliarMidiaUseCase
builder.Services.AdicionarAplicacao();

// ============================================================================
// Construção da aplicação
// ============================================================================
var app = builder.Build();

// ============================================================================
// Configuração do pipeline de requisições HTTP
// ============================================================================
// O pipeline define a ordem em que os middlewares processam as requisições.

// Em ambiente de desenvolvimento, habilitar Swagger para documentação e testes
if (app.Environment.IsDevelopment())
{
    // Habilita o endpoint do Swagger JSON
    app.UseSwagger();
    
    // Habilita a interface do Swagger UI no endpoint /swagger
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Catálogo de Mídia API v1");
    });
}

// Middleware de redirecionamento HTTPS
// Redireciona requisições HTTP para HTTPS automaticamente
app.UseHttpsRedirection();

// Middleware de autorização
// Preparado para uso futuro de autenticação e autorização (JWT, OAuth, etc.)
app.UseAuthorization();

// Mapear os endpoints dos controllers para as rotas configuradas
app.MapControllers();

// ============================================================================
// Configurações futuras
// ============================================================================
// Quando necessário, adicionar aqui:
// - Políticas de CORS para permitir acesso de origens específicas
// - Configuração de autenticação (JWT, OAuth, etc.)
// - Configuração de rate limiting
// - Configuração de tratamento global de erros
// - Health checks para monitoramento

// ============================================================================
// Iniciar a aplicação
// ============================================================================
app.Run();
