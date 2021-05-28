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
    }
}
