public class QueueRequestHandler : IRequestHandler
{
    private readonly Queue<int> _targetFloors = new();

    public void AddFloorRequest(int floor)
    {
        if (!_targetFloors.Contains(floor))
            _targetFloors.Enqueue(floor);
    }

    public bool HasRequests() => _targetFloors.Count > 0;

    public int? GetNextTarget(int currentFloor)
    {
        return _targetFloors.Count > 0 ? _targetFloors.Peek() : null;
    }

    public void RemoveCurrentTarget()
    {
        if (_targetFloors.Count > 0)
            _targetFloors.Dequeue();
    }
}