using DVT_Elevator.Domain.Enums;

public class SimpleMovementController : IMovementController
{
    public Direction CalculateDirection(int currentFloor, int targetFloor)
    {
        if (targetFloor > currentFloor) return Direction.Up;
        if (targetFloor < currentFloor) return Direction.Down;
        return Direction.Idle;
    }

    public int Move(int currentFloor, Direction direction)
    {
        return direction switch
        {
            Direction.Up => currentFloor + 1,
            Direction.Down => currentFloor - 1,
            _ => currentFloor
        };
    }
}