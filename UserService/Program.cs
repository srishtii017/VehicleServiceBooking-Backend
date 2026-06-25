using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using User_Management.Repository;
using User_Management.Services;
using User_Management.Middleware;
using Serilog;

namespace User_Management
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                // Configure Serilog
                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(builder.Configuration)
                    .Enrich.FromLogContext()
                    .CreateLogger();

                builder.Host.UseSerilog();

                // Database
                builder.Services.AddDbContext<Models.UserContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("UserDetails"))
                );

                // Register Repository and Service
                builder.Services.AddScoped<IUserRepository, UserRepository>();
                builder.Services.AddScoped<IUserService, UserService>();
                builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

                // Configure JWT
                var jwtSettings = builder.Configuration.GetSection("Jwt");
                var secretKey = jwtSettings["SecretKey"];
                var issuer = jwtSettings["Issuer"];
                var audience = jwtSettings["Audience"];

                builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                        ValidateIssuer = true,
                        ValidIssuer = issuer,
                        ValidateAudience = true,
                        ValidAudiences = audience!.Split(','),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

                builder.Services.AddControllers();

                // CORS
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowAngular", policy =>
                        policy.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod());
                });

                var app = builder.Build();

                // Middleware pipeline
                app.UseSerilogRequestLogging();
                app.UseMiddleware<GlobalExceptionHandler>();
                app.UseCors("AllowAngular");
                app.UseAuthentication();
                app.UseAuthorization();

                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application failed to start.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}