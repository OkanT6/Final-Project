using EruKampusSpor.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// CORS yapýlandýrmasýný ekle
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("https://192.168.1.10") // Buraya arkadaþýnýzýn IP adresini veya frontend'in adresini ekleyin.
        //policy.AllowAnyOrigin() // Tüm kökenlerden gelen istekleri kabul eder.
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




//// Kestrel server yapýlandýrmasýný güncelleyin
//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.Listen(System.Net.IPAddress.Any, 5000);  // 5000 portunu tüm IP'lerden dinler
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

// CORS'ý etkinleþtir
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // https redirection'ý açýk býrakýyoruz
app.UseAuthorization(); // Authorization middleware'ýný ekliyoruz

app.MapControllers(); // Controller'larý eþliyoruz

app.Run();
