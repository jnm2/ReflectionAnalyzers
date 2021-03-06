namespace ReflectionAnalyzers.Tests.REFL040PreferIsInstanceOfTypeTests
{
    using Gu.Roslyn.Asserts;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.Diagnostics;
    using NUnit.Framework;
    using ReflectionAnalyzers.Codefixes;

    public class CodeFix
    {
        private static readonly DiagnosticAnalyzer Analyzer = new IsAssignableFromAnalyzer();
        private static readonly CodeFixProvider Fix = new UseIsInstanceOfTypeFix();
        private static readonly ExpectedDiagnostic ExpectedDiagnostic = ExpectedDiagnostic.Create(REFL040PreferIsInstanceOfType.Descriptor);

        [Test]
        public void IsAssignableFromInstanceGetType()
        {
            var code = @"
namespace RoslynSandbox
{
    using System;

    class C
    {
        public bool M(Type t1, object o) => t1.IsAssignableFrom(o.GetType());
    }
}";

            var fixedCode = @"
namespace RoslynSandbox
{
    using System;

    class C
    {
        public bool M(Type t1, object o) => t1.IsInstanceOfType(o);
    }
}";

            AnalyzerAssert.CodeFix(Analyzer, Fix, ExpectedDiagnostic, code, fixedCode);
        }
    }
}
