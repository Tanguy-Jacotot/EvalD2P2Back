using System.Net;
using EvalD2P2.Services.Contracts;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace EvalD2P2.Api.Functions.Event;

public class DeleteEvent
{
    private readonly ILogger _logger;
    private readonly IEventService _eventService;
    
    public DeleteEvent(ILogger<AddEvent> logger, IEventService eventService)
    {
        _logger = logger;
        _eventService = eventService;
    }
    [Function("DeleteEvent")]
    public async Task<HttpResponseData> DeleteAsync(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "event/{id}")] HttpRequestData req,
        Guid id,
        FunctionContext executionContext)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        
        var response = req.CreateResponse(HttpStatusCode.OK);
        
        try
        {
            await _eventService.DeleteEventAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            response = req.CreateResponse(HttpStatusCode.InternalServerError);
        }
        
        return response;
    }
    
}