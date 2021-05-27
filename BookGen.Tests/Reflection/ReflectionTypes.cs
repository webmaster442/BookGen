namespace BookGen.Tests.Reflection
{
    public class TestGenericType<TFirst, TSecond> { }

    public interface ITestInterface { }

    public class TestClass { }

    public struct TestStruct { }

    public record TestRecord { }

    public enum TestEnum { }

    public delegate void TestDelegate();

    public abstract class TestBaseA { }

    public abstract class TestBaseB : TestBaseA { }

    public abstract class TestBaseC : TestBaseB { }

    public class TestInheritanceChain : TestBaseC, ITestInterface { }
}
