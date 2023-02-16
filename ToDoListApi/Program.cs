using ToDoListApi.Services;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen("v1", "Todo List API");
builder.Services.AddProblemDetails();
builder.Services.AddSingleton<ITodoItemService, TodoItemService>();
builder.Services.AddAuthentication("localhost", "localhost", "your-secret-key-that-must-be-at-least-16-characters-long");    
builder.Services.AddAuthorization("Bearer");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error-development");
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseRequestLocalization("uk");

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

