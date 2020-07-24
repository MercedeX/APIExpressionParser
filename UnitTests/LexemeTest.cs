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


            while(p.IsSafeToRead && p.Current.type != LexemeType.Alpha)
                p.Next();

            if(p.IsSafeToRead)
                Assert.IsTrue('N' == p.Current.data);
            else
                Assert.Fail("Incorrect position in provider");
        }

        [TestMethod("All empty input cannot be traversed")]
        public void I2()
        {
            ILexemeProvider p = new LexemeProvider("     ");

            while(p.IsSafeToRead)
                p.Next();

            Assert.IsFalse(p.IsSafeToRead);
            Assert.IsFalse(p.Next());
        }
    }
}
