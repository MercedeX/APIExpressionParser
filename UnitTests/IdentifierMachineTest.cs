using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser;
using Parser.Lexemes;
using Parser.Machines;

namespace UnitTests
{
    [TestClass, TestCategory("Identifiers")]
    public class IdentifierMachineTest
    {
        [TestMethod("Can extract identifier")]
        public void M1()
        {
            var p = new LexemeProvider("   Name");

            
            var p2 = new IdentifierMachine(p);

            var t = p2.Get();

            Assert.IsTrue(t.IsSome);
            Assert.IsTrue(t.FirstOrDefault() == "Name");
        }

    }
}