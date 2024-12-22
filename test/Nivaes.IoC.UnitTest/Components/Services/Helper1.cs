using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nivaes.IoC.UnitTest
{
    public class Helper1
    {
        private Helper2 _helper;

        public Helper1(Helper2 helper) 
        {
            _helper = helper;
        }

        public Guid Id { get; } = Guid.NewGuid();
    }

}
