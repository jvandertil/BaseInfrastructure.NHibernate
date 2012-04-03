using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaseInfrastructure.NHibernate.TestCore
{
    /// <summary>
    /// Provides a base for running Unit Tests.
    /// </summary>
    [TestClass]
    public abstract class TestBase
    {
        private bool _classInitCalled = false;

        [TestInitialize]
        public void TestInitialize()
        {
            if (!_classInitCalled)
            {
                BeforeAllTests();
            }

            Given();
            When();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            CleanUp();
        }

        /// <summary>
        /// This function is run once per class to set up dependencies needed by all classes.
        /// 
        /// Can be overriden, but be sure to call the base function aswell.
        /// </summary>
        protected virtual void BeforeAllTests()
        {
            _classInitCalled = true;
        }

        /// <summary>
        /// This function is run once for each test.
        /// 
        /// Use this function to set up dependencies and the environment.
        /// </summary>
        protected virtual void Given()
        {
            Trace.Listeners.Add(new ConsoleTraceListener());
        }

        /// <summary>
        /// This function is run once for each test.
        /// 
        /// Use this function to perform functionality that you want to assert.
        /// </summary>
        protected virtual void When() { }


        /// <summary>
        /// This function is run after a test.
        /// 
        /// Use it to clean up your stuff, and provide a clean environment for the next test.
        /// </summary>
        protected virtual void CleanUp() { }
    }
}
