using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using VehicleService.Middleware;
using VehicleService.Services;

namespace VehicleService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngular",
                    policy =>
                    {
                        policy
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });

            // Add services to the container.
            builder.Services.AddDbContext<Models.VehicleContext>((options) => options.UseSqlServer(builder.Configuration.GetConnectionString("VehicleDb")));
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IVehicleService, VehicleServiceImpl>();
            builder.Services.AddProblemDetails();



            //Configure JWT authentication
            var secret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? "VehicleServiceBooking@2026#TeamSecret!Key";
            var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "UserService";
            var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "VehicleService";

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudiences = audience.Split(','),
                    ValidateLifetime = true,

                };
            });
            Log.Logger = new LoggerConfiguration()

            .WriteTo.Console()

            .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)

            .CreateLogger();

            builder.Host.UseSerilog();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("AllowAngular");


            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.UseMiddleware<GlobalExceptionHandler>();

            app.Run();
        }
    }
}
    
   

