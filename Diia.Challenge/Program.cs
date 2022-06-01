using Diia.Challenge;
using Diia.Challenge.DAL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationContext, ApplicationContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection")));
builder.Services.AddScoped<ApplicationDataReader, ApplicationDataReaderSql>();
builder.Services.AddScoped<IConfigurationDataReader, ConfigurationDataReaderJson>();
builder.Services.AddTransient<AddressValidator, AddressValidator>();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

using (var context = new ApplicationContext(
    new DbContextOptionsBuilder<ApplicationContext>().
    UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection")).Options))
{
    context.Database.EnsureCreated();
}

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

public partial class Program{}
