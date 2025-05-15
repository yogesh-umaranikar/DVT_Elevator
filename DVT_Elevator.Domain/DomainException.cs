using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVT_Elevator.Domain
{
    public class DomainException : Exception
    {
        public string ErrorMessage { get; }

        public DomainException(string errorMessage) : base(errorMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            ErrorMessage = errorMessage;
            Console.WriteLine(ErrorMessage);
            Console.ResetColor();
        }
    }
}
