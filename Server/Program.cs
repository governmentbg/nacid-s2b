using AbOcr.Extensions;
using FileStorageNetCore;
using Hangfire;
using Infrastructure.AppSettings;
using ProxyKit;
using Server.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers()
    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
AppSettingsProvider.AddAppSettings(builder.Configuration);
builder.Services.ConfigureDbContextService();
builder.Services.ConfigureHangfire();
builder.Services.AddAbOrc(builder.Configuration.GetSection("OcrConfigurations"));
builder.Services.ConfigureRepositories();
builder.Services.ConfigureServices();
builder.Services.ConfigureAutoMapper();
builder.Services.ConfigureJobs();
builder.Services.ConfigureOpenIddict();
builder.Services.ConfigureSolr();
builder.Services.AddSignalR();
builder.Services.AddCors(options => {
    options.AddPolicy("CORSPolicy", builder => builder.AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed((hosts) => true));
});
builder.Services.AddFileStorage(builder.Configuration.GetSection("fileStorages"), builder.Configuration.GetSection("fileStoragesEncryption"));
builder.Services.AddProxy();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

app.UseCors("CORSPolicy");
app.UseHttpsRedirection();
app.UseHangfireDashboard();

app.UseAuthentication();
app.UseAuthorization();

app.ConfigureMiddlewares();

app.ConfigureStaticFiles();

app.ConfigureSignalRHubs();

app.MapControllers();

app.ConfigureProxy();

app.Run();
