using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NASATechAPI.DbContexts;
using NASATechAPI.Entities;
using NASATechAPI.Interfaces;
using NASATechAPI.Repository;

namespace NASATechAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SQLServerConnection>(
                options => options.UseSqlServer(Configuration.GetConnectionString("SQLServer")));

            //services.AddScoped<IUserRepository, UserRepository>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IRepositoryAsync<User>, RepositoryAsync<User>>();

            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

        }

        public void Configure(WebApplication app, IWebHostEnvironment env) 
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            var origins = Configuration
                       .GetSection("AllowedHosts").Value.Split(",");

            app.UseCors(builder => builder.WithOrigins(origins)
                                  .AllowAnyHeader()
                                  .AllowAnyMethod()
                                  );

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
