using System.Data;
using System.Net;
using System.Text.Json.Serialization;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using AspNetCore.ExceptionHandler;
using MangaMagnet.Api.Configurations;
using MangaMagnet.Api.Job;
using MangaMagnet.Api.Middlewares;
using MangaMagnet.Api.Models.Database;
using MangaMagnet.Api.Service;
using MangaMagnet.Api.Swagger;
using MangaMagnet.Core.Database;
using MangaMagnet.Core.Metadata;
using MangaMagnet.Core.Metadata.Providers.MangaDex;
using MangaMagnet.Core.Progress;
using MangaMagnet.Core.Progress.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Quartz;
using Quartz.Impl.AdoJobStore;
using Serilog;
using Serilog.Enrichers;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

var builder = WebApplication.CreateBuilder(args);

const string logTemplate =
    "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}\n{Exception}";

builder.Host.UseSerilog((_, _, loggerConfiguration) =>
{
    var loggerConfig = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
            true)
        .Build();

    loggerConfiguration
        .ReadFrom.Configuration(loggerConfig)
        .Enrich.WithThreadId()
        .Enrich.WithThreadName()
        .Enrich.WithProperty(ThreadNameEnricher.ThreadNamePropertyName, "Main")
        .Enrich.FromLogContext()
        .WriteTo.Console(
            outputTemplate: logTemplate,
            theme: SystemConsoleTheme.Colored);
});

var handler = new HttpClientHandler();
if (handler.SupportsAutomaticDecompression)
{
    handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli;
}

var httpClient = new HttpClient(handler);

builder.Services.AddSingleton(httpClient);
builder.Services.AddSingleton<MangaDexApiService>();
builder.Services.AddSingleton<MangaDexConverterService>();
builder.Services.AddSingleton<IMetadataFetcher, MetadataFetcherService>();
builder.Services.AddSingleton<EntityConverterService>();
builder.Services.AddHostedService<BroadcastProgressService>();
builder.Services.AddScoped<MangaService>();
builder.Services.AddScoped<MetadataService>();

// Progress services
builder.Services.AddSingleton<ProgressService>();
builder.Services.AddSingleton<WebSocketService>();
builder.Services.AddSingleton<WebSocketMiddleware>();

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiVersioning(o =>
{
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new ApiVersion(1, 0);
    o.ReportApiVersions = true;
    o.ApiVersionReader = ApiVersionReader.Combine(
        new HeaderApiVersionReader("X-Version"),
        new QueryStringApiVersionReader("v"),
        new UrlSegmentApiVersionReader());
})
    .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

builder.Services.AddSwaggerGen(c =>
{
	c.SchemaFilter<NullabilitySchemaFilter>();
	c.DocumentFilter<CustomModelDocumentFilter<ProgressTask>>();
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyOrigin()
        .AllowAnyHeader()
    );
});

builder.Services.UseExceptionBasedErrorHandling();

builder.Services.AddHealthChecks();

var dataSourceBuilder = new NpgsqlDataSourceBuilder(builder.Configuration.GetConnectionString("PostgreSQLConnection"));
dataSourceBuilder.MapEnum<MangaStatus>();
var dataSource = dataSourceBuilder.Build();

builder.Services.AddDbContext<BaseDatabaseContext, MangaMagnetDatabaseContext>(options => options
    .UseNpgsql(dataSource, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
    .UseSnakeCaseNamingConvention());

builder.Services.AddQuartz(q =>
{
    q.UsePersistentStore(o =>
    {
        o.UsePostgres(postgresOptions =>
        {
            postgresOptions.UseDriverDelegate<PostgreSQLDelegate>();
            postgresOptions.ConnectionString = builder.Configuration.GetConnectionString("QuartzPostgresSQLConnection")!;
            postgresOptions.TablePrefix = "qrtz_";
        });

        o.UseNewtonsoftJsonSerializer();
    });

    var updateMetadataKey = new JobKey("UpdateMetadataJob", "MangaMagnet");
    q.AddJob<UpdateMetadataJob>(updateMetadataKey);

    q.AddTrigger(t => t
		.WithIdentity("UpdateMetadataTrigger", "MangaMagnet")
		.ForJob(updateMetadataKey)
		.WithCronSchedule("0 0 * * * ?"));
});
builder.Services.AddQuartzHostedService(q =>
{
    q.WaitForJobsToComplete = true;
    q.AwaitApplicationStarted = true;
});

var app = builder.Build();

using (var scope = app.Services.GetService<IServiceScopeFactory>()?.CreateScope())
{
    var logger = app.Services.GetRequiredService<ILogger<MangaMagnetDatabaseContext>>();
    var dbContext = scope?.ServiceProvider.GetRequiredService<MangaMagnetDatabaseContext>();

    try
    {
        dbContext?.Database.Migrate();

        if (dbContext?.Database.GetDbConnection() is NpgsqlConnection npgsqlConnection)
        {
	        if (npgsqlConnection.State != ConnectionState.Open)
	        {
		        await npgsqlConnection.OpenAsync();
	        }

            try
            {
                await npgsqlConnection.ReloadTypesAsync();
            }
            finally
            {
                await npgsqlConnection.CloseAsync();
            }
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while running migrations on the database");
        throw;
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());

        options.DisplayOperationId();
        options.EnablePersistAuthorization();
        options.EnableDeepLinking();
    });
}

app.UseSerilogRequestLogging(opt => opt.GetLevel = (_, _, _) => LogEventLevel.Debug);

app.UseCors();

app.UseWebSockets();
app.UseMiddleware<WebSocketMiddleware>();
app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/_status/healthz");
app.MapHealthChecks("/_status/ready");

app.Run();
