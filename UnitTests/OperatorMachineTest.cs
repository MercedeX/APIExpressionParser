using System.Collections.Generic;
using System.Linq;

using LanguageExt;

using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            var p = new LexemeScanner("   AND OR NOT NOTO");


            var p2 = new OperatorMachine(p);

            var t = p2.Get();

            Assert.IsTrue(t.IsSome && t.First().@operator == Operators.AND);

            t = p2.Get();

            Assert.IsTrue(t.IsSome && t.First().@operator == Operators.OR);

            t = p2.Get();
            Assert.IsTrue(t.IsSome && t.First().@operator == Operators.NOT);

            t = p2.Get();
            Assert.IsTrue(t.IsNone);
        }

        [TestMethod("Garbage Input")]
        public void M2()
        {
            var data = " 77 AND45 OR( NO";

            var p = new LexemeScanner(data);
            var items = new List<Operators>();

            while(p.IsSafeToRead)
            {
                var machine =  new OperatorMachine(p);
                var op = machine.Get();

                if(op.IsSome)
                {
                    items.Add(op.First().@operator);
                    machine.Done();
                }
                else
                {
                    p.Next();
                }
            }

            Assert.IsTrue(items.Any(), "Nothing extracted");
            Assert.IsTrue(items.Count == 2, "not found required number of elements");
        }
    }
}