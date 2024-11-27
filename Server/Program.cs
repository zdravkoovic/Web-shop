using BLL.Services;
using Domain.Repositories;
using Domain.Services;
using Infrastructure.SQL.Database;
using Infrastructure.SQL.Database.Model;
using Infrastructure.SQL.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebShop.src.BLL.Services;
using WebShop.src.Domain.Repository.Interfaces;
using WebShop.src.Domain.Services;
using WebShop.src.Infrastructure.SQL.Repositories;
using System.Text.Json.Serialization;
using WebShop.src.API.Hubs;
using WebShop.src.Infrastructure.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders(); // Clear the default logging providers
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR(options => {
    // options.KeepAliveInterval = TimeSpan.FromSeconds(30);
    // options.ClientTimeoutInterval = TimeSpan.FromSeconds(60);
});

builder.Services.AddControllers(options =>{
    options.CacheProfiles.Add("NoCache", new CacheProfile(){ NoStore = true });

    options.CacheProfiles.Add("Any-60", new CacheProfile() { Location = ResponseCacheLocation.Any, Duration = 60 });
})
.AddJsonOptions(options => {
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddDbContext<Context>();
builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IAdminRepo, AdminRepo>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IAvatarImageRepo, AvatarImageRepo>();
builder.Services.AddScoped<IProductImageRepo, ProductImageRepo>();
builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<IProductImgService, ProductImgService>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddIdentity<ApiUser, IdentityRole>(options =>{
    // opcije za password
})
    .AddEntityFrameworkStores<Context>();

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme =
    JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    // opcije koje su korisne za validaciju tokena
    // Issuer, Audience, issuer signing key...
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(
                builder.Configuration["JWT:SigningKey"]!)
        )
    };
});

builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(options =>{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CustomException>();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseDefaultFiles();
app.UseRouting();
app.MapControllers();

// app.Use((context, next) =>
// {
//     context.Response.GetTypedHeaders().CacheControl = 
//     new Microsoft.Net.Http.Headers.CacheControlHeaderValue(){
//        NoCache = true,
//        NoStore = true 
//     };
      
//     return next.Invoke();
// });

app.UseCors(builder =>{
    builder
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .WithOrigins("http://127.0.0.1:5500", "http://localhost:4200");
});

app.UseSwagger();

app.Use(async (context, next) => {
    try
    {
        await next.Invoke();
    }
    catch (Exception err)
    {
        throw new Exception(err.Message);
    }
});

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<CommentHub>("/commentHub");

app.UseWebSockets();

app.Run();

