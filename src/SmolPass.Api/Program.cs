using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SmolPass.Api.Diagnostics;
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
string secretBase64 = builder.Configuration["Jwt:Secret"] ?? throw new InvalidOperationException("Jwt:Secret manquant. En dev : dotnet user-secrets set \"Jwt:Secret\" \"<clé-base64>\" dans SmolPass.Api.");
SymmetricSecurityKey signingKey = new(Convert.FromBase64String(secretBase64));
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true, //fais ce geste : true / false
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer manquant dans appsettings.json (section Jwt)."), // référence : contre cette valeur
            ValidateLifetime = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt: Audience manquant dans appsettings.json (section Jwt)."),
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,
            ClockSkew = TimeSpan.Zero,
        };
        options.MapInboundClaims = false; // les claims gardent leurs noms JWT natif : pas de traduction en jargon SOAP
    });

WebApplication app = builder.Build();

// === Pipeline HTTP ===
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseExceptionHandler();
app.UseHttpsRedirection();// rediriger HTTP → HTTPS
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();// router vers les controllers


app.Run();