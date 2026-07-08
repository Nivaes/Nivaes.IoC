using System.Diagnostics;

namespace Nivaes.IoC.UnitTest
{
    public interface IUserService1
    {
        Guid Id { get; }

        void PrintMessage();
    }

    public class UserService1 : IUserService1, IDisposable
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

        #region IDisposable
        public bool Disposed { get; set; }

        public void Dispose()
        {
            Disposed = true;
        }
        #endregion
    }
}
