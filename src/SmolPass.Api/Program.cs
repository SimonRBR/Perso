using SmolPass.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// === Services ===
builder.Services.AddOpenApi();
builder.Services.AddInfrastructure(builder.Configuration);

WebApplication app = builder.Build();

// === Pipeline HTTP ===
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();