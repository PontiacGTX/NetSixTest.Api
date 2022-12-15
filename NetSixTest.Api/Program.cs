using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NetSixTest.Data;
using NetSixTest.Data.Entity;
using NetSixTest.Data.Models;
using NetSixTest.Services.Services;
using NetSixTest.Services.Validation;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.ReferenceHandler  = ReferenceHandler.IgnoreCycles;
    o.JsonSerializerOptions.WriteIndented = true;
}).AddFluentValidation(options =>
{
    
    options.ImplicitlyValidateChildProperties = true;
    options.ImplicitlyValidateRootCollectionElements = true;

    options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
});
builder.Services.AddDbContext<AppDbContext>(o =>
{
    o.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));

});

builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ProductServices>();
builder.Services.AddScoped<IValidator<ProductModel>,ProductValidator>();
builder.Services.AddScoped<IValidator<CategoryModel>,CategoryValidator>();

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
using var scope = app.Services.CreateScope();
using (AppDbContext ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>())
{
    
    await ctx.Database.EnsureCreatedAsync();
    await ctx.Database.MigrateAsync();
}
app.UseCors(option => option.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
