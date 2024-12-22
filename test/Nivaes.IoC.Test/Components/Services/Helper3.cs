using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nivaes.IoC.Test
{
    public class Helper3
    {
        public Helper3() 
        {
        }

        public Guid Id { get; } = Guid.NewGuid();
    }

}
