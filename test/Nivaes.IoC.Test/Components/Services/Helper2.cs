using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nivaes.IoC.Test
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
