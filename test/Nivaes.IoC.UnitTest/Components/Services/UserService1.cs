using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nivaes.IoC.UnitTest
{
    public interface IUserService1
    {
        Guid Id { get; }

        void PrintMessage();
    }

    public class UserService1 : IUserService1
    {
        public Guid Id { get; } = Guid.NewGuid();

        private Helper1 helper;

        public UserService1(Helper1 helper)
        {
            this.helper = helper;
        }

        public void PrintMessage()
        {
            Debug.WriteLine($"UserService.PrintMessage {Id}");
        }
    }
}
