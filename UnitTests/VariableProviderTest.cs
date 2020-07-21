using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser;

namespace UnitTests
{
    [TestClass, TestCategory("Identifier Provider")]
    public class VariableProviderTest
    {
        [TestMethod("Can extract identifier")]
        public void M1()
        {
            var p = new LexemeProvider(" Name");
            p.Next();
            var p2 = new VariableProvider(p);

            var t = p2.Get();

            Assert.IsTrue(t.IsSome);
            Assert.IsTrue(t.FirstOrDefault() == "Customer");
        }

    }
}