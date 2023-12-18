using APIUsuarios.Data;
using Microsoft.EntityFrameworkCore;
//Utilizamos las referencias para utilizar el json.web.token
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);//AQUI SE AGREGAN EN EL CONTENEDOR

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    //Titulo diseno
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Clinica Medica", Version = "v1" });

});

// Agregar el manejo de sesiones
/*builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de expiración de la sesión
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});*/

builder.Services.AddDbContext<ApplicationDBContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("ConsStr"));
});

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

//Utilma referencia
app.UseAuthentication();


app.UseAuthorization();

app.MapControllers();

app.Run();


