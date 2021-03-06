namespace ReflectionAnalyzers.Tests.REFL036CheckNullTests
{
    using Gu.Roslyn.Asserts;
    using Microsoft.CodeAnalysis.Diagnostics;
    using NUnit.Framework;

    public class Diagnostics
    {
        private static readonly DiagnosticAnalyzer Analyzer = new GetTypeAnalyzer();
        private static readonly ExpectedDiagnostic ExpectedDiagnostic = ExpectedDiagnostic.Create(REFL036CheckNull.Descriptor);

        [TestCase("Get() => Type.GetType(\"C\").Assembly")]
        [TestCase("Get() => Type.GetType(\"C\", throwOnError: false).Assembly")]
        [TestCase("Get(System.Reflection.Assembly source) => source.GetType(\"C\").Assembly")]
        [TestCase("Get(System.Reflection.Assembly source) => source.GetType(\"C\", throwOnError: false).Assembly")]
        [TestCase("Get(System.Reflection.Emit.AssemblyBuilder source) => source.GetType(\"C\").Assembly")]
        [TestCase("Get(System.Reflection.Emit.AssemblyBuilder source) => source.GetType(\"C\", throwOnError: false).Assembly")]
        public void WhenMemberAccess(string body)
        {
            var code = @"
namespace RoslynSandbox
{
    using System;

    public class C
    {
        public static object Get() => Type.GetType(""C"").Assembly;
    }
}".AssertReplace("Get() => Type.GetType(\"C\").Assembly", body);
            AnalyzerAssert.Diagnostics(Analyzer, ExpectedDiagnostic, code);
        }
    }
}
