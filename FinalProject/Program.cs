using FinalProject.Application.Repositories;
using FinalProject.Domain.Entities;
using FinalProject.Persistence.Contexts;
using FinalProject.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<OnlineEventsDbContext>();
builder.Services.AddScoped<ICompanyReadRepository, CompanyReadRepository>();
builder.Services.AddScoped<ICompanyWriteRepository, CompanyWriteRepository>();
builder.Services.AddScoped<IEventReadRepository, EventReadRepository>();
builder.Services.AddScoped<IEventWriteRepository, EventWriteRepository>();
builder.Services.AddScoped<IMemberReadRepository, MemberReadRepository>();
builder.Services.AddScoped<IMemberWriteRepository, MemberWriteRepository>();
builder.Services.AddScoped<ITicketReadRepository, TicketReadRepository>();
builder.Services.AddScoped<ITicketWriteRepository, TicketWriteRepository>();
builder.Services.AddScoped<IAttendeeReadRepository, AttendeeReadRepository>();
builder.Services.AddScoped<IAttendeeWriteRepository, AttendeeWriteRepository>();


builder.Services.Configure<TokenOption>(builder.Configuration.GetSection("TokenOption"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    TokenOption tokenOption = builder.Configuration.GetSection("TokenOption").Get<TokenOption>();
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = tokenOption.Issuer,
        ValidAudience = tokenOption.Audience,
        ValidateIssuer = true,
        ValidateAudience = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOption.SecretKey))
    };
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
