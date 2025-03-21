using CRMServerApi.Data;
using CRMServerApi.Services;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<BirthdayEmailService>();

var app = builder.Build();

// middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();