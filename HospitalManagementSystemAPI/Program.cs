using HospitalManagementSystemAPI.Controllers.Responses;
using HospitalManagementSystemAPI.Models;
using HospitalManagementSystemAPI.Repositories;
using HospitalManagementSystemAPI.Repositories.Interfaces;
using HospitalManagementSystemAPI.Services;
using HospitalManagementSystemAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
            builder.Services
                .Configure<ApiBehaviorOptions>(
                    options =>
                    {
                        options.InvalidModelStateResponseFactory = ErrorResponse.CreateCustomErrorResponseForInvalidModel;
                    }
                );
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
            builder.Services.AddScoped<IRepository<Staff>, StaffRepository>();
            builder.Services.AddScoped<IRepository<User>, UserRepository>();
            builder.Services.AddScoped<IRepository<Patient>, PatientRepository>();
            builder.Services.AddScoped<IRepository<MedicalHistory>, MedicalHistoryRepository>();
            #endregion

            #region services
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IPatientService, PatientService>();
            #endregion

            #region CORS
            builder.Services.AddCors(opts =>
            {
                opts.AddPolicy("CORSPolicy", options =>
                {
                    options
                        .WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("CORSPolicy");
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
