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
            Assert.IsTrue('N' == p.Current);
        }

        [TestMethod("All empty input cannot be traversed")]
        public void I2()
        {
            ILexemeProvider p = new LexemeProvider("     ");

            Assert.IsFalse(p.Next());
        }
    }
}
