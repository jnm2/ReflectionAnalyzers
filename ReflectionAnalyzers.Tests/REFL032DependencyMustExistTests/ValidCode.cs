namespace ReflectionAnalyzers.Tests.REFL032DependencyMustExistTests
{
    using Gu.Roslyn.Asserts;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;
    using NUnit.Framework;

    public class ValidCode
    {
        private static readonly DiagnosticAnalyzer Analyzer = new DependencyAttributeAnalyzer();
        private static readonly DiagnosticDescriptor Descriptor = REFL032DependencyMustExist.Descriptor;

        [Test]
        public void GetMethodNoParameter()
        {
            var code = @"
using System.Runtime.CompilerServices;
[assembly: Dependency(""System.Collections"", LoadHint.Always)] ";
            AnalyzerAssert.Valid(Analyzer, Descriptor, code);
        }
    }
}
