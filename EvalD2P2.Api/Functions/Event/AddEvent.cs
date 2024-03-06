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
    
    [Function("UpdateEvent")]
    public async Task<HttpResponseData> UpdateAsync(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "event/{id}")] HttpRequestData req,
        Guid id,
        FunctionContext executionContext)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        
        var response = req.CreateResponse(HttpStatusCode.OK);
        
        try
        {
            var content = await new StreamReader(req.Body).ReadToEndAsync();
            var @event = JsonSerializer.Deserialize<Entities.Event>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            @event.IdEvent = id;
            await _eventService.UpdateEventAsync(@event);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            response = req.CreateResponse(HttpStatusCode.InternalServerError);
        }
        
        return response;
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