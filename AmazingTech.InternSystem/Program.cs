using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Repositories;
using AmazingTech.InternSystem.Services;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Mapping;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using swp391_be.API.Repositories.Tokens;
using swp391_be.API.Services.Name;
using System.Text;
using AmazingTech.InternSystem.Repositories.AmazingTech.InternSystem.Repositories;
using AmazingTech.InternSystem.Models.DTO;
using AmazingTech.InternSystem.Service;
using AmazingTech.InternSystem.Repositories.NhomZaloManagement;
using System.IdentityModel.Tokens.Jwt;

namespace AmazingTech.InternSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<IAppDbContext, AppDbContext>();
            builder.Services.AddDbContext<AppDbContext>();



            builder.Services.AddScoped<ITruongService, TruongService>();
            builder.Services.AddScoped<ITruongRepository, TruongRepository>();

            builder.Services.AddScoped<IKiThucTapService, KiThucTapService>();
            builder.Services.AddScoped<IKiThucTapRepository, KiThucTapRepository>();

            builder.Services.AddScoped<IInternInfoService, InternInfoService>();
            builder.Services.AddScoped<IInternInfoRepo, InternInfoRepository>();

            builder.Services.AddScoped<ITechRepo, TechRepository>();
            builder.Services.AddScoped<ITechService, TechService>();

            builder.Services.AddScoped<IDuAnService, DuAnService>();
            builder.Services.AddScoped<IDuAnRepo, DuAnRepository>();

            builder.Services.AddScoped<INhomZaloService, NhomZaloService>();
            builder.Services.AddScoped<INhomZaloRepo, NhomZaloRepository>();

            builder.Services.AddScoped<IViTriRepository, ViTriRepository>();
            builder.Services.AddScoped<IViTriService, ViTriService>();

            builder.Services.AddScoped<ILichPhongVanRepository, LichPhongVanRepository>();
            builder.Services.AddScoped<IGuiLichPhongVanService, LichPhongVanService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<INameService, NameService>();
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


            EmailSettingModel.Instance = builder.Configuration.GetSection("EmailSettings").Get<EmailSettingModel>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(typeof(Program).Assembly);



            //Inject
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));


            builder.Services.AddScoped<ITokenRepository, SQLTokenRepository>();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowAll",
                                  policy =>
                                  {
                                      policy.AllowAnyOrigin()
                                      .AllowAnyHeader()
                                      .AllowAnyMethod();
                                  });
            });

            builder.Services.AddIdentityCore<User>()
                .AddRoles<IdentityRole>()
                .AddTokenProvider<DataProtectorTokenProvider<User>>("Kong")
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 12;
                options.Password.RequiredUniqueChars = 1;
                options.User.RequireUniqueEmail = false;

            });
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    //ValidateIssuer = true,
                    //ValidateAudience = true,
                    //ValidateLifetime = true,
                    //ValidateIssuerSigningKey = true,
                    //ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    //ValidAudience = builder.Configuration["Jwt:Audience"],
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("de455d3d7f83bf393eea5aef43f474f4aac57e3e8d75f9118e60d526453002dc"))
                };

                //option.Events = new JwtBearerEvents
                //{
                //    OnTokenValidated = context =>
                //    {
                //        var token = context.SecurityToken as JwtSecurityToken;
                //        if (token != null)
                //        {
                //            context.Fail("Token is invalid.");
                //        }

                //        return Task.CompletedTask;
                //    }
                //};
            });



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}