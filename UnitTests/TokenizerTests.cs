using System.Collections.Generic;
using LanguageExt;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Parser;

namespace UnitTests
{
    [TestCategory("Tokenizer"), TestClass]
    public class TokenizerTests
    {
        [TestMethod]
        public void M()
        {
            var data = "Name='John'AND age=45ORtype<'Senior'";
            var t = new Tokenizer(data);

            var list = new List<Token>();
            foreach(var it in t.GetTokens())
            {
                list.Add(it);
            }
        }
        
    }
}