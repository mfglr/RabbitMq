using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RabbitMq.Excel.Models;
using RabbitMq.Excel.Services;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton(
	sp => new ConnectionFactory() { HostName = "localhost" }
);
builder.Services.AddSingleton(sp => {
		var connectionFactory = sp.GetRequiredService<ConnectionFactory>();
		var publisher = new RabbitMQPublisher(connectionFactory);
		publisher.Connect();
		return publisher;
	}
);

builder.Services.AddDbContext<AppDbContext>(
	(sp, options) => {
		var configuration = sp.GetRequiredService<IConfiguration>();
		options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
	}
);

builder.Services
	.AddIdentity<IdentityUser, IdentityRole>(
		opt => {
			opt.User.RequireUniqueEmail = true;
		}
	)
	.AddEntityFrameworkStores<AppDbContext>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	//Migration
	var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
	context.Database.Migrate();

	//add some users if they dont exist
	if (!context.Users.Any())
	{
		var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
		userManager
			.CreateAsync(
				new IdentityUser() { UserName = "test0", Email = "test0@outlook.com" },
				"123456789Mfg."
			)
			.Wait();

		userManager.
			CreateAsync(
				new IdentityUser() { UserName = "test1", Email = "test1@outlook.com" },
				"123456789Mfg."
			)
			.Wait();
	}

    //initializing rabbitMQ connection;
    var publihser = scope.ServiceProvider.GetRequiredService<RabbitMQPublisher>();
	try
	{
        publihser.Connect();
		Console.WriteLine("Initialized the connection of RabbitMQ");
    }
    catch ( Exception ex )
	{
		Console.WriteLine( "There have been some errors When initializing the connection of RabbitMQ" );
	}


}

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
