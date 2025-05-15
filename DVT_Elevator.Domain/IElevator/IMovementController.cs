using DVT_Elevator.Domain.Enums;

/// <summary>
/// Controls movement logic of the elevator (direction and travel).
/// </summary>
public interface IMovementController
{
    Direction CalculateDirection(int currentFloor, int targetFloor);
    int Move(int currentFloor, Direction direction);
}