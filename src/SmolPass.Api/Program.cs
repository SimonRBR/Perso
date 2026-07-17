using SmolPass.Api.Services;
using SmolPass.Application;
using SmolPass.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// === Services ===
builder.Services.AddOpenApi();
builder.Services.AddControllers();                          // le guichet existe
builder.Services.AddInfrastructure(builder.Configuration);  // repos + DbContext (Phase 3)
builder.Services.AddApplication();                          // les 8 use cases (Phase 4)
builder.Services.AddSingleton<JwtTokenGenerator>();                          


WebApplication app = builder.Build();

// === Pipeline HTTP ===
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();// rediriger HTTP Å® HTTPS
app.MapControllers();// router vers les controllers


app.Run();