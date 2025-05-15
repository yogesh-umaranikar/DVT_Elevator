using DVT_Elevator.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVT_Elevator.Domain
{
    public interface IElevator
    {
        int Id { get; }
        int CurrentFloor { get; }
        Direction Direction { get; }
       // bool IsMoving { get; }
        int Passengers { get; }
        int Capacity { get; }

        void AddRequest(int floor, int passengers);
        void Step();
        string Status();
    }
}
