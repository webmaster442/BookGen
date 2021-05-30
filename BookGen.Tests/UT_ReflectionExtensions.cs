using BookGen.AssemblyDocument;
using BookGen.AssemblyDocument.Domain;
using BookGen.Tests.Assemblydoc;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_ReflectionExtensions
    {
        [TestCase(typeof(ITestInterface), TypeType.Interface)]
        [TestCase(typeof(TestClass), TypeType.Class)]
        [TestCase(typeof(TestStruct), TypeType.Struct)]
        [TestCase(typeof(Assemblydoc.TestDelegate), TypeType.Delegate)]
        [TestCase(typeof(TestEnum), TypeType.Enum)]
        [TestCase(typeof(TestRecord), TypeType.Record)]
        public void EnsureThat_GetTypeType_ReturnsCorrect(Type input, TypeType expected)
        {
            var result = input.GetTypeType();
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void EnsureThat_GetInheritanceChain_ReturnsCorrect()
        {
            var result = typeof(TestInheritanceChain).GetInheritanceChain();
            CollectionAssert.AreEqual(new[]
            {
                typeof(TestBaseC),
                typeof(TestBaseB),
                typeof(TestBaseA),
                typeof(object),
            }, result);
        }

        [TestCase(nameof(TestClass.GetProperty), "protected internal int GetProperty { get; }")]
        [TestCase(nameof(TestClass.GetProperty2), "internal int GetProperty2 { get; }")]
        [TestCase(nameof(TestClass.NormalProperty), "public int NormalProperty { get; set; }")]
        [TestCase(nameof(TestClass.InitProperty), "public int InitProperty { get; init; }")]
        [TestCase("Portected", "protected int Portected { get; }")]
        [TestCase("Private", "private int Private { get; }")]
        public void EnsureThat_GetPropertyCode_ReturnsCorrect(string propName, string expected)
        {
            var prop = typeof(TestClass).GetProperty(propName,
                                                     System.Reflection.BindingFlags.Public 
                                                     | System.Reflection.BindingFlags.NonPublic
                                                     | System.Reflection.BindingFlags.Instance);
            var result = prop.GetPropertyCode();
            Assert.AreEqual(expected, result);
        }

        [TestCase(typeof(int), "int")]
        [TestCase(typeof(uint), "uint")]
        [TestCase(typeof(sbyte), "sbyte")]
        [TestCase(typeof(byte), "byte")]
        [TestCase(typeof(long), "long")]
        [TestCase(typeof(ulong), "ulong")]
        [TestCase(typeof(short), "short")]
        [TestCase(typeof(ushort), "ushort")]
        [TestCase(typeof(double), "double")]
        [TestCase(typeof(float), "float")]
        [TestCase(typeof(string), "string")]
        [TestCase(typeof(decimal), "decimal")]
        [TestCase(typeof(Exception), "System.Exception")]
        [TestCase(typeof(IEnumerable<int>), "System.Collections.Generic.IEnumerable<T>")]
        [TestCase(typeof(Dictionary<string, int>), "System.Collections.Generic.Dictionary<TKey, TValue>")]
        public void EnsureThat_GetNormalizedTypeName_ReturnsCorrect(Type input, string expected)
        {
            var result = input.GetNormalizedTypeName();
            Assert.AreEqual(expected, result);
        }

        [TestCase(typeof(int), "[int](https://docs.microsoft.com/en-us/dotnet/api/system.int32)")]
        [TestCase(typeof(Exception), "[System.Exception](https://docs.microsoft.com/en-us/dotnet/api/system.exception)")]
        [TestCase(typeof(List<string>), "[System.Collections.Generic.List\\<T\\>](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)")]
        [TestCase(typeof(Dictionary<string, int>), "[System.Collections.Generic.Dictionary\\<TKey, TValue\\>](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2)")]
        [TestCase(typeof(IEnumerable<string>), "[System.Collections.Generic.IEnumerable\\<T\\>](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)")]
        public void EnsureThat_GetMarkdownDocLinkFromType_ReturnsCorrect(Type input, string expected)
        {
            var result = input.GetMarkdownDocLinkFromType();
            Assert.AreEqual(expected, result);
        }
    }
}
