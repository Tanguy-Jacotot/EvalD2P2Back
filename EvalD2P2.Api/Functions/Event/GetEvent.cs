using System.Net;
using System.Text.Json;
using EvalD2P2.Services.Contracts;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace EvalD2P2.Api.Functions.Event;

public class GetEvent
{
    private readonly ILogger _logger;
    private readonly IEventService _eventService;
    
    public GetEvent(ILogger<AddEvent> logger, IEventService eventService)
    {
        _logger = logger;
        _eventService = eventService;
    }
    
    [Function("GetAll")]
    public async Task<HttpResponseData> GetAllAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "event")] HttpRequestData req,
        FunctionContext executionContext)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        
        var response = req.CreateResponse(HttpStatusCode.OK);
        
        try
        {
            var events = await _eventService.GetAllEventsAsync();
            var json = JsonSerializer.Serialize(events);
            await response.WriteStringAsync(json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            response = req.CreateResponse(HttpStatusCode.InternalServerError);
        }
        
        return response;
    }
    
    
    [Function("GetById")]
    public async Task<HttpResponseData> GetByIdAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "event/{id}")] HttpRequestData req,
        Guid id,
        FunctionContext executionContext)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        
        var response = req.CreateResponse(HttpStatusCode.OK);
        
        try
        {
            var @event = await _eventService.GetEventByIdAsync(id);
            var json = JsonSerializer.Serialize(@event);
            await response.WriteStringAsync(json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            response = req.CreateResponse(HttpStatusCode.InternalServerError);
        }
        
        return response;
    }
    
}