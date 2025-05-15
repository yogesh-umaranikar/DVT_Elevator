using DVT_Elevator.Domain.Enums;
using DVT_Elevator.Domain;

/// <summary>
/// A concrete implementation of Elevator with basic movement, queuing logic and returning Status.
/// </summary>
public class StandardElevator : IElevator
{
    public int Id { get; }
    public int CurrentFloor { get; private set; }
    public Direction Direction { get; private set; } = Direction.Idle;
    public int Capacity { get; }
    public int Passengers { get; private set; }

    private readonly IRequestHandler _requestHandler;
    private readonly IMovementController _movementController;

    public StandardElevator(int id, int capacity,
        IRequestHandler requestHandler,
        IMovementController movementController)
    {
        Id = id;
        Capacity = capacity;
        _requestHandler = requestHandler;
        _movementController = movementController;
    }

    /// <summary>
    /// Adds a floor request and passengers, based on the capacity.
    /// </summary>
    public void AddRequest(int floor, int passengers)
    {
        if (Passengers + passengers > Capacity)
        {
            Console.WriteLine($"Elevator {Id}: Over capacity");
            return;
        }

        Passengers += passengers;
        _requestHandler.AddFloorRequest(floor);
    }

    /// <summary>
    /// Executes a single operational step:
    /// - In case of no requests, the elevator is idle.
    /// - If on a target floor, unload and dequeue.
    /// - Otherwise, move one floor toward the target.
    /// </summary>
    public void Step()
    {
        if (!_requestHandler.HasRequests())
        {
            Direction = Direction.Idle;
            return;
        }

        var target = _requestHandler.GetNextTarget(CurrentFloor);
        if (!target.HasValue) return;

        if (CurrentFloor == target.Value)
        {
            _requestHandler.RemoveCurrentTarget();
            Passengers = 0; // simulate exit
        }
        else
        {
            Direction = _movementController.CalculateDirection(CurrentFloor, target.Value);
            CurrentFloor = _movementController.Move(CurrentFloor, Direction);
        }
    }

    /// <summary>
    /// Returns a status of the elevator.
    /// </summary>
    public string Status()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        return $"Elevator {Id} with Capacity {Capacity} | Current Floor: {CurrentFloor} | Dir: {Direction} | Passengers: {Passengers}";
    }
}

