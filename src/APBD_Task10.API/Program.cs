using System.Text;
using APBD_Task10.API.Helpers.Middleware;
using APBD_Task10.API.Helpers.Services;
using APBD_Task10.App;
using APBD_Task10.App.Helpers.Options;
using APBD_Task10.App.Services;
using APBD_Task10.App.Services.Position;
using APBD_Task10.App.Services.Role;
using APBD_Task10.Infrastructure;
using APBD_Task10.Infrastructure.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<MasterContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DatabaseConnection"),
            b => b.MigrationsAssembly("APBD_Task10.API"))
        );

var jwtConfigData = builder.Configuration.GetSection("Jwt");

builder.Services.Configure<JwtOptions>(jwtConfigData);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(120),
        ValidIssuer = jwtConfigData["Issuer"],
        ValidAudience = jwtConfigData["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfigData["Key"]))
    };

    opt.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token-Expired", "true");
            }

            return Task.CompletedTask;
        }
    };
}).AddJwtBearer("IgnoreTokenExpirationScheme", opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ClockSkew = TimeSpan.FromMinutes(120),
        ValidIssuer = jwtConfigData["Issuer"],
        ValidAudience = jwtConfigData["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfigData["Key"]))
    };
});
builder.Services.AddScoped<IDeviceService, DeviceService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IDeviceTypeService, DeviceTypeService>();
builder.Services.AddScoped<IPositionService, PositionService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddSingleton<ValidationConfigService>( 
    (s) => new ValidationConfigService(
        builder.Configuration.GetValue<string>("AdditionalPropertiesValidationConfigPath")
        ));

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<AdditionalPropertiesValidationMiddleware>();

app.Run();