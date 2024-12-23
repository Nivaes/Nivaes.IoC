using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nivaes.IoC.UnitTest
{
    public interface IUserService3
    {
        Guid Id { get; }
        void PrintMessage();
    }

    public class UserService3 : IUserService3
    {
        public Guid Id { get; } = Guid.NewGuid();

        private Helper3 helper;

        public UserService3(Helper3 helper)
        {
            this.helper = helper;
        }

        public void PrintMessage()
        {
            Debug.WriteLine($"UserService.PrintMessage {Id}");
        }
    }
}
