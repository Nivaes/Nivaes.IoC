using System;

namespace Nivaes.IoC
{
    public class ScopedWithoutScopeException : Exception
    {
        public ScopedWithoutScopeException(string message)
            : base(message)
        {

        }
    }
}