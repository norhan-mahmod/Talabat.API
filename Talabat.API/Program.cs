using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.API.Extension;
using Talabat.API.Middlewares;
using Talabat.Core.Entities.Identity;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

namespace Talabat.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #region Databases Connections
            // for DbContext 
            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            // for redis connection
            builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
            {
                var connection = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connection);
            });
            // for Identity DbContext
            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });
            #endregion

            // (Extension Services) Extesion method that we did to make this file less crowded 
            builder.Services.AddApplicationServices();
            builder.Services.AddIdentityServices(builder.Configuration);

            var app = builder.Build();

            // seed
            // to do update database from code
            var scop = app.Services.CreateScope(); // to get Scoped Services
            var services = scop.ServiceProvider; // to do Dependency Injection
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                // Ask Clr to create object from StoreContext Explicitly
                var dbContext = services.GetRequiredService<StoreContext>();
                await dbContext.Database.MigrateAsync(); // update database
                await StoreContextSeed.SeedAsync(dbContext);
                
                // for update databse of identity by code
                var identityContext = services.GetRequiredService<AppIdentityDbContext>();
                await identityContext.Database.MigrateAsync();

                // for user seed
                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUsersAsync(userManager);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An Error Occured during apply Migration");
            }


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            
            app.UseMiddleware<ExceptionMiddleware>(); // to execute middleware that handle server error (exception)
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // redirect to handle NotFound Endpoint (when request endpoint that doesn't exist)
            app.UseStatusCodePagesWithRedirects("/errors/{0}");

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}