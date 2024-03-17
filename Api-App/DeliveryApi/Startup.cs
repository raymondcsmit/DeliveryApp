using Core;
using Delivery.Repository;
using Delivery.Services.Abstract;
using Delivery.Services.Implementation;
using DeliveryApp.DAL;
using DeliveryApp.DAL.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace DeliveryApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<EntityColumnMappingProvider>(provider =>
            {
                return new EntityColumnMappingProvider(Configuration.GetSection("EntityColumnMappings"));
            });
            services.Configure<RepositoryOptions>(Configuration.GetSection("RepositoryOptions"));
            services.AddIdentity<User, Role>().AddDefaultTokenProviders();

            // Register ApplicationUserStore as the implementation for user management
            services.AddScoped<IUserStore<User>, UserService>();
            services.AddScoped<IRoleStore<Role>, RoleService>();
            services.AddScoped<IUserPasswordStore<User>, UserService>();
            services.AddScoped<TokenService>();
            services.AddScoped<DeliveryContext>();
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ILookupServiceFactory, LookupServiceFactory>();
            services.AddScoped<IDeliveryService, DeliveryService>();
            services.AddScoped<ISchedulerService, SchedulerService>();
            var secretKey = Configuration.GetValue<string>("SecretKey");
            var key = Encoding.ASCII.GetBytes(secretKey); // Replace with your own secret key

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };

                });
            services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowClientApp",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200")
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });
            services.AddControllers();
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DeliveryApp", Version = "v1" });
            //});
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DeliveryApp", Version = "v1" });
                // Define the OAuth2.0 scheme that's being used (e.g., bearer token)
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                // Make sure the JWT token is applied globally
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DeliveryApp v1"));
            }
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("AllowClientApp");
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
