using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser;
using Parser.Lexemes;
using Parser.Machines;

namespace UnitTests
{
    [TestClass, TestCategory("Strings")]
    public class StringTests
    {
        [TestMethod("Multiple Strings")]
        public void M()
        {
            var p = new LexemeScanner("\'Hello World\" this \' ");
            var m = new StringMachine(p);

            var tokens = new List<Token>();
            var garbage = new StringBuilder();

            while(p.IsSafeToRead)
            {
                var t0 = m.Get();
                t0.Match(x =>
                {
                    tokens.Add(x);
                    m.Done();
                }, () =>
                {
                    garbage.Append(p.Current.data);
                    p.Next();
                });
            }

            Assert.IsTrue(tokens.Count == 1);
            Assert.IsTrue(garbage.Length > 0);
        }
    }
}