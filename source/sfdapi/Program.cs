using System.Text.Json.Serialization;
using Slickflow.AI.Service;
using Slickflow.AI.Configuration;
using Slickflow.Engine.Service;
using sfdapi.Services;

var builder = WebApplication.CreateBuilder(args);


// Services
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.PropertyNamingPolicy = null;
        o.JsonSerializerOptions.DictionaryKeyPolicy = null;
        o.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        o.JsonSerializerOptions.MaxDepth = 1000;
    });

builder.Services.AddCors(o => o.AddPolicy("MyCorsPolicy", corsBuilder =>
{
    corsBuilder.WithOrigins("http://localhost:5000", "http://localhost:3000")
               .AllowAnyMethod()
               .AllowAnyHeader();
}));

// Initialize DB connection string
var configuration = builder.Configuration;
var dbType = ConfigurationExtensions.GetConnectionString(configuration, "WfDBConnectionType");
var sqlConnectionString = ConfigurationExtensions.GetConnectionString(configuration, "WfDBConnectionString");
Slickflow.Data.DBTypeExtenstions.InitConnectionString(dbType, sqlConnectionString);

// Bind AIService section into options (QianWen/OpenAI)
var aiOptions = AiAppConfigProviderOptions.Load(configuration);
builder.Services.AddSingleton(aiOptions);
ApiKeyCryptoHelper.Configure(aiOptions);

builder.Services.AddScoped<IAiModelDataService, AiModelDataService>();
builder.Services.AddScoped<IAiFastCallingService, AiFastCallingService>();
builder.Services.AddScoped<IWorkflowService, WorkflowService>();
builder.Services.AddScoped<SfBpmnServiceBuilder>();

builder.Services.AddControllers();

var app = builder.Build();


// CORS must be after UseRouting and before MapEndpoints
app.UseCors("MyCorsPolicy");

app.UseAuthorization();

app.MapControllerRoute(
    name: "defaultApi",
    pattern: "api/{controller}/{action}/{id?}");

app.MapControllers();

app.Run();
