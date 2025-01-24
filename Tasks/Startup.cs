using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Tasks.Entities;
using Tasks.Filters;
using Tasks.Manager;

namespace Tasks
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {

            // Configure Filters
            services.AddControllers(opciones =>
            {
                //Log para captar todos los exeptions no capturados
                opciones.Filters.Add(typeof(FilterExeption));
            }).AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles).AddNewtonsoftJson();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            string projectName = Assembly.GetEntryAssembly().GetName().Name;//GET PROYECT NAME
                                                                            //Configure Logger
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Fatal)
                .WriteTo.File($"logs/{projectName}-.txt", rollingInterval: RollingInterval.Day, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSerilog(Log.Logger);
            });
            // AUTENTICACIÓN JWT
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
                AddJwtBearer(opciones => opciones.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWTKey") ?? Configuration["JWTKey"])),
                    ClockSkew = TimeSpan.Zero
                });
            //HttpContextAccessor
            services.AddHttpContextAccessor();
            //DataProtectionTokenProviderOptions
            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(24); // o el tiempo que desees
            });

            // IDENTITY (USER SYSTEM)
            services.AddScoped<IdentityDbContext<CustomUserIdentity>, ApplicationDbContext>();
            services.AddIdentity<CustomUserIdentity, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            Console.WriteLine("IDENTITY (USER SYSTEM) -> Ready");
            //Security on login
            services.AddScoped<CustomUserIdentityManager>();
            services.AddScoped<SignInManager<CustomUserIdentity>>();
            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 3; // Número de intentos fallidos permitidos antes de bloquear la cuenta
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(60); // Duración del bloqueo de cuenta
            });
            //SWAGGER
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "JAZZ API",
                    Version = "v1",
                    Description = "API RESTful For the App",
                    Contact = new OpenApiContact
                    {
                        Email = "soporte@jazz.com",
                        Name = "Jazz Soft",
                        Url = new Uri("https://jazz.com")
                    },
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new string[]{}
                    }
            });

                var archivoXML = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var rutaXML = Path.Combine(AppContext.BaseDirectory, archivoXML);
                c.IncludeXmlComments(rutaXML);
            });
            //Configure Database
            var connectionString = Configuration.GetConnectionString("DefaultConnection") ?? "";
            var url = Environment.GetEnvironmentVariable("DATABASE_URL");
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Equals("Development"))
            {
                Console.WriteLine("dev");

                services.AddDbContext<ApplicationDbContext>(options =>
               options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            }
            else
            {
                Console.WriteLine("prod");
                services.AddDbContext<ApplicationDbContext>(options =>
               options.UseNpgsql(Environment.GetEnvironmentVariable("DATABASE_URL")));
                Console.WriteLine("prod");
            }
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            //CORS
            services.AddCors(opciones =>
            {
                opciones.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("https://www.apirequest.io").AllowAnyMethod().AllowAnyHeader();
                });
            });
            Console.WriteLine("CORS");
            services.AddSignalR();
            Console.WriteLine("AddSignalR");




        }

        [Obsolete]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Console.WriteLine("Configurando servicios");
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {

                Console.WriteLine("Antes de aplicar migraciones ");
                var dbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                Console.WriteLine("Migrations success");
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
