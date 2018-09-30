// ReSharper disable All
namespace ValidCode
{
    using NUnit.Framework;
    using System.Reflection;

    public class GenericMember
    {
        [Test]
        public void Valid()
        {
            Assert.NotNull(typeof(GenericMember).GetMethod(nameof(this.Id), BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            Assert.NotNull(typeof(GenericMember).GetNestedType("Bar`1", BindingFlags.Public));
        }

        public T Id<T>(T value) => value;

        public class Bar<T>
        {
        }
    }
}
