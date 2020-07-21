using Microsoft.VisualStudio.TestTools.UnitTesting;

using NuGet.Frameworks;

using Parser;

namespace UnitTests
{
    [TestClass]
    public class LexemeTest
    {
        [TestMethod("Can detect 1st Char")]
        public void Input1()
        {
            ILexemeProvider p = new LexemeProvider("      Nexus is valid");

            if(p.Next())
                Assert.IsTrue('N' == p.Current);
            else
                Assert.Fail("Could not find the first character");
        }

        [TestMethod("All empty input cannot be traversed")]
        public void I2()
        {
            ILexemeProvider p = new LexemeProvider("     ");

            Assert.IsFalse(p.Next());
        }
    }
}
