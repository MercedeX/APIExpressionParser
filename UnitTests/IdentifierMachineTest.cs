using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Parser.Lexemes;
using Parser.Machines;

namespace UnitTests
{
    [TestClass, TestCategory("Identifiers")]
    public class IdentifierMachineTest
    {
        IdentifierMachine Create(ILexemeScanner provider) => new IdentifierMachine(provider);

        [TestMethod("Can extract identifier")]
        public void M1()
        {
            var p = new LexemeScanner("   Name");


            var p2 = new IdentifierMachine(p);

            var t = p2.Get();

            Assert.IsTrue(t.IsSome);
            Assert.IsTrue(t.FirstOrDefault().value == "Name");
        }


        [TestMethod("extract identifiers with symbols")]
        public void I3()
        {
            ILexemeScanner p = new LexemeScanner(" +aa'yu _ 45 ");

            var items = new List<string>();
            while(p.IsSafeToRead)
            {
                var machine = Create(p);
                var t0 = machine.Get();

                if(t0.IsSome)
                {
                    items.Add(t0.First().value);
                    machine.Done();
                }
                else
                {
                    p.Next();
                }
            }

            Assert.IsTrue(items.Count == 2);
            Assert.IsTrue(items[0] == "aa" && items[1] == "yu");
        }

    }
}