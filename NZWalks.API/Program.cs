
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Mappings;
using NZWalks.API.Repositories;
using NZWalks.API.Walks;

namespace NZWalks.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            // Register NZWalksDBContext with SQL Server configuration
            builder.Services.AddDbContext<NZWalksDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksConnectionString")));

            builder.Services.AddScoped<IRegionRepository,SQLRegionRepository>();
            builder.Services.AddScoped<IWalkRepository, SQLWalkRepository>();


            builder.Services.AddAutoMapper(cfg =>
            {
                // If this is a personal, educational, or small project, 
                // you can generate a free community license on automapper.io
                cfg.LicenseKey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxODIwMzYxNjAwIiwiaWF0IjoiMTc4ODgzNjk4MyIsImFjY291bnRfaWQiOiIwMWEwN2VmZDg1Y2Y3MWZmYWIxZjg1MDNjYzBlY2U1MCIsImN1c3RvbWVyX2lkIjoiMDFhMDdlZmQ4NWNmNzFmZmFiMWY4NTAzY2MwZWNlNTAiLCJzdWJfaWQiOiItIiwiZWRpdGlvbiI6IjAiLCJ0eXBlIjoiMiJ9.C39qOqscKM_eEnAKCgc0GQYSc7qv-P26UzAGYAKuMlwfDjho0ZgnKP7q3Lwvn7jmjrB7fFF9wkpli_fk7eAgTWnBp3v1WxoidMdQmm_uZggTsaCNa5bc3PCFY_BF6KoUPc7CF7hvGMOZaJRxebmkuXohObR6qO9J0PSiTe7Gj6KsPJ4L0KXPCbGHXG2zV8WW721zS0paV5JkST7-PhfAhwelK3xRHoiYbqddNP6ZP5AOIRxovPWfAQNAhYd7UyewM3zkTgMgrjpneHyZU7p_GDZox0Bne2hWQp-YyFQ_XlnUs58um7gCsFOHfpyqmEa-t5cVhQHIHnswIZYjN5EcAA";
            }, typeof(Program));



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
