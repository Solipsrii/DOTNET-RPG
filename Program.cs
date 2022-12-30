global using DOTNET_RPG.Models;
global using DOTNET_RPG.Models.Enums;
global using Microsoft.EntityFrameworkCore;
global using DOTNET_RPG.Data;
global using DOTNET_RPG.Services;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

//establishing connection string with DataContext object!
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c => {
   
    //add the BEARER token:
    //for more info, look in AuthRepository.cs for CreateToken
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme { //oauth2 is IMPORTANT HERE, WHY!? Why: apparently it's hardcoded as a reference for OpenApiScheme.OpenApiRefernece.
        Description = "Standard authoraization header using the Bearer scheme, e.g: \"bearer {token} \"",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    c.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddHttpContextAccessor();

//TODO: remind oneself how's addScope relevant in this project?
builder.Services.AddScoped<ICharacterService, CharacterService>(); //add the ability for pages to "inject" objects, like to instance objects on their own. Constructor and all.
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IWeaponService, WeaponService>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

//adding Authenticaion scheme:
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters 
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

//allows to retrieve current Claims data on the fly, like the authenticated user's id.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseAuthentication(); //enables the middleware to allow authentication with the settings we set up~

app.UseAuthorization();

app.MapControllers();

app.Run();
