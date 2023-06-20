using EmployeeServices.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var NorthwindconnectionString = builder.Configuration.GetConnectionString("Northwind");
builder.Services.AddDbContext<NorthwindContext>(options =>
    options.UseSqlServer(NorthwindconnectionString));

builder.Services.AddControllers();

//�w�qCROS����
string MyAllow = "MyAllow";
builder.Services.AddCors(options => {
    options.AddPolicy(name: MyAllow,policy=>policy.WithOrigins("*").WithHeaders("*").WithMethods("*"));
});


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

//�M��CROS�����A�bProgram.cs�̭��|�M�Ψ�Ҧ���Controller
//app.UseCors(MyAllow);
//�M��CROS�����A�p�G�u���ӧO�}��Aapp.UseCors()�A�A���ح����[�F��A�bController���]�w
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
