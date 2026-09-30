using Azure.Identity;
using EmployeeAccountFunction.Interfaces;
using EmployeeAccountFunction.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Graph;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();


builder.Services.AddSingleton<GraphServiceClient>(_ =>
{
    var credentials = new DefaultAzureCredential();
    return new GraphServiceClient(credentials, new[] { "https://graph.microsoft.com/.default" });
});

builder.Services.AddTransient<IEntraUserService, EntraUserService>();

builder.Build().Run();
