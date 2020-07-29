using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Parser.Lexemes;
using Parser.Machines;

namespace UnitTests
{
    [TestClass]
    public class LexemeTest
    {

        [TestMethod("Can detect 1st Char")]
        public void Input1()
        {
            ILexemeScanner p = new LexemeScanner("      Nexus is valid");


            while(p.IsSafeToRead && p.Current.type != LexemeType.Alpha)
            {
                p.Next();
            }

            if(p.IsSafeToRead)
            {
                Assert.IsTrue('N' == p.Current.data);
            }
            else
            {
                Assert.Fail("Incorrect position in provider");
            }
        }

        [TestMethod("All empty input cannot be traversed")]
        public void I2()
        {
            ILexemeScanner p = new LexemeScanner("     ");

            while(p.IsSafeToRead)
            {
                p.Next();
            }

            Assert.IsFalse(p.IsSafeToRead);
            Assert.IsFalse(p.Next());
        }

        [TestMethod("Parent and Child")]
        public void P1()
        {
            var lp = new LexemeScanner("Hello = New AND Parent = SOSO ");
            var items = new List<object>();

            while(lp.IsSafeToRead)
            {
                var idm = new IdentifierMachine(lp);
                var canContinue = true;

                do
                {
                    var t0 = idm.Get();
                    t0.Match(x =>
                    {
                        items.Add(x);
                        idm.Done();
                    }, () => canContinue = false);
                } while(canContinue);



                var om = new OperatorMachine(lp);
                canContinue = true;

                do
                {
                    var t0 = om.Get();
                    t0.Match(x =>
                    {
                        items.Add(x);
                        om.Done();
                    }, () => canContinue = false);

                } while(canContinue);
            }
        }



    }
}

