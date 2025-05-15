using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVT_Elevator.Domain;

namespace DVT_Elevator.Application
{
    public class ElevatorController
    {
        private readonly Dispatcher dispatcher;

        public ElevatorController(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        public void HandleInput(string input)
        {
            var parts = input.Split();
            if (parts.Length == 3 && parts[0] == "call"
                && int.TryParse(parts[1], out int floor)
                && int.TryParse(parts[2], out int passengers))
            {
                dispatcher.RequestElevator(floor, passengers);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input. Use: call <floor> <passengers>");
            }
            Console.ResetColor();
        }

        public void Step() => dispatcher.StepAll();

        public void DisplayStatus()
        {
            foreach (var status in dispatcher.GetStatuses())
                Console.WriteLine(status);
        }
    }
}
