using ToDoListApi.Services;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using ToDoListApi.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.Extensions.Options;

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
builder.Services.AddLocalization();
builder.Services.AddScoped<IStringLocalizer, StringLocalizerService>();


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

app.UseHttpsRedirection();

app.MapControllers();

app.UseRequestLocalization("uk-UA");

app.Run();

