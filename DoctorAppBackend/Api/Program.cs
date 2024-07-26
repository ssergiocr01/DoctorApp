using Api.Extensiones;
using Api.Middleware;
using Data.Inicializador;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AgregarServiciosAplicacion(builder.Configuration);
builder.Services.AgregarServicioIdentidad(builder.Configuration);
builder.Services.AddScoped<IdbInicializador, DbInicializador>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMiddleware<ExceptionMiddleware>();
app.UseStatusCodePagesWithReExecute("/errores/{0}");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod());

app.UseAuthentication();

app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();

	try
	{
		var inicializador = services.GetRequiredService<IdbInicializador>();
		inicializador.Inicializar();
	}
	catch (Exception ex)
	{
		var logger = loggerFactory.CreateLogger<Program>();
		logger.LogError(ex, "Un error ocurrió al ejecutar la migración");
	}
}

app.MapControllers();

app.Run();
