using HospitalManagementSystemAPI.Models;
using HospitalManagementSystemAPI.Repositories;
using HospitalManagementSystemAPI.Repositories.Interfaces;
using HospitalManagementSystemAPI.Services;
using HospitalManagementSystemAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystemAPI
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

            #region context
                    builder.Services.AddDbContext<HospitalManagementSystemContext>(
                            options => options.UseSqlServer(
                                    builder.Configuration.GetConnectionString("SQLServer")
                                )
                        );
            #endregion

            #region repositories
            builder.Services.AddScoped<IRepository<Role>, RoleRepository>();
            #endregion

            #region services
            builder.Services.AddScoped<IAdminService, AdminService>();
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
