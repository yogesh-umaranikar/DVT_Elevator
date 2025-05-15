using NUnit.Framework;
using Moq;
using DVT_Elevator.Domain.Enums;

namespace DVT_Elevator.Tests
{
    [TestFixture]
    public class StandardElevatorTests
    {
        private const int ElevatorId = 1;
        private const int Capacity = 5;

        private Mock<IRequestHandler>? requestHandlerMock;
        private Mock<IMovementController>? movementControllerMock;
        private StandardElevator? elevator;

        [SetUp]
        public void Setup()
        {
            requestHandlerMock = new Mock<IRequestHandler>();
            movementControllerMock = new Mock<IMovementController>();
            elevator = new StandardElevator(ElevatorId, Capacity, requestHandlerMock.Object, movementControllerMock.Object);
        }

        [Test]
        public void AddRequest_UnderCapacity_AddsRequestAndPassengers()
        {
            // Act
            elevator.AddRequest(3, 3);

            // Assert
            Assert.That(elevator.Passengers, Is.EqualTo(3));
            requestHandlerMock.Verify(r => r.AddFloorRequest(3), Times.Once);
        }

        [Test]
        public void AddRequest_OverCapacity_DoesNotAddPassengers()
        {
            // Act
            elevator.AddRequest(3, 10);

            // Assert
            Assert.That(elevator.Passengers, Is.EqualTo(0));
            requestHandlerMock.Verify(r => r.AddFloorRequest(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void Step_WithTargetAbove_MovesUpOneFloor()
        {
            // Arrange
            requestHandlerMock.Setup(r => r.HasRequests()).Returns(true);
            requestHandlerMock.Setup(r => r.GetNextTarget(0)).Returns(3);
            movementControllerMock.Setup(m => m.CalculateDirection(0, 3)).Returns(Direction.Up);
            movementControllerMock.Setup(m => m.Move(0, Direction.Up)).Returns(1);

            // Act
            elevator.Step();

            // Assert
            Assert.That(elevator.CurrentFloor, Is.EqualTo(1));
            Assert.That(elevator.Direction, Is.EqualTo(Direction.Up));
        }

        [Test]
        public void Step_WhenArrivedAtTarget_DequeuesAndUnloads()
        {
            // Arrange
            requestHandlerMock.Setup(r => r.HasRequests()).Returns(true);
            requestHandlerMock.Setup(r => r.GetNextTarget(0)).Returns(0); // Already at target

            elevator.AddRequest(0, 2); // Load passengers

            // Act
            elevator.Step();

            // Assert
            requestHandlerMock.Verify(r => r.RemoveCurrentTarget(), Times.Once);
            Assert.That(elevator.Passengers, Is.EqualTo(0)); // Simulated unload
        }

        [Test]
        public void Step_WhenNoRequests_SetsIdle()
        {
            // Arrange
            requestHandlerMock.Setup(r => r.HasRequests()).Returns(false);

            // Act
            elevator.Step();

            // Assert
            Assert.That(elevator.Direction, Is.EqualTo(Direction.Idle));
        }

        [Test]
        public void GetStatus_ReturnsValidString()
        {
            // Arrange
            elevator.AddRequest(5, 3);

            // Act
            var status = elevator.Status();

            // Assert
            Assert.That(status, Does.Contain("Elevator"));
            Assert.That(status, Does.Contain("Floor"));
            Assert.That(status, Does.Contain("Dir"));
            Assert.That(status, Does.Contain("Passengers"));
        }
    }
}
