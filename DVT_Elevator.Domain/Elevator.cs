using DVT_Elevator.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVT_Elevator.Domain
{
    /// <summary>
    /// Abstract base class representing a generic elevator.
    /// Enforces core functionality that all elevator types must implement.
    /// </summary>
    public abstract class Elevator
    {
        protected int Id { get; }
        protected int CurrentFloor { get; set; }
        protected Direction Direction { get; set; } = Direction.Idle;
        protected bool IsMoving => Direction != Direction.Idle;
        protected int Capacity { get; }
        protected int Passengers { get; set; }

        private Queue<int> targetFloors = new();

        public Elevator(int id, int capacity)
        {
            Id = id;
            Capacity = capacity;
        }

        /// <summary>
        /// Handle a request to move to a specific floor with a certain number of passengers.
        /// </summary>
        public abstract void AddRequest(int floor, int passengers);

        /// <summary>
        /// Advance the elevator's state by one unit.
        /// </summary>
        public abstract void Step();

        /// <summary>
        /// Return a string representing the elevator's current state.
        /// </summary>
        public abstract string Status();
    }
}
