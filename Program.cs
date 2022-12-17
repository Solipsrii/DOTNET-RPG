global using DOTNET_RPG.Models;
global using DOTNET_RPG.Models.Enums;
global using Microsoft.EntityFrameworkCore;
global using DOTNET_RPG.Data;
global using DOTNET_RPG.Services;


var builder = WebApplication.CreateBuilder(args);

//establishing connection string with DataContext object!
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//TODO: remind oneself how's addScope relevant in this project?
builder.Services.AddScoped<ICharacterService, CharacterService>(); //add the ability for pages to "inject" objects, like to instance objects on their own. Constructor and all.
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
