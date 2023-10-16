using Microsoft.EntityFrameworkCore;
using ms_usuario;
using ms_usuario.Domains;
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

// Add services to the container.

builder.Services.AddControllers();
builder.Services.SetupDbContext(builder.Configuration.GetValue<string>("ConnectionStrings:DbContext"));
builder.Services.SetupRepositories();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(AreaInteresse).Assembly));
builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(Conquistas).Assembly));
builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(Sociedade).Assembly));
builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(Usuario).Assembly));
builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(UsuarioAreaInteresse).Assembly));
builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(UsuarioConquistas).Assembly));
builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(UsuarioPerfil).Assembly));

var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Services.CreateScope().ServiceProvider.GetRequiredService<UsuarioDbContext>().Database.Migrate();

app.Run();
