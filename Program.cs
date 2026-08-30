using MediatR;
using Microsoft.EntityFrameworkCore;
using ms_usuario;
using ms_usuario.Extensions;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy
                          .AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                      });
});

// Configurar explicitamente a porta HTTPS
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5002); // Porta HTTP
    //options.ListenAnyIP(5001, listenOptions => listenOptions.UseHttps()); // Porta HTTPS (opcional)
});

builder.WebHost.UseUrls("http://*:5002");

// Add services to the container.

builder.Services.AddControllers();
builder.Services.SetupDbContext(builder.Configuration.GetValue<string>("ConnectionStrings:DbContext"));
builder.Services.SetupRepositories();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(configuration =>
    configuration.RegisterServicesFromAssembly(typeof(UsuarioDbContext).Assembly));

var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
app.UseRouting();
app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    c.RoutePrefix = "swagger";  // Isso vai permitir acessar o Swagger via http://localhost:8100/
});

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

using (IServiceScope scope = app.Services.CreateScope())
{
    UsuarioDbContext dbContext = scope.ServiceProvider.GetRequiredService<UsuarioDbContext>();
    dbContext.Database.Migrate();
}

app.Run();
