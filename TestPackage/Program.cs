using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Optimal.Framework.Configuration;
using Optimal.Framework.Data;
using TestPackage.Controllers;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddDbContext<MyDbContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"));
//});
builder.Services.ConfigureApplicationServices(builder.Configuration);
builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
