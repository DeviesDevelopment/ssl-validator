using Microsoft.AspNetCore.ResponseCompression;

using SSLValidator.Server.Hubs;
using SSLValidator.Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddResponseCompression(opts =>
{
	opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
		new[] { "application/octet-stream" });
});
builder.Services.AddStackExchangeRedisCache(opt =>
{
	opt.Configuration = builder.Configuration.GetConnectionString("redis");
	opt.InstanceName = "sslValidator_";
});
builder.Services.AddHostedService<UpdateDomains>();

var app = builder.Build();

app.UseResponseCompression();

if (app.Environment.IsDevelopment())
{
	app.UseWebAssemblyDebugging();
}
else
{
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}

//app.UseHttpsRedirection();
app.UsePathBase("http://localhost:5224");
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapHub<DomainHub>("/domainhub");
app.MapFallbackToFile("index.html");

app.Run();
