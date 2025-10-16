using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Core.Interfaces;
using PersonalFinanceTracker.Core.Services;
using PersonalFinanceTracker.Data;

namespace PersonalFinanceTracker.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //builder.Services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(
                    builder.Configuration.GetConnectionString("DefaultConnection2"),
                    new MySqlServerVersion(new Version(8, 0, 32))
                ));

            // Add services to the container.
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ITransactionService, TransactionService>();

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
