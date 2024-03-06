using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EvalD2P2.Api.Functions.Event;
using EvalD2P2.Entities;
using EvalD2P2.Services.Contracts;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Moq;


[TestClass]
public class EventFunctionTests
{
    private Mock<ILogger<AddEvent>> _loggerMock;
    private Mock<IEventService> _eventServiceMock;
    private Mock<HttpRequestData> _httpRequestMock;
    private Mock<FunctionContext> _functionContextMock;

    [TestInitialize]
    public void Initialize()
    {
        _loggerMock = new Mock<ILogger<AddEvent>>();
        _eventServiceMock = new Mock<IEventService>();
        _httpRequestMock = new Mock<HttpRequestData>(MockBehavior.Strict, new Mock<FunctionContext>().Object);
        _functionContextMock = new Mock<FunctionContext>();
    }

    [TestMethod]
    public async Task AddEvent_ShouldReturn200_WhenEventIsAddedSuccessfully()
    {
        // Arrange
        var memoryStream =
            new MemoryStream(Encoding.UTF8.GetBytes("{\"Title\":\"Test Event\",\"Description\":\"Test Description\"}"));
        var responseMock = new Mock<HttpResponseData>(MockBehavior.Strict, _functionContextMock.Object);
        responseMock.Setup(r => r.StatusCode).Returns(HttpStatusCode.OK);
        _httpRequestMock.Setup(r => r.Body).Returns(memoryStream);
        _httpRequestMock.Setup(r => r.CreateResponse()).Returns(responseMock.Object);

        _eventServiceMock.Setup(service => service.AddEventAsync(It.IsAny<Event>())).Returns(Task.CompletedTask);

        var function = new AddEvent(_loggerMock.Object, _eventServiceMock.Object);

        // Act
        var response = await function.RunAsync(_httpRequestMock.Object, _functionContextMock.Object);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        _eventServiceMock.Verify(service => service.AddEventAsync(It.IsAny<Event>()), Times.Once);
    }

    [TestMethod]
    public async Task AddEvent_ShouldReturn500_WhenEventIsNotAddedSuccessfully()
    {
        // Arrange
        var memoryStream =
            new MemoryStream(Encoding.UTF8.GetBytes("{\"Title\":\"Test Event\",\"Description\":\"Test Description\"}"));
        var responseMock = new Mock<HttpResponseData>(MockBehavior.Strict, _functionContextMock.Object);
        responseMock.Setup(r => r.StatusCode).Returns(HttpStatusCode.InternalServerError);
        _httpRequestMock.Setup(r => r.Body).Returns(memoryStream);
        _httpRequestMock.Setup(r => r.CreateResponse()).Returns(responseMock.Object);

        _eventServiceMock.Setup(service => service.AddEventAsync(It.IsAny<Event>()))
            .Throws(new Exception("Test Exception"));

        var function = new AddEvent(_loggerMock.Object, _eventServiceMock.Object);

        // Act
        var response = await function.RunAsync(_httpRequestMock.Object, _functionContextMock.Object);

        // Assert
        Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        _eventServiceMock.Verify(service => service.AddEventAsync(It.IsAny<Event>()), Times.Once);
    }
}

   
