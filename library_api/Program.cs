using library_api.Services;
using library_api.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json.Serialization;

namespace library_api
{
	public class Program
	{
		public Program()
		{
			JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
		}

		public static void Main(string[] args)
		{
			JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services
				.AddControllers(options => options.Conventions.Add(new SwaggerVersionControl()))
				.AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles).AddNewtonsoftJson();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();

			builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(opciones => opciones.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = false,
					ValidateAudience = false,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT_SECRET"])),
					ClockSkew = TimeSpan.Zero
				});

			builder.Services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo { Title = "library_api", Version = "v1" });

				options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header
				});

				options.OperationFilter<HATEOASParameter>();

				options.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							}
						},
						new string[]{}
					}
				});

			});

			builder.Services.AddAutoMapper(typeof(Program));
			builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDBContext>().AddDefaultTokenProviders();

			builder.Services.AddAuthorization(options => options.AddPolicy("Admin", policy => policy.RequireClaim("Admin")));

			builder.Services.AddTransient<SetHATEOASLinks>();
			builder.Services.AddTransient<HATEOASAuthorFilterAttribute>();
			builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI(options =>
				{
					options.SwaggerEndpoint("v1/swagger.json", "library_api v1");
				});
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}