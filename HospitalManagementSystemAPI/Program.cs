using HospitalManagementSystemAPI.Controllers.Responses;
using HospitalManagementSystemAPI.Models;
using HospitalManagementSystemAPI.Repositories;
using HospitalManagementSystemAPI.Repositories.Interfaces;
using HospitalManagementSystemAPI.Services;
using HospitalManagementSystemAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

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
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey:JWT"]))
                    };

                });

            #region context
            builder.Services.AddDbContext<HospitalManagementSystemContext>(
                            options => options.UseSqlServer(
                                    builder.Configuration.GetConnectionString("SQLServer")
                                )
                        );
            #endregion

            #region repositories
            builder.Services.AddScoped<IRepository<Staff>, StaffRepository>();
            builder.Services.AddScoped<IRepository<User>, UserRepository>();
            builder.Services.AddScoped<IRepository<Patient>, PatientRepository>();
            builder.Services.AddScoped<IRepository<MedicalHistory>, MedicalHistoryRepository>();
            builder.Services.AddScoped<IRepository<Nurse>, NurseRepository>();
            builder.Services.AddScoped<IRepository<Doctor>, DoctorRepository>();
            builder.Services.AddScoped<IRepository<Appointment>, AppointmentRepository>();
            builder.Services.AddScoped<IRepository<Medicine>, MedicineRepository>();
            builder.Services.AddScoped<IRepository<Prescription>, PrescriptionRepository>();
            builder.Services.AddScoped<IRepository<PrescriptionItem>, PrescriptionItemRepository>();
            #endregion

            #region services
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IPatientService, PatientService>();
            builder.Services.AddScoped<INurseService, NurseService>();
            builder.Services.AddScoped<IDoctorService, DoctorService>();
            builder.Services.AddScoped<IAppointmentService, AppointmentService>();
            builder.Services.AddScoped<IMedicineService, MedicineService>();
            builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
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
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
