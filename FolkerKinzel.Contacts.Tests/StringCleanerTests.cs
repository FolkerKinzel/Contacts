using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.Contacts;



namespace FolkerKinzel.Contacts.Tests
{
    [TestClass()]
    public class StringCleanerTests
    {
        [TestMethod]
        public void CleanDataEntryTest()
        {
            string? s = " Hallo\n, dies  ist ein Teststring!  ";

            s = StringCleaner.CleanDataEntry(s);

            Assert.IsNotNull(s);
            Assert.IsTrue(s!.StartsWith("Hallo", StringComparison.Ordinal));
            Assert.IsTrue(s.EndsWith("Teststring!", StringComparison.Ordinal));

#if NETCOREAPP3_1
            Assert.IsFalse(s.Contains('\n', StringComparison.Ordinal));
            Assert.IsFalse(s.Contains("  ", StringComparison.Ordinal));
#else
            Assert.IsFalse(s.Contains("\n"));
            Assert.IsFalse(s.Contains("  "));
#endif
        }
    }
}
