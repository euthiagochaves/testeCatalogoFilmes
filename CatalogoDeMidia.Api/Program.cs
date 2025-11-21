using CatalogoDeMidia.Aplicacao;
using CatalogoDeMidia.Infraestrutura.Persistencia.Configuracoes;

var builder = WebApplication.CreateBuilder(args);

// Adicionar controllers ao container
builder.Services.AddControllers();

// Configurar OpenAPI/Swagger para documentação da API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Catálogo de Mídia API",
        Version = "v1",
        Description = "API para gerenciamento de catálogo de mídias (filmes e séries)."
    });
});

// Registrar serviços da camada de Infraestrutura (DbContext, repositórios)
builder.Services.AdicionarInfraestrutura(builder.Configuration);

// Registrar serviços da camada de Aplicação (casos de uso)
builder.Services.AdicionarAplicacao();

var app = builder.Build();

// Configurar o pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Catálogo de Mídia API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
