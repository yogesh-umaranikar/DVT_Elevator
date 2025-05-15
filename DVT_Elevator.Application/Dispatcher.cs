using DVT_Elevator.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVT_Elevator.Application
{
    public class Dispatcher
    {
        private readonly List<IElevator> elevators;

        public Dispatcher(List<IElevator> elevators)
        {
            this.elevators = elevators;
        }

        public void RequestElevator(int floor, int passengers)
        {
            var bestElevator = elevators
                .Where(e => e.Passengers + passengers <= e.Capacity)
                .OrderBy(e => Math.Abs(e.CurrentFloor - floor))
                .FirstOrDefault();

            if (bestElevator != null)
                bestElevator.AddRequest(floor, passengers);
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("All elevators are full or unavailable.");
                Console.ResetColor();
            }
        }

        public void StepAll()
        {
            foreach (var e in elevators)
                e.Step();
        }

        public IEnumerable<string> GetStatuses() => elevators.Select(e => e.Status());
    }
}
