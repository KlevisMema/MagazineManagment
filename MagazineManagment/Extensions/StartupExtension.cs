﻿using FluentValidation;
using MagazineManagment.BLL.RepositoryServices;
using MagazineManagment.BLL.RepositoryServices.GenericService;
using MagazineManagment.BLL.RepositoryServices.ServiceInterfaces;
using MagazineManagment.BLL.Services;
using MagazineManagment.DAL.DataContext;
using MagazineManagment.DTO.FluentValidators;
using MagazineManagment.DTO.Mappings;
using MagazineManagment.DTO.ViewModels;
using MagazineManagment.Web.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

namespace MagazineManagment.Web.Extensions
{
    public static class StartupExtension
    {
        public static IServiceCollection InjectServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            services.AddTransient<IProfileService, ProfileService>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddSession();

            services.Configure<JwtConfig>(configuration.GetSection("JWTConfig"));

            services.AddTransient(typeof(IGenericRepository<,,>) , typeof(GenericRepository<,,>));

            services.AddScoped<IValidator<CategoryCreateViewModel>, CategoryCreateValidator>();
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization using JWT Bearer security scheme"
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "bearerAuth",
                            },
                        },
                        Array.Empty<string>()
                    }
                });
            });

            services.AddAuthentication(a =>
            {
                a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            .AddJwtBearer(options =>
            {
                var key = Encoding.ASCII.GetBytes(configuration["JWTConfig:Key"]);

                options.TokenValidationParameters = new()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = true,
                };
            });

            return services;
        }
    }
}