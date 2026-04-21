using a_private_bank_main.Contracts;
using a_private_bank_main.Models;
using a_private_bank_main.Resources;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPersonResourceContracts, GetPerson>();
builder.Services.AddScoped<IDataResourceContracts, ImportData>();
builder.Services.AddDbContext<AprivateBankContext>(options =>
    options.UseSqlServer((builder.Configuration.GetConnectionString("DefaultConnection"))));

var app = builder.Build();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = ""; 
    });
}

    //app.UseHttpsRedirection();

    app.UseAuthorization();


    app.MapControllers();

    app.Run();


// Configure the HTTP request pipeline.

