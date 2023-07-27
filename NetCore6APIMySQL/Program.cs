using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using NetCore6APIMySQL.Data;
using NetCore6APIMySQL.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// De esta forma se crea una conexion cada vez que se llama, no optima para produccion.
var mySQLConfiguration = new MySQLConfiguration(builder.Configuration.GetConnectionString("MySqlConnection"));
builder.Services.AddSingleton(mySQLConfiguration);// Para siempre tener la cadena de conexion

// Des esta forma se debe llamar la conexio, solo cuando no hay una activa
//builder.Services.AddSingleton(new MySqlConnection(builder.Configuration.GetConnectionString("MySqlConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>(); // Agregar al contenedor de dependencias la instancia al repositorio

// Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("NewPolicy", app =>
    {
        app.AllowAnyOrigin()
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

app.UseCors("NewPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
