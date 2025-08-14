// System
using System;
using System.Text;

// Microsoft
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

// Proyecto
using Huerto_Urbano_Backend.Contexto;
using Huerto_Urbano_Backend.Dto;
using Huerto_Urbano_Backend.Middlewares;
using Huerto_Urbano_Backend.Recursos;
using Huerto_Urbano_Backend.Interfaces;



var builder = WebApplication.CreateBuilder(args);



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();



builder.Services.AddDbContext<ContextoBdApp>(options =>
options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), // Conexi�n para cliente
new MySqlServerVersion(new Version(8, 0, 32)))
);

builder.Services.AddDbContext<ContextoBdAdmin>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("AdminConnection"),  
        new MySqlServerVersion(new Version(8, 0, 36)))
                   .EnableSensitiveDataLogging(); // solo en desarrollo
});

builder.Services.AddDbContext<ContextoBdEmployee>(options =>

options.UseMySql(builder.Configuration.GetConnectionString("EmployeeConnection"),

new MySqlServerVersion(new Version(8, 0, 36)))

);


builder.Services.AddControllers();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // URL donde corre Angular
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});


// Configuración de JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });


builder.Services.AddAuthorization();

//builder.Services.AddScoped<IServicioToken, ServicioToken>();


var app = builder.Build();

// Migración automática solo para ContextoBdAdmin
using (var scope = app.Services.CreateScope())
{
    var dbAdmin = scope.ServiceProvider.GetRequiredService<ContextoBdAdmin>();
    dbAdmin.Database.Migrate();
}

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularFrontend");

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();


app.MapControllers();

app.Run();
