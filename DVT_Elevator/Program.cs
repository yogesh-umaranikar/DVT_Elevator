using DVT_Elevator.Application;
using DVT_Elevator.Domain;

namespace DVT_Elevator
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var elevators = new List<IElevator>
            {
                new StandardElevator(1, 5, requestHandler: new QueueRequestHandler(), 
                movementController: new SimpleMovementController()),
                new StandardElevator(2, 5, requestHandler: new QueueRequestHandler(),
                movementController: new SimpleMovementController()),
                new StandardElevator(3, 10, requestHandler: new QueueRequestHandler(),
                movementController: new SimpleMovementController())
            };

            var dispatcher = new Dispatcher(elevators);
            var controller = new ElevatorController(dispatcher);

            bool running = true;
            while (running)
            {
                Console.WriteLine("Elevator Simulation Started (type 'exit' to quit)");
                Console.WriteLine("Commands: call <floor> <passengers>");

                controller.Step();
                controller.DisplayStatus();

                if (Console.KeyAvailable)
                {
                    var input = Console.ReadLine();
                    if (input == null) continue;

                    if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                        break;

                    controller.HandleInput(input);
                }

                Thread.Sleep(1000);
                Console.Clear();
            }
        }
    }
}
