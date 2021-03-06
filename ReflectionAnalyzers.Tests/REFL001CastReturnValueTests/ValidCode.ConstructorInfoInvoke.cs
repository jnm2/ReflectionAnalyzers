namespace ReflectionAnalyzers.Tests.REFL001CastReturnValueTests
{
    using Gu.Roslyn.Asserts;
    using Microsoft.CodeAnalysis.Diagnostics;
    using NUnit.Framework;

    public partial class ValidCode
    {
        public class ConstructorInfoInvoke
        {
            private static readonly DiagnosticAnalyzer Analyzer = new InvokeAnalyzer();
            private static readonly ExpectedDiagnostic ExpectedDiagnostic = ExpectedDiagnostic.Create(REFL001CastReturnValue.Descriptor);

            [Test]
            public void AssigningLocal()
            {
                var code = @"
namespace RoslynSandbox
{
    public class C
    {
        public C(int i)
        {
            var value = ↓typeof(C).GetConstructor(new[] { typeof(int) }).Invoke(new object[] { 1 });
        }
    }
}";

                AnalyzerAssert.Diagnostics(Analyzer, ExpectedDiagnostic, code);
            }
        }
    }
}
