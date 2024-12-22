using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nivaes.IoC.UnitTest
{
    public class UserService : IUserService
    {
        public Guid Id { get; } = Guid.NewGuid();

        private Helper1 helper;

        public UserService(Helper1 helper)
        {
            this.helper = helper;
        }

        public void PrintMessage()
        {
            Console.WriteLine($"UserService.PrintMessage {Id}");
        }
    }
}
