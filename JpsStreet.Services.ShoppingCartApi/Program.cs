using AutoMapper;
using JpsStreet.Message.RabbiMQ;
using JpsStreet.Services.ShoppingCartApi;
using JpsStreet.Services.ShoppingCartApi.Data;
using JpsStreet.Services.ShoppingCartApi.Extensions;
using JpsStreet.Services.ShoppingCartApi.Service;
using JpsStreet.Services.ShoppingCartApi.Service.IService;
using JpsStreet.Services.ShoppingCartApi.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ShoppingCartAppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
// Creating IMapper object
IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
// For Registering mapper
builder.Services.AddSingleton(mapper);
// Finally Use Auto mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Register Services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<BackendApiAuthenticationHttpClientHandler>();
// Register RabbitMQ
builder.Services.AddScoped<IMessageRabbitMQ, MessageRabbitMQ>();

// Register Client Services
builder.Services.AddHttpClient("Product", u =>
    u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductApiBase"]))
    .AddHttpMessageHandler<BackendApiAuthenticationHttpClientHandler>();
builder.Services.AddHttpClient("Coupon", u =>
    u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:CouponApiBase"]))
    .AddHttpMessageHandler<BackendApiAuthenticationHttpClientHandler>();

//Register HttpContextAccessor
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type= ReferenceType.SecurityScheme,
                    Id=JwtBearerDefaults.AuthenticationScheme
                }
            }, new string[]{}
        }
    });
});

builder.AddAppAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
ApplyMigration();
app.MapControllers();

app.Run();

void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<ShoppingCartAppDbContext>();
        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}