namespace Nivaes.IoC.UnitTest
{
    public class Helper2
    {
        private Helper3 _helper;
        public Helper2(Helper3 helper)
        {
            _helper = helper;
        }

        public Guid Id { get; } = Guid.NewGuid();
    }

}
