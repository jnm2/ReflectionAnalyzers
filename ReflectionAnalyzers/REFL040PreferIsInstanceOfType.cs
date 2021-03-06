namespace ReflectionAnalyzers
{
    using Microsoft.CodeAnalysis;

    internal static class REFL040PreferIsInstanceOfType
    {
        internal const string DiagnosticId = "REFL040";

        internal static readonly DiagnosticDescriptor Descriptor = new DiagnosticDescriptor(
            id: DiagnosticId,
            title: "Prefer type.IsInstanceOfType(...).",
            messageFormat: "Prefer {0}.IsInstanceOfType({1}).",
            category: AnalyzerCategory.SystemReflection,
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: "Prefer type.IsInstanceOfType(...).",
            helpLinkUri: HelpLink.ForId(DiagnosticId));
    }
}
