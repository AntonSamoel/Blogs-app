using Blogs.Api;
using Blogs.Core;
using Blogs.Core.Constants;
using Blogs.Core.Interfaces;
using Blogs.Core.Models.AuthModels;
using Blogs.DataAccess.Data;
using Blogs.DataAccess.Services;
using Blogs.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
    builder => builder.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod());
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo()
    {
        Version = "v1",
        Title = "Blogs API",
        Description = "",
        Contact = new OpenApiContact()
        {
            Name = "",
            Email = "",
            Url = new Uri("https://mydomain.com")
        }
    });

    o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter the JWT Key"
    });

    o.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                    {
                       new OpenApiSecurityScheme()
                       {
                          Reference = new OpenApiReference()
                          {
                             Type = ReferenceType.SecurityScheme,
                             Id = "Bearer"
                          },
                          Name = "Bearer",
                          In = ParameterLocation.Header
                       },
                       new List<string>()
                    }
                });
});

builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DeploymentConnection"));
});


#region Dependecy Injection

builder.Services.AddServiceDependencies().AddApiDependencies().AddCoreDependencies();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
     .AddJwtBearer(o =>
     {
         o.RequireHttpsMetadata = false;
         o.SaveToken = false;
         o.TokenValidationParameters = new TokenValidationParameters
         {
             ValidateIssuerSigningKey = true,
             ValidateIssuer = true,
             ValidateAudience = true,
             ValidateLifetime = true,
             ValidIssuer = builder.Configuration["JWT:Issuer"],
             ValidAudience = builder.Configuration["JWT:Audience"],
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
             ClockSkew = TimeSpan.Zero // do not give more time than expiration
         };
     });

#endregion

builder.Services.AddTransient<IImageService, ImageService>();

builder.Services.AddMvc();
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseCors("AllowAnyOrigin");

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
