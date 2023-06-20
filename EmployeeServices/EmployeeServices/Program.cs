using EmployeeServices.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var NorthwindconnectionString = builder.Configuration.GetConnectionString("Northwind");
builder.Services.AddDbContext<NorthwindContext>(options =>
    options.UseSqlServer(NorthwindconnectionString));

builder.Services.AddControllers();

//定義CROS策略
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

//套用CROS策略，在Program.cs裡面會套用到所有的Controller
//app.UseCors(MyAllow);
//套用CROS策略，如果只有個別開放，app.UseCors()，括號裏面不加東西，在Controller做設定
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
