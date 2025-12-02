
using MonitoringSolution.Services.Interfaces;
using MonitoringSolution.Services;
using Npgsql;
using MonitoringSolution.Repositories.Interfaces;
using MonitoringSolution.Repositories;

namespace MonitoringSolution
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("http://localhost:3000", "http://frontend:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            builder.Services.AddNpgsqlDataSource(builder.Configuration.GetConnectionString("DefaultConnection"));

            // Services
            builder.Services.AddScoped<ISensorService, SensorService>();
            builder.Services.AddScoped<ISensorRepository, SensorRepository>();

            builder.Services.AddHostedService<SensorDataEmulator>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowFrontend");
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
