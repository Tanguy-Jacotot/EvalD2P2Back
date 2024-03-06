using System.Net;
using System.Text.Json;
using EvalD2P2.Services.Contracts;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace EvalD2P2.Api.Functions.Event;

public class AddEvent
{
    private readonly ILogger _logger;
    private readonly IEventService _eventService;
    
    public AddEvent(ILogger<AddEvent> logger, IEventService eventService)
    {
        _logger = logger;
        _eventService = eventService;
    }
    
    [Function("AddEvent")]
    public async Task<HttpResponseData> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "event")] HttpRequestData req,
        FunctionContext executionContext)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        
        var response = req.CreateResponse(HttpStatusCode.OK);
        
        try
        {
            var content = await new StreamReader(req.Body).ReadToEndAsync();
            var @event = JsonSerializer.Deserialize<Entities.Event>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            await _eventService.AddEventAsync(@event);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            response = req.CreateResponse(HttpStatusCode.InternalServerError);
        }
        
        return response;
    }
    
}