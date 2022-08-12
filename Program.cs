using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection.Metadata;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(
 opt =>
 {
     opt.AddSecurityDefinition("AccountsOpenID", new OpenApiSecurityScheme
     {
         In = ParameterLocation.Header,
         Name = "Authorization",
         Type = SecuritySchemeType.OpenIdConnect,
         OpenIdConnectUrl = new Uri("https://HOST/realms/REALME/.well-known/openid-configuration"),




     });

     opt.AddSecurityRequirement(new OpenApiSecurityRequirement
     {
         {
             new OpenApiSecurityScheme
             {
                 Reference = new OpenApiReference
                 {
                     Type = ReferenceType.SecurityScheme,
                     Id = "Authorization"
                 }
             },
             new string[] {}
         }
     });

 });



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://kc.pstefano.com.br/realms/dotnet_keycloak";

        options.TokenValidationParameters = new TokenValidationParameters
        {

            ValidateIssuer = true,

            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = false,
            ValidIssuer = "https://kc.pstefano.com.br/realms/dotnet_keycloak",


            ValidAudience = "dotnet_teste",

        };
    });


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