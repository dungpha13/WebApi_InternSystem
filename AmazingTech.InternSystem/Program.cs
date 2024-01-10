using AmazingTech.InternSystem.Controllers;
using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Repositories;
using AmazingTech.InternSystem.Services;

namespace AmazingTech.InternSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<IAppDbContext, AppDbContext>();
            builder.Services.AddScoped<IFileReaderService, FileReaderService>();  // Register your ExcelReaderService

            builder.Services.AddScoped<ITruongService, TruongService>();
            builder.Services.AddScoped<ITruongRepository, TruongRepository>();

            builder.Services.AddScoped<IKiThucTapService, KiThucTapService>();
            builder.Services.AddScoped<IKiThucTapRepository, KiThucTapRepository>();

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}