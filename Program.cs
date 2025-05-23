using ProcessPDFAllyantTask.Contracts;
using ProcessPDFAllyantTask.Services;
using Microsoft.OpenApi.Models;


namespace ProcessPDFAllyantTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IPDFService, PDFService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseRouting();
            app.MapControllers();

            app.UseHttpsRedirection();

            app.UseAuthorization();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.Run();
        }
    }
}
