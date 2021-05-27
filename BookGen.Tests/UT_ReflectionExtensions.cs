﻿using BookGen.AssemblyDocument;
using BookGen.AssemblyDocument.Domain;
using BookGen.Tests.Reflection;
using NUnit.Framework;
using System;

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_ReflectionExtensions
    {
        [TestCase(typeof(ITestInterface), TypeType.Interface)]
        [TestCase(typeof(TestClass), TypeType.Class)]
        [TestCase(typeof(TestStruct), TypeType.Struct)]
        [TestCase(typeof(Reflection.TestDelegate), TypeType.Delegate)]
        [TestCase(typeof(TestEnum), TypeType.Enum)]
        [TestCase(typeof(TestRecord), TypeType.Record)]
        public void EnsureThat_GetTypeType_ReturnsCorrect(Type input, TypeType expected)
        {
            var result = input.GetTypeType();
            Assert.AreEqual(expected, result);
        }

        [TestCase(typeof(TestGenericType<string, string>), "<TFirst, TSecond>")]
        [TestCase(typeof(string), "")]
        public void EnsureThat_GetTypeArgumentString_ReturnsCorrect(Type input, string expected)
        {
            var result = input.GetTypeArgumentString();
            Assert.AreEqual(expected, result);
        }
    }
}
