namespace Nivaes.IoC.Benchmarks
{
    #region IUserService
    public interface IUserService1
    {
    }

    public class UserService1 : IUserService1
    {

        public UserService1(Helper1 helper1)
        {
        }

        public Guid Id { get; } = Guid.NewGuid();
    }

    public interface IUserService2
    {
    }

    public class UserService2 : IUserService2
    {

        public UserService2(Helper1 helper1)
        {
        }

        public Guid Id { get; } = Guid.NewGuid();
    }

    public interface IUserService3
    {
    }

    public class UserService3 : IUserService3
    {

        public UserService3(Helper1 helper1)
        {
        }

        public Guid Id { get; } = Guid.NewGuid();
    }


    public interface IUserService4
    {
    }

    public class UserService4 : IUserService4
    {

        public UserService4(Helper1 helper1)
        {
        }

        public Guid Id { get; } = Guid.NewGuid();
    }

    public interface IUserService5
    {
    }

    public class UserService5 : IUserService5
    {

        public UserService5(Helper1 helper1)
        {
        }

        public Guid Id { get; } = Guid.NewGuid();
    }

    public interface IUserService6
    {
    }

    public class UserService6 : IUserService6
    {

        public UserService6(Helper1 helper1)
        {
        }

        public Guid Id { get; } = Guid.NewGuid();
    }

    public interface IUserService7
    {
    }

    public class UserService7 : IUserService7
    {

        public UserService7(Helper1 helper1)
        {
        }

        public Guid Id { get; } = Guid.NewGuid();
    }

    public interface IUserService8
    {
    }

    public class UserService8 : IUserService8
    {

        public UserService8(Helper1 helper1)
        {
        }

        public Guid Id { get; } = Guid.NewGuid();
    }

    public interface IUserService9
    {
    }

    public class UserService9 : IUserService9
    {

        public UserService9()
        {
        }

        public Guid Id { get; } = Guid.NewGuid();
    }

    public interface IUserService10
    {
    }

    public class UserService10 : IUserService10
    {

        public UserService10(Helper1 helper1)
        {
        }

        public Guid Id { get; } = Guid.NewGuid();
    }

    public interface IUserService11
    {
    }

    public class UserService11 : IUserService11
    {

        public UserService11(Helper1 helper1)
        {
        }

        public Guid Id { get; } = Guid.NewGuid();
    }
    #endregion

    #region Helper
    public class Helper1
    {
    }

    public class Helper2
    {
        private readonly Helper1 helper1;

        public Helper2(Helper1 helper1)
        {
            this.helper1 = helper1;
        }
    }
    #endregion

    #region SingleHelper
    public class SingleHelper1
    {
        private readonly Helper1 helper1;
        private readonly Helper2 helper2;

        public SingleHelper1(Helper1 helper1, Helper2 helper2)
        {
            this.helper1 = helper1;
            this.helper2 = helper2;
        }
    }

    public class SingleHelper2
    {
        private readonly Helper1 helper1;

        public SingleHelper2(Helper1 helper1)
        {
            this.helper1 = helper1;
        }
    }

    public class SingleHelper3
    {
        private readonly Helper2 helper2;

        public SingleHelper3(Helper2 helper2)
        {
            this.helper2 = helper2;
        }
    }
    #endregion

    #region SingleService
    public class SingleService1(SingleHelper1 helper)
    {
        private readonly SingleHelper1 helper = helper;
    }

    public class SingleService2(SingleHelper2 helper)
    {
        private readonly SingleHelper2 helper = helper;
    }

    public class SingleService3(SingleHelper2 helper)
    {
        private readonly SingleHelper2 helper = helper;
    }

    public class SingleService4(SingleHelper2 helper)
    {
        private readonly SingleHelper2 helper = helper;
    }
    #endregion
}
