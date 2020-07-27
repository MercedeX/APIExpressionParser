using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser;
using Parser.Lexemes;
using Parser.Machines;

namespace UnitTests
{
    [TestClass, TestCategory("Operators")]
    public class OperatorMachineTest
    {
        [TestMethod("AND OR NOT")]
        public void M1()
        {
            var p = new LexemeProvider("   AND OR NOT NOTO");


            var p2 = new OperatorMachine(p);

            var t = p2.Get();

            Assert.IsTrue(t.IsSome && t.First() == Operators.AND);

            t = p2.Get();

            Assert.IsTrue(t.IsSome &&  t.First() == Operators.OR );

            t = p2.Get();
            Assert.IsTrue(t.IsSome && t.First() == Operators.NOT);

            t = p2.Get();
            Assert.IsTrue(t.IsNone);
        }

    }
}