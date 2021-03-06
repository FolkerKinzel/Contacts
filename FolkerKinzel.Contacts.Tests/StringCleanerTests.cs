﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;



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

#if NET45
            Assert.IsFalse(s.Contains("\n"));
            Assert.IsFalse(s.Contains("  "));
#else
            Assert.IsFalse(s.Contains('\n', StringComparison.Ordinal));
            Assert.IsFalse(s.Contains("  ", StringComparison.Ordinal));
#endif
        }
    }
}
