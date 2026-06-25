using BookingService.Model;
using BookingService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims; // <-- Is namespace ko add karna mat bhulna

namespace BookingService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("BookingDb")));

            builder.Services.AddHttpClient();
            builder.Services.AddScoped<IBookingService, BookingServiceImp>();

            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];

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

                    // 1. Dono Issuers ko array me validate karo
                    ValidateIssuer = true,
                    ValidIssuers = new[] { "UserService", "ownerService" },

                    // 2. Dono Audiences ko array me validate karo
                    ValidateAudience = true,
                    ValidAudiences = new[] { "BookingService", "ServiceCenterService", "VehicleService" },

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,

                    // 3. Roles capabilities enabled kiya
                    NameClaimType = ClaimTypes.Name,
                    RoleClaimType = ClaimTypes.Role
                };
            });

            builder.Services.AddAuthorization();
            builder.Services.AddControllers();

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