using Microsoft.CodeAnalysis;

namespace Nivaes.IoC
{
    public static class Descriptors
    {
        public static readonly DiagnosticDescriptor ClassIsNotPartial = new(
            "NI001",
            "IoCServerContainer has to be a partial class",
            "The {0} ia not partial class. It is essential to enable source generation.",
            "Nivaes.IoC",
            DiagnosticSeverity.Error,
            true);

        public static readonly DiagnosticDescriptor BootstrapIsNotOverrided = new(
            "NI002",
            "IoCServerContainer does not override the Bootstrap method",
            "The {0} does not override the Bootstrap method. Override the Bootstrap method to enable source generation.",
            "Nivaes.IoC",
            DiagnosticSeverity.Error,
            true);

        public static readonly DiagnosticDescriptor CreateScopeIsOverrided = new(
            "NI003",
            "IoCServerContainer has override for the CreateScope method",
            "The {0} has override for the CreateScope method. There is no need to override the CreateScope. It will be overrided by the source generator.",
            "Nivaes.IoC",
            DiagnosticSeverity.Error,
            true);

        public static readonly DiagnosticDescriptor StatementsNotAllowed = new(
            "NI004",
            "The Bootstrap method does not allow statements",
            "The Bootstrap method does not allow statements. Use only method calls from the IIoCServiceContainerBootstrapper.",
            "Nivaes.IoC",
            DiagnosticSeverity.Error,
            true);
        
        public static readonly DiagnosticDescriptor MultipleTypeRegistrationsNotAllowed = new(
            "NI005",
            "The multiple type registrations are not allowed",
            "The multiple type registrations are not allowed",
            "Nivaes.IoC",
            DiagnosticSeverity.Error,
            true);
        
        public static readonly DiagnosticDescriptor OnlyOneConstructorWithArgumentAllowed = new(
            "NI006",
            "Only one constructor with argument allowed",
            "Only one constructor with argument allowed",
            "Nivaes.IoC",
            DiagnosticSeverity.Error,
            true);
    }
}
