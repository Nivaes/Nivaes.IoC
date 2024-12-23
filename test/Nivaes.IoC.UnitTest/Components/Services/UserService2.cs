using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nivaes.IoC.UnitTest
{
    public interface IUserService2
    {
        Guid Id { get; }

        void PrintMessage();
    }

    public class UserService2 : IUserService2
    {
        public Guid Id { get; } = Guid.NewGuid();

        private Helper2 helper;

        public UserService2(Helper2 helper)
        {
            this.helper = helper;
        }

        public void PrintMessage()
        {
            Debug.WriteLine($"UserService.PrintMessage {Id}");
        }
    }
}
