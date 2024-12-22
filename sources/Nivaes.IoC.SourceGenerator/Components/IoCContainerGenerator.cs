using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Nivaes.IoC
{
    [Generator]
    public class IoCContainerGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var generate = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: static (s, _) => IsSyntaxTargetForGeneration(s), // select enums with attributes
                    transform: static (ctx, ct) => GetSemanticTargetForGeneration(ctx, ct)) // select enums with the [EnumExtensions] attribute and extract details
                .Where(static m => m.entries is not null); // Filter out errors that we don't care about

            static bool IsSyntaxTargetForGeneration(SyntaxNode syntaxNode)
            {
                switch (syntaxNode)
                {
                    case ClassDeclarationSyntax classDeclaration:
                        if (classDeclaration.BaseList?.Types
                                .Any(o => o.Type.ToString().EndsWith("IoCContainer")) ?? false)
                        {
                            return true;
                        }

                        break;
                }
                return false;
            }

            static (INamedTypeSymbol? containerType, string? identifierText, IEnumerable<ServiceEntry>? entries) GetSemanticTargetForGeneration(GeneratorSyntaxContext context, CancellationToken ct)
            {
                if (context.Node is ClassDeclarationSyntax classDeclaration)
                {
                    if (classDeclaration == null)
                        return (null, null, null);

                    var bootstrapMethod = classDeclaration?
                        .DescendantNodes()
                        .OfType<MethodDeclarationSyntax>()
                        .FirstOrDefault(o => o.Identifier.Text == "Bootstrap");

                    if (bootstrapMethod == null)
                    {
                        return (null, null, null);
                    }

                    var invocations = bootstrapMethod
                        .DescendantNodes()
                        .OfType<InvocationExpressionSyntax>()
                        .ToArray();


                    var semantic = context.SemanticModel;
                    var entries = new List<ServiceEntry>();
                    foreach (var invocation in invocations)
                    {
                        if (ct.IsCancellationRequested)
                            return (null, null, null);

                        if (invocation.Expression is MemberAccessExpressionSyntax member &&
                            member.Name is GenericNameSyntax generic)
                        {
                            switch (generic.Identifier.Text)
                            {
                                case "AddSingleton":
                                    AddTypes(entries, ServiceEntry.LifetimeKind.Singleton, generic, semantic);
                                    break;

                                case "AddTransient":
                                    AddTypes(entries, ServiceEntry.LifetimeKind.Transient, generic, semantic);
                                    break;

                                case "AddScoped":
                                    AddTypes(entries, ServiceEntry.LifetimeKind.Scoped, generic, semantic);
                                    break;
                            }
                        }
                    }

                    var groupedEntries = entries
                       .GroupBy(o => o.Interface, SymbolEqualityComparer.Default)
                       .ToArray();

                    if (classDeclaration == null)
                        return (null, null, null);

                    var containerType = semantic?.GetDeclaredSymbol(classDeclaration, ct);

                    return (containerType, classDeclaration?.Identifier.Text, entries);
                }
                else
                {
                    return (null, null, null);
                }
            }

            context.RegisterSourceOutput(generate, (context, action) =>
            {
                var groupedEntries = action.entries
                    .GroupBy(o => o.Interface, SymbolEqualityComparer.Default)
                    .ToArray();

                foreach (var entry in groupedEntries)
                {
                    if (context.CancellationToken.IsCancellationRequested)
                        return;

                    if (entry.Count() > 1)
                    {
                        foreach (var serviceEntry in entry)
                        {
                            context.ReportDiagnostic(
                                Diagnostic.Create(
                                    Descriptors.MultipleTypeRegistrationsNotAllowed,
                                    serviceEntry.Syntax.GetLocation()));
                        }

                        return;
                    }
                }

                var transients = new HashSet<string>(action.entries
                    .Where(o => o.Lifetime == ServiceEntry.LifetimeKind.Transient)
                    .Select(o => o.Interface.ToGlobalName()));

                var source =
                    @$"// This file generated for Nivaes.IoC.
// <auto-generated/>
using System;
using System.Linq;
using System.Collections.Generic;
using Nivaes.IoC;

namespace {action.containerType?.ContainingNamespace}
{{
    {IoCAnalyzer.CodeGenerationAttribute}
    public sealed partial class {action.containerType?.Name}
    {{{groupedEntries
                .Select(o =>
                    $@"
        private struct {o.First().Interface.ToCreatorName()} : ICreator<{o.First().Interface.ToGlobalName()}>
        {{
            public {o.First().Interface.ToGlobalName()} Create(IIoCResolver resolver, IOverrides overrides)
            {{
                {ResolveConstructorBodyWithOverrides(o.First().Implementation)}
                {ResolveConstructorWithOverrides(o.First().Implementation, transients)}
            }}

            public {o.First().Interface.ToGlobalName()} Create(IIoCResolver resolver)
            {{
                return {ResolveConstructor(context, o.First(), transients)};
            }}
        }}")
                .JoinWithNewLine()}

        public {action.containerType?.Name}()
        {{
{groupedEntries.Select(o =>
                {
                    if (o.Count() == 1)
                    {
                        var entry = o.First();
                        var (propertyToStore, resolver) = MapResolver(entry);

                        return $@"          {propertyToStore}.Add(typeof({entry.Interface.ToGlobalName()}), new {resolver}<{entry.Interface.ToCreatorName()}, {entry.Interface.ToGlobalName()}>());";
                    }

                    return "";
                })
        .JoinWithNewLine()}
        }}

        private {action.containerType?.Name}(Dictionary<Type, IInstanceResolver> resolvers, Dictionary<Type, IInstanceResolver> scopedResolvers, bool scope = false)
            : base(resolvers, scopedResolvers, scope)
        {{
        }}

        public override IIoCResolver CreateScope()
        {{
            var newScope = ScopedResolvers.ToDictionary(o => o.Key, o => o.Value.Duplicate());
            return new {action.containerType?.Name}(Resolvers, newScope, true);
        }}

        public override IIoCResolver Clone()
        {{
            var copy = Resolvers.ToDictionary(o => o.Key, o => o.Value.Duplicate());
            var scopedCopy = ScopedResolvers.ToDictionary(o => o.Key, o => o.Value.Duplicate());
            return new {action.containerType?.Name}(copy, scopedCopy, false);
        }}
    }}
}}
";
            context.AddSource(action.identifierText + "_IoCContainer", source);

            });
        }

        private (string, string) MapResolver(ServiceEntry entry)
        {
            switch (entry.Lifetime)
            {
                case ServiceEntry.LifetimeKind.Singleton:
                    return ("Resolvers", "SingletonResolver");
                case ServiceEntry.LifetimeKind.Transient:
                    return ("Resolvers", "TransientResolver");
                case ServiceEntry.LifetimeKind.Scoped:
                    return ("ScopedResolvers", "SingletonResolver");
                default:
                    return ("", "");
            }
        }

        private static string ResolveConstructor(SourceProductionContext context, ServiceEntry entry, HashSet<string> transients)
        {
            var implementation = entry.Implementation;
            var constructors = implementation.GetMembers()
                .OfType<IMethodSymbol>()
                .Where(o => o.MethodKind == MethodKind.Constructor)
                .ToArray();

            var constructorsWithArguments = constructors
                .Where(o => o.Parameters.Length > 0)
                .ToArray();

            if (constructorsWithArguments.Length > 1)
            {
                context.ReportDiagnostic(Diagnostic
                    .Create(
                        Descriptors.OnlyOneConstructorWithArgumentAllowed,
                        entry.Syntax.GetLocation()));
            }

            var constructor = constructorsWithArguments.FirstOrDefault() ?? constructors.First();
            var arguments = constructor.Parameters.Select(o => o.Type).ToArray();
            var argumentsText = arguments.Select(o => transients.Contains(o.ToGlobalName()) ? $"default({o.ToCreatorName()}).Create(resolver)" : $"resolver.Resolve<{o.ToGlobalName()}>()");
            return $"new {implementation.ToGlobalName()}({argumentsText.Join()})";
        }

        private static string ResolveConstructorBodyWithOverrides(ITypeSymbol typeSymbol)
        {
            var members = typeSymbol.GetMembers()
                .OfType<IMethodSymbol>()
                .Where(o => o.MethodKind == MethodKind.Constructor)
                .ToArray();

            var constructor = members.First();
            var arguments = constructor.Parameters
                .Select((o, i) => @$"
                var constructor{o.Name.ToLower()}{i}Override = overrides.Constructor.Overrides.TryGetValue(""{o.Name.ToLower()}"", out var constructor{o.Name.ToLower()}{i}Value);
                var dependency{o.Name.ToLower()}{i}Override = overrides.Dependency.Overrides.TryGetValue(typeof({o.Type.ToGlobalName()}), out var dependency{o.Name.ToLower()}{i}Func);
")
                .ToArray();

            return arguments.JoinWithNewLine();
        }

        private static string ResolveConstructorWithOverrides(ITypeSymbol typeSymbol, HashSet<string> transients)
        {
            var members = typeSymbol.GetMembers()
                .OfType<IMethodSymbol>()
                .Where(o => o.MethodKind == MethodKind.Constructor)
                .ToArray();

            var constructor = members.First();
            var variables = constructor.Parameters
                .Select(ResolveVariable);

            var parameters = constructor.Parameters
                .Select((o, i) => $"{o.Name.ToLower()}{i.ToString()}")
                .Join();

            return @$"
                var nextOverrides = Overrides.Create();
                nextOverrides.Dependency = overrides.Dependency;

                {variables.JoinWithNewLine()}
                
                return new {typeSymbol.ToGlobalName()}({parameters});";

            string ResolveVariable(IParameterSymbol parameter, int index)
            {
                var parameterName = $"{parameter.Name.ToLower()}{index.ToString()}";
                var parameterType = parameter.Type.ToGlobalName();
                var result = $@"
                {parameterType} {parameterName} = default;
                if(constructor{parameterName}Override)
                {{
                    {parameterName} = ({parameterType})constructor{parameterName}Value;
                }}

                if(!constructor{parameterName}Override && dependency{parameterName}Override)
                {{
                    {parameterName} = ({parameterType})dependency{parameterName}Func();
                }}
                
                if(!constructor{parameterName}Override && !dependency{parameterName}Override)
                {{
                    {parameterName} = {(transients.Contains(parameter.ToGlobalName()) ? $"default({parameter.ToCreatorName()}).Create(resolver, nextOverrides)" : $"resolver.Resolve<{parameterType}>(nextOverrides)")};
                }}
";

                return result;
            }
        }

        private static void AddTypes(List<ServiceEntry> singletons, ServiceEntry.LifetimeKind lifetimeKind, GenericNameSyntax generic, SemanticModel semantic)
        {
            if (generic.TypeArgumentList.Arguments.Count == 1)
            {
                var type = generic.TypeArgumentList.Arguments.First();
                var symbols = ExtractTypeSymbols(type, type);
                singletons.Add(new ServiceEntry(lifetimeKind, generic, symbols.Interface, symbols.Implementation));
            }

            if (generic.TypeArgumentList.Arguments.Count == 2)
            {
                var interfaceType = generic.TypeArgumentList.Arguments.First();
                var implementationType = generic.TypeArgumentList.Arguments.Last();

                var symbols = ExtractTypeSymbols(interfaceType, implementationType);
                singletons.Add(new ServiceEntry(lifetimeKind, generic, symbols.Interface, symbols.Implementation));
            }

            (ITypeSymbol Interface, ITypeSymbol Implementation) ExtractTypeSymbols(TypeSyntax interfaceType, TypeSyntax implementationType)
            {
                var interfaceSymbol = semantic.GetSpeculativeTypeInfo(interfaceType.SpanStart, interfaceType,
                    SpeculativeBindingOption.BindAsTypeOrNamespace);
                var implementationSymbol = semantic.GetSpeculativeTypeInfo(implementationType.SpanStart,
                    implementationType, SpeculativeBindingOption.BindAsTypeOrNamespace);

                return (interfaceSymbol.Type!, implementationSymbol.Type!);
            }

        }

        private class ServiceEntry
        {
            public enum LifetimeKind
            {
                Singleton,
                Transient,
                Scoped,
            }

            public ServiceEntry(LifetimeKind lifetime, GenericNameSyntax syntax, ITypeSymbol @interface, ITypeSymbol implementation)
            {
                Lifetime = lifetime;
                Syntax = syntax;
                Interface = @interface;
                Implementation = implementation;
            }

            public LifetimeKind Lifetime { get; }
            public GenericNameSyntax Syntax { get; }
            public ITypeSymbol Interface { get; }
            public ITypeSymbol Implementation { get; }
        }
    }
}
