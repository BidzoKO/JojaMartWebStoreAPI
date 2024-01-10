using JojaMartAPI;
using JojaMartAPI.Services;
using JojaMartAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddHttpClient();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:JwtConfiguration:SecretKey").Value!)),
            ValidAudience = "https://localhost:7177/",
            ValidIssuer = "https://localhost:7177/",
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ClockSkew = TimeSpan.Zero,
        };
    });

builder.Services.AddHttpContextAccessor();

// CORS policy
builder.Services.AddCors(p => p.AddPolicy("corsPolicy", build =>
{
    build.WithOrigins("http://127.0.0.1:5500").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
}));

//Setup database connection
var connection = String.Empty;

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
    connection = builder.Configuration.GetConnectionString("CONNECTIONSTRING");
}
else
{
    connection = Environment.GetEnvironmentVariable("CONNECTIONSTRING");
}

builder.Services.AddDbContext<JojaMartDbContext>(options =>
    options.UseSqlServer(connection));


// Build app
var app = builder.Build();


// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("corsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

