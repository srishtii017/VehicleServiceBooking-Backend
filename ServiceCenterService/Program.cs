using System.Text;
using System.Security.Claims; // <-- Is namespace ko add karna mat bhulna
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using ServiceCenterService.Services;

namespace ServiceCenterService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Host.UseSerilog();

            builder.Services.AddScoped<IServiceCenterService, ServiceCenterServiceImpl>();

            builder.Services.AddDbContext<Models.ServiceCenterContext>((options) =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("ServiceCenterDb"))
            );
            builder.Services.AddControllers().AddXmlSerializerFormatters();
            builder.Services.AddHttpClient();

            // Configure JWT authentication
            // Hardcoded secret key backup me vahi rakh rhe hain jo sab jagah chal rha h
            var secret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? "VehicleServiceBooking@2026#TeamSecret!Key";

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
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
                    ValidIssuers = new[] { "ownerService", "UserService" },
                    ValidateAudience = true,
                    ValidAudiences = new[] { "ServiceCenterService", "VehicleService", "BookingService" },

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    NameClaimType = ClaimTypes.Name,
                    RoleClaimType = ClaimTypes.Role
                };
            });

            // Authorization middleware validation support ke liye
            builder.Services.AddAuthorization();

            builder.Services.AddCors(options =>
                options.AddPolicy("policy1", policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.AllowAnyOrigin();
                }));

            var app = builder.Build();

            app.UseCors("policy1");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}