using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Mon_resto_backend.Models;
using Mon_resto_backend.Models.Repository;
using MonResto_backend.Services;

using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Vérification de la configuration de la chaîne de connexion à la base de données
var connectionString = builder.Configuration.GetConnectionString("dbcon");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("La chaîne de connexion à la base de données est manquante.");
}

builder.Services.AddDbContext<MonRestoDbContext>(options => options.UseSqlServer(connectionString));

// Ajouter les services requis
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

// Ajouter JwtService
builder.Services.AddSingleton<JwtService>();

// Ajouter PasswordHasher pour hasher les mots de passe
builder.Services.AddScoped<PasswordHasher<User>>();

// Configuration de l'authentification JWT
var jwtSecretKey = builder.Configuration["JWT:SecretKey"];
if (string.IsNullOrEmpty(jwtSecretKey))
{
    throw new InvalidOperationException("La clé secrète JWT est manquante.");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey))
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Mon Resto API",
        Version = "v1",
        Description = "API pour la gestion du restaurant.",
    });

    // Ajouter la configuration pour JWT
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Entrez 'Bearer' [espace] et votre token JWT dans le champ ci-dessous.\n\nExemple: 'Bearer abc123xyz'"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Configuration de CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173") // L'URL de votre frontend React
          .AllowAnyHeader()
          .AllowAnyMethod();
    });
});

// Dépendances des repositories
builder.Services.AddScoped<ICategorieRepository, CategorieRepository>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IPanierRepository, PanierRepository>();
builder.Services.AddScoped<ICommandeRepository, CommandeRepository>();
builder.Services.AddScoped<IOffreRepository, OffreRepository>();



var app = builder.Build();

// Ajouter un administrateur par défaut si nécessaire
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MonRestoDbContext>();
    var passwordHasher = scope.ServiceProvider.GetRequiredService<PasswordHasher<User>>();

    // Assurez-vous que la base de données est créée
    context.Database.EnsureCreated();

    // Vérifiez si un utilisateur administrateur existe déjà
    if (!context.Users.Any(u => u.Email == "admin@monresto.com"))
    {
        var admin = new User
        {
            Nom = "Admin",
            Email = "admin@monresto.com",
            Role = "Admin",
            PasswordHash = passwordHasher.HashPassword(null, "password123")
        };

        context.Users.Add(admin);
        context.SaveChanges();
    }
}

// Configuration du pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(); // Important : Toujours avant UseAuthentication
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
