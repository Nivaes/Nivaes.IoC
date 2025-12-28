using System.Diagnostics;

namespace Nivaes.IoC;

internal static class ExceptionHelper
{
    [DebuggerHidden]
    public static void ScopedWithoutScopeException(string fullName)
    {
        throw new ScopedWithoutScopeException($"Type {fullName} is registred as scoped, but you are trying to create it without scope.");
    }

    [DebuggerHidden]
    public static void ServiceIsNotRegistered(string fullName)
    {
        throw new ServiceIsNotRegisteredException($"Type {fullName} is missing in resolver.");
    }
}
