using ArmoredCoreSixEmblemBrowser.Data.Contexts.EmblemBrowser;
using ArmoredCoreSixEmblemBrowser.Data.Contexts.EmblemBrowser.Repositories;
using ArmoredCoreSixEmblemBrowser.Domain;
using ArmoredCoreSixEmblemBrowser.Domain.Services;
using Microsoft.EntityFrameworkCore;
using EmblemUnitOfWork = ArmoredCoreSixEmblemBrowser.Data.Contexts.EmblemBrowser.EmblemUnitOfWork;

var builder = WebApplication.CreateBuilder(args);

var isDev = builder.Environment.IsDevelopment();

var connectionString = builder.Configuration.GetConnectionString("EmblemBrowser");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<EmblemBrowserContext>(optionsAction =>
{
    if (isDev)
    {
        optionsAction.EnableDetailedErrors();
        optionsAction.EnableSensitiveDataLogging();
    }

    //for now
    optionsAction.UseNpgsql(connectionString, settings =>
    {
        settings.MigrationsAssembly("ArmoredCoreSixEmblemBrowser.Web");
    });
});

builder.Services.AddScoped<IEmblemRepository, EmblemRepository>();
builder.Services.AddScoped<EmblemUnitOfWork>();
builder.Services.AddScoped<IEmblemBrowserService, EmblemBrowserService>();
//use odata for search eventually
//builder.Services.AddOdata();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();