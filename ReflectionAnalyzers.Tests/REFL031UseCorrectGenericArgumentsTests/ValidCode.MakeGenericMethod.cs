namespace ReflectionAnalyzers.Tests.REFL031UseCorrectGenericArgumentsTests
{
    using Gu.Roslyn.Asserts;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;
    using NUnit.Framework;

    public partial class ValidCode
    {
        public class MakeGenericMethod
        {
            private static readonly DiagnosticAnalyzer Analyzer = new MakeGenericAnalyzer();
            private static readonly DiagnosticDescriptor Descriptor = REFL031UseCorrectGenericArguments.Descriptor;

            [Test]
            public void SingleUnconstrained()
            {
                var code = @"
namespace RoslynSandbox
{
    using System;

    public class C
    {
        public static void Bar<T>()
        {
            var method = typeof(C).GetMethod(nameof(C.Bar)).MakeGenericMethod(typeof(int));
        }
    }
}";
                AnalyzerAssert.Valid(Analyzer, Descriptor, code);
            }

            [TestCase("where T : class",            "typeof(string)")]
            [TestCase("where T : struct",           "typeof(int)")]
            [TestCase("where T : IComparable",      "typeof(int)")]
            [TestCase("where T : IComparable<T>",   "typeof(int)")]
            [TestCase("where T : IComparable<int>", "typeof(int)")]
            [TestCase("where T : new()",            "typeof(C)")]
            public void ConstrainedParameter(string constraint, string arg)
            {
                var code = @"
namespace RoslynSandbox
{
    using System;

    public class C
    {
        public static void Bar<T>()
            where T : class
        {
            var method = typeof(C).GetMethod(nameof(C.Bar)).MakeGenericMethod(typeof(int));
        }
    }
}".AssertReplace("where T : class", constraint)
  .AssertReplace("typeof(int)", arg);

                AnalyzerAssert.Valid(Analyzer, Descriptor, code);
            }
        }
    }
}
