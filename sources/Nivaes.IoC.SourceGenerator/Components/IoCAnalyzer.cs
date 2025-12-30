namespace Nivaes.IoC;

public static class IoCAnalyzer
{
    public static string Version { get; } = typeof(IoCAnalyzer).Assembly.GetName().Version.ToString();
    public static string CodeGenerationAttribute { get; } = $@"[System.CodeDom.Compiler.GeneratedCode(""Nivaes.Ioc"", ""{Version}"")]";
}
