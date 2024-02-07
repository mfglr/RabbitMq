using Microsoft.EntityFrameworkCore;
using RabbitMq.Watermark.BackgroundServices;
using RabbitMq.Watermark.Models;
using RabbitMq.Watermark.Services;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<Context>( x => x.UseInMemoryDatabase("RabbitMq"));
builder.Services.AddSingleton( serviceProvider => new ConnectionFactory() { HostName = "localhost", DispatchConsumersAsync = true } );
builder.Services.AddSingleton(
	serviceProvider =>
	{
		var connectionFactory = serviceProvider.GetRequiredService<ConnectionFactory>();
		var client = new RabbitMQPublisher(connectionFactory);
		client.Connect();
		return client;
	}
);
builder.Services.AddHostedService<WaterMarkBackgroundService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
