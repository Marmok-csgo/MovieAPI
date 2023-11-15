using System.Text;
using FileUpload.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MovieAPI.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value!))
    };
});

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddTransient<IManageImage, ManageImage>();




// получаем строку подключения из файла конфигурации
string connection = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddDbContext<MovieContext>(opt =>
    opt.UseNpgsql(connection));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder => builder.AllowAnyOrigin());
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Uploads/StaticContent")),
    RequestPath = "/Uploads/StaticContent"
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();