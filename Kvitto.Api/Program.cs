using Kvitto.Api.Extensions;
using Kvitto.Core.Repositories;
using Kvitto.Data;
using Kvitto.Data.Data;
using Kvitto.Data.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<KvittoApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("KvittoApiContext") ?? throw new InvalidOperationException("Connection string 'KvittoApiContext' not found.")));

// Add services to the container.
builder.Services.AddScoped<IUoW, UoW>();

builder.Services.AddControllers(opt => opt.ReturnHttpNotAcceptable = true)
    .AddNewtonsoftJson()
    .AddXmlDataContractSerializerFormatters();
builder.Services.AddAutoMapper(typeof(KvittoMappings));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.SeedDataAsync().GetAwaiter().GetResult();

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
