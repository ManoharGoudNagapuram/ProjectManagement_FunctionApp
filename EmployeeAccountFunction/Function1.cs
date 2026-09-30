using Azure.Messaging.ServiceBus;
using EmployeeAccountFunction.Interfaces;
using EmployeeAccountFunction.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmployeeAccountFunction;

public class Function1
{
    private readonly ILogger<Function1> _logger;
    private readonly IEntraUserService _entraUserService;

    public Function1(ILogger<Function1> logger, IEntraUserService entraUserService)
    {
        _logger = logger;
        _entraUserService = entraUserService;
    }

    [Function(nameof(Function1))]
    public async Task Run(
        [ServiceBusTrigger("employee-account-queue", Connection = "ServiceBusConnection")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Message ID: {id}", message.MessageId);
        _logger.LogInformation("Message Body: {body}", message.Body);
        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

       var reqModel= JsonSerializer.Deserialize<UserModel>(message.Body);

        //todo: check duplicate user before sending to create in entra id
        if (reqModel != null)
        {
            string domainName = "manoharnagapuramoutlook.onmicrosoft.com";
            var displayName= $"{reqModel.FirstName} {reqModel.LastName}";
            var mailNickname = $"{reqModel.FirstName}.{reqModel.LastName}";
            var upn = $"{reqModel.FirstName}.{reqModel.LastName}@{domainName}";
            var tempPassword = reqModel.LastName[0].ToString().ToUpper()+ reqModel.LastName.Substring(1) + "@"+Guid.NewGuid().ToString("N").Substring(0, 8);

           var result= await _entraUserService.CreateUserAsync(displayName, upn, mailNickname, tempPassword);
        }

        // Complete the message
        await messageActions.CompleteMessageAsync(message); 
    }
}