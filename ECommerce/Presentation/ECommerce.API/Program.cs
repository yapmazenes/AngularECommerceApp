using ECommerce.API.Configurations.Serilog.ColumnWriters;
using ECommerce.API.Extensions;
using ECommerce.API.Filters;
using ECommerce.Application;
using ECommerce.Infrastructure;
using ECommerce.Infrastructure.Filters;
using ECommerce.Infrastructure.Services.Storage.Local;
using ECommerce.Persistence;
using ECommerce.Socket;
using ECommerce.Socket.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Sinks.PostgreSQL;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpContextAccessor();
builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();
builder.Services.AddSignalRServices();
builder.Services.AddStorage<LocalStorage>();

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins("http://localhost:4200", "https://localhost:4200").
    AllowAnyHeader().AllowAnyMethod().AllowCredentials()));

builder.Services.AddControllers(x =>
{
    x.Filters.Add<ValidationFilter>();
    x.Filters.Add<RolePermissionFilter>();
}).ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

Logger log = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt")
    .WriteTo.PostgreSQL(builder.Configuration.GetConnectionString("PostgreSQL"), "Logs", needAutoCreateTable: true,
                        columnOptions: new Dictionary<string, ColumnWriterBase>
                        {
                            {"message",new RenderedMessageColumnWriter() },
                            {"message_template",new MessageTemplateColumnWriter() },
                            {"level",new LevelColumnWriter() },
                            {"time_stamp",new TimestampColumnWriter() },
                            {"exception",new ExceptionColumnWriter() },
                            {"log_event",new LogEventSerializedColumnWriter() },
                            {"user_name",new UsernameColumnWriter() },

                        })
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog(log);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer("Admin", options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateAudience = true, //Olusturulacak olan Token degerini kimlerin/hangi originlerin/ sitelerin kullanacagini belirledigimiz degerdir.
                        ValidateIssuer = true, //Olusturulan Token degerini kimin dagittigini ifade eder
                        ValidateLifetime = true, //Olusturulan Token degerinin gecerlilik suresini kontrol edecek olan dogrulamadir.
                        ValidateIssuerSigningKey = true, //Uretilecek olan token degerinin uygulamamiza ait oldugunu ifade eden security key verisinin dogrulamasi
                        ValidAudience = builder.Configuration["Token:Audience"],
                        ValidIssuer = builder.Configuration["Token:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
                        LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,
                        NameClaimType = ClaimTypes.Name
                    };
                });

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionHandler(app.Services.GetRequiredService<ILogger<Program>>());
app.UseStaticFiles();

app.UseSerilogRequestLogging();
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    var userName = context.User?.Identity?.IsAuthenticated == true ? context.User.Identity.Name : null;

    LogContext.PushProperty("userName", userName);
    await next();
});

app.MapControllers();
app.MapHubs();

app.Run();
