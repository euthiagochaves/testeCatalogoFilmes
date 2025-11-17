var builder = WebApplication.CreateBuilder(args);

// TODO: Registrar serviços da camada de Aplicação (casos de uso)
// Exemplo: builder.Services.AddScoped<IAdicionarMidiaUseCase, AdicionarMidiaUseCase>();
// Exemplo: builder.Services.AddScoped<IListarMidiasUseCase, ListarMidiasUseCase>();
// Exemplo: builder.Services.AddScoped<IAvaliarMidiaUseCase, AvaliarMidiaUseCase>();

// TODO: Registrar serviços da camada de Infraestrutura (DbContext, repositórios)
// Exemplo: builder.Services.AdicionarInfraestrutura(connectionString);
// Connection string para SQLite: "Data Source=catalogodemidia.db"

// Adicionar controllers ao container
builder.Services.AddControllers();

// Configurar OpenAPI/Swagger para documentação da API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar o pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// TODO: Mapear os endpoints/controllers de mídias
// Para controllers: app.MapControllers();
// Para minimal API: app.MapGet("/midias", ...), app.MapPost("/midias", ...), etc.
app.MapControllers();

app.Run();
