using HPBank.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using HPBank.Helpers;
using HPBank.Repository.Interfaces;
using HPBank.Repository;
using HPBank.Repository.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddLog4Net("log4net.config");

// Add services to the container.

var MyAllowAnyOrigins = "_myAllowAnyOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowAnyOrigins, policy =>
    {
        policy.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<HPBankDBContext>();
builder.Services.AddAutoMapper(typeof(ApplicationModelMapping));
builder.Services.AddTransient<ICustomersRepository, CustomersRepository>();
builder.Services.AddTransient<IAccountsRepository, AccountsRepository>();
builder.Services.AddTransient<ITransactionsRepository, TransactionsRepository>();
builder.Services.AddTransient<IInterestRateRepository, SavingAccountService>();
builder.Services.AddTransient<IInterestRateRepository, CurrentAccountService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(MyAllowAnyOrigins);
app.UseAuthorization();

app.MapControllers();

app.Run();
