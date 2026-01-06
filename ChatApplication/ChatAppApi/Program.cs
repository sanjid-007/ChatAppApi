

using ChatApp.Domain.Interfaces;
using ChatApp.Infrastructure.Repositories;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
var dbProvider = builder.Configuration["DbSettings:Provider"];
switch (dbProvider)
{
    case "MongoDb":
        builder.Services.AddSingleton<IMongoClient>(sp => {
            var connectionString = builder.Configuration["DbSettings:MongoConnection:Connection"];
            return new MongoClient(connectionString);
        });
        builder.Services.AddSingleton<IMongoDatabase>(sp => { 
            var client = sp.GetRequiredService<IMongoClient>();
            var databaseName = builder.Configuration["DbSettings:MongoConnection:DbName"];
            return client.GetDatabase(databaseName);
        });
        break;
}

builder.Services.AddScoped<IMessageRepository, MongoMessageRepositories>();

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
