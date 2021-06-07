namespace BookGen.Tests.Assemblydoc
{
    /// <summary>
    /// A simple class
    /// </summary>
    public class TestClass 
    {
        /// <summary>
        /// A gettable property
        /// </summary>
        protected internal int GetProperty { get; }

        /// <summary>
        /// An other gettable property
        /// </summary>
        internal int GetProperty2 { get; private set; }

        /// <summary>
        /// A gettable and settable property
        /// </summary>
        public int NormalProperty { get; set; }

        /// <summary>
        /// A gettabe and init property
        /// </summary>
        public int InitProperty { get; init; }

        /// <summary>
        /// A protected property
        /// </summary>
        protected int Portected { get; }

        /// <summary>
        /// A private property
        /// </summary>
        private int Private { get; }

        /// <summary>
        /// Test class constructor
        /// </summary>
        public TestClass() { }

        /// <summary>
        /// Test class constructor 2
        /// </summary>
        /// <param name="a">A parameter</param>
        /// <param name="b">B parameter</param>
        public TestClass(int a, int b) { }

        /// <summary>
        /// A simple void method
        /// </summary>
        public void VoidMethod() { }

        /// <summary>
        /// A void method with parameters
        /// </summary>
        /// <param name="first">first parameter</param>
        /// <param name="second">second parameter</param>
        public void VoidMethodParams(int first, int second) { }

        /// <summary>
        /// A reference and out parameter method
        /// </summary>
        /// <param name="parameter">reference parameter</param>
        /// <param name="another">out parameter</param>
        /// <returns>return value</returns>
        public int RefOutMethod(ref int parameter, out int another)
        {
            another = 1;
            return parameter;
        }
    }
}
