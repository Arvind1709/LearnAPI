using LearnAPI.AppDbContext;
using LearnAPI.Repositories.IRepository;
using LearnAPI.Repositories.Repository;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Database Connection
builder.Services.AddDbContext<BookNookDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDb")));
// Dependencies --
builder.Services.AddSingleton<IStateRepository, StateRepository>();
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
