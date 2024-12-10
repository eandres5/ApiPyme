using ApiPyme.Context;
using ApiPyme.Repositories;
using ApiPyme.RepositoriesImpl;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Abstractions;
using System.Text;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Configuration.AddJsonFile("appsettings.json");
var _secretKey = builder.Configuration.GetSection("settings").GetSection("secretKey").ToString();
var keyBytes = Encoding.UTF8.GetBytes(_secretKey);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(config => { 
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config => { 
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

// cadena de conexion para la base de datos
var _config = builder.Configuration;
var _connection = _config.GetConnectionString("MySqlConnection");
// registrar servicios para la conexion
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(_connection, ServerVersion.AutoDetect(_connection)));

builder.Services.AddScoped<IProductoRepository, ProductoRepositoryImpl>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepositoryImpl>();
builder.Services.AddScoped<IRolRepository, RolRepositoryImpl>();
builder.Services.AddScoped<IUsuarioRolRepository, UsuarioRolRepositoryImpl>();
builder.Services.AddScoped<ICompraRepository, CompraRepositoryImpl>();
builder.Services.AddScoped<IComprobanteRepository, ComprobanteRepositoryImpl>();
builder.Services.AddScoped<IDetalleComprobanteRepository, DetalleComprobanteRepositoryImpl>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200") // Cambia la URL a la de tu aplicación Angular
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(builder =>
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader());
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
