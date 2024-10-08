using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Repositories;
using AmazingTech.InternSystem.Services;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Mapping;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AmazingTech.InternSystem.Models.DTO;
using AmazingTech.InternSystem.Service;

using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.OpenApi.Models;
using AmazingTech.InternSystem.Repositories.NhomZaloManagement;
using AmazingTech.InternSystem.Services.Name;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

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

            builder.Services.AddScoped<IUserService, UserService>();

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

            builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
            builder.Services.AddScoped<IDashboardService, DashboardService>();

            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<INameService, NameService>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();
            builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
            builder.Services.AddScoped<IUserViTriRepository, UserViTriRepository>();
            builder.Services.AddScoped<IQuestionService, QuestionService>();
            builder.Services.AddScoped<IInterviewRepo, InterviewRepository>();
            builder.Services.AddScoped<IInterviewService, InterviewService>();

            builder.Services.AddScoped<IEmailUserStatusRepo, EmailUserStatusRepo>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin",
                    b =>
                    {
                        b.SetIsOriginAllowed(host => true)
                            //.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

           

            EmailSettingModel.Instance = builder.Configuration.GetSection("EmailSettings").Get<EmailSettingModel>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    BearerFormat ="JWT",
                    Scheme = "Bearer"
                });

            });
            builder.Services.AddSwaggerGen(w =>
            {
                w.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new string[] { } 
                    }
                });
            });
            builder.Services.AddAutoMapper(typeof(Program).Assembly);


            //Inject
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));


           
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));



            builder.Services.AddIdentityCore<User>()
                .AddRoles<IdentityRole>()
                .AddSignInManager()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;
                options.User.RequireUniqueEmail = false;
                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
                options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultProvider;

            }).Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromMinutes(15));
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
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

                option.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var token = context.SecurityToken as JwtSecurityToken;
                        if (token != null && !JwtGenerator.IsTokenValid(token.RawData))
                        {
                            context.Fail("Token is invalid.");
                        }

                        return Task.CompletedTask;
                    }
                };
            })
            .AddCookie()
            .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
            {
                options.ClientId = builder.Configuration["Google:ClientID"];
                options.ClientSecret = builder.Configuration["Google:ClientSecret"];
                //options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //options.Events = new OAuthEvents
                //{
                //    OnRedirectToAuthorizationEndpoint = context =>
                //    {
                //        context.Response.StatusCode = 401; // Set the status code to 401 instead of redirecting
                //        return Task.CompletedTask;
                //    }
                //};
            });




            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

            string accountDefaultHttpProtocol = configuration["AppSettings:ACCOUNT_DEFAULT_HTTP_PROTOCOL"];
            app.Use((context, next) =>
            {
                context.Request.Scheme = "https";
                return next(context);
            });





            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();




            app.UseCors("AllowOrigin");


            app.MapControllers();

            app.Run();
        }
    }
}
