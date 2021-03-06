namespace ReflectionAnalyzers.Tests.REFL019NoMemberMatchesTheTypesTests
{
    using Gu.Roslyn.Asserts;
    using Microsoft.CodeAnalysis.Diagnostics;
    using NUnit.Framework;

    public class Diagnostics
    {
        private static readonly DiagnosticAnalyzer Analyzer = new GetXAnalyzer();
        private static readonly ExpectedDiagnostic ExpectedDiagnostic = ExpectedDiagnostic.Create(REFL019NoMemberMatchesTheTypes.Descriptor);

        [TestCase("GetConstructor(↓Type.EmptyTypes)")]
        [TestCase("GetConstructor(↓Array.Empty<Type>())")]
        [TestCase("GetConstructor(↓new Type[0])")]
        [TestCase("GetConstructor(↓new Type[1] { typeof(double) })")]
        [TestCase("GetConstructor(↓new Type[] { typeof(double) })")]
        [TestCase("GetConstructor(↓new[] { typeof(double) })")]
        [TestCase("GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, ↓Type.EmptyTypes, null)")]
        public void GetConstructor(string call)
        {
            var code = @"
namespace RoslynSandbox
{
    using System;
    using System.Reflection;

    public class C
    {
        public C(int value)
        {
            var ctor = typeof(C).GetConstructor(↓Type.EmptyTypes);
        }
    }
}".AssertReplace("GetConstructor(↓Type.EmptyTypes)", call);

            AnalyzerAssert.Diagnostics(Analyzer, ExpectedDiagnostic, code);
        }

        [TestCase("typeof(C).GetMethod(nameof(Static), BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly, null, ↓new[] { typeof(int) }, null)")]
        [TestCase("typeof(C).GetMethod(nameof(this.Public), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly, null, ↓new[] { typeof(int) }, null)")]
        [TestCase("typeof(C).GetMethod(nameof(this.ToString), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly, null, ↓new[] { typeof(int) }, null)")]
        public void GetMethodNoParameters(string call)
        {
            var code = @"
namespace RoslynSandbox
{
    using System;
    using System.Reflection;

    class C
    {
        public object Get() => typeof(C).GetMethod(nameof(Static), BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly, null, ↓new[] { typeof(int) }, null);

        public static int Static() => 0;

        public int Public() => 0;

        public override string ToString() => string.Empty;

        private int Private() => 0;
    }
}".AssertReplace("typeof(C).GetMethod(nameof(Static), BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly, null, ↓new[] { typeof(int) }, null)", call);

            AnalyzerAssert.Diagnostics(Analyzer, ExpectedDiagnostic, code);
        }

        [TestCase("typeof(C).GetMethod(nameof(Static), BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly, null, ↓new[] { typeof(double) }, null)")]
        [TestCase("typeof(C).GetMethod(nameof(this.Public), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly, null, ↓new[] { typeof(double) }, null)")]
        public void GetMethodOneParameter(string call)
        {
            var code = @"
namespace RoslynSandbox
{
    using System;
    using System.Reflection;

    class C
    {
        public object Get() => typeof(C).GetMethod(nameof(Static), BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly, null, ↓new[] { typeof(double) }, null);

        public static int Static(int i) => i;

        public int Public(int i) => i;

        private int Private(int i) => i;
    }
}".AssertReplace("typeof(C).GetMethod(nameof(Static), BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly, null, ↓new[] { typeof(double) }, null)", call);

            AnalyzerAssert.Diagnostics(Analyzer, ExpectedDiagnostic, code);
        }

        [TestCase("typeof(Array).GetMethod(nameof(Array.CreateInstance), new[] { typeof(Type), typeof(IEnumerable<int>) })")]
        [TestCase("typeof(Array).GetMethod(nameof(Array.CreateInstance), new[] { typeof(Type), typeof(int), typeof(IEnumerable<int>) })")]
        public void OverloadResolution(string call)
        {
            var code = @"
namespace RoslynSandbox
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    class C
    {
        public object Get => typeof(Array).GetMethod(nameof(Array.CreateInstance), new[] { typeof(Type), typeof(int), typeof(IEnumerable<int>) });
    }
}".AssertReplace("typeof(Array).GetMethod(nameof(Array.CreateInstance), new[] { typeof(Type), typeof(int), typeof(IEnumerable<int>) })", call);

            AnalyzerAssert.Diagnostics(Analyzer, ExpectedDiagnostic, code);
        }
    }
}
