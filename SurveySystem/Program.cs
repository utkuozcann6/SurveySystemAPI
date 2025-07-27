using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using SurveySystem.Services;
using SurveySystem.Data;
using MySql.Data.MySqlClient;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Servisleri ekle
builder.Services.AddControllers();

// Veritaban� ba�lant�s�n� ayarla
builder.Services.AddScoped<IDbConnection>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new MySqlConnection(connectionString);
});

// Servisleri DI ile kaydet
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISurveyService, SurveyService>();
builder.Services.AddScoped<IVoteService, VoteService>();

// DAO'lar� DI ile kaydet
builder.Services.AddScoped<IUserDao, UserDao>();
builder.Services.AddScoped<ISurveyDao, SurveyDao>();
builder.Services.AddScoped<IVoteDao, VoteDao>();

// JWT ayarlar�n� yap�land�r
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

// Swagger'� ekle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Anket Sistemi API",
        Version = "v1",
        Description = "Kullan�c�lar�n anket olu�turup oy verebilece�i API"
    });

    // Swagger i�in JWT kimlik do�rulama
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();