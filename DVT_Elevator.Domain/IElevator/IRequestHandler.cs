/// <summary>
/// Handles logic related to tracking and managing floor requests.
/// </summary>
public interface IRequestHandler
{
    void AddFloorRequest(int floor);
    bool HasRequests();
    int? GetNextTarget(int currentFloor);
    void RemoveCurrentTarget();
}