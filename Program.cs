using Microsoft.EntityFrameworkCore;
using Backend.Data;
var builder = WebApplication.CreateBuilder(args);

//soporte a MySQL
var connectionString = builder.Configuration.GetConnectionString("DataContext");
builder.Services.AddDbContext<DataContext>(options => 
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

//Soporte para CORS
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:3001", "http://localhost:8080")
            .AllowAnyHeader()
            .WithMethods("GET", "POST", "PUT", "DELETE");
        });
});

//Funcionalidad de controladores
builder.Services.AddControllers();
//documentacuón de API
builder.Services.AddSwaggerGen();
//construcción de la aplicación web
var app = builder.Build();
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); 
}
//rutas para endppoints de los controladores
app.UseRouting();
//uso de Cors con policy
app.UseCors();
//uso de rutas sin especificar por default
app.MapControllers();
//ejecución de la aplicación
app.Run();
