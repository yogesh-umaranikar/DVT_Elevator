using Moq;
using Xunit;


public class StandardElevatorTests
{
    private const int ElevatorId = 1;
    private const int Capacity = 5;

    [Fact]
    public void AddRequest_ShouldAddPassengerAndRequest_WhenUnderCapacity()
    {
        // Arrange
        var requestHandlerMock = new Mock<IRequestHandler>();
        var movementControllerMock = new Mock<IMovementController>();
        var elevator = new StandardElevator(ElevatorId, Capacity, requestHandlerMock.Object, movementControllerMock.Object);

        // Act
        elevator.AddRequest(floor: 3, passengers: 3);

        // Assert
        Assert.Equal(3, elevator.Passengers);
        requestHandlerMock.Verify(r => r.AddFloorRequest(3), Times.Once);
    }

    [Fact]
    public void AddRequest_ShouldNotAddPassenger_WhenOverCapacity()
    {
        // Arrange
        var requestHandlerMock = new Mock<IRequestHandler>();
        var movementControllerMock = new Mock<IMovementController>();
        var elevator = new StandardElevator(ElevatorId, Capacity, requestHandlerMock.Object, movementControllerMock.Object);

        // Act
        elevator.AddRequest(floor: 3, passengers: 6); // Exceeds capacity

        // Assert
        Assert.Equal(0, elevator.Passengers);
        requestHandlerMock.Verify(r => r.AddFloorRequest(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void Step_ShouldSetDirectionAndMove_WhenTargetAbove()
    {
        // Arrange
        var requestHandlerMock = new Mock<IRequestHandler>();
        requestHandlerMock.Setup(r => r.HasRequests()).Returns(true);
        requestHandlerMock.Setup(r => r.GetNextTarget(0)).Returns(3);

        var movementControllerMock = new Mock<IMovementController>();
        movementControllerMock.Setup(m => m.CalculateDirection(0, 3)).Returns(Direction.Up);
        movementControllerMock.Setup(m => m.Move(0, Direction.Up)).Returns(1);

        var elevator = new StandardElevator(ElevatorId, Capacity, requestHandlerMock.Object, movementControllerMock.Object);

        // Act
        elevator.Step();

        // Assert
        Assert.Equal(Direction.Up, elevator.Direction);
        Assert.Equal(1, elevator.CurrentFloor);
    }

    [Fact]
    public void Step_ShouldDequeueRequest_WhenOnTargetFloor()
    {
        // Arrange
        var requestHandlerMock = new Mock<IRequestHandler>();
        requestHandlerMock.Setup(r => r.HasRequests()).Returns(true);
        requestHandlerMock.Setup(r => r.GetNextTarget(2)).Returns(2);

        var movementControllerMock = new Mock<IMovementController>();

        var elevator = new StandardElevator(ElevatorId, Capacity, requestHandlerMock.Object, movementControllerMock.Object);
        elevator.AddRequest(2, 2); // bring in passengers and a request
        elevator.Step(); // should hit the target floor

        // Act
        elevator.Step();

        // Assert
        requestHandlerMock.Verify(r => r.RemoveCurrentTarget(), Times.Once);
        Assert.Equal(0, elevator.Passengers); // Simulated exit
    }

    [Fact]
    public void Step_ShouldSetDirectionToIdle_WhenNoRequests()
    {
        // Arrange
        var requestHandlerMock = new Mock<IRequestHandler>();
        requestHandlerMock.Setup(r => r.HasRequests()).Returns(false);

        var movementControllerMock = new Mock<IMovementController>();

        var elevator = new StandardElevator(ElevatorId, Capacity, requestHandlerMock.Object, movementControllerMock.Object);

        // Act
        elevator.Step();

        // Assert
        Assert.Equal(Direction.Idle, elevator.Direction);
    }

    [Fact]
    public void GetStatus_ShouldReturnFormattedString()
    {
        // Arrange
        var requestHandlerMock = new Mock<IRequestHandler>();
        var movementControllerMock = new Mock<IMovementController>();

        var elevator = new StandardElevator(ElevatorId, Capacity, requestHandlerMock.Object, movementControllerMock.Object);
        elevator.AddRequest(2, 2);
        elevator.Step();

        // Act
        string status = elevator.GetStatus();

        // Assert
        Assert.Contains("Elevator 1", status);
        Assert.Contains("Floor:", status);
        Assert.Contains("Dir:", status);
        Assert.Contains("Passengers:", status);
    }
}
