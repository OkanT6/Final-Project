using EruKampusSpor.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// CORS yap�land�rmas�n� ekle
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("https://192.168.1.10") // Buraya arkada��n�z�n IP adresini veya frontend'in adresini ekleyin.
        //policy.AllowAnyOrigin() // T�m k�kenlerden gelen istekleri kabul eder.
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext with SQL Server connection string
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);




//// Kestrel server yap�land�rmas�n� g�ncelleyin
//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.Listen(System.Net.IPAddress.Any, 5000);  // 5000 portunu t�m IP'lerden dinler
//});

builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(System.Net.IPAddress.Any, 5000); // HTTP
    options.Listen(System.Net.IPAddress.Any, 5001, listenOptions => // HTTPS
    {
        listenOptions.UseHttps();
    });
});

var app = builder.Build();

// CORS'� etkinle�tir
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // https redirection'� a��k b�rak�yoruz
app.UseAuthorization(); // Authorization middleware'�n� ekliyoruz

app.MapControllers(); // Controller'lar� e�liyoruz

app.Run();
