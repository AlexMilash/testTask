
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using TestTask.HackerRankClient.Extensions;
using TestTask.Middlewares;
using TestTask.PostingsClient.Contracts;
using TestTask.Validation;

namespace TestTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            builder.Services.AddControllers();
            builder.Services.AddHackerRankServices(builder.Configuration);
            builder.Services.AddTransient<IValidator<PageRequest>, PageRequestValidator>();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();
            app.UseMiddleware<TestExceptionHandlerMiddleware>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
