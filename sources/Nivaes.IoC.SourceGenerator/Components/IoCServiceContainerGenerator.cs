namespace Nivaes.IoC
{
    using System.Text;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    [Generator(LanguageNames.CSharp)]
    public class IoCServiceContainerGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
//#if DEBUG
//            System.Diagnostics.Debugger.Launch();
//#endif

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
                                .Any(o => o.Type.ToString().EndsWith("IoCServiceContainer")) ?? false)
                        {
                            return true;
                        }

                        break;
                }
                return false;
            }

            static (INamedTypeSymbol? containerType, string[] identifiersText, IEnumerable<ServiceEntry>? entries) GetSemanticTargetForGeneration(GeneratorSyntaxContext context, CancellationToken ct)
            {
                if (context.Node is ClassDeclarationSyntax classDeclaration)
                {
                    if (classDeclaration == null)
                        return (null, [], null);

                    var bootstrapMethod = classDeclaration?
                        .DescendantNodes()
                        .OfType<MethodDeclarationSyntax>()
                        .FirstOrDefault(o => o.Identifier.Text == "Bootstrap");

                    if (bootstrapMethod == null)
                    {
                        return (null, [], null);
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
                            return (null, [], null);

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

                    if (classDeclaration == null)
                        return (null, [], null);

                    var containerType = semantic?.GetDeclaredSymbol(classDeclaration, ct);

                    List<string> identifiesText = [];
                    var classNode = classDeclaration;
                    do
                    {
                        identifiesText.Add(classNode.Identifier.Text);
                    } while ((classNode = classNode.Parent as ClassDeclarationSyntax) != null);

                    return (containerType, identifiesText.ToArray(), entries);
                }
                else
                {
                    return (null, [], null);
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

                var prefixClass = new StringBuilder();
                var sufixClass = new StringBuilder();
                for (int i = action.identifiersText.Length-1; i > 0 ; i--)
                {
                    prefixClass.AppendLine($"public partial class {action.identifiersText[i]}{{");
                    sufixClass.AppendLine($"}}");
                }

                var source =
@$"// This file generated for Nivaes.IoC.
// <auto-generated/>
using System;
using System.Linq;
using System.Collections.Generic;
using Nivaes.IoC;

namespace {action.containerType?.ContainingNamespace}
{{
    {prefixClass}
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
            KeyInstanceResolverValue<IInstanceResolver>[] mResolvers = [{
                groupedEntries
                    .Where(o =>
                    {
                        if (o.Count() == 1)
                        {
                            var entry = o.First();
                            return entry.Lifetime == ServiceEntry.LifetimeKind.Singleton || entry.Lifetime == ServiceEntry.LifetimeKind.Transient;
                        }
                        return false;
                    })
                    .Select(o =>
                    {
                        var entry = o.First();
                        if (entry.Lifetime == ServiceEntry.LifetimeKind.Singleton)
                        {
                            return $@"new KeyInstanceResolverValue<IInstanceResolver>(typeof({entry.Interface.ToGlobalName()}), new SingletonResolver<{entry.Interface.ToCreatorName()}, {entry.Interface.ToGlobalName()}>()),";
                        }
                        else if (entry.Lifetime == ServiceEntry.LifetimeKind.Transient)
                        {
                            return $@"new KeyInstanceResolverValue<IInstanceResolver>(typeof({entry.Interface.ToGlobalName()}), new TransientResolver<{entry.Interface.ToCreatorName()}, {entry.Interface.ToGlobalName()}>()),";
                        }
                        else
                        {
                            return string.Empty;
                        }
                    })
                    .JoinWithNewLine()
            }];

            KeyInstanceResolverValue<IInstanceResolver>[] mScopedResolvers = [{groupedEntries
                    .Where(o =>
                    {
                        if (o.Count() == 1)
                        {
                            var entry = o.First();
                            return entry.Lifetime == ServiceEntry.LifetimeKind.Scoped;
                        }
                        return false;
                    })
                    .Select(o =>
                    {
                        var entry = o.First();
                        return $@"new KeyInstanceResolverValue<IInstanceResolver>(typeof({entry.Interface.ToGlobalName()}), new SingletonResolver<{entry.Interface.ToCreatorName()}, {entry.Interface.ToGlobalName()}>()),";
                    })
                    .JoinWithNewLine()
            }];

            LoadData(mResolvers, mScopedResolvers);
        }}

        private {action.containerType?.Name}(IoTCollection<IInstanceResolver> resolvers, IoTCollection<IInstanceResolver> scopedResolvers, bool scope = false)
            : base(resolvers, scopedResolvers, scope)
        {{
        }}

        public override IIoCResolver CreateScope()
        {{
            var newScope = mScopedResolvers.Clone();
            return new {action.containerType?.Name}(mResolvers, newScope, true);
        }}

        public override IIoCResolver Clone()
        {{
            var copy = mResolvers.Clone();
            var scopedCopy = mScopedResolvers.Clone();
            return new {action.containerType?.Name}(copy, scopedCopy, false);
        }}
    }}
    {sufixClass}
}}
";
                var sourceName = action.identifiersText.Reverse().Where(o => !string.IsNullOrWhiteSpace(o)).Join("_");
                context.AddSource(sourceName + "_IoCServiceContainer", source);
            });
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

            var constructor = constructorsWithArguments.FirstOrDefault() ?? constructors[0];
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

            var constructor = members[0];
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

            var constructor = members[0];
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

        private sealed class ServiceEntry
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
